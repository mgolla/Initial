using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.BusinessObjects.Implementation;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Adapter.Helper;
using QR.IPrism.Models.ViewModels;
using QR.IPrism.Utility;
using QR.IPrism.Models.Shared;
using QR.IPrism.EntityObjects.Shared;

namespace QR.IPrism.Adapter.Implementation
{
    public class RosterAdapter : IRosterAdapter
    {
        private IRosterDao _rosterDao = new RosterDao();
        int tempID = 1;
        /// <summary>
        /// Get Rosters according to logged person  
        /// </summary>
        /// <returns></returns>
        public async Task<RosterViewModel> GetRostersAsyc(RosterFilterModel filterInput)
        {
            //Define variables 
            List<RosterModel> rosterList = new List<RosterModel>();
            List<RosterModel> finalRosterList = new List<RosterModel>();
            List<DateTime> existingDates = new List<DateTime>(); //You fill with values
            RosterViewModel vm = new RosterViewModel();
            vm.IsDataLoaded = IsDataLoaded.No;
            var range = "";
            //assign filters according weekly or monthly  
            if (filterInput.RosterTypeID == RosterType.Weekly)
            {
                ISharedAdapter sharedAdapter = new SharedAdapter();
                var data = sharedAdapter.GetCommonInfoAsyc(RosterType.WeeklyRosterDays).Result;
                int leftvalue = -10;
                int rightvalue = 10;
                if (data != null)
                {
                    var lVal = data.Where(x => x.Value == RosterType.LeftValue).FirstOrDefault();
                    if (lVal != null)
                    {
                        leftvalue = -(Convert.ToInt32(lVal.Text));
                    }
                    var rVal = data.Where(x => x.Value == RosterType.RightValue).FirstOrDefault();
                    if (rVal != null)
                    {
                        rightvalue = (Convert.ToInt32(rVal.Text));
                    }

                }
                if (filterInput.IsLanding)//Identify as first time loading the page -if landing is true
                {

                    var today = DateTime.Today;
                    filterInput.StartDate = today.AddDays(leftvalue);
                    filterInput.EndDate = today.AddDays(rightvalue);
                }
                else //not landing time 
                {
                    if (filterInput.IsNext == FilterButton.Next) //if user clicked the next button 
                    {
                        filterInput.StartDate = (DateTime)filterInput.EndDate;
                        filterInput.EndDate = filterInput.StartDate.AddDays(leftvalue + rightvalue);
                    }
                    else if (filterInput.IsNext == FilterButton.Previous) //if user clicked the previous button 
                    {
                        filterInput.EndDate = (DateTime)filterInput.StartDate;
                        filterInput.StartDate = filterInput.EndDate.AddDays(-(leftvalue + rightvalue));
                    }
                }
                var sDate = Common.DateFormateddMMMyyyyWithSpace(filterInput.StartDate).Split(' ');
                var eDate = Common.DateFormateddMMMyyyyWithSpace(filterInput.EndDate).Split(' ');
                vm.Range = sDate[0] + " " + sDate[1] + " - " + eDate[0] + " " + eDate[1];
                range = vm.Range;
            }
            else //Monthly
            {
                var today = DateTime.Today;
                if (filterInput.IsLanding)//Identify as first time loading the page -if landing is true
                {
                    filterInput.StartDate = new DateTime(today.Year, today.Month, 1);
                    filterInput.EndDate = filterInput.StartDate.AddMonths(1).AddDays(-1);
                }
                else //not landing time 
                {
                    if (filterInput.IsNext == FilterButton.Next) //if user clicked the next button 
                    {
                        filterInput.StartDate = filterInput.StartDate.AddMonths(1).AddDays(0);
                        filterInput.EndDate = filterInput.StartDate.AddMonths(1).AddDays(-1);
                    }
                    else if (filterInput.IsNext == FilterButton.Previous) //if user clicked the previous button
                    {
                        filterInput.StartDate = filterInput.StartDate.AddMonths(-1).AddDays(0);
                        filterInput.EndDate = filterInput.StartDate.AddMonths(1).AddDays(-1);
                    }
                }
                var sDate = Common.DateFormateddMMMyyyyWithSpace(filterInput.StartDate).Split(' ');
                var eDate = Common.DateFormateddMMMyyyyWithSpace(filterInput.EndDate).Split(' ');
                vm.Range = sDate[0] + " " + sDate[1] + " - " + eDate[0] + " " + eDate[1];
                range = vm.Range;
            }

            RosterViewModelEO rosterVM = await _rosterDao.GetRostersAsyc(Mapper.Map(filterInput, new RosterFilterEO())); //Get roster data from stored procedure                        


            Mapper.Map<RosterViewModelEO, RosterViewModel>(rosterVM, vm);


            rosterList = vm.Rosters;
            vm.Range = range;
            if (vm.IsWorking && vm.IsDataLoaded == IsDataLoaded.Yes)
            {
                DateTime currentDate = filterInput.StartDate;
                DateTime endDate = Convert.ToDateTime(filterInput.EndDate);

                existingDates = rosterList.Select(x => Convert.ToDateTime(x.DutyDate.Replace(" ", "-"))).Distinct().ToList(); // Get all existing dates 
                tempID = tempID + 500;
                while (currentDate <= endDate)
                {
                    if (!existingDates.Contains(currentDate)) // check whether current date is not in list - filling missing dates 
                    {
                        existingDates.Add(currentDate);
                        RosterModel model = new RosterModel();
                        model.DutyDate = Common.DateFormateddMMMyyyyWithSpace(currentDate);
                        model.DutyDateActualDateName = Convert.ToDateTime(currentDate).ToString("ddd");
                        model.IsOnFlight = true;
                        model.TempID = tempID++;
                        finalRosterList.Add(model);
                    }
                    else
                    {
                        var dutyDate = Common.DateFormateddMMMyyyyWithSpace(currentDate);
                        var dateList = rosterList.Where(x => x.DutyDate == dutyDate);
                        if (dateList.Count() > 0)
                        {
                            RosterModel model = new RosterModel();
                            model = dateList.FirstOrDefault();
                           

                            //if (Convert.ToDateTime(model.StandardTimeDepartureUtc) >= DateTime.Now.AddHours(9))
                            //{
                            //    model.AssessmentAllowed = true;
                            //}
                            model.AssessmentAllowed = CheckAssessmentAllowed(Convert.ToDateTime(model.StandardTimeDepartureUtc));

                            var extraRosters = dateList.Skip(1).ToList();

                            model.Rosters = extraRosters;
                            foreach (var item in extraRosters)
                            {
                                item.AssessmentAllowed = CheckAssessmentAllowed(Convert.ToDateTime(item.StandardTimeDepartureUtc));
                            }
                            model.CountOfExtraRosters = extraRosters.Count();
                            finalRosterList.Add(model);
                        }
                    }
                    //Increment date
                    currentDate = currentDate.AddDays(1);
                }

              
                vm.Rosters = finalRosterList;
                vm.TimeFormats = new List<LookupModel>();
                vm.CurrentDateUTC = DateTime.UtcNow.ToString("ddd") + " , " + DateTime.UtcNow.ToLongDateString() + " " + DateTime.UtcNow.ToString("h:m:s t");
             
                var selectedDate = DateTime.Today;
                if (filterInput.LastSelectedDate != null)
                {
                    selectedDate = Convert.ToDateTime(filterInput.LastSelectedDate);
                }

                var selectedRoster = finalRosterList.Where(x => x.DutyDate == selectedDate.ToString("dd MMM yyyy")).FirstOrDefault();
                var selectedTempId = 0;
                if (selectedRoster != null)
                {
                    selectedTempId = selectedRoster.TempID;
                }
                vm.SelectedRosterTempID = selectedTempId;
            }
            vm.StartDate = filterInput.StartDate;
            vm.EndDate = filterInput.EndDate;
            vm.TimeFormat = filterInput.TimeFormat;
            return vm;
        }

