
using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.BusinessObjects.Implementation;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Adapter.Helper;
using QR.IPrism.Models.ViewModels;
using QR.IPrism.Utility;
using QR.IPrism.Models.Shared;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Util;
using System;
using Lucene.Net.Analysis;
using System.Configuration;
using System.Text.RegularExpressions;


namespace QR.IPrism.Adapter.Implementation
{
    public class SearchAdapter : ISearchAdapter
    {
        private ISearchDao _searchDao = new SearchDao();

        /// <summary>
        /// Get Searchs 
        /// </summary>
        /// <returns></returns>
        public async Task<List<FlightInfoModel>> GetFlightInfosAsyc(CommonFilterModel filterInput)
        {
            //Define variables 
            List<FlightInfoModel> searchList = new List<FlightInfoModel>();


            List<FlightInfoEO> search = await _searchDao.GetFlightInfosAsyc(Mapper.Map(filterInput, new CommonFilterEO())); //Get Search data from stored procedure                        
            Mapper.Map<List<FlightInfoEO>, List<FlightInfoModel>>(search, searchList);

            return searchList;
        }

        public async Task<SearchViewModel> GetSearchAsyc(SearchFilterModel filterInput)
        {
            DateTime start = DateTime.Now;
            SearchViewModel searchViewModel = new SearchViewModel();
            searchViewModel.SearchModels = new List<SearchModel>();
            searchViewModel.MaxResults = filterInput.MaxSearchResult;
            // create the searcher
            // index is placed in "index" subdirectory
            // string indexDirectory = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/index");
            string indexDirectory = filterInput.IndexDirectory;
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

            IndexSearcher searcher = new IndexSearcher(FSDirectory.Open(indexDirectory));
            searcher.SetDefaultFieldSortScoring(true, true);


            // parse the query, "text" is the default field to search
            MultiFieldQueryParser parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new string[] { "text" }, analyzer);
            //var parser = new QueryParser(Version.LUCENE_30, "text", analyzer);
            parser.AllowLeadingWildcard = true;
            parser.MultiTermRewriteMethod = MultiTermQuery.SCORING_BOOLEAN_QUERY_REWRITE;
            parser.DefaultOperator = QueryParser.Operator.AND;


            string cleanedQuery = filterInput.Query.Replace(@"\n", string.Empty)
                .Replace(@"\a", string.Empty)
                .Replace(@"\r", string.Empty)
                .Replace(@"\", string.Empty);

            Query query = parser.Parse(QueryParser.Escape(cleanedQuery));
            //var wildcardQuery = new WildcardQuery(new Term("text", "*" + this.Query + "*")); 
            //wildcardQuery.RewriteMethod = MultiTermQuery.SCORING_BOOLEAN_QUERY_REWRITE;
            //wildcardQuery.DefaultOperator = QueryParser.Operator.AND;

            //Query query = wildcardQuery;


            // search
            TopDocs hits = searcher.Search(query, 200);
            searchViewModel.Total = hits.TotalHits;

            // create highlighter
            IFormatter formatter = new SimpleHTMLFormatter("<span style=\"font-weight:bold;\">", "</span>");
            SimpleFragmenter fragmenter = new SimpleFragmenter(80);


            QueryScorer scorer = new QueryScorer(query);
            Highlighter highlighter = new Highlighter(formatter, scorer);
            highlighter.TextFragmenter = fragmenter;

            // initialize StartAt
            searchViewModel.StartAt = InitStartAt(filterInput.Start, searchViewModel.Total, searchViewModel.LastPageStartsAt);

            // how many items we should show - less than defined at the end of the results
            int resultsCount = Math.Min(searchViewModel.Total, searchViewModel.MaxResults + searchViewModel.StartAt);

            for (int i = searchViewModel.StartAt; i < resultsCount; i++)
            {
                // get the document from index
                Lucene.Net.Documents.Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);

                TokenStream stream = analyzer.TokenStream("", new System.IO.StringReader(doc.Get("text")));
                String sample = highlighter.GetBestFragments(stream, doc.Get("text"), 2, "...");

                String path = doc.Get("path");
                //when highlighter not return the string this part work 
                if (string.IsNullOrEmpty(sample) && !string.IsNullOrEmpty(doc.Get("text")))
                {
                    var fullDocumet = doc.Get("text");
                    var regex = new Regex("[^.!?;]*(" + cleanedQuery + ")[^.!?;]*");
                    var mathes = regex.Matches(fullDocumet);
                    int senetenceCount = mathes.Count > 0 ? 1 : 0;
                    if (senetenceCount > 0)
                    {
                        var result = Enumerable.Range(0, senetenceCount).Select(index => mathes[index].Value).FirstOrDefault();
                        sample = Regex.Replace(result, cleanedQuery, "<span style=\"font-weight:bold;\">" + cleanedQuery + "</span>", RegexOptions.IgnoreCase);
                    }
                    else {
                        var result = truncateBySent(fullDocumet, cleanedQuery);
                        sample = Regex.Replace(result, cleanedQuery, "<span style=\"font-weight:bold;\">" + cleanedQuery + "</span>", RegexOptions.IgnoreCase);
                    }

                }


                SearchModel searchModel = new SearchModel();
                searchModel.Id = doc.Get("id");
                //path and url remvoed - Document show work as doc library
                searchModel.Title = doc.Get("title");
                searchModel.Path = path;
                searchModel.URL = "";
                searchModel.DocType = filterInput.DocType;
                searchModel.FileCode = filterInput.FileCode;


                searchModel.Sample = sample;
                searchViewModel.SearchModels.Add(searchModel);


            }
            searcher.Dispose();

