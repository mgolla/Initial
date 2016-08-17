/*********************************************************************
 * Name          : AssessmentAdapter.cs
 * Description   : Implements IAssessmentAdapter.
 * Create Date   : 29th Feb 2016
 * Last Modified : 29th Feb 2016
 * Copyright By  : Qatar Airways
 *********************************************************************/

using AutoMapper;
using QR.IPrism.Adapter.Interfaces;
using QR.IPrism.BusinessObjects.Data_Layer.Shared;
using QR.IPrism.BusinessObjects.Implementation;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.Models.Module;
using QR.IPrism.Models.ViewModels;
using QR.IPrism.Models.Shared;
using QR.IPrism.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Adapter.Implementation
{
    public class AssessmentAdapter : IAssessmentAdapter
    {
        #region Private Variables
        private readonly IAssessmentDao _assessmentDao = new AssessmentDao();
        private readonly ISharedDao _sharedDao = new SharedDao();
        #endregion

        #region IAssessmentListAdapter Implementation

        /// <summary>
        /// Function that returns View Assement Details
        /// </summary>
        /// <param name="assessmentID">assessmentID</param>
        /// <returns>AssessmentViewModel entity contains list results of Objectives, Competencies Default, 
        /// Competencies EE, Ratings and IDP details along with Assessment IDP</returns>
        public async Task<AssessmentViewModel> GetAssessmentDetailsAsync(String assessmentID)
        {
            AssessmentViewModel assessmentVM = new AssessmentViewModel();
            AssessmentModel assmentModel = new AssessmentModel();
            List<RatingGuideLineModel> lstRatGudline = new List<RatingGuideLineModel>();
            AssessmentEO assessment = await _assessmentDao.GetAssessmentDetailsAsync(assessmentID);

            if (!string.IsNullOrEmpty(assessment.ReasonForNonSubmission))
            {
                assessment.IsRecorded = 2;
            }
            else if (assessment.AssessmentStatus.Contains("Saved") || assessment.AssessmentStatus.Contains("Complete") || assessment.AssessmentStatus.Contains("Accepted"))
            {
                assessment.IsRecorded = 1;
            }


            Mapper.Map<AssessmentEO, AssessmentModel>(assessment, assmentModel);

            List<RatingGuideLineEO> ratGuidline = await _assessmentDao.GetRatingGuidelinesAsync();
            Mapper.Map<List<RatingGuideLineEO>, List<RatingGuideLineModel>>(ratGuidline, lstRatGudline);

            assessmentVM.Assessment = assmentModel;
            assessmentVM.RatingGuideLines = lstRatGudline;
            return assessmentVM;
        }

        /// <summary>
        /// Function that returns EOAssessment entity contains IList results of Objectives, Competencies Default, Competencies EE and Ratings
        /// </summary>
        /// <param name="grade"></param>
        /// <returns>AssessmentViewModel entity contains IList results of Objectives, Competencies Default, Competencies EE and Ratings</returns>
        public async Task<AssessmentViewModel> GetAssessmnetByGradeAsync(string grade)
        {
            AssessmentViewModel assessmentVM = new AssessmentViewModel();
            AssessmentModel assmentModel = new AssessmentModel();
            List<RatingGuideLineModel> lstRatGudline = new List<RatingGuideLineModel>();
            AssessmentEO assessment = await _assessmentDao.GetAssessmnetByGradeAsync(grade);
            if (!string.IsNullOrEmpty(assessment.ReasonForNonSubmission))
            {
                assessment.IsRecorded = 2;
            }
            else
            {
                if (assessment.Objectives != null && assessment.Objectives.Count() > 0 && !string.IsNullOrEmpty(assessment.Objectives.FirstOrDefault().Comments))
                {
                    assessment.IsRecorded = 1;
                }
            }

            Mapper.Map<AssessmentEO, AssessmentModel>(assessment, assmentModel);

            List<RatingGuideLineEO> ratGuidline = await _assessmentDao.GetRatingGuidelinesAsync();
            Mapper.Map<List<RatingGuideLineEO>, List<RatingGuideLineModel>>(ratGuidline, lstRatGudline);

            assessmentVM.Assessment = assmentModel;
            assessmentVM.RatingGuideLines = lstRatGudline;
            return assessmentVM;
        }

        /// <summary>
        /// Function returns Assessor, Assessee and Flight Details to display on Assessment screen
        /// </summary>
        /// <param name="assessmentID"></param>
        /// <returns>AssessmentEO entity object containing Assessor, Assessee and Flight Details</returns>
        public async Task<AssessmentModel> GetAssessorAssesseeFlightDetailsAsync(string assessmentID)
        {
            return Mapper.Map(await _assessmentDao.GetAssessorAssesseeFlightDetailsAsync(assessmentID), new AssessmentModel());
        }

        /// <summary>
        /// Fetch all rating guidelines for crew
        /// </summary>
        /// <returns>List of EORatingGuideLine</returns>
        public async Task<IEnumerable<RatingGuideLineModel>> GetRatingGuidelinesAsync()
        {
            return Mapper.Map(await _assessmentDao.GetRatingGuidelinesAsync(), new List<RatingGuideLineModel>());
        }

        #endregion

        /// <summary>
        /// Function to get the list of IDP type
        /// </summary>
        /// <returns>type name and type id</returns>
        public async Task<Dictionary<string, string>> GetIDPTypeAsync()
        {
            return Mapper.Map(await _assessmentDao.GetIDPTypeAsync(), new Dictionary<string, string>());
        }

        ///// <summary>
        ///// Function to get Assessment Objective Percentages
        ///// </summary>
        ///// <returns>Percentage & Id</returns>
        //public async Task<Dictionary<string, string>> GetAssessmentObjectivePercentagesAsync()
        //{
        //    return Mapper.Map(await _assessmentDao.GetAssessmentObjectivePercentagesAsync(), new Dictionary<string, string>());
        //}

        /// <summary>
        /// Function Insert or Update Assessement details along with Assessment Objective details and Assessment Competency details
        /// </summary>
        /// <param name="objAssessment">EOAssessment</param>
        /// <returns>EOAssessment</returns
        public async Task<AssessmentModel> Insert_UpdateAssessObjectCompDetailsAsync(AssessmentModel objAssessmentModel, string staffNo)
        {
            var assessmentID = "";
            // Assessment details
            AssessmentModel objAssessmentDtl = ValidateAndAssignData(objAssessmentModel);

            AssessmentEO objAssessment = Mapper.Map(objAssessmentDtl, new AssessmentEO());
            if ((!string.IsNullOrEmpty(objAssessment.ReasonForNonSubmission)))
            {
                objAssessment.AssessmentStatus = Assessment.ELAPSED;
                objAssessment.IsActive = Assessment.YES;
                assessmentID = await _assessmentDao.Insert_UpdateAssessmentDetAsync(objAssessment);
                return Mapper.Map<AssessmentEO, AssessmentModel>(objAssessment, objAssessmentModel);
            }

            assessmentID = await _assessmentDao.Insert_UpdateAssessmentDetAsync(objAssessment);
            objAssessment.AssessmentID = assessmentID;

            if (objAssessment.Objectives != null)
            {
                if (objAssessment.Objectives.Any())
                {
                    objAssessment.Objectives.ToList().ForEach(Objective =>
                   {
                       Objective.AssessmentID = assessmentID;
                       Objective.CreatedBy = objAssessment.CreatedBy;
                       Objective.IsActive = objAssessment.IsActive;
                       Objective.AssessmentObjectiveID = _assessmentDao.Insert_UpdateAssessObjectDetAsync(Objective).Result;

                       Objective.Competencies.ToList().ForEach(Competency =>
                       {
                           Competency.CreatedBy = objAssessment.CreatedBy;
                           Competency.IsActive = objAssessment.IsActive;
                           Competency.AssessmentObjectiveID = Objective.AssessmentObjectiveID;

                           Competency.AssessmentCompID = _assessmentDao.Insert_UpdateAssessCompDetAsync(Competency).Result;

                       });
                       Objective.ExceedsExpectations.ToList().ForEach(ExceedsExpectation =>
                       {
                           ExceedsExpectation.IsActive = objAssessment.IsActive;
                           ExceedsExpectation.CreatedBy = objAssessment.CreatedBy;
                           ExceedsExpectation.AssessmentObjectiveID = Objective.AssessmentObjectiveID;
                           // ExceedsExpectation.IsEEChecked = ExceedsExpectation.IsEEChecked;
                           ExceedsExpectation.AssessmentCompID = _assessmentDao.Insert_UpdateAssessCompDetAsync(ExceedsExpectation).Result;

                       });
                   });
                }
            }

            _assessmentDao.Delete_AssessIDPDetailsAsync(assessmentID);
            objAssessment.Objectives.ForEach(obj =>
            {
                if (obj.AssmIDPs != null && obj.AssmIDPs.Count() > 0)
                {
                    obj.AssmIDPs.ForEach(idp =>
                    {
                        idp.AssessmentID = assessmentID;
                        idp.CreatedBy = objAssessment.CreatedBy;
                    });
                    obj.AssmIDPs = Insert_AssessIDPDetails(obj.AssmIDPs);
                }
            });

            if (string.Compare(objAssessment.AssessmentStatus.ToUpper(), Constants.AssessmentStatus.Complete.ToString().ToUpper()) == 0)
            {
                objAssessment.Objectives.ForEach(obj =>
                {
                    if (obj.AssmIDPs != null && obj.AssmIDPs.Count() > 0)
                    {
                        obj.AssmIDPs.ForEach(idp =>

                         Insert_AssessIDP(idp));

                    }
                });

                // insert notification
                var notification = AssessmentNotification(objAssessmentModel);
                await _sharedDao.InsertCrewNotificationDetails(notification, staffNo);
            }

            return Mapper.Map<AssessmentEO, AssessmentModel>(objAssessment, objAssessmentModel);
        }

        private NotificationDetailsEO AssessmentNotification(AssessmentModel model)
        {           
            var notificationDetails = new NotificationDetailsEO();
            notificationDetails.Description = Assessment.Assessments;
            notificationDetails.Type = Assessment.Assessments;
            notificationDetails.Status = NotificationStatus.Sent_For_Approval;
            notificationDetails.Date = DateTime.Now.ToUniversalTime();
            int slaDays = _assessmentDao.GetAssessmentSlaConfiguration().Result;
            if (slaDays != 0)
                notificationDetails.ActionByDate = DateTime.Now.ToUniversalTime().AddDays(slaDays);
            else
                notificationDetails.ActionByDate = DateTime.Now.ToUniversalTime().AddHours(24);
            notificationDetails.Severity = HousingConstants.Severity_High;
            notificationDetails.ToCrewId = model.AssesseeCrewDetID;
            notificationDetails.FromCrewId = model.AssessorCrewDetID;
            notificationDetails.RequestGuid = model.AssessmentID;

            return notificationDetails;
        }

        #region Insert Assessment IDP Details
        /// <summary>
        /// Function that insert Assessment IDP Details
        /// </summary>
        /// <param name="iDPDetails">List<AssessmentIDPEO></param>
        /// <returns>List<AssessmentIDPModel></returns>
        public List<AssessmentIDPEO> Insert_AssessIDPDetails(List<AssessmentIDPEO> idpDetails)
        {
            List<AssessmentIDPEO> lstIDPDet = new List<AssessmentIDPEO>();

            if (idpDetails.Any())
            {
                for (int i = 0; i < idpDetails.Count; i++)
                {
                    lstIDPDet.Add(_assessmentDao.Insert_AssessIDPDetailsAsync(idpDetails[i]).Result);
                }
            }

            return lstIDPDet;
        }
        #endregion

        #region Insert Assessment IDP
        /// <summary>
        /// Function that insert Assessment IDP
        /// </summary>
        /// <param name="assessIDP">AssessmentIDPEO</param>
        /// <returns>AssessmentIDPEO</returns>
        public AssessmentIDPEO Insert_AssessIDP(AssessmentIDPEO assessIDP)
        {
            return _assessmentDao.Insert_AssessIDPAsync(assessIDP).Result;
        }
        #endregion

        public AssessmentModel ValidateAndAssignData(AssessmentModel objAssessmentModel)
        {

            Dictionary<string, string> dictPercentage = new Dictionary<string, string>();
            dictPercentage = _assessmentDao.GetAssessmentObjectivePercentagesAsync().Result;
            string perEE = dictPercentage[Assessment.EE];
            string perDefault = dictPercentage[Assessment.DEFAULT];
            int IDPCount = 0;
            objAssessmentModel.TotalScore = 0;
            objAssessmentModel.Objectives.ToList().ForEach(Objective =>
            {

                int countCOM = 0, countDEV = 0, countSD = 0, countObs = 0, countEEAchived = 0; decimal per_comp_Score = 0; decimal per_ee_Score = 0;
                if (Objective.Competencies.Count > 0 && Objective.ExceedsExpectations.Count > 0)
                {
                    per_comp_Score = Convert.ToDecimal((Objective.Weightage * Convert.ToDecimal(perDefault) / 100)) / Convert.ToDecimal(Objective.Competencies.Count);
                    per_ee_Score = Convert.ToDecimal((Objective.Weightage * Convert.ToDecimal(perEE) / 100)) / Convert.ToDecimal(Objective.ExceedsExpectations.Count);

                }
                // Calculate per default competency and ee competency based on the percentage rule(80% of objective weightage for defult & 20% of objective weightage for ee)

                Objective.ObjScore = 0;
                Objective.Competencies.ToList().ForEach(Competency =>
                {
                    Competency.IsActive = Assessment.YES;
                    if (Competency.Rating == Assessment.COM)
                    {

                        objAssessmentModel.RatingGuidelines.ToList().ForEach(RatingGuideline =>
                         {
                             if (RatingGuideline.Rating == Assessment.COM)
                             {
                                 Competency.RatingID = RatingGuideline.RatingGuidelineID;
                             }
                         });
                        Competency.CompetencyScore = Math.Round(per_comp_Score, 2);
                        Objective.ObjScore += Math.Round(per_comp_Score, 2);
                        countCOM++;
                    }
                    else if (Competency.Rating == Assessment.DEV)
                    {
                        objAssessmentModel.RatingGuidelines.ToList().ForEach(RatingGuideline =>
                        {
                            if (RatingGuideline.Rating == Assessment.DEV)
                            {
                                Competency.RatingID = RatingGuideline.RatingGuidelineID;
                            }
                        });

                        countDEV++;
                    }
                    else if (Competency.Rating == Assessment.SD)
                    {
                        objAssessmentModel.RatingGuidelines.ToList().ForEach(RatingGuideline =>
                        {
                            if (RatingGuideline.Rating == Assessment.SD)
                            {
                                Competency.RatingID = RatingGuideline.RatingGuidelineID;
                            }
                        });

                        countSD++;
                    }
                    else
                    {
                        Competency.Rating = string.Empty;
                        Competency.RatingID = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(Competency.Comments))
                    {
                        countObs++;
                    }

                    // if (eventType == CommonConstants.SUBMITID)
                    if ((Competency.Rating == Assessment.SD || Competency.Rating == Assessment.DEV))
                    {
                        if (Objective.AssmIDPs.Count > 0)
                        {
                            Objective.AssmIDPs.ToList().ForEach(AssmIDP =>
                            {
                                if (AssmIDP.CompetencyName == Competency.CompetencyName)
                                {
                                    AssmIDP.CompConfigID = Competency.CompetencyId;
                                    AssmIDP.CreatedBy = objAssessmentModel.CreatedBy;
                                    AssmIDP.CompConfigID = Competency.CompetencyConfigId;
                                    AssmIDP.Situation = AssmIDP.Observation;

                                }

                            });
                        }

                        IDPCount = IDPCount + 1;
                    }
                });

                Objective.ExceedsExpectations.ToList().ForEach(ExceedsExpectation =>
                {

                    if (ExceedsExpectation.IsEEChecked == "true")
                    {
                        ExceedsExpectation.IsEEChecked = Assessment.YES;
                    }
                    else
                    {
                        ExceedsExpectation.IsEEChecked = "N";
                    }
                    ExceedsExpectation.IsActive = Assessment.YES;
                    if (ExceedsExpectation.IsEEChecked == Assessment.YES)
                    {
                        countEEAchived++;
                        ExceedsExpectation.CompetencyScore = Math.Round(per_ee_Score, 2);
                        Objective.ObjScore += Math.Round(per_ee_Score, 2);

                    }
                });
                if (countCOM == Objective.Competencies.Count && (countEEAchived == 1 || countEEAchived == 2))
                {
                    Objective.Rating = Assessment.EE;
                    Objective.RatingDescription = Assessment.EEDESC;
                }
                else if (countCOM == Objective.Competencies.Count && countEEAchived >= 3)
                {
                    Objective.Rating = Assessment.SEE;
                    Objective.RatingDescription = Assessment.SEEDESC;
                }
                else if (countCOM == Objective.Competencies.Count && countEEAchived == 0)
                {
                    Objective.Rating = Assessment.COM;
                    Objective.RatingDescription = Assessment.COMDESC;
                }
                else if ((countDEV == 1 || countDEV == 2) && countCOM == Objective.Competencies.Count - countDEV)
                {
                    Objective.Rating = Assessment.DEV;
                    Objective.RatingDescription = Assessment.DEVDESC;
                }
                else if ((countDEV >= 3) && countCOM == Objective.Competencies.Count - countDEV)
                {
                    Objective.Rating = Assessment.SD;
                    Objective.RatingDescription = Assessment.SDDESC;
                }
                else if (countSD >= 1)
                {
                    Objective.Rating = Assessment.SD;
                    Objective.RatingDescription = Assessment.SDDESC;
                }
                else if (countDEV >= 1)
                {
                    Objective.Rating = Assessment.DEV;
                    Objective.RatingDescription = Assessment.DEVDESC;
                }
                else if (countCOM >= 1)
                {
                    Objective.Rating = Assessment.COM;
                    Objective.RatingDescription = Assessment.COMDESC;
                }
                objAssessmentModel.TotalScore = objAssessmentModel.TotalScore + Math.Round(Objective.ObjScore);
                Objective.IsActive = Assessment.YES;

                Objective.AssmIDPs.ToList().ForEach(AssmIDP =>
                {

                    AssmIDP.AssessmentID = objAssessmentModel.AssessmentID;
                    AssmIDP.IsActive = Assessment.YES;
                    AssmIDP.AssesseeCrewDetailsID = objAssessmentModel.AssesseeCrewDetID;

                });
            });

            //AutoSave & Save
            if (objAssessmentModel.AutoSave == Assessment.YES)
            {
                if (objAssessmentModel.AssessmentStatus == Assessment.DELAYED || objAssessmentModel.AssessmentStatus == Assessment.DELAYEDSAVE)
                {
                    objAssessmentModel.AssessmentStatus = Assessment.DELAYEDSAVE;
                }
                else if (objAssessmentModel.AssessmentStatus == Assessment.SCHEDULED || objAssessmentModel.AssessmentStatus == Assessment.SCHEDULEDSAVE)
                {
                    objAssessmentModel.AssessmentStatus = Assessment.SCHEDULEDSAVE;
                }
            }
            else
            {
                objAssessmentModel.AssessmentStatus = Assessment.COMPLETED;
            }


            objAssessmentModel.IsActive = Assessment.YES;

            return objAssessmentModel;

        }

        public void DeleteAssessment(string id) 
        {
            _assessmentDao.DeleteAssessment(id);
        }

        public async Task<AssessmentBehaviourModel> GetBehaviourActionIssue(string behaviourID)
        {
            return Mapper.Map(await _assessmentDao.GetBehaviourActionIssue(behaviourID), new AssessmentBehaviourModel());
        }

        public async Task<string> InsertIDPCrewComment(string requestId, string createdBy, string crewCommentsRequired, string crewComments)
        {
            return await _assessmentDao.InsertIDPCrewComment(requestId, createdBy, crewCommentsRequired, crewComments);
        }
    }
}