        private bool CheckAssessmentAllowed(DateTime dept)
        {

            return (dept >= DateTime.Now.AddHours(9) ? true : false);
        }

        public async Task<RosterViewModel> GetRosterForAssmt(RosterFilterModel filterInput)
        {
            var rosters = GetRostersAsyc(filterInput).Result;

            //foreach (var roster in rosters.Rosters)
            //{
            //    var std = Convert.ToDateTime(roster.StandardTimeDepartureUtc);
            //    var validDt = DateTime.Now.AddHours(9);
            //    if ( std >= validDt)
            //    {
            //        roster.AssessmentAllowed = true;
            //    }
            //}

            return rosters;
        }

        public async Task<RosterViewModel> GetWeeklyRostersAsyc(RosterFilterModel filterInput)
        {
            //Define variables 
            List<RosterModel> rosterList = new List<RosterModel>();
            List<RosterModel> finalRosterList = new List<RosterModel>();
            List<DateTime> existingDates = new List<DateTime>();
            RosterViewModel vm = new RosterViewModel();
            vm.IsDataLoaded = IsDataLoaded.No;

            vm.TimeFormat = filterInput.TimeFormat;
            int leftvalue = filterInput.LeftValue;
            int rightvalue = filterInput.RightValue;

            var today = DateTime.Today;
            filterInput.StartDate = today.AddDays(leftvalue);
            filterInput.EndDate = today.AddDays(rightvalue);


            RosterViewModelEO rosterVM = await _rosterDao.GetRostersAsyc(Mapper.Map(filterInput, new RosterFilterEO())); //Get roster data from stored procedure                        
            Mapper.Map<RosterViewModelEO, RosterViewModel>(rosterVM, vm);
            rosterList = vm.Rosters;
            if (vm.IsWorking && vm.IsDataLoaded == IsDataLoaded.Yes)
            {


                DateTime currentDate = filterInput.StartDate;
                DateTime endDate = Convert.ToDateTime(filterInput.EndDate);

                existingDates = rosterList.Select(x => Convert.ToDateTime(x.DutyDate.Replace(" ", "-"))).Distinct().ToList(); // Get all existing dates 
                tempID = tempID + 500;
                while (currentDate <= endDate)
                {
                    if (!existingDates.Contains(currentDate)) // check whether current date is not in list - filling missing dates 
                    {
                        existingDates.Add(currentDate);
                        RosterModel model = new RosterModel();
                        model.DutyDate = Common.DateFormateddMMMyyyyWithSpace(currentDate);
                        model.IsOnFlight = true;
                        model.TempID = tempID++;
                        finalRosterList.Add(model);
                    }
                    else
                    {
                        var dutyDate = Common.DateFormateddMMMyyyyWithSpace(currentDate);
                        var dateList = rosterList.Where(x => x.DutyDate == dutyDate);
                        if (dateList.Count() > 0)
                        {
                            RosterModel model = new RosterModel();
                            model = dateList.FirstOrDefault();

                            var extraRosters = dateList.Skip(1).ToList();

                            model.Rosters = extraRosters;
                            model.CountOfExtraRosters = extraRosters.Count();
                            finalRosterList.Add(model);
                        }
                    }
                    //Increment date
                    currentDate = currentDate.AddDays(1);
                }

                vm.Rosters = finalRosterList;
                vm.TimeFormats = new List<LookupModel>();
            
                var selectedDate = DateTime.Today;
                if (filterInput.LastSelectedDate != null)
                {
                    selectedDate = Convert.ToDateTime(filterInput.LastSelectedDate);

                }

                var selectedRoster = finalRosterList.Where(x => x.DutyDate == selectedDate.ToString("dd MMM yyyy")).FirstOrDefault();
                var selectedTempId = 1;
                if (selectedRoster != null)
                {
                    selectedTempId = selectedRoster.TempID;
                }
                vm.SelectedRosterTempID = selectedTempId;
            }
            vm.TimeFormat = filterInput.TimeFormat;
            vm.StartDate = filterInput.StartDate;
            vm.EndDate = filterInput.EndDate;
            return vm;
        }

