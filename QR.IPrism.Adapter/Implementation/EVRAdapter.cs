using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.BusinessObjects.Implementation;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.Shared;
using QR.IPrism.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Implementation
{
    public class EVRAdapter : IEVRAdapter
    {
        #region Private Variables
        private readonly IEVRDao _evrDao = new EVRDao();
        #endregion

        public async Task<IEnumerable<EVRResultModel>> GetEVRSearchResult(EVRRequestFilterModel inputs)
        {
            var filter = Mapper.Map(inputs, new EVRRequestFilterEO());
            return Mapper.Map(await _evrDao.GetEVRSearchResultAsyc(filter), new List<EVRResultModel>());
        }

        public async Task<List<EVRDraftOutputModel>> GetDraftVRForUser(string flightDetsId, string userId)
        {
            return Mapper.Map(await _evrDao.GetDraftVRForUserAsyc(flightDetsId, userId), new List<EVRDraftOutputModel>());
        }


        public async Task<List<EVRListsModel>> GetVRLastTenFlight(string userId)
        {
            return Mapper.Map(await _evrDao.GetVRLastTenFlightAsyc(userId), new List<EVRListsModel>());
        }

        public async Task<List<EVRResultModel>> GetLastTenVRs(string userId)
        {
            return Mapper.Map(await _evrDao.GetLastTenVRs(userId), new List<EVRResultModel>());
        }

        public async Task<List<EVRPendingModel>> GetPendingVRForUser(string userId)
        {
            return Mapper.Map(await _evrDao.GetPendingVRForUserAsyc(userId), new List<EVRPendingModel>());
        }


        public void UpdateNoVR(string flightDetId, string crewDetailId, string userId)
        {
            _evrDao.UpdateNoVRAsyc(flightDetId, crewDetailId, userId);
        }

        public async Task<List<EVRDraftOutputModel>> GetSubmittedEVRs(string flightDetId, string crewDetailId, string userId)
        {
            return Mapper.Map(await _evrDao.GetSubmittedEVRsAsyc(flightDetId, crewDetailId, userId), new List<EVRDraftOutputModel>());
        }

        public async Task<EVRCrewViewModel> GetEVRViewDetailsCrew(VRIdModel vrId)
        {
            var filter = Mapper.Map(vrId, new VRIdEO());
            return Mapper.Map(await _evrDao.GetVRDetailsCrewAsyc(filter), new EVRCrewViewModel());
        }

        public async Task<ViewVREnterVRModel> GetVRDetailEnterVR(VRIdModel vrId)
        {
            var filter = Mapper.Map(vrId, new VRIdEO());
            return Mapper.Map(await _evrDao.GetVRDetailEnterVRAsyc(filter), new ViewVREnterVRModel());
        }


        public void DeleteVR(string VRId)
        {
            _evrDao.DeleteVRAsyc(VRId);
        }

        public async Task<EVRInsertOutputModel> InsertVoyageReport(EVRReportMainModel inputs, string staffId, string staffNumber, string staffUserId)
        {
            // 25-Jul-2016 : Commenting the below code as when we save an EVR, it has to store via a workflow into the SQL Server 
            //               and then from there it has to store back in the IPRISM oracle databae. As previously thought though its not 
            //               required, but as now in PRISM its used and workflow is using WWF (Windows Workflow Foundation), it's been 
            //               decided to call the service and do the insert update of the EVR. The below code will be commented and we will 
            //               use the service to insert update. 
            //                  NOTE:   All the previous code is entact (without any change), so that in future if its decided that the workflow 
            //                          totally removed and only IPRISM database will be used, then we can reuse the below lines of code.
            //                              JUST UNCOMMENT BELOW TWO LINES OF CODE (for that case), and remove the SERVICE calling for INSERT UPDATE.

            var filter = Mapper.Map(inputs, new EVRReportMainEO());
            return Mapper.Map(await _evrDao.InsertVoyageReportAsyc(filter, staffId, staffNumber, staffUserId), new EVRInsertOutputModel());

            //return Mapper.Map(SaveVoyageReport(inputs, staffNumber, staffUserId), new EVRInsertOutputModel());
        }

        //private ProxyVRDetails.VRId SaveVoyageReport(EVRReportMainModel filter, string staffId, string staffNumber, string staffUserId)
        private EVRInsertOutputEO SaveVoyageReport(EVRReportMainModel filter, string staffNumber, string staffUserId)
        {
            string deptID = string.Empty;
            string vrCrewsId = string.Empty;
            string crew = string.Empty;
            IEnumerable<ProxyVRDetails.Department> dept;
            string sourceFile = string.Empty;
            string destinationFile = string.Empty;
            string sendMail = string.Empty;
            //ProxyVRDetails.VRId objSavedVR = new ProxyVRDetails.VRId();
            EVRInsertOutputEO objSavedVR = new EVRInsertOutputEO();

            List<ProxyVRDetails.VRPassengers> vrPassengers = new List<ProxyVRDetails.VRPassengers>();
            List<ProxyVRDetails.VRDocument> vrDocuments = new List<ProxyVRDetails.VRDocument>();
            ProxyVRDetails.VRInsertUpdate vrInsertUpdate = new ProxyVRDetails.VRInsertUpdate();

            vrInsertUpdate.FlightDetsID = filter.vrInsertUpdate.FlightDetId;

            vrInsertUpdate.ReportAboutID = filter.vrInsertUpdate.ReportAbtId;
            vrInsertUpdate.CategoryId = filter.vrInsertUpdate.CategoryId;
            vrInsertUpdate.SubCategoryId = filter.vrInsertUpdate.SubCategoryId;


            vrInsertUpdate.IsCritical = filter.vrInsertUpdate.IsCritical;
            vrInsertUpdate.IsVrRestricted = filter.vrInsertUpdate.IsVrRestricted;
            vrInsertUpdate.IsInfoVr = "N";
            vrInsertUpdate.IsNotIfNotReq = "N";
            vrInsertUpdate.IsCSR = filter.vrInsertUpdate.IsCSR;
            vrInsertUpdate.IsOHS = filter.vrInsertUpdate.IsOHS;
            vrInsertUpdate.IsPO = filter.vrInsertUpdate.IsPO;

            // Kept if-else logic as seems to be the scenario where all coments like Facts, Results, Actions are getting combined.
            // if (filter.vrInsertUpdate.IsNew.Trim() == "Y")
            {

                vrInsertUpdate.Facts = filter.vrInsertUpdate.vrFactComment;
                vrInsertUpdate.Action = filter.vrInsertUpdate.vrActionComment;
                vrInsertUpdate.Result = filter.vrInsertUpdate.vrResultComment;
                //vrInsertUpdate.IsNew = filter.vrInsertUpdate.IsNew; //Should alwasy be "Y"
                vrInsertUpdate.IsNew = "Y";
            }
            //else
            //{

            //    vrInsertUpdate.Comments = txtComment.Text;
            //    vrInsertUpdate.IsNew = "N";
            //}


            vrInsertUpdate.IsCabInClassNA = filter.vrInsertUpdate.IsCabinClassNA;
            vrInsertUpdate.IsCabInClassFC = filter.vrInsertUpdate.IsCabinFirstClass;
            vrInsertUpdate.IsCabInClassJC = filter.vrInsertUpdate.IsCabinBusinessClass;
            vrInsertUpdate.IsCabInClassYC = filter.vrInsertUpdate.IsCabinEconomyClass;

            vrInsertUpdate.IsRuleSetChanged = filter.vrInsertUpdate.IsRuleSetChanged;

            vrInsertUpdate.VrInstanceId = filter.vrInsertUpdate.VrInstanceId;
            vrInsertUpdate.IsAdmin = "N";
            vrInsertUpdate.StaffNumber = staffNumber;
            vrInsertUpdate.UserId = staffUserId;

            vrInsertUpdate.NODId = filter.vrInsertUpdate.NODId;
            vrInsertUpdate.VrCrewsId = filter.vrInsertUpdate.VrCrewsId;

            ProxyVRDetails.VRPassengers wdVRPsngr = null;
            foreach (EVRPassengerModel singlrPsngr in filter.VRPassengers)
            {
                if (singlrPsngr != null)
                {
                    wdVRPsngr = null;
                    wdVRPsngr = new ProxyVRDetails.VRPassengers();

                    wdVRPsngr.FFPNumber = singlrPsngr.FFNo;
                    wdVRPsngr.PassengerName = singlrPsngr.PsngrName;
                    wdVRPsngr.SeatNumber = singlrPsngr.SeatNo;

                    vrPassengers.Add(wdVRPsngr);
                }

            }

            vrInsertUpdate.VrPassengers = (from item in vrPassengers select item as ProxyVRDetails.VRPassengers).ToArray();

            vrInsertUpdate.VrStatusName = filter.vrInsertUpdate.Status;

            int vrNo = 0;

            ProxyVRDetails.VRDetailsServiceClient VrDet = new ProxyVRDetails.VRDetailsServiceClient();


            if (filter.vrInsertUpdate.InsertType.Equals("Insert", StringComparison.OrdinalIgnoreCase))
            {
                ProxyVRDetails.VRIdNo vrIdNo = VrDet.InsertVoyageReport(vrInsertUpdate);

                vrNo = vrIdNo.VrNo;
                objSavedVR.VrId = vrIdNo.VrId;
                objSavedVR.VrNo = vrIdNo.VrNo.ToString();
                objSavedVR.VrInstanceId = vrIdNo.VrInstanceId;

                if (vrNo == 0)
                {
                    return objSavedVR;
                }
            }
            else if (filter.vrInsertUpdate.InsertType.Equals("Update", StringComparison.OrdinalIgnoreCase))
            {
                objSavedVR.VrId = filter.vrInsertUpdate.VrId;

                if (filter.vrInsertUpdate.VrId != string.Empty)
                {
                    vrInsertUpdate.VrId = filter.vrInsertUpdate.VrId;
                    int.TryParse(filter.vrInsertUpdate.VrNo, out vrNo);
                    vrInsertUpdate.VrNo = vrNo;
                    vrInsertUpdate.VrInstanceId = filter.vrInsertUpdate.VrInstanceId;
                    VrDet.UpdateVoyageReport(vrInsertUpdate);
                }
            }

            VrDet.Close();
           

            return objSavedVR;
        }
    }
}
