using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using b2b_solution.Models;
using System.Data;
using System.Web.Security;
using System.Configuration;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;


namespace b2b_solution.Controllers
{
    [CheckSession]
    public class ProfileController : Controller
    {
        DataModel dm = new DataModel(); 
        // GET: Profile
        public ActionResult Account()
        {
            return View();
        }
      

        public ActionResult AddNewAddr(ShippingAddress address)
        {
            HandleError handleError = new HandleError();
            string[] arr = { Session["userID"].ToString(), address.BuldgName, address.RoomNo , address.Street, address.LandMark, address.State };
             DataTable dt =  dm.loadList("InsNewAddr", "sp_B2B_Profile" ,  Session["CusID"].ToString() , arr);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    int Res = Int32.Parse(dt.Rows[0]["Res"].ToString());
                    if (Res > 0)
                    {
                        handleError.mode = 1;
                    }
                    else
                    {
                        handleError.mode = 0;
                    }
                }
                else
                {
                    handleError.mode = -1;
                }
            }
            catch (Exception ex)
            {
                handleError.mode=-1;
            }

            return Json(handleError, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SetDefault(string cuaID)
        {
            HandleError handleError = new HandleError();
            string[] arr = { Session["userID"].ToString() };
            DataTable dt = dm.loadList("SetDefAddr", "sp_B2B_Profile", cuaID, arr);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    int Res = Int32.Parse(dt.Rows[0]["Res"].ToString());
                    if (Res > 0)
                    {
                        handleError.mode = 1;
                    }
                    else
                    {
                        handleError.mode = 0;
                    }
                }
                else
                {
                    handleError.mode = -1;
                }
            }
            catch (Exception ex)
            {
                handleError.mode = -1;
            }

