using QR.IPrism.Models.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.ViewModels
{
   public class SearchViewModel
    {
       public List<SearchModel> SearchModels { get; set; }
     
       public int StartAt { get; set; }

       /// <summary>
       /// First item on page (user format).
       /// </summary>
       public int FromItem { get; set; }

       /// <summary>
       /// Last item on page (user format).
       /// </summary>
       public int ToItem { get; set; }

       /// <summary>
       /// Total items returned by search.
       /// </summary>
       public int Total { get; set; }

       /// <summary>
       /// Time it took to make the search.
       /// </summary>
       public TimeSpan Duration { get; set; }

       /// <summary>
       /// How many items can be showed on one page.
       /// </summary>
       public  int MaxResults = 10;

       public int PageCount
		{
			get 
			{
				//return (Total - 1) / MaxResults; // floor
                return Total / MaxResults + (Total % MaxResults > 0 ? 1 : 0);
			}
		}

		/// <summary>
		/// First item of the last page
		/// </summary>
       public int LastPageStartsAt
		{
			get
			{
                return (((PageCount * MaxResults) - MaxResults)>0?((PageCount * MaxResults) - MaxResults):0);
			}
		}
    }
}