            searchViewModel.SearchModels = searchViewModel.SearchModels.Distinct().ToList();
            // result information
            searchViewModel.Duration = DateTime.Now - start;
            searchViewModel.FromItem = resultsCount > 0 ? searchViewModel.StartAt + 1 : 0;
            searchViewModel.ToItem = Math.Min(searchViewModel.StartAt + searchViewModel.MaxResults, searchViewModel.Total);
            return searchViewModel;
        }

        public static string truncateBySent(string source, string searchWord, int sentPrepend = 1, int sentAppend = 1, bool onlyShowFirst = true, string viewMoreTag = "", bool alwaysShowViewMoreTag = false, string startTruncTag = "", bool returnSourceIfKeywordNotFound = false, string returnNotFound = "")
        {
            //going to be the final string
            string truncated = "";

            //parse source sentences
            string[] sents = Regex.Split(source, @"(?<=[.?!;])\s+(?=\p{Lu})");

            //create some search start & end holders
            int i = 0;
            int ssent = -1;
            int esent = 0;

            //find start / end
            foreach (string sent in sents)
            {
                //search using regex for word boundaries \b
                if (Regex.IsMatch(sent, "\\b" + searchWord + "\\b", RegexOptions.IgnoreCase))
                {
                    if (ssent == -1)
                    {
                        ssent = i;
                    }
                    else
                    {
                        esent = i;
                    }
                }

                i++;
            }

            //make final string:

            if (esent == 0 || onlyShowFirst == true) esent = ssent;

            i = 0;

            foreach (string sent in sents)
            {
                if (i == ssent - sentPrepend || i == ssent || i == esent + sentAppend || (i >= ssent - sentPrepend && i <= esent + sentAppend))
                {
                    truncated = truncated + sent + " ";
                }

                i++;
            }

            //add view more

            if (esent + sentAppend + 1 < sents.Count() || alwaysShowViewMoreTag == true)
            {
                truncated = truncated + viewMoreTag;
            }

            //add beginning tag
            if (ssent - sentPrepend > 0)
            {
                truncated = startTruncTag + truncated;
            }

            //check if anything was even found:

            if (ssent == -1)
            {
                if (returnSourceIfKeywordNotFound)
                { truncated = source; }
                else
                {
                    truncated = returnNotFound;
                }
            }

            //and now return the final string - do a trim and remove double spaces.
            //did i ever mention how much i despise double spaces?
            return truncated.Trim().Replace("  ", " ");
        }
        /// <summary>
        /// Initializes StartAt value. Checks for bad values.
        /// </summary>
        /// <returns></returns>
        private int InitStartAt(int start, int total, int LastPageStartsAt)
        {
            try
            {
                int sa = start;

                // too small starting item, return first page
                if (sa < 0)
                    return 0;

                // too big starting item, return last page
                if (sa >= total - 1)
                {
                    return LastPageStartsAt;
                }

                return sa;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Get WeatherInfos 
        /// </summary>
        /// <returns></returns>
        public async Task<List<WeatherInfoModel>> GetWeatherInfosAsyc(CommonFilterModel filterInput)
        {
            //Define variables 
            List<WeatherInfoModel> weatherInfoList = new List<WeatherInfoModel>();
            WeatherInfoViewModel vm = new WeatherInfoViewModel();


            List<WeatherInfoEO> weatherInfo = await _searchDao.GetWeatherInfosAsyc(Mapper.Map(filterInput, new CommonFilterEO())); //Get WeatherInfo data from stored procedure                        
            Mapper.Map<List<WeatherInfoEO>, List<WeatherInfoModel>>(weatherInfo, weatherInfoList);


            return weatherInfoList;
        }

        /// <summary>
        /// Get WeatherInfos 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CrewLocatorModel>> GetCrewLocatorsAsyc()
        {
            //Define variables 
            List< CrewLocatorModel>  CrewLocatorList = new List< CrewLocatorModel>();

            List<CrewLocatorEO> CrewLocator = await _searchDao.GetCrewLocatorsAsyc(); //Get  CrewLocator data from stored procedure                        
            Mapper.Map<List< CrewLocatorEO>, List< CrewLocatorModel>>( CrewLocator,  CrewLocatorList);

            return  CrewLocatorList;
        }
        /// <summary>
        /// Get TrainingTransports 
        /// </summary>
        /// <returns></returns>
        public async Task<List<TrainingTransportModel>> GetTrainingTransportsAsyc(CommonFilterModel filterInput)
        {
            //Define variables 
            List<TrainingTransportModel> trainingTransportList = new List<TrainingTransportModel>();
            TrainingTransportViewModel vm = new TrainingTransportViewModel();


            List<TrainingTransportEO> trainingTransport = await _searchDao.GetTrainingTransportsAsyc(Mapper.Map(filterInput, new CommonFilterEO())); //Get TrainingTransport data from stored procedure                        
            Mapper.Map<List<TrainingTransportEO>, List<TrainingTransportModel>>(trainingTransport, trainingTransportList);

            return trainingTransportList;
        }

        /// <summary>
        /// Get CurrencyDetails 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CurrencyDetailModel>> GetCurrencyDetailsAsyc(CommonFilterModel filterInput)
        {
            //Define variables 
            List<CurrencyDetailModel> currencyDetailList = new List<CurrencyDetailModel>();

            List<CurrencyDetailEO> currencyDetail = await _searchDao.GetCurrencyDetailsAsyc(Mapper.Map(filterInput, new CommonFilterEO())); //Get CurrencyDetail data from stored procedure                        
            Mapper.Map<List<CurrencyDetailEO>, List<CurrencyDetailModel>>(currencyDetail, currencyDetailList);

            return currencyDetailList;
        }

        /// <summary>
        /// Get LocationCrewDetails 
        /// </summary>
        /// <returns></returns>
        public async Task<List<LocationCrewDetailModel>> GetLocationCrewDetailsAsyc(CommonFilterModel filterInput)
        {
            //Define variables 
            List<LocationCrewDetailModel> locationCrewDetailList = new List<LocationCrewDetailModel>();

            List<LocationCrewDetailEO> locationCrewDetail = await _searchDao.GetLocationCrewDetailsAsyc(Mapper.Map(filterInput, new CommonFilterEO())); //Get LocationCrewDetail data from stored procedure                        
            Mapper.Map<List<LocationCrewDetailEO>, List<LocationCrewDetailModel>>(locationCrewDetail, locationCrewDetailList);


            return locationCrewDetailList;
        }

        /// <summary>
        /// Get DutySummarys 
        /// </summary>
        /// <returns></returns>
        public async Task<List<DutySummaryModel>> GetDutySummarysAsyc(CommonFilterModel filterInput)
        {
            //Define variables 
            List<DutySummaryModel> dutySummaryList = new List<DutySummaryModel>();


            List<DutySummaryEO> dutySummary = await _searchDao.GetDutySummarysAsyc(Mapper.Map(filterInput, new CommonFilterEO())); //Get DutySummary data from stored procedure                        
            Mapper.Map<List<DutySummaryEO>, List<DutySummaryModel>>(dutySummary, dutySummaryList);
            
            return dutySummaryList;
        }

        /// <summary>
        /// Get LocationFlights 
        /// </summary>
        /// <returns></returns>
        public async Task<List<LocationFlightModel>> GetLocationFlightsAsyc(CommonFilterModel filterInput)
        {
            //Define variables 
            List<LocationFlightModel> locationFlightList = new List<LocationFlightModel>();

            List<LocationFlightEO> locationFlight = await _searchDao.GetLocationFlightsAsyc(Mapper.Map(filterInput, new CommonFilterEO())); //Get LocationFlight data from stored procedure                        
            Mapper.Map<List<LocationFlightEO>, List<LocationFlightModel>>(locationFlight, locationFlightList);

            return locationFlightList;
        }

        public async Task<String> RunCrewLocatorProcess()
        {

            return await _searchDao.RunCrewLocatorProcess();
        }
      
        public async Task<List<AssessmentSearchModel>> GetAutoSuggestStaffInfo(string filter)
        {
            //Define variables 
            List<AssessmentSearchModel> crewSearchList = new List<AssessmentSearchModel>();

            List<AssessmentSearchEO> crewSearch = await _searchDao.GetAutoSuggestStaffInfo(filter); //Get CurrencyDetail data from stored procedure                        
            Mapper.Map<List<AssessmentSearchEO>, List<AssessmentSearchModel>>(crewSearch, crewSearchList);

            return crewSearchList;
        }

    }
}