        public async Task<List<LookupModel>> GetCodeExplanationsAsyc(string filters)
        {
            List<LookupModel> lookupModels = new List<LookupModel>();

            List<LookupEO> Lookups = await _rosterDao.GetCodeExplanationsAsyc(filters); //Get roster data from stored procedure                        
            Mapper.Map<List<LookupEO>, List<LookupModel>>(Lookups, lookupModels);

            return lookupModels;
        }

        public async Task<List<LookupModel>> GetUTCDiffAsyc(string filters)
        {
            List<LookupModel> lookupModels = new List<LookupModel>();

            List<LookupEO> Lookups = await _rosterDao.GetUTCDiffAsyc(filters); //Get roster data from stored procedure                        
            Mapper.Map<List<LookupEO>, List<LookupModel>>(Lookups, lookupModels);

            return lookupModels;
        }

        public async Task<List<HotelInfoModel>> GetPrintHotelInfosAsyc(HotelInfoFilterModel filterInput)
        {
            List<HotelInfoModel> hotelModels = new List<HotelInfoModel>();

            List<HotelInfoEO> Lookups = await _rosterDao.GetPrintHotelInfosAsyc(Mapper.Map(filterInput, new HotelInfoFilterEO()));
            Mapper.Map<List<HotelInfoEO>, List<HotelInfoModel>>(Lookups, hotelModels);

            return hotelModels;
        }

    }
}
