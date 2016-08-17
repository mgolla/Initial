using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using QR.IPrism.BusinessObjects.Interfaces;
using QR.IPrism.Enterprise;
using QR.IPrism.EntityObjects.Module;
using QR.IPrism.EntityObjects.Shared;
using QR.IPrism.Utility;

namespace QR.IPrism.BusinessObjects.Data_Layer.Shared
{
    public class SharedDao : ISharedDao
    {
        public async Task<List<MessageEO>> GetMessageListAsyc()
        {
            List<MessageEO> responseList = new List<MessageEO>();
            MessageEO response = null;
            List<ODPCommandParameter> perametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("po_message", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_shared_pkg.get_prism_msg_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new MessageEO();
                            response.MessageCode = objReader["msg_code"] != null ? objReader["msg_code"].ToString() : string.Empty;
                            response.Message = objReader["message"] != null ? objReader["message"].ToString() : string.Empty;
                            response.Module = objReader["mod_desc"] != null ? objReader["mod_desc"].ToString() : string.Empty;
                            response.Type = objReader["type_desc"] != null ? objReader["type_desc"].ToString() : string.Empty;
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
        public List<MessageEO> GetMessageList()
        {
            List<MessageEO> responseList = new List<MessageEO>();
            MessageEO response = null;
            List<ODPCommandParameter> perametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("po_message", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = ODPDataAccess.ExecuteReader("prism_shared_pkg.get_prism_msg_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new MessageEO();
                            response.MessageCode = objReader["msg_code"] != null ? objReader["msg_code"].ToString() : string.Empty;
                            response.Message = objReader["message"] != null ? objReader["message"].ToString() : string.Empty;
                            response.Module = objReader["mod_desc"] != null ? objReader["mod_desc"].ToString() : string.Empty;
                            response.Type = objReader["type_desc"] != null ? objReader["type_desc"].ToString() : string.Empty;
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
        #region Menulist

        /*public async Task<List<MenuEO>> GetUserMenuListAsyc(string staffNumber)
        {
            List<MenuEO> menuList = new List<MenuEO>();



            menuList.Add(new MenuEO()
            {
                MenuId = 1000,
                MenuName = "My Space",
                ParentId = 0,
                Rank = 1,
                Url = string.Empty,
                Sref = string.Empty,
                IconClass = "mySpace"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1001,
                MenuName = "eVR",
                ParentId = 0,
                Rank = 2,
                Url = string.Empty,
                Sref = string.Empty,
                IconClass = "evr"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1002,
                MenuName = "Performance",
                ParentId = 0,
                Rank = 3,
                Url = string.Empty,
                Sref = string.Empty,
                IconClass = "performance"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1003,
                MenuName = "Requests",
                ParentId = 0,
                Rank = 4,
                Url = string.Empty,
                Sref = string.Empty,
                IconClass = "requests"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1004,
                MenuName = "IVR & Contacts",
                ParentId = 0,
                Rank = 5,
                Url = string.Empty,
                Sref = "IvrMain",
                IconClass = "exam"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1022,
                MenuName = "SVP Communication ",
                ParentId = 0,
                Rank = 6,
                Url = string.Empty,
                Sref = string.Empty,
                IsMobileLink = true,
                IconClass = "svpcommun"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1033,
                MenuName = "News ",
                ParentId = 0,
                Rank = 7,
                Url = string.Empty,
                Sref = string.Empty,
                IsMobileLink = true,
                IconClass = "newsall"
            });
          
            return GetChildMenuList(menuList);
        }

        public List<MenuEO> GetChildMenuList(List<MenuEO> menuList)
        {
            menuList.Add(new MenuEO()
            {
                MenuId = 1006,
                MenuName = "My Profile",
                ParentId = 1000,
                Rank = 1,
                Url = "/mp",
                Controller = "ipm.request.controller",
                Template = "/app/ipm/partials/ipmRequest.html",
                Sref = "cpPersonaldetails.details"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1007,
                MenuName = "My Roaster",
                ParentId = 1000,
                Rank = 2,
                Url = "/ms/mr",
                Controller = "ipm.home.controller",
                Template = "/app/ipm/partials/ipmHome.html",
                Sref = "ngimp"
            });
            
            menuList.Add(new MenuEO()
            {
                MenuId = 1008,
                MenuName = "My Transport",
                ParentId = 1000,
                Rank = 3,
                Url = "/iptr",
                Controller = "ipm.searchTransport.controller",
                Template = "/app/ipm/partials/ipmSearchTransport.html",
                Sref = "iptr"
            });

            
            menuList.Add(new MenuEO()
            {
                MenuId = 1009,
                MenuName = "Enter eVR",
                ParentId = 1001,
                Rank = 1,
                //Url = "/evr",
                //Sref = "searchDelayFlight"
                Url = "/evtlsts",
                Sref = "evrlstsState"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1010,
                MenuName = "Add/Edit Flight Details",
                ParentId = 1001,
                Rank = 2,
                Url = "/aef",
                Sref = "addEditFlight"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1011,
                MenuName = "Enter Flight Delay",
                ParentId = 1001,
                Rank = 3,
                Url = "/efd",
                Sref = "enterFlightDelay"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1012,
                MenuName = "Search eVR",
                ParentId = 1001,
                Rank = 4,
                Url = "/evrs",
                Sref = "evrSearch"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1013,
                MenuName = "Search Flight Delay",
                ParentId = 1001,
                Rank = 5,
                Url = "/sdf",
                Sref = "searchDelayFlight"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1014,
                MenuName = "eVR Guid",
                ParentId = 1001,
                Rank = 6,
                Url = "/evrGuid",
                Sref = "evrguid"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1015,
                MenuName = "Housing",
                ParentId = 1003,
                Rank = 1,
                Url = "/hs",
                Sref = "housing"
            });

            menuList.Add(new MenuEO()
            {
                MenuId = 1016,
                MenuName = "My Assessment List",
                ParentId = 1002,
                Rank = 1,
                Url = "/ma",
                Sref = "myasmnt"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1017,
                MenuName = "Unscheduled Assessments",
                ParentId = 1002,
                Rank = 2,
                Url = "/unasdtl",
                Sref = "unasmntdtl"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1018,
                MenuName = "Search Assessments",
                ParentId = 1002,
                Rank = 3,
                Url = "/sral",
                Sref = "srchasmnt"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1019,
                MenuName = "Crew Assessment Details",
                ParentId = 1002,
                Rank = 4,
                Url = "/cbsral",
                Sref = "crwonbrdsrchasmnt"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1020,
                MenuName = "PO Assessment List",
                ParentId = 1002,
                Rank = 5,
                Url = "/poasm",
                Sref = "poasmnt"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1021,
                MenuName = "My Previous Assessments",
                ParentId = 1002,
                Rank = 6,
                Url = "/pasmnt",
                Sref = "prevassessment"
            });
          
            menuList.Add(new MenuEO()
            {
                MenuId = 1023,
                MenuName = "Rosters",
                ParentId = 0,
                Rank = 1,
                Url = "/r",
                Sref = "rostermobile",
                IsMobileLink = true,
                IconClass = "roasters"
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1024,
                MenuName = "Alert & Notification",
                ParentId = 0,
                Rank = 2,
                Url = "/r",
                Sref = "alertnmobile",
                IsMobileLink = true,
                IconClass = "alertsnotif"
            });
            //menuList.Add(new MenuEO()
            //{
            //    MenuId = 1025,
            //    MenuName = "My Request",
            //    ParentId = 0,
            //    Rank = 3,
            //    Url = "/r",
            //    Sref = "myreqmobile",
            //    IsMobileLink = true
            //});
            menuList.Add(new MenuEO()
            {
                MenuId = 1026,
                MenuName = "SVP Tweets",
                ParentId = 1022,
                Rank = 4,
                Url = "/r",
                Sref = "svpmobile",
                IsMobileLink = true
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1027,
                MenuName = "Department News",
                ParentId = 1033,
                Rank = 5,
                Url = "/r",
                Sref = "depnmobile",
                IsMobileLink = true
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1028,
                MenuName = "SVP Messages",
                ParentId = 1022,
                Rank = 6,
                Url = "/r",
                Sref = "svpmmobile",
                IsMobileLink = true
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1029,
                MenuName = "Monthly IFE Guide",
                ParentId = 1033,
                Rank = 7,
                Url = "/r",
                Sref = "ifegmobile",
                IsMobileLink = true
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1030,
                MenuName = "Airline News",
                ParentId = 1033,
                Rank = 8,
                Url = "/r",
                Sref = "airlinemobile",
                IsMobileLink = true
            });
            menuList.Add(new MenuEO()
            {
                MenuId = 1031,
                MenuName = "Useful Links",
                ParentId = 0,
                Rank = 9,
                Url = "/r",
                Sref = "userflinkmobile",
                IsMobileLink = true,
                IconClass = "usefulLink"
            });
            menuList.Add(new MenuEO()
         {
             MenuId = 1032,
             MenuName = "Document Library",
             ParentId = 0,
             Rank = 10,
             Url = "/r",
             Sref = "doclibmobile",
             IsMobileLink = true,
             IconClass = "docLibraryicon"
         });
            menuList.Add(new MenuEO()
            {
                MenuId = 1032,
                MenuName = "Our Vision",
                ParentId = 0,
                Rank = 11,
                Url = "/r",
                Sref = "visionmobile",
                IsMobileLink = true,
                IconClass = "ourVison"
            });
           


            return menuList;
        }*/

        public async Task<List<MenuEO>> GetUserMenuListAsyc(string detailsId)
        {
            List<MenuEO> response = null;
            MenuEO menu = null;
            List<ODPCommandParameter> perametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    response = new List<MenuEO>();
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_crewdetailsid", detailsId, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_staff_menu", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_shared_pkg.get_user_menu_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            menu = new MenuEO();
                            menu.MenuId = objReader["menuid"] != null ? Convert.ToInt32(objReader["menuid"].ToString()) : 0;
                            menu.MenuName = objReader["menuname"] != null ? Convert.ToString(objReader["menuname"].ToString()) : string.Empty;
                            menu.ParentId = objReader["menuparentid"] != null && !string.IsNullOrEmpty(objReader["menuparentid"].ToString()) ? Convert.ToInt32(objReader["menuparentid"].ToString()) : 0;
                            menu.Rank = objReader["menurank"] != null ? Convert.ToInt32(objReader["menurank"].ToString()) : 0;
                            menu.Sref = objReader["sref"] != null ? Convert.ToString(objReader["sref"].ToString()) : string.Empty;
                            menu.IconClass = objReader["iconclass"] != null ? Convert.ToString(objReader["iconclass"].ToString()) : string.Empty;
                            menu.IsMobileLink = objReader["ismobilelink"] != null && objReader["ismobilelink"].ToString().Equals("1") ? true : false;
                            response.Add(menu);
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
            return response;
        }


        #endregion
        public async Task<UserContextEO> GetUserContextAsync(string staffNumber)
        {
            UserContextEO response = null;
            RoleEO role = new RoleEO();
            List<ODPCommandParameter> perametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_staffno", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_staff_det", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_shared_pkg.get_user_context_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new UserContextEO();
                            response.StaffNumber = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
                            response.CrewDetailsId = objReader["crewdetailsid"] != null ? objReader["crewdetailsid"].ToString() : string.Empty;
                            response.UserId = objReader["userid"] != null ? objReader["userid"].ToString() : string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return response;
        }
        public UserContextEO GetUserContext(string staffNumber)
        {
            UserContextEO response = null;
            RoleEO role = new RoleEO();
            List<ODPCommandParameter> perametersList = null;
            List<RoleEO> roleList = null;
            RoleEO roleEo = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_staffno", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_staff_det", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = ODPDataAccess.ExecuteReader("prism_shared_pkg.get_user_context_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new UserContextEO();
                            response.StaffNumber = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
                            response.CrewDetailsId = objReader["crewdetailsid"] != null ? objReader["crewdetailsid"].ToString() : string.Empty;
                            response.UserId = objReader["userid"] != null ? objReader["userid"].ToString() : string.Empty;
                            response.StaffName = objReader["staffname"] != null ? objReader["staffname"].ToString() : string.Empty;
                            response.Grade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;
                            response.DateOfBirth = objReader["dob"] != null ? objReader["dob"].ToString() : string.Empty;
                            response.JoiningDate = objReader["doj"] != null ? objReader["doj"].ToString().Split(new char[] { ' ' })[0] : string.Empty;
                            response.AdminKey = objReader["admin_key"] != null ? objReader["admin_key"].ToString() : string.Empty;
                            if (objReader["Role"] != null)
                            {
                                roleList = new List<RoleEO>();
                                roleEo = new RoleEO();
                                roleEo.Name = objReader["Role"].ToString();
                                roleList.Add(roleEo);
                                response.Role = roleList;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
                finally
                {
                    ODPDataAccess.CloseConnection();
                }
            }
            return response;
        }
        public async Task<bool> CreateAnalyticEntryAsync(AnalyticEO analytics)
        {
            bool result = false;
            ODPDataAccess sqlODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    List<ODPCommandParameter> perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_staff_number", analytics.StaffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_category", analytics.Category, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_action", analytics.Action, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_label", analytics.OptionLabel, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_value", analytics.OptionValue, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_resolution", analytics.Resolution, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_device", analytics.Device, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_browser", analytics.Browser, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_user_agent", analytics.UserAgent, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_is_tablet", analytics.IsTablet, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_ip_address", analytics.IpAddress, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_datetime", analytics.DateTime, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_isinserted", ParameterDirection.Output, OracleDbType.Varchar2, 100));
                    Oracle.DataAccess.Client.OracleCommand command = await sqlODPDataAccess.ExecuteSPNonQueryParmAsync("prism_shared_pkg.prism_analytics_inset_proc", perametersList);
                    result = Convert.ToBoolean(command.Parameters["po_isinserted"].Value.ToString());
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlODPDataAccess.CloseConnection();
                }
                return result;
            }
        }
        #region UserToken

        public ClientEO FindAPIClient(string clientId)
        {
            ClientEO response = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_client_id", clientId, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_client_details", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = ODPDataAccess.ExecuteReader("prism_shared_pkg.get_api_client_detials_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new ClientEO();
                            response.ClientId = objReader["client_id"] != null ? objReader["client_id"].ToString() : string.Empty;
                            response.Key = objReader["client_key"] != null ? objReader["client_key"].ToString() : string.Empty;
                            response.Name = objReader["client_name"] != null ? objReader["client_name"].ToString() : string.Empty;
                            response.ApplicationType = objReader["application_type"] != null ? Convert.ToInt32(objReader["application_type"]) : 0;
                            response.TokenLifeTime = objReader["token_life_time"] != null ? Convert.ToInt32(objReader["token_life_time"]) : 0;
                            response.AllowedOrigin = objReader["allowed_orgin"] != null ? objReader["allowed_orgin"].ToString() : string.Empty;
                            response.IsActive = objReader["is_active"] != null ? Convert.ToString(objReader["is_active"]) : string.Empty;
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
            return response;
        }

        public async Task<bool> AddUserTokenAsync(UserTokenEO token)
        {
            ClientEO response = null;
            bool result = false;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_user_token_id", token.UserTokenId, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_subject", token.Subject, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_client_id", token.ClientId, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_staffno", token.StaffNo, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_issued_on", Common.ToOracleDate(token.IssuedOn), ParameterDirection.Input, OracleDbType.Date));
                    perametersList.Add(new ODPCommandParameter("pi_expires_on", Common.ToOracleDate(token.ExpiresOn), ParameterDirection.Input, OracleDbType.Date));
                    perametersList.Add(new ODPCommandParameter("pi_token", token.Token, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_message", ParameterDirection.Output, OracleDbType.Varchar2, 100));
                    Oracle.DataAccess.Client.OracleCommand command = await ODPDataAccess.ExecuteSPNonQueryParmAsync("prism_shared_pkg.add_user_tokens_proc", perametersList);
                    result = Convert.ToBoolean(command.Parameters["po_message"].Value.ToString());
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
            return result;
        }

        public async Task<bool> RemoveUserTokenAsync(string userTokenId)
        {
            bool result = false;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                List<ODPCommandParameter> perametersList = null;
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_user_token_id", userTokenId, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_message", ParameterDirection.Output, OracleDbType.Varchar2, 100));
                    Oracle.DataAccess.Client.OracleCommand command = await ODPDataAccess.ExecuteSPNonQueryParmAsync("prism_shared_pkg.remove_user_tokens_proc", perametersList);
                    result = Convert.ToBoolean(command.Parameters["po_message"].Value.ToString());
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
            return result;
        }
        public async Task<UserTokenEO> FindUserTokenAsync(string userTokenId)
        {
            UserTokenEO response = null;
            List<ODPCommandParameter> perametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_user_token_id", userTokenId, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_user_token", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_shared_pkg.get_user_tokens_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            response = new UserTokenEO();
                            response.UserTokenId = objReader["user_token_id"] != null ? objReader["user_token_id"].ToString() : string.Empty;
                            response.StaffNo = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
                            response.ClientId = objReader["client_id"] != null ? objReader["client_id"].ToString() : string.Empty;
                            response.IssuedOn = objReader["issued_on"] != null ? (DateTime?)Convert.ToDateTime(objReader["issued_on"]) : null;
                            response.ExpiresOn = objReader["expires_on"] != null ? (DateTime?)Convert.ToDateTime(objReader["expires_on"]) : null;
                            response.Token = objReader["token"] != null ? objReader["token"].ToString() : string.Empty;
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
            return response;
        }

        #endregion
        #region LookUp
        public async Task<IEnumerable<LookupEO>> GetCommonInfoAsyc(string type)
        {
            List<LookupEO> response = new List<LookupEO>();
            LookupEO lookupEo = null;
            List<ODPCommandParameter> parametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<ODPCommandParameter>();
                    parametersList.Add(new ODPCommandParameter("pi_infotype", type, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("po_info_details", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_shared_pkg.get_comn_info_proc", parametersList))
                    {
                        while (objReader.Read())
                        {
                            lookupEo = new LookupEO();
                            lookupEo.Text = objReader["infovalue"] != null ? objReader["infovalue"].ToString() : string.Empty;
                            lookupEo.Value = objReader["infocode"] != null ? objReader["infocode"].ToString() : string.Empty;
                            lookupEo.Type = objReader["infotype"] != null ? objReader["infotype"].ToString() : string.Empty;
                            response.Add(lookupEo);
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
            return response;
        }
        public List<LookupEO> GetCommonInfo(string type)
        {
            List<LookupEO> response = new List<LookupEO>();
            LookupEO lookupEo = null;
            List<ODPCommandParameter> perametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("pi_infotype", type, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("po_info_details", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = ODPDataAccess.ExecuteReader("prism_shared_pkg.get_comn_info_proc", perametersList))
                    {
                        while (objReader.Read())
                        {
                            lookupEo = new LookupEO();
                            lookupEo.Text = objReader["infovalue"] != null ? objReader["infovalue"].ToString() : string.Empty;
                            lookupEo.Value = objReader["infocode"] != null ? objReader["infocode"].ToString() : string.Empty;
                            lookupEo.Type = objReader["infotype"] != null ? objReader["infotype"].ToString() : string.Empty;
                            response.Add(lookupEo);
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
            return response;
        }

        public async Task<IEnumerable<LookupEO>> GetGradeAsync()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            List<ODPCommandParameter> perametersList = new List<ODPCommandParameter>();
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                perametersList.Add(new ODPCommandParameter("po_searchresult", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("pbms_performance_search_pkg.getgrade", perametersList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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

            return responseList;
        }


        /// Gets All lookup code for Assessment status  multiselect list in Assessment search
        public async Task<IEnumerable<LookupEO>> GetAssmtStatusAsync()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            List<ODPCommandParameter> perametersList = new List<ODPCommandParameter>();
            try
            {
                perametersList.Add(new ODPCommandParameter("po_searchresult", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("pbms_performance_search_pkg.getstatus", perametersList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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

            return responseList;
        }

        public async Task<IEnumerable<LookupEO>> GetPendingAssessmentAsync()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            List<ODPCommandParameter> perametersList = null;
            try
            {
                perametersList = new List<ODPCommandParameter>();
                perametersList.Add(new ODPCommandParameter("cur_months", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_assessment.getPendingMonths", perametersList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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

            return responseList;
        }

        public async Task<IEnumerable<LookupEO>> GetGradeCSDCS()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            List<ODPCommandParameter> perametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                perametersList = new List<ODPCommandParameter>();
                perametersList.Add(new ODPCommandParameter("po_searchresult", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_assessment.getgradePending", perametersList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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

            return responseList;
        }


        public async Task<IEnumerable<LookupEO>> GetSectorAsync()
        {
            List<LookupEO> response = new List<LookupEO>();
            LookupEO row = null;
            List<CommandParameter> parametersList = new List<CommandParameter>();
            DBFramework dbframework = new DBFramework(Constants.CONNECTION_STR);

            try
            {
                parametersList.Add(new CommandParameter("pi_sector", String.Empty, ParameterDirection.Input, DbType.String));

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("PRISM_MIG_SEARCH.get_sector", parametersList, "cur_sector"))
                {
                    while (objReader.Read())
                    {
                        row = new LookupEO();
                        row.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        row.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        row.FilterText = objReader["lookupdesc"] != null ? objReader["lookupdesc"].ToString() : string.Empty;
                        response.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return response;
        }

        //public async Task<IEnumerable<LookupEO>> GetSectorFrom()
        //{
        //    List<LookupEO> responseList = new List<LookupEO>();
        //    LookupEO lookupEO = null;
        //    ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
        //    List<ODPCommandParameter> perametersList = null;
        //    try
        //    {
        //        perametersList = new List<ODPCommandParameter>();
        //        perametersList.Add(new ODPCommandParameter("cur_sector", ParameterDirection.Output, OracleDbType.RefCursor));
        //        using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_assessment.get_sector", perametersList))
        //        {
        //            while (objReader.Read())
        //            {
        //                lookupEO = new LookupEO();
        //                lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
        //                lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
        //                responseList.Add(lookupEO);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        ODPDataAccess.CloseConnection();
        //    }

        //    return responseList;
        //}

        //public async Task<IEnumerable<LookupEO>> GetSectorTo()
        //{
        //    List<LookupEO> responseList = new List<LookupEO>();
        //    LookupEO lookupEO = null;
        //    ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
        //    List<ODPCommandParameter> perametersList = null;
        //    try
        //    {
        //        perametersList = new List<ODPCommandParameter>();
        //        perametersList.Add(new ODPCommandParameter("cur_sector", ParameterDirection.Output, OracleDbType.RefCursor));
        //        using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_assessment.get_sector", perametersList))
        //        {
        //            while (objReader.Read())
        //            {
        //                lookupEO = new LookupEO();
        //                lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
        //                lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
        //                responseList.Add(lookupEO);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        ODPDataAccess.CloseConnection();
        //    }

        //    return responseList;
        //}

        /// <summary>
        /// Get all lookup code for housing search request dropdown.
        /// </summary>
        /// <param name="type">Lookup type name</param>
        /// <returns></returns>
        public async Task<IEnumerable<LookupEO>> GetHousingRequestTypeAsync()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            List<ODPCommandParameter> perametersList = null;
            try
            {
                perametersList = new List<ODPCommandParameter>();
                perametersList.Add(new ODPCommandParameter("prc_requesttype", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("housing_service_request_pkg.get_hm_request_type", perametersList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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

            return responseList;
        }

        /// <summary>
        /// Gets all lookup code for housing search Status dropdown
        /// </summary>
        /// <returns>List of Status</returns>
        public async Task<IEnumerable<LookupEO>> GetHousingStatusAsync()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            List<ODPCommandParameter> perametersList = null;
            try
            {
                perametersList = new List<ODPCommandParameter>();
                perametersList.Add(new ODPCommandParameter("prc_requeststatus", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("housing_service_request_pkg.get_hs_request_status", perametersList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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

            return responseList;
        }

        /// <summary>
        /// Gets all lookup code for staff request Status drop down
        public async Task<IEnumerable<LookupEO>> GetHousingRequestTypeByStaffAsync(string staffNo)
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            List<ODPCommandParameter> parametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parametersList = new List<ODPCommandParameter>();
                parametersList.Add(new ODPCommandParameter("pi_crew_id", staffNo, ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("prc_requesttype", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("PRISM_MIG_HOUSING.get_hm_request_type", parametersList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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
            return responseList;
        }

        /// <summary>
        /// Gets all lookup code for housing search Status dropdown
        /// </summary>
        /// <returns>List of Status</returns>
        public async Task<IEnumerable<LookupEO>> GetStayOutRequestType()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            List<ODPCommandParameter> perametersList = null;
            try
            {
                perametersList = new List<ODPCommandParameter>();
                perametersList.Add(new ODPCommandParameter("prc_sorequesttype", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("service_request_pkg.get_stayoutrequesttypes", perametersList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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

            return responseList;
        }

        /// <summary>
        /// Gets all lookup code for swap room type
        /// </summary>
        /// <returns>List of Status</returns>
        public async Task<IEnumerable<LookupEO>> GetSwapRoomsRequestType()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            List<ODPCommandParameter> perametersList = null;
            try
            {
                perametersList = new List<ODPCommandParameter>();
                perametersList.Add(new ODPCommandParameter("prc_srrequesttype", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("service_request_pkg.get_swaproomrequesttypes", perametersList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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

            return responseList;
        }

        public async Task<IEnumerable<LookupEO>> GetAllLookUpDetails(string lookUpName)
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            List<ODPCommandParameter> parameterList;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("piv_lookuptypename", lookUpName, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pio_ref_cur_masterdata", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("ivrs_master_data_pkg.GetAllLookUpDetails", parameterList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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
            return responseList;
        }

        public async Task<IEnumerable<LookupEO>> GetAllLookUpWithParentDetails(string lookUpName, string parentlookuptypename, string parentlookupname)
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            List<ODPCommandParameter> parameterList;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("piv_lookuptypename", lookUpName, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("piv_parentlookuptypename", parentlookuptypename, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("piv_parentlookupname", parentlookupname, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pio_ref_cur_masterdata", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("ivrs_master_data_pkg.GetAllLookUpWithParentDetails", parameterList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        lookupEO.ParentLookUpId = objReader["parentlookupid"] != null ? objReader["parentlookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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
            return responseList;
        }

        #endregion

        //public void InsertAttachment(FileEO docs)
        //{
        //    ODPDataAccess sqlODPDataAccess = new ODPDataAccess(Constants.ORACLE_CONNECTION_STRING_ODP);
        //    {
        //        try
        //        {

        //            List<ODPCommandParameter> perametersList = new List<ODPCommandParameter>();
        //            perametersList.Add(new ODPCommandParameter("entity_type", docs.ClassType, ParameterDirection.Input, OracleDbType.Varchar2));
        //            perametersList.Add(new ODPCommandParameter("entitykey_id", docs.ClassKeyId, ParameterDirection.Input, OracleDbType.Varchar2));
        //            perametersList.Add(new ODPCommandParameter("attachment_name", docs.FileName, ParameterDirection.Input, OracleDbType.Varchar2));
        //            perametersList.Add(new ODPCommandParameter("attachment_content", docs.FileContent, ParameterDirection.Input, DbType.Object));
        //            perametersList.Add(new ODPCommandParameter("created_by", docs.CreatedBy, ParameterDirection.Input, OracleDbType.Varchar2));
        //            sqlODPDataAccess.ExecuteSPNonQueryParmTest("ALERT_CREW_PKG.insert_attachment", perametersList);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //        finally
        //        {
        //            sqlODPDataAccess.CloseConnection();
        //        }
        //    }
        //}

        public void InsertAttachment(FileEO docs)
        {
            ODPDataAccess sqlODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    List<ODPCommandParameter> perametersList = new List<ODPCommandParameter>();
                    perametersList.Add(new ODPCommandParameter("entity_type", docs.ClassType, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("entitykey_id", docs.ClassKeyId, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("attachment_name", docs.FileName, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("attachment_content", docs.FileContent, ParameterDirection.Input, OracleDbType.Blob));
                    perametersList.Add(new ODPCommandParameter("created_by", docs.CreatedBy, ParameterDirection.Input, OracleDbType.Varchar2));
                    sqlODPDataAccess.ExecuteSPNonQuery("ALERT_CREW_PKG.insert_attachment", perametersList);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlODPDataAccess.CloseConnection();
                }
            }
        }

        public void InsertEVRAttachment(FileEO docs, string userId)
        {
            string po_flag = string.Empty;
            ODPDataAccess sqlODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    List<ODPCommandParameter> perametersList = new List<ODPCommandParameter>();

                    perametersList.Add(new ODPCommandParameter("pi_doc_name", docs.FileNamePrefix + "_" + docs.FileName, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_doc_path", "", ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_vr_id", docs.ClassKeyId, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_user_id", userId, ParameterDirection.Input, OracleDbType.Varchar2));

                    perametersList.Add(new ODPCommandParameter("po_flag", po_flag, ParameterDirection.Output, OracleDbType.Varchar2, 100));

                    sqlODPDataAccess.ExecuteSPNonQuery("ivrs_enter_vr_pkg.InsertVRDocument", perametersList);

                    InsertEVRBlobAttachment(docs, userId);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlODPDataAccess.CloseConnection();
                }
            }
        }

        public void InsertEVRBlobAttachment(FileEO docs, string userId)
        {
            ODPDataAccess sqlODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    List<ODPCommandParameter> perametersList = new List<ODPCommandParameter>();


                    perametersList.Add(new ODPCommandParameter("pi_contenttype", docs.ClassType, ParameterDirection.Input, OracleDbType.Varchar2));
                    //perametersList.Add(new ODPCommandParameter("pi_contentid", docs.ClassKeyId, ParameterDirection.Input, OracleDbType.Varchar2));
                    //Existing procedure need VRNo to reflect back the Attachments and not its Id.
                    perametersList.Add(new ODPCommandParameter("pi_contentid", docs.FileNamePrefix, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_attachment", docs.FileContent, ParameterDirection.Input, OracleDbType.Blob));
                    perametersList.Add(new ODPCommandParameter("pi_attachmentname", docs.FileName, ParameterDirection.Input, OracleDbType.Varchar2));
                    perametersList.Add(new ODPCommandParameter("pi_createdby", userId, ParameterDirection.Input, OracleDbType.Varchar2));

                    sqlODPDataAccess.ExecuteSPNonQuery("comn_global_pkg.addattachment", perametersList);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    sqlODPDataAccess.CloseConnection();
                }
            }
        }


        public async void EVR_SetDeletedDocInActive(List<String> docs)
        {
            List<ODPCommandParameter> parametersList;
            foreach (var doc in docs)
            {
                ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
                {
                    try
                    {
                        parametersList = new List<ODPCommandParameter>();
                        parametersList.Add(new ODPCommandParameter("PI_AttachmentID", doc, ParameterDirection.Input, OracleDbType.Varchar2));
                        await ODPDataAccess.ExecuteSPNonQueryParmAsync("prism_mig_evr.setEVRDeletedDocInActive", parametersList);
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
            }
        }


        public async void DeleteAttachments_comn(List<String> docs)
        {
            List<ODPCommandParameter> parametersList;
            foreach (var doc in docs)
            {
                ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
                {
                    try
                    {
                        parametersList = new List<ODPCommandParameter>();
                        parametersList.Add(new ODPCommandParameter("PI_AttachmentID", doc, ParameterDirection.Input, OracleDbType.Varchar2));
                        await ODPDataAccess.ExecuteSPNonQueryParmAsync("COMN_GLOBAL_PKG.DeleteAttachment", parametersList);
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
            }
        }

        public async void DeleteAttachments_entity(List<String> docs)
        {
            List<ODPCommandParameter> parameterList;
            foreach (var doc in docs)
            {
                ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
                try
                {
                    parameterList = new List<ODPCommandParameter>();
                    parameterList.Add(new ODPCommandParameter("AttachmentId", doc, ParameterDirection.Input, OracleDbType.Varchar2));
                    await ODPDataAccess.ExecuteSPNonQueryParmAsync("ALERT_CREW_PKG.delete_attachment", parameterList);
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

        }


        public async Task<IEnumerable<FileEO>> GetAttachments(string requestId)
        {
            List<FileEO> response = new List<FileEO>();
            FileEO file = null;
            List<ODPCommandParameter> parametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<ODPCommandParameter>();
                    parametersList.Add(new ODPCommandParameter("EntityKeyId", requestId, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("cur_attachments", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("ALERT_CREW_PKG.get_alert_attachments", parametersList))
                    {
                        while (objReader.Read())
                        {
                            file = new FileEO();
                            file.FileId = objReader["attachmentid"] != null ? objReader["attachmentid"].ToString() : string.Empty;
                            file.ClassKeyId = objReader["alertid"] != null ? objReader["alertid"].ToString() : string.Empty;
                            file.FileName = objReader["attachmentname"] != null ? objReader["attachmentname"].ToString() : string.Empty;
                            file.EntityTypeId = objReader["entity_type"] != null ? objReader["entity_type"].ToString() : string.Empty;
                            file.FileContent = objReader["attachmentcontent"] as byte[];
                            response.Add(file);
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
            return response;
        }

        /// <summary>
        /// Get the Look Up details given the lookuptype name for comn_config_look_up.
        /// </summary>
        /// <param name="lookUpTypeName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LookupEO>> GetConfigLookUpDetails(string lookUpTypeName)
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            List<ODPCommandParameter> parametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parametersList = new List<ODPCommandParameter>();
                parametersList.Add(new ODPCommandParameter("pi_lookuptypename", lookUpTypeName, ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("po_cur_lookupdetails", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("comn_global_pkg.getconfiglookupdetails", parametersList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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

            return responseList;

        }

        /// <summary>
        /// Gets all lookup code for housing search Status dropdown
        /// </summary>
        /// <returns>List of Status</returns>
        public async Task<IEnumerable<LookupEO>> GetReasonForRecNonAsmtAsync()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            List<ODPCommandParameter> perametersList = null;
            try
            {
                perametersList = new List<ODPCommandParameter>();
                perametersList.Add(new ODPCommandParameter("po_reasonfornonsubmission", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("PBMS_RECORDASSESSMENT_PKG.get_reasonfornonsubmission", perametersList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupid"] != null ? objReader["lookupid"].ToString() : string.Empty;
                        responseList.Add(lookupEO);
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

            return responseList;
        }

        #region Notification Details
        public async Task<bool> InsertCrewNotificationDetails(NotificationDetailsEO input, string staffNo)
        {
            int response = 0;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();

                parameterList.Add(new ODPCommandParameter("pi_notification_desc", input.Description, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_notification_type", input.Type, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_notification_date", Common.ToOracleDate(input.Date), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_actionby_date", Common.ToOracleDate(input.ActionByDate), ParameterDirection.Input, OracleDbType.Date));
                parameterList.Add(new ODPCommandParameter("pi_severity", input.Severity, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_notification_to_crewid", input.ToCrewId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_notification_from_crewid", staffNo, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_notification_status", input.Status, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_request_id", input.RequestId ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_request_guid", input.RequestGuid, ParameterDirection.Input, OracleDbType.Varchar2));

                await ODPDataAccess.ExecuteSPNonQueryParmAsync("hm_notification_pkg.insert_notification_details", parameterList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            return true;
        }

        public async Task<bool> UpdateCrewNotificationDetails(NotificationDetailsEO input)
        {
            var rowUpdated = false;
            List<ODPCommandParameter> parameterList = new List<ODPCommandParameter>(); ;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList.Add(new ODPCommandParameter("pi_notification_id", input.Id, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("pi_notification_status", input.Status, ParameterDirection.Input, OracleDbType.Varchar2));
                await ODPDataAccess.ExecuteSPNonQueryParmAsync("hm_notification_pkg.update_notification_status", parameterList);
                rowUpdated = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ODPDataAccess.CloseConnection();
            }

            return rowUpdated;
        }

        public async Task<IEnumerable<NotificationDetailsEO>> SearchNotifications(NotificationDetailsEO input)
        {
            List<NotificationDetailsEO> response = new List<NotificationDetailsEO>();
            NotificationDetailsEO row = null;
            List<ODPCommandParameter> parametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<ODPCommandParameter>();
                    parametersList.Add(new ODPCommandParameter("pi_notification_id", input.Id, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("pi_notification_to_crewid", input.ToCrewId, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("pi_notification_type", input.Type ?? string.Empty, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("pi_notification_date", Common.ToOracleDate(input.Date), ParameterDirection.Input, OracleDbType.Date));
                    parametersList.Add(new ODPCommandParameter("pi_actionby_date", Common.ToOracleDate(input.ActionByDate), ParameterDirection.Input, OracleDbType.Date));
                    parametersList.Add(new ODPCommandParameter("prc_notification", ParameterDirection.Output, OracleDbType.RefCursor));
                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("hm_notification_pkg.get_crew_notification_data", parametersList))
                    {
                        while (objReader.Read())
                        {
                            row = new NotificationDetailsEO();
                            row.Id = objReader["notificationid"] != null ? objReader["notificationid"].ToString() : string.Empty;
                            row.Description = objReader["notificationdesc"] != null ? objReader["notificationdesc"].ToString() : string.Empty;
                            row.Type = objReader["notificationtype"] != null ? objReader["notificationtype"].ToString() : string.Empty;
                            row.Date = objReader["notificationdate"] != null && objReader["notificationdate"].ToString() != string.Empty ?
                               Convert.ToDateTime(objReader["notificationdate"].ToString()) : (DateTime?)null;

                            row.ActionByDate = objReader["actionbydate"] != null && objReader["actionbydate"].ToString() != string.Empty ?
                               Convert.ToDateTime(objReader["actionbydate"].ToString()) : (DateTime?)null;
                            row.Severity = objReader["severity"] != null ? objReader["severity"].ToString() : string.Empty;
                            row.ToCrewId = objReader["notificationtocrewid"] != null ? objReader["notificationtocrewid"].ToString() : string.Empty;
                            row.ToCrewId = objReader["notificationtocrewid"] != null ? objReader["notificationtocrewid"].ToString() : string.Empty;
                            row.FromCrewId = objReader["notificationfromcrewid"] != null ? objReader["notificationfromcrewid"].ToString() : string.Empty;
                            row.Status = objReader["notificationstatus"] != null ? objReader["notificationstatus"].ToString() : string.Empty;
                            row.RequestId = objReader["requestid"] != null ? objReader["requestid"].ToString() : string.Empty;
                            row.RequestGuid = objReader["requestguid"] != null ? objReader["requestguid"].ToString() : string.Empty;

                            response.Add(row);
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
            return response;
        }

        public async Task<int> GetNotificationsCount(string staffID)
        {

            int count = 0;
            List<ODPCommandParameter> parametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<ODPCommandParameter>();
                    parametersList.Add(new ODPCommandParameter("pi_staffno", staffID, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("po_count", ParameterDirection.Output, OracleDbType.Int32, 100));



                    Oracle.DataAccess.Client.OracleCommand command = await ODPDataAccess.ExecuteSPNonQueryParmAsync("prism_ipa_dashboard.get_top_alert_notifi_cnt_proc", parametersList);
                    count = command.Parameters["po_count"] != null ? Convert.ToInt32(command.Parameters["po_count"].Value.ToString()) : 0;

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
            return count;
        }

        #endregion

        public async void InsertComment(MessageEO message)
        {
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    List<ODPCommandParameter> parameterList = new List<ODPCommandParameter>();

                    parameterList.Add(new ODPCommandParameter("alert_type", message.Type, ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("alert_id", message.Id, ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("comment_description", message.Message, ParameterDirection.Input, OracleDbType.Varchar2));
                    parameterList.Add(new ODPCommandParameter("created_by", message.CreatedBy, ParameterDirection.Input, OracleDbType.Varchar2));
                    await ODPDataAccess.ExecuteSPNonQueryParmAsync("ALERT_CREW_PKG.insert_comment", parameterList);
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
        }

        public async Task<IEnumerable<MessageEO>> GetAllComments(string requestId)
        {
            List<MessageEO> response = new List<MessageEO>();
            MessageEO row = null;
            List<ODPCommandParameter> parameterList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<ODPCommandParameter>();
                parameterList.Add(new ODPCommandParameter("CrewAlertId", requestId, ParameterDirection.Input, OracleDbType.Varchar2));
                parameterList.Add(new ODPCommandParameter("cur_comments", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("ALERT_CREW_PKG.get_alert_comments", parameterList))
                {
                    while (objReader.Read())
                    {
                        row = new MessageEO();
                        row.Id = objReader["commentid"] != null ? objReader["commentid"].ToString() : string.Empty;
                        row.Message = objReader["commentdescription"] != null ? objReader["commentdescription"].ToString() : string.Empty;
                        row.CreatedDate = objReader["commentdate"] != null ? objReader["commentdate"].ToString() : string.Empty;
                        row.CreatedBy = objReader["crewname"] != null ? objReader["crewname"].ToString() : string.Empty;
                        response.Add(row);
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

            return response;
        }

        public async Task<UserContextEO> GetCrewPersonalDetailsAsync(string staffNumber, string imagePath, string imageType)
        {
            UserContextEO response = null;
            List<ODPCommandParameter> parametersList = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<ODPCommandParameter>();

                    parametersList.Add(new ODPCommandParameter("pi_staffno", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("pi_key", Constants.DECREPT_KEY, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("po_personaldet", ParameterDirection.Output, OracleDbType.RefCursor));
                    parametersList.Add(new ODPCommandParameter("po_accomm", ParameterDirection.Output, OracleDbType.RefCursor));
                    parametersList.Add(new ODPCommandParameter("po_own_accomm", ParameterDirection.Output, OracleDbType.RefCursor));

                    response = new UserContextEO();

                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_crewprofile.get_crew_personaldet", parametersList))
                    {
                        while (objReader.Read())
                        {

                            response.StaffNumber = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
                            response.StaffName = objReader["staffname"] != null ? objReader["staffname"].ToString() : string.Empty;
                            response.Gender = objReader["gender"] != null ? objReader["gender"].ToString() : string.Empty;
                            response.Grade = objReader["grade"] != null ? objReader["grade"].ToString() : string.Empty;
                            response.Nationality = objReader["nationality"] != null ? objReader["nationality"].ToString() : string.Empty;
                            response.JoiningDate = objReader["doj"] != null ? objReader["doj"].ToString() : string.Empty;
                            response.DateOfBirth = objReader["dob"] != null ? objReader["dob"].ToString() : string.Empty;
                            response.NextToKIN = objReader["emergency_contact"] != null ? objReader["emergency_contact"].ToString() : string.Empty;
                            response.RPNumber = objReader["rp_number"] != null ? objReader["rp_number"].ToString() : string.Empty;
                            response.RPExpiryDate = objReader["rpexpirydate"] != null && objReader["rpexpirydate"].ToString() != string.Empty ? (DateTime?)Convert.ToDateTime(objReader["rpexpirydate"]) : null;
                            response.ReportingTo = objReader["reporting_to"] != null ? objReader["reporting_to"].ToString() : string.Empty;
                            response.Email = objReader["emailid"] != null ? objReader["emailid"].ToString() : string.Empty;
                            response.Contact = objReader["crew_contact_no"] != null ? objReader["crew_contact_no"].ToString() : string.Empty;
                            //response.CrewPhoto = objReader["photo_blob"] as byte[];
                            response.CrewPhotoUrl = imagePath + "/" + staffNumber + imageType;

                            response.PermanentAddress = objReader["permanent_addr"] != null ? objReader["permanent_addr"].ToString() : string.Empty;

                            response.FlightFlownInCurrentGrade = objReader["flights_flown"] != null && objReader["flights_flown"].ToString() != string.Empty ? Convert.ToInt32(objReader["flights_flown"]) : 0;

                            response.ExpInCurrentGradeInMonths = objReader["grade_exp"] != null && objReader["grade_exp"].ToString() != string.Empty ? (objReader["grade_exp"]).ToString() : string.Empty;
                            response.ExpInMonths = objReader["qr_exp"] != null && objReader["qr_exp"].ToString() != string.Empty ? (objReader["qr_exp"]).ToString() : string.Empty;
                        }

                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            response.FlatNumber = objReader["flatnumber"] != null && objReader["flatnumber"].ToString() != string.Empty ? "#" + objReader["flatnumber"].ToString() + "," : string.Empty;
                            response.BuildingNumber = objReader["bldgname"] != null && objReader["bldgname"].ToString() != string.Empty ? objReader["bldgname"].ToString() + "," : string.Empty;
                            response.StreetNumber = objReader["streetno"] != null && objReader["streetno"].ToString() != string.Empty ? objReader["streetno"].ToString() + "," : string.Empty;
                            response.BuildingArea = objReader["bldgarea"] != null && objReader["bldgarea"].ToString() != string.Empty ? objReader["bldgarea"].ToString() : string.Empty;

                            response.CurrentAccomodation = response.FlatNumber + response.BuildingNumber + response.StreetNumber + response.BuildingArea;
                        }

                        objReader.NextResult();
                        while (objReader.Read())
                        {
                            response.FlatNumber = string.Empty; // There is no column depicting FlatNumber
                            response.BuildingNumber = objReader["hsr_buildingname"] != null && objReader["hsr_buildingname"].ToString() != string.Empty ? objReader["hsr_buildingname"].ToString() + "," : string.Empty;
                            response.StreetNumber = objReader["hsr_ad_streetno"] != null && objReader["hsr_ad_streetno"].ToString() != string.Empty ? objReader["hsr_ad_streetno"].ToString() + "," : string.Empty;
                            response.BuildingArea = objReader["hsr_ad_area"] != null && objReader["hsr_ad_area"].ToString() != string.Empty ? objReader["hsr_ad_area"].ToString() + "," : string.Empty;

                            response.CurrentAccomodation = response.FlatNumber + response.BuildingNumber + response.StreetNumber + response.BuildingArea;
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

            return response;
        }

        #region EVR Related

        /// <summary>
        /// Get all eVR Status
        /// </summary>
        /// <returns></returns>

        public async Task<IEnumerable<LookupEO>> GetAllVRStatusAsync()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            //List<LookupEO> responseEOList = new List<LookupEO>();
            LookupEO lookupEO = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            List<ODPCommandParameter> perametersList = null;
            try
            {
                perametersList = new List<ODPCommandParameter>();
                perametersList.Add(new ODPCommandParameter("po_masterstatus", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_evr.get_vrmasterstatus_proc", perametersList))
                {
                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["statusname"] != null ? objReader["statusname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["statusid"] != null ? objReader["statusid"].ToString() : string.Empty;

                        responseList.Add(lookupEO);
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

            return responseList;
        }

        /// <summary>
        /// Get all Report About
        /// </summary>
        /// <returns></returns>

        //public async Task<IEnumerable<LookupEO>> GetAllVRSectorsAsync()
        //{
        //    List<LookupEO> responseList = new List<LookupEO>();
        //    List<ODPCommandParameter> parametersList = null;
        //    LookupEO lookupEO = null;

        //    ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
        //    try
        //    {
        //        parametersList = new List<ODPCommandParameter>();
        //        parametersList.Add(new ODPCommandParameter("pi_sector", "", ParameterDirection.Input, OracleDbType.Varchar2));
        //        parametersList.Add(new ODPCommandParameter("cur_sector", ParameterDirection.Output, OracleDbType.RefCursor));
        //        using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("COMN_AUTOSEARCH_PKG.get_sector", parametersList))
        //        {


        //            while (objReader.Read())
        //            {
        //                lookupEO = new LookupEO();
        //                lookupEO.Text = objReader["lookupname"] != null && objReader["lookupdesc"] != null ? objReader["lookupname"].ToString() : string.Empty;
        //                lookupEO.Value = objReader["lookupdesc"] != null ? objReader["lookupdesc"].ToString() : string.Empty;

        //                responseList.Add(lookupEO);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        ODPDataAccess.CloseConnection();
        //    }

        //    return responseList;
        //}

        public async Task<IEnumerable<LookupEO>> GetAllCoutriesAsync()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            List<ODPCommandParameter> parametersList = null;
            LookupEO lookupEO = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parametersList = new List<ODPCommandParameter>();
                parametersList.Add(new ODPCommandParameter("pi_country", "", ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("po_country_list", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_search.get_country_list_proc", parametersList))
                {

                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["COUNTRYNAME"] != null && objReader["COUNTRYNAME"] != null ? objReader["COUNTRYNAME"].ToString() : string.Empty;
                        lookupEO.Value = objReader["COUNTRYNAME"] != null ? objReader["COUNTRYNAME"].ToString() : string.Empty;

                        responseList.Add(lookupEO);
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

            return responseList;
        }

        public async Task<IEnumerable<LookupEO>> GetAllAirportCodesAsync()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            List<ODPCommandParameter> parametersList = null;
            LookupEO lookupEO = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parametersList = new List<ODPCommandParameter>();
                parametersList.Add(new ODPCommandParameter("pi_sector", "", ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("cur_sector", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("COMN_AUTOSEARCH_PKG.get_sector", parametersList))
                {


                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Text = objReader["lookupname"] != null && objReader["lookupname"] != null ? objReader["lookupname"].ToString() : string.Empty;
                        lookupEO.Value = objReader["lookupdesc"] != null ? objReader["lookupdesc"].ToString() : string.Empty;

                        responseList.Add(lookupEO);
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

            return responseList;
        }

        public async Task<IEnumerable<LookupEO>> GetAllCurrencyCodesAsync()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            List<ODPCommandParameter> parametersList = null;
            LookupEO lookupEO = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parametersList = new List<ODPCommandParameter>();
                parametersList.Add(new ODPCommandParameter("pi_curr_code", "", ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("pi_airport_code", "", ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("pi_city", "", ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("pi_country", "", ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("po_currency_list", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_search.get_currency_list_proc", parametersList))
                {

                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();

                        lookupEO.Text = objReader["COUNTRY"] != null && objReader["COUNTRY"] != null ? objReader["COUNTRY"].ToString() : string.Empty;

                        lookupEO.Value = objReader["CURRENCY"] != null && objReader["CURRENCY"] != null ? objReader["CURRENCY"].ToString() : string.Empty;
                        lookupEO.Text = lookupEO.Value + " - " + lookupEO.Text;

                        responseList.Add(lookupEO);
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

            return responseList;
        }

        public async Task<IEnumerable<LookupEO>> GetAllCitiesAsync()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            List<ODPCommandParameter> parametersList = null;
            LookupEO lookupEO = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parametersList = new List<ODPCommandParameter>();
                parametersList.Add(new ODPCommandParameter("pi_city", "", ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("pi_country", "", ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("po_cities_list", ParameterDirection.Output, OracleDbType.RefCursor));

                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync(" prism_mig_search.get_cities_list_proc", parametersList))
                {

                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();

                        lookupEO.Text = objReader["COUNTRY"] != null && objReader["COUNTRY"] != null ? objReader["COUNTRY"].ToString() : string.Empty;
                        lookupEO.Value = objReader["CITY"] != null && objReader["CITY"] != null ? objReader["CITY"].ToString() : string.Empty;

                        lookupEO.Text = lookupEO.Value + " - " + lookupEO.Text;
                        responseList.Add(lookupEO);
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

            return responseList;
        }

        /// <summary>
        /// Get all Report About List
        /// </summary>
        /// <returns></returns>

        public async Task<IEnumerable<LookupEO>> GetAllReportAboutAsync()
        {
            List<LookupEO> responseList = new List<LookupEO>();
            LookupEO lookupEO = null;
            List<ODPCommandParameter> perametersList = null;
            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                perametersList = new List<ODPCommandParameter>();
                perametersList.Add(new ODPCommandParameter("pio_ref_cur_masterdata", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("ivrs_master_data_pkg.getallreportaboutsbydept", perametersList))
                {

                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Value = objReader["reportaboutid"] != null ? objReader["reportaboutid"].ToString() : string.Empty;
                        lookupEO.Type = objReader["departmentname"] != null ? objReader["departmentname"].ToString() : string.Empty;
                        lookupEO.Text = objReader["reportaboutname"] != null ? objReader["reportaboutname"].ToString() : string.Empty;
                        //lookupEO.Type = objReader["departmentname"] != null ? objReader["departmentname"].ToString() : string.Empty;

                        responseList.Add(lookupEO);
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

            return responseList;
        }

        /// <summary>
        /// Get all Category for Report About List
        /// </summary>
        /// <returns></returns>

        public async Task<IEnumerable<LookupEO>> GetAllCategoryForReportAboutAsync(string reportAbtId)
        {
            List<LookupEO> responseList = new List<LookupEO>();
            List<ODPCommandParameter> parametersList = null;
            LookupEO lookupEO = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parametersList = new List<ODPCommandParameter>();

                parametersList.Add(new ODPCommandParameter("pi_report_about_id", reportAbtId, ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("pio_ref_cur_masterdata", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("ivrs_master_data_pkg.getcategoryforreportabout", parametersList))
                {

                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Value = objReader["categoryid"] != null ? objReader["categoryid"].ToString() : string.Empty;
                        lookupEO.Text = objReader["categoryname"] != null ? objReader["categoryname"].ToString() : string.Empty;

                        responseList.Add(lookupEO);
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

            return responseList;
        }

        /// <summary>
        /// Get all Sub Category for a selected Category.
        /// </summary>
        /// <returns></returns>

        public async Task<IEnumerable<LookupEO>> GetAllCategoryForSubCategoryAsync(string categoryId)
        {
            List<LookupEO> responseList = new List<LookupEO>();
            List<ODPCommandParameter> parametersList = null;
            LookupEO lookupEO = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parametersList = new List<ODPCommandParameter>();
                parametersList.Add(new ODPCommandParameter("pi_category_id", categoryId, ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("pio_ref_cur_masterdata", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("ivrs_master_data_pkg.getsubcategoryforcategory", parametersList))
                {

                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Value = objReader["subcategoryid"] != null ? objReader["subcategoryid"].ToString() : string.Empty;
                        lookupEO.Text = objReader["subcategoryname"] != null ? objReader["subcategoryname"].ToString() : string.Empty;

                        responseList.Add(lookupEO);
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

            return responseList;
        }

        public async Task<IEnumerable<LookupEO>> GetEVROwnerNonOwnerAsyc(string[] filterIds)
        {
            List<LookupEO> responseList = new List<LookupEO>();
            List<ODPCommandParameter> parametersList = null;
            LookupEO lookupEO = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parametersList = new List<ODPCommandParameter>();

                parametersList.Add(new ODPCommandParameter("pi_report_about_id", filterIds[0], ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("pi_category_id", filterIds[1], ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("pi_sub_category_id", filterIds[2], ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("po_ref_cur_dept", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("ivrs_enter_vr_pkg.GetDeptForSubCategory", parametersList))
                {

                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();

                        lookupEO.Value = objReader["owner"] != null ? objReader["owner"].ToString() : string.Empty;
                        lookupEO.Text = objReader["nonowner"] != null ? objReader["nonowner"].ToString() : string.Empty;

                        responseList.Add(lookupEO);
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

            return responseList;
        }


        /// <summary>
        /// Get all Departments for EVR
        /// </summary>
        /// <returns></returns>

        public async Task<IEnumerable<LookupEO>> GetAllDepartForEVRAsync(string[] filterIds)
        {
            List<LookupEO> responseList = new List<LookupEO>();
            List<ODPCommandParameter> parametersList = null;
            LookupEO lookupEO = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            try
            {
                parametersList = new List<ODPCommandParameter>();

                parametersList.Add(new ODPCommandParameter("pi_report_about_id", filterIds[0], ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("pi_category_id", filterIds[1], ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("pi_sub_category_id", filterIds[2], ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("po_ref_cur_dept", ParameterDirection.Output, OracleDbType.RefCursor));
                using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("ivrs_enter_vr_pkg.getotherdepartments", parametersList))
                {

                    while (objReader.Read())
                    {
                        lookupEO = new LookupEO();
                        lookupEO.Value = objReader["deptid"] != null ? objReader["deptid"].ToString() : string.Empty;
                        lookupEO.Text = objReader["deptname"] != null ? objReader["deptname"].ToString() : string.Empty;

                        responseList.Add(lookupEO);
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

            return responseList;
        }

        #endregion

        public async Task<UserContextEO> GetCrewPersonalDetailsHeaderAsync(string staffNumber, string imagePath,String imagetype)
        {
            UserContextEO response = null;
            List<ODPCommandParameter> parametersList = null;

            ODPDataAccess ODPDataAccess = new ODPDataAccess(Constants.CONNECTION_STR);
            {
                try
                {
                    parametersList = new List<ODPCommandParameter>();

                    parametersList.Add(new ODPCommandParameter("pi_staffno", staffNumber, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("pi_key", Constants.DECREPT_KEY, ParameterDirection.Input, OracleDbType.Varchar2));
                    parametersList.Add(new ODPCommandParameter("po_photodet", ParameterDirection.Output, OracleDbType.RefCursor));

                    response = new UserContextEO();

                    using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("prism_mig_crewprofile.get_crew_photodet_proc", parametersList))
                    {
                        while (objReader.Read())
                        {

                            response.StaffNumber = objReader["staffno"] != null ? objReader["staffno"].ToString() : string.Empty;
                            response.StaffName = objReader["STAFFNAME"] != null ? objReader["STAFFNAME"].ToString() : string.Empty;
                            response.CrewDetailsId = objReader["CREWDETAILSID"] != null ? objReader["CREWDETAILSID"].ToString() : string.Empty;
                            //response.CrewPhoto = objReader["PHOTO_BLOB"] as byte[];
                            response.CrewPhotoUrl = imagePath + "/" + staffNumber +imagetype;
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

            return response;
        }

        public async Task<IEnumerable<KeyContactsEO>> GetKeyContactsAsync()
        {
            List<KeyContactsEO> response = new List<KeyContactsEO>();
            KeyContactsEO row = null;
            List<CommandParameter> parameterList = null;
            DBFramework dbframework = new DBFramework(Constants.CONNECTION_STR);

            try
            {
                parameterList = new List<CommandParameter>();

                using (IDataReader objReader = await dbframework.ExecuteSPReaderAsync("prism_ipa_dashboard.get_keycountdetails_proc", parameterList, "po_keycontacts"))
                {
                    while (objReader.Read())
                    {
                        row = new KeyContactsEO();
                        row.CONTACT_ID = objReader["CONTACT_ID"] != null ? objReader["CONTACT_ID"].ToString() : string.Empty;
                        row.TITLE = objReader["TITLE"] != null ? objReader["TITLE"].ToString() : string.Empty;
                        row.PHONE = objReader["PHONE"] != null ? objReader["PHONE"].ToString() : string.Empty;
                        row.MOBILE = objReader["MOBILE"] != null ? objReader["MOBILE"].ToString() : string.Empty;
                        row.EXTENTIONS = objReader["EXTENTIONS"] != null ? objReader["EXTENTIONS"].ToString() : string.Empty;
                        row.EMAIL = objReader["EMAIL"] != null ? objReader["EMAIL"].ToString() : string.Empty;
                        row.PRISM_ID = objReader["PRISM_ID"] != null ? objReader["PRISM_ID"].ToString() : string.Empty;
                        row.IS_ACTIVE = objReader["IS_ACTIVE"] != null ? objReader["IS_ACTIVE"].ToString() : string.Empty;
                        row.CREATEDDATE = objReader["CREATEDDATE"] != null ? objReader["CREATEDDATE"].ToString() : string.Empty;
                        row.CREATEDBY = objReader["CREATEDBY"] != null ? objReader["CREATEDBY"].ToString() : string.Empty;
                        row.MODIFIEDDATE = objReader["MODIFIEDDATE"] != null ? objReader["MODIFIEDDATE"].ToString() : string.Empty;
                        row.MODIFIEDBY = objReader["MODIFIEDBY"] != null ? objReader["MODIFIEDBY"].ToString() : string.Empty;
                        row.SP_ID = objReader["SP_ID"] != null ? objReader["SP_ID"].ToString() : string.Empty;
                        row.SP_CREATEDDATE = objReader["SP_CREATEDDATE"] != null ? objReader["SP_CREATEDDATE"].ToString() : string.Empty;
                        row.SP_MODIFIEDDATE = objReader["SP_MODIFIEDDATE"] != null ? objReader["SP_MODIFIEDDATE"].ToString() : string.Empty;
                        row.SP_CREATEDBY = objReader["SP_CREATEDBY"] != null ? objReader["SP_CREATEDBY"].ToString() : string.Empty;
                        row.SP_MODIFIEDBY = objReader["SP_MODIFIEDBY"] != null ? objReader["SP_MODIFIEDBY"].ToString() : string.Empty;
                        row.FAX = objReader["FAX"] != null ? objReader["FAX"].ToString() : string.Empty;
                        row.PREFEREDTIME = objReader["PREFEREDTIME"] != null ? objReader["PREFEREDTIME"].ToString() : string.Empty;

                        response.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return response;
        }


        public async Task<string> GetConfigFromDBAsyc(string key)
        {
            string response = string.Empty;
            List<ODPCommandParameter> parametersList = null;

            ODPDataAccess dbframework = new ODPDataAccess(Constants.CONNECTION_STR);

            try
            {
                parametersList = new List<ODPCommandParameter>();

                parametersList.Add(new ODPCommandParameter("pi_keyName", key, ParameterDirection.Input, OracleDbType.Varchar2));
                parametersList.Add(new ODPCommandParameter("po_keyvalue", ParameterDirection.Output, OracleDbType.Varchar2, 2500));

                DbCommand command = await dbframework.ExecuteSPNonQueryParmAsync("comn_configuration_pkg.get_app_configuration", parametersList);
                response = !command.Parameters["po_keyvalue"].Value.Equals(Oracle.DataAccess.Types.OracleString.Null) ? command.Parameters["po_keyvalue"].Value.ToString() : string.Empty;

                //using (IDataReader objReader = await ODPDataAccess.ExecuteSPReaderAsync("comn_configuration_pkg.get_app_configuration", parametersList))
                //{

                //    while (objReader.Read())
                //    {
                //        response = objReader["po_keyvalue"] != null ? objReader["po_keyvalue"].ToString() : string.Empty;

                //    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbframework.CloseConnection();
            }

            return response;
        }
    }
}

