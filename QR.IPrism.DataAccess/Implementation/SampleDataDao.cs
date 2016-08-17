using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.BusinessObjects.Implementation
{
    public static class SampleDataDao
    {
        static int tempID = 1;
        public static RosterViewModelEO GetSampleRostersAsyc(RosterFilterEO filterInput)
        {
            RosterViewModelEO rosterViewModelEO = new RosterViewModelEO();
            rosterViewModelEO.IsWorking = true;
            List<RosterEO> responseList = new List<RosterEO>();
            rosterViewModelEO.Rosters = responseList;
            RosterEO response = null;
            DateTime currentDate = filterInput.StartDate;
            DateTime endDate = Convert.ToDateTime(filterInput.EndDate);
            int count = 0;
            while (currentDate <= endDate)
            {
                count++;
                response = new RosterEO();
                if ((count % 2)==0)
                {
                    response.Flight = "1";
                   
                }else{
                    response.Flight = "874";
                }
                response.TempID =  tempID++;
                response.StaffID = "70000";
                response.StaffName = "Sample name ";
                response.Cc = "";
                response.Flight = "1";
                response.DutyDate = Common.DateFormateddMMMyyyyWithSpace(currentDate);
                response.DutyDateActual = response.DutyDate;
                response.DutyDateActualDateName = Convert.ToDateTime(currentDate).ToString("ddd");
                response.Departure = "DOH";
                response.Arrival = "LHR";
                responseList.Add(response);
                rosterViewModelEO.Rosters.Add(response);
                response = new RosterEO();
                if ((count % 2) == 0)
                {
                    response.Flight = "1";

                }
                else
                {
                    response.Flight = "874";
                }

                response.StaffID = "70000";
                response.StaffName = "Sample name ";
                response.Cc = "";
                response.Flight = "1";
                response.DutyDate = Common.DateFormateddMMMyyyyWithSpace(currentDate);
                response.DutyDateActual = response.DutyDate;
                response.Departure = "DOH";
                response.Arrival = "LHR";
                responseList.Add(response);
                response.TempID = tempID++;
                responseList.Add(response);
                response.TempID = tempID++;
                responseList.Add(response);
                rosterViewModelEO.Rosters.Add(response);
                //Increment date
                currentDate = currentDate.AddDays(1);
            }

            return rosterViewModelEO;
        }
    }
}