            return Json(handleError, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelAddr(string cuaID)
        {
            HandleError handleError = new HandleError();

            DataTable dt = dm.loadList("DelAddress", "sp_B2B_Profile", cuaID);
            try
            {
                if (dt.Rows.Count > 0)
                {
                    int Res = Int32.Parse(dt.Rows[0]["Res"].ToString());
                    if (Res > 0)
                    {
                        handleError.mode = 1;
                    }
                    else
                    {
                        handleError.mode = 0;
                    }
                }
                else
                {
                    handleError.mode = -1;
                }
            }
            catch (Exception ex)
            {
                handleError.mode = -1;
            }

            return Json(handleError, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdPassword(string cPass, string nPass, string uName)
        {
            HandleError handleError = new HandleError();
            MembershipUser user;
            try
            {
                user = Membership.GetUser(uName);
                Boolean isSucess = user.ChangePassword(cPass, nPass);
                if (isSucess)
                {
                    handleError.mode = 1;
                }
                else
                {
                    handleError.mode = 0;
                }
            }
            catch (Exception ex)
            {
                handleError.mode = -1;
            }

            return Json(handleError, JsonRequestBehavior.AllowGet);
        }

		public ActionResult UserProfiles()
		{
			return View();
		}

		public JsonResult GetCusRoles()
        {
            
            DataTable dt = dm.loadList("SelCusRoles", "sp_User");

            List<CusRoles> lst = new List<CusRoles>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CusRoles lstOrders = new CusRoles();
                lstOrders.rolName = dt.Rows[i]["crl_Name"].ToString();
                lstOrders.rolID = dt.Rows[i]["crl_ID"].ToString();
                lst.Add(lstOrders);
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
		public JsonResult GetCusType()
		{

            DataTable dttype = dm.loadList("GetCusType", "sp_User", Session["userID"].ToString());
            List<CusType> lst = new List<CusType>();

            if (dttype.Rows[0]["Type"].ToString() == "BR")
            {
                CusType lstOrders = new CusType();
                lstOrders.TypeName = "BRANCH";
                lstOrders.TypeCode = "BR";
                lst.Add(lstOrders);
            }
            else
            {
                DataTable dt = dm.loadList("SelCusType", "sp_User");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CusType lstOrders = new CusType();
                    lstOrders.TypeName = dt.Rows[i]["TypeName"].ToString();
                    lstOrders.TypeCode = dt.Rows[i]["TypeCode"].ToString();
                    lst.Add(lstOrders);
                }
            }


            return Json(lst, JsonRequestBehavior.AllowGet);
        }

		public JsonResult InsUserReq(CreateUser crUser)
        {
            HandleError handleError = new HandleError();
            MembershipUser user = Membership.GetUser(crUser.email);
            if (user != null)
            {
                handleError.mode = -1;
            }
            else
            {
                string cusid, cshid;
                try
                {
                    DataTable dtHeader = new DataTable();

                    if (crUser.type == "BR" && crUser.cusid.ToString() == "0")
                    {
                        dtHeader= dm.loadList("GeHeaderId", "sp_User", Session["CusID"].ToString());
                        cshid = dtHeader.Rows[0]["cus_csh_Id"].ToString();
                        cusid = Session["CusID"].ToString();
                    }
                    else if (crUser.type == "HO")
                    {
                        dtHeader = dm.loadList("GeHeaderId", "sp_User", Session["CusID"].ToString());
                        cshid = dtHeader.Rows[0]["cus_csh_Id"].ToString();
                        cusid = "0";
                    }
                    else
                    {
                        cusid = crUser.cusid.ToString();
                        dtHeader = dm.loadList("GeHeaderId", "sp_User", crUser.cusid.ToString());
                        cshid = dtHeader.Rows[0]["cus_csh_Id"].ToString();
                        
                    }

                    string password = ConfigurationManager.AppSettings.Get("defaultPass").ToString();
                    user = Membership.CreateUser(crUser.email, password);
					string[] arr = { crUser.firstName, crUser.lastName, crUser.contactNum, crUser.email, Session["UserID"].ToString(), cusid, crUser.roles, crUser.type,cshid };
   					DataTable dt = dm.loadList("InsUser", "sp_User", user.ProviderUserKey.ToString(), arr);
                    if (dt.Rows.Count > 0)
                    {
                        handleError.mode = Int32.Parse( dt.Rows[0]["res"].ToString());
                    }
                    else
                    {
                        handleError.mode = -2;
                    }
                }
                catch (Exception ex)
                {
                    Membership.DeleteUser(crUser.email);
                    handleError.mode = -2;
                }   
            }

            return Json(handleError, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetUsers([DataSourceRequest] DataSourceRequest request)
        {
            DataTable dt = dm.loadList("SelUsers", "sp_User",  Session["CusID"].ToString());

            List<Userlist> lst = new List<Userlist>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Userlist lstOrders = new Userlist();
                lstOrders.Id = dt.Rows[0]["ID"].ToString();
                lstOrders.FirstName = dt.Rows[i]["FirstName"].ToString();
                lstOrders.LastName = dt.Rows[i]["LastName"].ToString();
                lstOrders.Email = dt.Rows[i]["Email"].ToString();
                lstOrders.MobileNumber = dt.Rows[i]["MobileNumber"].ToString();
                lstOrders.CreatedOn = dt.Rows[i]["CreatedOn"].ToString();
                lstOrders.Status = dt.Rows[i]["UserStatus"].ToString();
                lstOrders.ApprStatus = dt.Rows[i]["ApprStatus"].ToString();
                lstOrders.Roles = dt.Rows[i]["Roles"].ToString();
				lstOrders.type = dt.Rows[i]["Type"].ToString();
                lstOrders.Customer = dt.Rows[i]["Customer"].ToString();
                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.Userlist
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                CreatedOn = p.CreatedOn,
                MobileNumber = p.MobileNumber,
                Roles = p.Roles,
                Email = p.Email,
                Status = p.Status,
                ApprStatus = p.ApprStatus,
                type = p.type,
                Customer = p.Customer,
                Id=p.Id
			});
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult OrderEligibility()
        {
            string Roles = Session["Roles"].ToString();
            string stas = "0";
            if (Roles.Contains("AD") || Roles.Contains("OT"))
            {
                stas = "1";
            }

            var res = new
            {
                res = stas
            };
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetSelectedCustomerId(string customerId, string cusName)
        {
            HandleError handleError = new HandleError();

            // Set the customer ID in TempDataJsonRequestBehavior
            TempData["SelectedCustomerId"] = customerId;
            Session["CusID"] = customerId.ToString();
            Session["cusName"] = cusName.ToString();
            
            return Json(customerId, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectCustomerByType()
        {
            HandleError handleError = new HandleError();

            DataTable dt = dm.loadList("GeCustomerByType", "sp_User", Session["CusID"].ToString());
            List<DCustomer> lst = new List<DCustomer>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DCustomer lstOrders = new DCustomer();
                lstOrders.cusID = dt.Rows[i]["cus_ID"].ToString();
                lstOrders.cusName = dt.Rows[i]["Cus_Name"].ToString();
                lst.Add(lstOrders);
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetHeaderId(string Mode)
        {
            HandleError handleError = new HandleError();

            DataTable dt = dm.loadList("GeHeaderId", "sp_User", Session["CusID"].ToString());
            List<DCustomer> lst = new List<DCustomer>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DCustomer lstOrders = new DCustomer();
                lstOrders.cusID = dt.Rows[i]["cus_csh_Id"].ToString();
                lst.Add(lstOrders);
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResetPassword(string email)
        {
            string stats = "", id = "";
            MembershipUser user;
           
            DataTable dt = dm.loadList("CheckEmail", "sp_User", email);
            HandleError handleError = new HandleError();
            if (dt.Rows.Count > 0)
            {
                List<ResetPass> lst = new List<ResetPass>();
                try
                {
                    string username = dt.Rows[0]["UserName"].ToString();
                    user = Membership.GetUser(username);
                    user.UnlockUser();



                    string NewPass = user.ResetPassword();
                  


                    
                    ResetPass lstOrders = new ResetPass();
                    lstOrders.pass = NewPass;
                    lstOrders.res = "S";
                    return Json(lstOrders, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex) 
                {
                    return Json(handleError, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(handleError, JsonRequestBehavior.AllowGet);
            }

            
            
        }
        public ActionResult SelUsersByID(string ID)
        {
            DataTable dt = dm.loadList("SelUsersByID", "sp_User", ID);

            HandleError handleError = new HandleError();
            if (dt.Rows.Count > 0)
            {
                Userlist lstOrders = new Userlist();
                lstOrders.Id = dt.Rows[0]["ID"].ToString();
                lstOrders.FirstName = dt.Rows[0]["FirstName"].ToString();
                lstOrders.Email = dt.Rows[0]["Email"].ToString();
                lstOrders.MobileNumber = dt.Rows[0]["MobileNumber"].ToString();
                lstOrders.CreatedOn = dt.Rows[0]["CreatedOn"].ToString();
                lstOrders.Status = dt.Rows[0]["UserStatus"].ToString();
                lstOrders.ApprStatus = dt.Rows[0]["ApprStatus"].ToString();
                lstOrders.Roles = dt.Rows[0]["Roles"].ToString();
                lstOrders.type = dt.Rows[0]["Type"].ToString();
                lstOrders.Customer = dt.Rows[0]["Customer"].ToString();
                return Json(lstOrders, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(handleError, JsonRequestBehavior.AllowGet);
            }
            
        }



    }
}