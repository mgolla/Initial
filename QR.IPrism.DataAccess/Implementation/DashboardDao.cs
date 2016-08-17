using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR.IPrism.Enterprise;
using QR.IPrism.Utility;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.EntityObjects.Module;
using System.Data;
using System.Data.Common;
using Oracle.DataAccess.Client;

namespace QR.IPrism.BusinessObjects.Implementation
{
    public class DashboardDao : IDashboardDao
    {
        public async Task<List<NotificationAlertSVPEO>> GetNotificationAlertSVPsAsyc(NotificationAlertSVPFilterEO filterInput)
        {

            List<NotificationAlertSVPEO> responseList = new List<NotificationAlertSVPEO>();
            NotificationAlertSVPEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_staffno", filterInput.StaffID, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_cur_staff", ParameterDirection.Output, OracleDbType.RefCursor));
                    FileFilterEO filterFileEOInput = new FileFilterEO();

                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_ipa_dashboard.get_alert_notify_proc_mig", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new NotificationAlertSVPEO();

                            response.Doc = objReader["doc"] != null && objReader["doc"].ToString() != string.Empty ? (System.String)(objReader["doc"]) : default(System.String);
                            if (response.Doc.Contains("ALERT"))
                            {
                                response.Doc = "A";
                            }
                            else
                            {
                                response.Doc = "N";
                            }
                            response.DocDate = objReader["doc_date"] != null && objReader["doc_date"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["doc_date"])) : default(System.String);
                            response.DocId = objReader["doc_id"] != null && objReader["doc_id"].ToString() != string.Empty ? (System.String)(objReader["doc_id"]) : default(System.String);
                            response.DocDesc = objReader["doc_desc"] != null && objReader["doc_desc"].ToString() != string.Empty ? (System.String)(objReader["doc_desc"]) : default(System.String);
                            response.DocType = objReader["doc_type"] != null && objReader["doc_type"].ToString() != string.Empty ? (System.String)(objReader["doc_type"]) : default(System.String);
                            response.AlertFolder = objReader["alert_folder"] != null && objReader["alert_folder"].ToString() != string.Empty ? (System.String)(objReader["alert_folder"]) : default(System.String);
                            response.AlertFolderName = objReader["alert_folder_name"] != null && objReader["alert_folder_name"].ToString() != string.Empty ? (System.String)(objReader["alert_folder_name"]) : default(System.String);
                            response.FileCode = objReader["file_code"] != null && objReader["file_code"].ToString() != string.Empty ? (System.String)(objReader["file_code"]) : default(System.String);
                            response.FileId = objReader["file_id"] != null && objReader["file_id"].ToString() != string.Empty ? (System.String)(objReader["file_id"]) : default(System.String);

                            response.Link = objReader["link_det"] != null && objReader["link_det"].ToString() != string.Empty ? (System.String)(objReader["link_det"]) : default(System.String);

                            response.IsViewed = objReader["Is_Viewed"] != null && objReader["Is_Viewed"].ToString() != string.Empty ? objReader["Is_Viewed"].ToString().ToLower() == "y" : false;

                            //if (response.FileId != null && response.FileCode != null)
                            //{

                            //    filterFileEOInput.FileId = response.FileId;
                            //    filterFileEOInput.FileCode = response.FileCode;
                            //    var data = GetFilesAsyc(filterFileEOInput).Result;
                            //    if (data != null)
                            //    {
                            //        response.File = data.FirstOrDefault();
                            //    }

                            //}
                            responseList.Add(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<List<NotificationAlertSVPEO>> GetAlertNotificationHeaderAsyc(NotificationAlertSVPFilterEO filterInput)
        {

            List<NotificationAlertSVPEO> responseList = new List<NotificationAlertSVPEO>();
            NotificationAlertSVPEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_staffno", filterInput.StaffID, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_cur_staff", ParameterDirection.Output, OracleDbType.RefCursor));
                    FileFilterEO filterFileEOInput = new FileFilterEO();

                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_ipa_dashboard.get_top_alert_notifi_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new NotificationAlertSVPEO();

                            response.Doc = objReader["doc"] != null && objReader["doc"].ToString() != string.Empty ? (System.String)(objReader["doc"]) : default(System.String);
                            if (response.Doc.Contains("ALERT"))
                            {
                                response.Doc = "A";
                            }
                            else
                            {
                                response.Doc = "N";
                            }
                            response.DocDate = objReader["doc_date"] != null && objReader["doc_date"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["doc_date"])) : default(System.String);
                            response.DocId = objReader["doc_id"] != null && objReader["doc_id"].ToString() != string.Empty ? (System.String)(objReader["doc_id"]) : default(System.String);
                            response.DocDesc = objReader["doc_desc"] != null && objReader["doc_desc"].ToString() != string.Empty ? (System.String)(objReader["doc_desc"]) : default(System.String);
                            response.DocType = objReader["doc_type"] != null && objReader["doc_type"].ToString() != string.Empty ? (System.String)(objReader["doc_type"]) : default(System.String);
                            response.AlertFolder = objReader["alert_folder"] != null && objReader["alert_folder"].ToString() != string.Empty ? (System.String)(objReader["alert_folder"]) : default(System.String);
                            response.AlertFolderName = objReader["alert_folder_name"] != null && objReader["alert_folder_name"].ToString() != string.Empty ? (System.String)(objReader["alert_folder_name"]) : default(System.String);
                            response.FileCode = objReader["file_code"] != null && objReader["file_code"].ToString() != string.Empty ? (System.String)(objReader["file_code"]) : default(System.String);
                            response.FileId = objReader["file_id"] != null && objReader["file_id"].ToString() != string.Empty ? (System.String)(objReader["file_id"]) : default(System.String);

                            response.Link = objReader["link_det"] != null && objReader["link_det"].ToString() != string.Empty ? (System.String)(objReader["link_det"]) : default(System.String);

                           

                            var isView = objReader["is_viewed"] != null && objReader["is_viewed"].ToString() != string.Empty ? Convert.ToString(objReader["is_viewed"]) : default(System.String);
                            if (isView != null && isView.Contains("Y"))
                            {
                                response.IsViewed = true;
                            }
                            else
                            {
                                response.IsViewed = false;
                            }
                            responseList.Add(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<bool> UpdateAlertNotificationOnHeader(NotificationAlertSVPFilterEO filterInput)
        {

            List<NotificationAlertSVPEO> responseList = new List<NotificationAlertSVPEO>();
            NotificationAlertSVPEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    if (filterInput.StaffID != null && filterInput.Type != null && filterInput.StaffID != string.Empty && filterInput.Type != string.Empty)
                    {
                        perametersList = new List<ODPCommandParameter>();
                        perametersList.Add(new ODPCommandParameter("pi_staffno", filterInput.StaffID, ParameterDirection.Input, OracleDbType.Varchar2));
                        perametersList.Add(new ODPCommandParameter("pi_codes", filterInput.Type, ParameterDirection.Input, OracleDbType.Varchar2));

                        DbCommand command = await ODPDataAccess.ExecuteSPNonQueryParmAsync("prism_ipa_dashboard.add_viewed_alert_notifi_proc", perametersList);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return true;
        }

        public async Task<List<NotificationAlertSVPEO>> GetSVPMessagesAsyc()
        {

            List<NotificationAlertSVPEO> responseList = new List<NotificationAlertSVPEO>();
            NotificationAlertSVPEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("po_svpmessage", ParameterDirection.Output, OracleDbType.RefCursor));
                    FileFilterEO filterFileEOInput = new FileFilterEO();
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_ipa_dashboard.get_svpmessage_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            //if (objReader["doc_type"].ToString().Trim() != "FOLDER" && objReader["doc_type"].ToString().Trim() != "PDF")
                            //{
                                response = new NotificationAlertSVPEO();

                                response.Doc = objReader["doc"] != null && objReader["doc"].ToString() != string.Empty ? (System.String)(objReader["doc"]) : default(System.String);
                                response.DocDate = objReader["doc_date"] != null && objReader["doc_date"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["doc_date"])) : default(System.String);

                                response.DocId = objReader["doc_id"] != null && objReader["doc_id"].ToString() != string.Empty ? (System.String)(objReader["doc_id"]) : default(System.String);
                                response.DocDesc = objReader["doc_desc"] != null && objReader["doc_desc"].ToString() != string.Empty ? (System.String)(objReader["doc_desc"]) : default(System.String);
                                response.DocType = objReader["doc_type"] != null && objReader["doc_type"].ToString() != string.Empty ? (System.String)(objReader["doc_type"]) : default(System.String);
                                //response.AlertFolder = objReader["alert_folder"] != null && objReader["alert_folder"].ToString() != string.Empty ? (System.String)(objReader["alert_folder"]) : default(System.String);
                                //response.AlertFolderName = objReader["alert_folder_name"] != null && objReader["alert_folder_name"].ToString() != string.Empty ? (System.String)(objReader["alert_folder_name"]) : default(System.String);
                                response.FileCode = objReader["file_code"] != null && objReader["file_code"].ToString() != string.Empty ? (System.String)(objReader["file_code"]) : default(System.String);
                                response.FileId = objReader["file_id"] != null && objReader["file_id"].ToString() != string.Empty ? (System.String)(objReader["file_id"]) : default(System.String);
                                response.FileName = objReader["file_name"] != null && objReader["file_name"].ToString() != string.Empty ? (System.String)(objReader["file_name"]) : default(System.String);

                                //if (response.FileId != null && response.FileCode != null)
                                //{
                                //    filterFileEOInput.FileId = response.FileId;
                                //    filterFileEOInput.FileCode = response.FileCode;
                                //    var data = GetFilesAsyc(filterFileEOInput).Result;
                                //    if (data != null)
                                //    {
                                //        response.File = data.FirstOrDefault();
                                //        response.File.FileContent = null;
                                //    }
                                //}

                                FileEO images = new FileEO();
                                images.FileType = objReader["IMAGETYPE"] != null && objReader["IMAGETYPE"].ToString() != string.Empty ? (System.String)(objReader["IMAGETYPE"]) : default(System.String);
                                images.FileName = objReader["THUMBFILENAME"] != null && objReader["THUMBFILENAME"].ToString() != string.Empty ? (System.String)(objReader["THUMBFILENAME"]) : default(System.String);
                                images.FileContent = objReader["THUMBIMAGE"] != null && objReader["THUMBIMAGE"].ToString() != string.Empty ? (System.Byte[])(objReader["THUMBIMAGE"]) : default(System.Byte[]);
                                if (images.FileType != null && images.FileContent != null)
                                {
                                    response.THUMBIMAGE = images;
                                }

                                responseList.Add(response);
                            }
                        //}
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<List<DocumentEO>> GetDocumentsAsyc(DocumentFilterEO filterInput)
        {

            List<DocumentEO> responseList = new List<DocumentEO>();
            DocumentEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {

                    //if (filterInput.DocumentId == null)
                    //{
                    //    filterInput.DocumentId = string.Empty;
                    //}
                    if (filterInput.DocumentPath == null)
                    {
                        filterInput.DocumentPath = string.Empty;
                    }
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_documentid", filterInput.DocumentId, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_documentpath", filterInput.DocumentPath, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_documents", ParameterDirection.Output, OracleDbType.RefCursor));
                    FileFilterEO filterFileEOInput = new FileFilterEO();
                 
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_ipa_dashboard.get_documents_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new DocumentEO();

                            response.RowType = objReader["row_type"] != null && objReader["row_type"].ToString() != string.Empty ? (System.String)(objReader["row_type"]) : default(System.String);
                            response.DocumentId = objReader["documentid"] != null && objReader["documentid"].ToString() != string.Empty ? (objReader["documentid"].ToString()) : default(System.String);
                            response.DocumentName = objReader["documentname"] != null && objReader["documentname"].ToString() != string.Empty ? (System.String)(objReader["documentname"]) : default(System.String);
                            response.FileCode = objReader["file_code"] != null && objReader["file_code"].ToString() != string.Empty ? (System.String)(objReader["file_code"]) : default(System.String);
                            response.FileId = objReader["file_id"] != null && objReader["file_id"].ToString() != string.Empty ? (objReader["file_id"].ToString()) : default(System.String);
                            //if (response.FileId != null && response.FileCode != null)
                            //{

                            //    filterFileEOInput.FileId = response.FileId;
                            //    filterFileEOInput.FileCode = response.FileCode;
                            //    var data = GetFilesAsyc(filterFileEOInput).Result;
                            //    if (data != null)
                            //    {
                            //        response.File = data.FirstOrDefault();
                            //    }

                            //}
                            response.DocumentCount = objReader["file_count"] != null && objReader["file_count"].ToString() != string.Empty ? Convert.ToInt32(objReader["file_count"]) : 0;
                            responseList.Add(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<List<DepartmentNewsIFEGuideEO>> GetNewsAsycByType(NewsFilterEO filter)
        {

            List<DepartmentNewsIFEGuideEO> responseList = new List<DepartmentNewsIFEGuideEO>();
            DepartmentNewsIFEGuideEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    
                    if (filter.NewsType == NewsType.DepartmentNews)
                    {
                        perametersList.Add(new ODPCommandParameter("po_data", ParameterDirection.Output, OracleDbType.RefCursor));
                        using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_ipa_dashboard.get_deptnews_proc", perametersList))
                        {
                            while (objReader.Read())
                            {
                                response = new DepartmentNewsIFEGuideEO();

                                response.NewstTitle = objReader["newstitle"] != null && objReader["newstitle"].ToString() != string.Empty ? (System.String)(objReader["newstitle"]) : default(System.String);
                                response.DocFileName = objReader["docfilename"] != null && objReader["docfilename"].ToString() != string.Empty ? (System.String)(objReader["docfilename"]) : default(System.String);
                                response.FileType = objReader["filetype"] != null && objReader["filetype"].ToString() != string.Empty ? (System.String)(objReader["filetype"]) : default(System.String);
                                response.ThumbImage = objReader["thumbimage"] != null && objReader["thumbimage"].ToString() != string.Empty ? (System.Byte[])(objReader["thumbimage"]) : default(System.Byte[]);
                                response.ThumbFileName = objReader["thumbfilename"] != null && objReader["thumbfilename"].ToString() != string.Empty ? (System.String)(objReader["thumbfilename"]) : default(System.String);
                                response.ImageType = objReader["imagetype"] != null && objReader["imagetype"].ToString() != string.Empty ? (System.String)(objReader["imagetype"]) : default(System.String);
                                response.FileCode = objReader["file_code"] != null && objReader["file_code"].ToString() != string.Empty ? (System.String)(objReader["file_code"]) : default(System.String);
                                response.FileId = objReader["file_id"] != null && objReader["file_id"].ToString() != string.Empty ? Convert.ToString(objReader["file_id"]) : default(System.String);
                                //response.ValidFrom = "16 Jan 2016, 8:48 AM";
                                response.DocType = objReader["DOC_TYPE"] != null && objReader["DOC_TYPE"].ToString() != string.Empty ? (System.String)(objReader["DOC_TYPE"]) : default(System.String);

                                response.ValidFrom = objReader["SP_MODIFIEDDATE"] != null && objReader["SP_MODIFIEDDATE"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["SP_MODIFIEDDATE"])) + "," + String.Format("{0:t}", Convert.ToDateTime(objReader["SP_MODIFIEDDATE"])) : default(System.String);

                                responseList.Add(response);
                            }
                        }
                    }
                    else if (filter.NewsType == NewsType.AirlineNews)
                    {
                        perametersList.Clear();
                        perametersList.Add(new ODPCommandParameter("po_data", ParameterDirection.Output, OracleDbType.RefCursor));
                        using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_ipa_dashboard.get_deptnews_proc", perametersList))
                        {
                            while (objReader.Read())
                            {
                                response = new DepartmentNewsIFEGuideEO();

                                response.NewstTitle = objReader["newstitle"] != null && objReader["newstitle"].ToString() != string.Empty ? (System.String)(objReader["newstitle"]) : default(System.String);
                                response.DocFileName = objReader["docfilename"] != null && objReader["docfilename"].ToString() != string.Empty ? (System.String)(objReader["docfilename"]) : default(System.String);
                                response.FileType = objReader["filetype"] != null && objReader["filetype"].ToString() != string.Empty ? (System.String)(objReader["filetype"]) : default(System.String);
                                response.ThumbImage = objReader["thumbimage"] != null && objReader["thumbimage"].ToString() != string.Empty ? (System.Byte[])(objReader["thumbimage"]) : default(System.Byte[]);
                                response.ThumbFileName = objReader["thumbfilename"] != null && objReader["thumbfilename"].ToString() != string.Empty ? (System.String)(objReader["thumbfilename"]) : default(System.String);
                                response.ImageType = objReader["imagetype"] != null && objReader["imagetype"].ToString() != string.Empty ? (System.String)(objReader["imagetype"]) : default(System.String);
                                response.FileCode = objReader["file_code"] != null && objReader["file_code"].ToString() != string.Empty ? (System.String)(objReader["file_code"]) : default(System.String);
                                response.FileId = objReader["file_id"] != null && objReader["file_id"].ToString() != string.Empty ? Convert.ToString(objReader["file_id"]) : default(System.String);
                                response.DocType = objReader["DOC_TYPE"] != null && objReader["DOC_TYPE"].ToString() != string.Empty ? (System.String)(objReader["DOC_TYPE"]) : default(System.String);

                                response.ValidFrom = objReader["SP_MODIFIEDDATE"] != null && objReader["SP_MODIFIEDDATE"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["SP_MODIFIEDDATE"])) + "," + String.Format("{0:t}", Convert.ToDateTime(objReader["SP_MODIFIEDDATE"])) : default(System.String);

                                responseList.Add(response);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<List<DepartmentNewsIFEGuideEO>> GetIFEGuidesAsyc()
        {

            List<DepartmentNewsIFEGuideEO> responseList = new List<DepartmentNewsIFEGuideEO>();
            DepartmentNewsIFEGuideEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("po_data", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_ipa_dashboard.get_ifeguide_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new DepartmentNewsIFEGuideEO();

                            response.NewstTitle = objReader["newstitle"] != null && objReader["newstitle"].ToString() != string.Empty ? (System.String)(objReader["newstitle"]) : default(System.String);
                            response.DocFileName = objReader["docfilename"] != null && objReader["docfilename"].ToString() != string.Empty ? (System.String)(objReader["docfilename"]) : default(System.String);
                            response.FileType = objReader["filetype"] != null && objReader["filetype"].ToString() != string.Empty ? (System.String)(objReader["filetype"]) : default(System.String);
                            response.ThumbImage = objReader["thumbimage"] != null && objReader["thumbimage"].ToString() != string.Empty ? (System.Byte[])(objReader["thumbimage"]) : default(System.Byte[]);
                            response.ThumbFileName = objReader["thumbfilename"] != null && objReader["thumbfilename"].ToString() != string.Empty ? (System.String)(objReader["thumbfilename"]) : default(System.String);
                            response.ImageType = objReader["imagetype"] != null && objReader["imagetype"].ToString() != string.Empty ? (System.String)(objReader["imagetype"]) : default(System.String);
                            response.FileCode = objReader["file_code"] != null && objReader["file_code"].ToString() != string.Empty ? (System.String)(objReader["file_code"]) : default(System.String);
                            response.FileId = objReader["file_id"] != null && objReader["file_id"].ToString() != string.Empty ? Convert.ToString(objReader["file_id"]) : default(System.String);
                            // response.ValidFrom = "16 Jan 2016, 8:48 AM";
                            response.ValidFrom = objReader["SP_MODIFIEDDATE"] != null && objReader["SP_MODIFIEDDATE"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["SP_MODIFIEDDATE"])) + "," + String.Format("{0:t}", Convert.ToDateTime(objReader["SP_MODIFIEDDATE"])) : default(System.String);
                            response.DocType = objReader["DOC_TYPE"] != null && objReader["DOC_TYPE"].ToString() != string.Empty ? (System.String)(objReader["DOC_TYPE"]) : default(System.String);


                            responseList.Add(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<List<FileEO>> GetFilesAsyc(FileFilterEO filterInput)
        {

            List<FileEO> responseList = new List<FileEO>();
            FileEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_file_code", filterInput.FileCode, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_file_id", filterInput.FileId, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_data", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_ipa_dashboard.get_files_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new FileEO();

                            response.FileName = objReader["filename"] != null && objReader["filename"].ToString() != string.Empty ? (System.String)(objReader["filename"]) : default(System.String);
                            response.FileType = objReader["filetype"] != null && objReader["filetype"].ToString() != string.Empty ? (System.String)(objReader["filetype"]) : default(System.String);
                            response.FileContent = objReader["filecontent"] != null && objReader["filecontent"].ToString() != string.Empty ? (System.Byte[])(objReader["filecontent"]) : default(System.Byte[]);

                            if (response.FileContent !=null) {
                                responseList.Add(response);
                            }
                           
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<List<LinkEO>> GetLinksAsyc()
        {

            List<LinkEO> responseList = new List<LinkEO>();
            LinkEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("po_cur_links", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_ipa_dashboard.get_links_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new LinkEO();

                            response.LinkGroup = objReader["link_group"] != null && objReader["link_group"].ToString() != string.Empty ? (System.String)(objReader["link_group"]) : default(System.String);
                            response.LinkDisplay = objReader["linkdisplay"] != null && objReader["linkdisplay"].ToString() != string.Empty ? (System.String)(objReader["linkdisplay"]) : default(System.String);
                            response.LinkName = objReader["linkname"] != null && objReader["linkname"].ToString() != string.Empty ? (System.String)(objReader["linkname"]) : default(System.String);
                            response.LinkUrl = objReader["linkurl"] != null && objReader["linkurl"].ToString() != string.Empty ? (System.String)(objReader["linkurl"]) : default(System.String);
                            response.OpenIn = objReader["openin"] != null && objReader["openin"].ToString() != string.Empty ? (System.String)(objReader["openin"]) : default(System.String);
                            response.LinkType = objReader["linktype"] != null && objReader["linktype"].ToString() != string.Empty ? (System.String)(objReader["linktype"]) : default(System.String);
                            response.LinkImage = objReader["linkimage"] != null && objReader["linkimage"].ToString() != string.Empty ? (System.Byte[])(objReader["linkimage"]) : default(System.Byte[]);
                            response.LinkImageDesc = objReader["linkimagedesc"] != null && objReader["linkimagedesc"].ToString() != string.Empty ? (System.String)(objReader["linkimagedesc"]) : default(System.String);

                            responseList.Add(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<List<VisionMissionEO>> GetVisionMissionsAsyc()
        {

            List<VisionMissionEO> responseList = new List<VisionMissionEO>();
            VisionMissionEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("po_cur_visionmission", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_ipa_dashboard.get_visionmission_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new VisionMissionEO();

                            response.ComnDocName = objReader["comndocname"] != null && objReader["comndocname"].ToString() != string.Empty ? (System.String)(objReader["comndocname"]) : default(System.String);
                            response.FileName = objReader["filename"] != null && objReader["filename"].ToString() != string.Empty ? (System.String)(objReader["filename"]) : default(System.String);
                            response.FileType = objReader["filetype"] != null && objReader["filetype"].ToString() != string.Empty ? (System.String)(objReader["filetype"]) : default(System.String);
                            response.FileContent = objReader["filecontent"] != null && objReader["filecontent"].ToString() != string.Empty ? (System.Byte[])(objReader["filecontent"]) : null;

                            responseList.Add(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return responseList;
        }

        public async Task<List<MyRequestEO>> GetMyRequestsAsyc(MyRequestFilterEO filterInput)
        {

            List<MyRequestEO> responseList = new List<MyRequestEO>();
            MyRequestEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_staffno", filterInput.StaffID, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_myrequests", ParameterDirection.Output, OracleDbType.RefCursor));

                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_ipa_dashboard.get_myrequests_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new MyRequestEO();
                          
                            response.RequestedDate = objReader["requested_date"] != null && objReader["requested_date"].ToString() != string.Empty ? Common.DateFormateddMMMyyyyWithSpace(Convert.ToDateTime(objReader["requested_date"])) : default(System.String);
                            response.MyrequestType = objReader["myrequest_type"] != null && objReader["myrequest_type"].ToString() != string.Empty ? (System.String)(objReader["myrequest_type"]) : default(System.String);
                            response.RequestId = objReader["request_id"] != null && objReader["request_id"].ToString() != string.Empty ? (System.String)(objReader["request_id"]) : default(System.String);
                            response.RequsetNo = objReader["requset_no"] != null && objReader["requset_no"].ToString() != string.Empty ? (System.String)(objReader["requset_no"]) : default(System.String);
                            response.RequestType = objReader["request_type"] != null && objReader["request_type"].ToString() != string.Empty ? (System.String)(objReader["request_type"]) : default(System.String);
                            response.ReqestStatus = objReader["reqest_status"] != null && objReader["reqest_status"].ToString() != string.Empty ? (System.String)(objReader["reqest_status"]) : default(System.String);
                            response.Comment = objReader["comments"] != null && objReader["comments"].ToString() != string.Empty ? (System.String)(objReader["comments"]) : default(System.String);
                            response.RequestorComments = objReader["requestor_comments"] != null && objReader["requestor_comments"].ToString() != string.Empty ? (System.String)(objReader["requestor_comments"]) : default(System.String);

                            responseList.Add(response);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return responseList;
        }
    }
}
