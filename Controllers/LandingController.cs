using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services;
using b2b_solution.Models;

namespace b2b_solution.Controllers
{
    public class LandingController : Controller
    {
        DataModel dm = new DataModel();
        // GET: Landing
        public ActionResult Landing()
        {
           if (Session["UserID"] != null && Session["isVerified"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Brands()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
			Creds creds = new Creds();
			Session["userID"] = null;
			try
            {
                if (Request.Cookies["username"] != null)
                {
					ViewBag.username = Request.Cookies["username"].Value;
					ViewBag.password = Request.Cookies["password"].Value;
					ViewBag.check = true;
                }
                else
                {
					ViewBag.username = "";
					ViewBag.password = "";
					ViewBag.check = false;
				}
              
            }
            catch(Exception ex)
            {
             ViewBag.username = "";
             ViewBag.password = "";
                ViewBag.check = false;
            }
            return View(creds);
        }
       
        [HttpPost]
        public ActionResult Login(Creds creds)
        {
            Session["userID"] = null;
			HandleError handleError = new HandleError();
            
            if (creds.username == null)
            {
                handleError.message = "Please enter username";
                handleError.mode = 1;
                return Json(handleError, JsonRequestBehavior.AllowGet);
            }
            if (creds.password == null)
            {
                handleError.message = "Please enter password";
                handleError.mode = 2;
                return Json(handleError, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (Membership.ValidateUser(creds.username, creds.password))
                {
                    //LOGIN SUCCESS
                    DataTable dtUser = dm.loadList("IsNullCustIDS", "sp_User", creds.username);
                    Session["userID"] = dtUser.Rows[0]["ID"].ToString();
                   // Session["CusID"] = dtUser.Rows[0]["cus_ID"].ToString();
                    Session["UserName"] = dtUser.Rows[0]["name"].ToString();
                    Session["LoginID"] = dtUser.Rows[0]["UserName"].ToString();
                    Session["Roles"] = dtUser.Rows[0]["Roles"].ToString();
                    Session["RoleName"] = dtUser.Rows[0]["RoleName"].ToString();
                    Session["CutOff"] = dtUser.Rows[0]["Timings"].ToString();
                   // Session["cusName"] = dtUser.Rows[0]["cus_Name"].ToString();
                    Session["cusHeader"] = dtUser.Rows[0]["csh_Name"].ToString();
                    Session["Type"] = dtUser.Rows[0]["Type"].ToString();

                    ViewBag.UserType = dtUser.Rows[0]["Type"].ToString();

                  
                    if (Session["Type"].ToString() == "BR") {
						handleError.UserType = "BR";
						Session["CusID"] = dtUser.Rows[0]["cus_ID"].ToString();
                        Session["cusName"] = dtUser.Rows[0]["cus_Name"].ToString();
                       
                    }
                    else
                    {
						handleError.UserType = "HO";
						try
                        {
							var selectedCusID = Request.Cookies["selectedCusID"];
							var selectedCusName = Request.Cookies["selectedCusName"];
							if (selectedCusID != null)
							{
								Session["CusID"] = selectedCusID.Value;
							}
							if (selectedCusName != null)
							{
								Session["cusName"] = Uri.UnescapeDataString(selectedCusName.Value);
							}
						}
						catch(Exception ex)
                        {

                        }

                       
                    }

                    string NewUser = dtUser.Rows[0]["NewUser"].ToString();
                    string isVerified = dtUser.Rows[0]["isVerified"].ToString();

                   if (isVerified.Equals("N"))
                    {
                        Session["isVerified"] = "0";
                        handleError.message = "Your account is not yet verified, kindly proceed with verification";
                        handleError.mode = 4;
                        return Json(handleError, JsonRequestBehavior.AllowGet);
                    }
                    else if (NewUser.Equals(""))
                    {
                        Session["isVerified"] = null;
                        handleError.message = "New User";
                        handleError.mode = 5;
                        return Json(handleError, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {

                       
                        LoginAudit(dtUser.Rows[0]["ID"].ToString(), creds.username, "Success on B2B Portal");

                        if (creds.remembercheck)
                        {
                            Response.Cookies.Clear();
                            Response.Cookies["username"].Value = creds.username;
                            Response.Cookies["password"].Value = creds.password;
                        }
                        else
                        {
                            Response.Cookies.Clear();
                        }

                        int VerStatus = GenerateOTP(Session["userID"].ToString());

                        if (VerStatus == 0)
                        {
                            Session["isVerified"] = "1";
                            handleError.message = "Login successfull";
                            handleError.mode = 3;
                            handleError.UserType = Session["Type"].ToString();
                        }
                        else
                        {

                            Session["isVerified"] = null;
                            handleError.message = "Login successfull";
                            handleError.mode = 6;
                        }
                      
                        return Json(handleError, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    handleError.mode = 4;
                    handleError.message = "Invalid Username/Password";
                    LoginAudit("", creds.username, "Failure due to invalid user credentials");
                    return Json(handleError, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                handleError.mode = 4;
                handleError.message = ex.Message.ToString();
                return Json( handleError , JsonRequestBehavior.AllowGet);
            }
        }

        public void LoginAudit(String LoginID, String UserName, String Status)
        {
            string Mode;
            string[] Paras;
            Paras = new string[2];
            Paras[0] = UserName;
            Paras[1] = Status;

            string Res = "0";
            Mode = "AuditLog";

            try
            {

                Res = dm.SaveData("sp_Report", Mode, LoginID, Paras);
            }
            catch (Exception ex)
            {
            }
        }

        public ActionResult ForgotPassword()
        {

            return View();
        }

        public ActionResult Activate()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }
       
        public JsonResult AccountSetup(string email)
        {
            string stats = "" , id = "";
            DataTable dt = dm.loadList("CheckEmail", "sp_User", email);
            string password = ConfigurationManager.AppSettings.Get("defaultPass").ToString();
            if (dt.Rows.Count > 0)
            {
                stats = dt.Rows[0]["isVerified"].ToString();
                id = dt.Rows[0]["ID"].ToString();
                if (stats == "N")
                {
                    Guid guid = Guid.NewGuid();
                    //
                    DataTable dtEmail = dm.loadList("GetEmailInfo", "sp_User");
                    if (dtEmail.Rows.Count > 0)
                    {
                        string fromEmail = dtEmail.Rows[0]["FromMail"].ToString();
                        string fromName = dtEmail.Rows[0]["FromName"].ToString();
                        string subject = dtEmail.Rows[0]["sub"].ToString();
                        string toName = dtEmail.Rows[0]["ToName"].ToString();
                        string body = dtEmail.Rows[0]["htmlBody"].ToString();
                        string URL = dtEmail.Rows[0]["URL"].ToString();
                        URL += guid.ToString();
                        body =  body.Replace("{0}" , URL);
                        body = body.Replace("{1}", password);

                        string[] arr = { email, guid.ToString() };
                        DataTable dtInsEmail = dm.loadList("InsVerifyUser", "sp_User" , id , arr);
                        try
                        {
                            if (dtInsEmail.Rows.Count > 0)
                            {
                                dm.Execute(fromEmail, fromName, subject, toName, email, body, "", "");
                                stats = "S";
                            }
                        }
                        catch (Exception ex)
                        {
                            stats = "T";
                        }
                    }
                }
                else
                {
                    stats = "Y";
                }
            }
            else
            {
                stats = "0";
            }

            var lst = new
            {
                res = stats
            };
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Verify(string url)
        {
            DataTable dt = dm.loadList("ActivateAccount", "sp_User", url);
            try
            {
                string res = dt.Rows[0]["res"].ToString();
                if (res.Equals("1"))
                {
                    return View(1);
                }
                else if (res.Equals("-1"))
                {
                    //LINK EXPIRED
                    return View(-1);
                }
                else
                {
                    //Could Not find the account
                    return View(0);
                }
            }catch (Exception ex)
            {
                return View();
            }
        }

        public JsonResult ResetPass(string email)
        {
            string stats = "", id = "";
            MembershipUser user;
           
            DataTable dt = dm.loadList("CheckEmail", "sp_User", email);
            if (dt.Rows.Count > 0)
            {
                try
                {
                    string username = dt.Rows[0]["UserName"].ToString();
                    user = Membership.GetUser(username);
                    user.UnlockUser();



                    string NewPass = user.ResetPassword();
                    string[] arr = { email };
                    string svd = dm.SaveData("sp_User", "UpdNewUserStatusToNull", null, arr);

                    DataTable dtEmail = dm.loadList("GetResetEmailInfo", "sp_User");
                    if (dtEmail.Rows.Count > 0)
                    {
                        string fromEmail = dtEmail.Rows[0]["FromMail"].ToString();
                        string fromName = dtEmail.Rows[0]["FromName"].ToString();
                        string subject = dtEmail.Rows[0]["sub"].ToString();
                        string toName = dtEmail.Rows[0]["ToName"].ToString();
                        string body = dtEmail.Rows[0]["htmlBody"].ToString();
                        body = body.Replace("{0}", NewPass);

                        try
                        {
                            dm.Execute(fromEmail, fromName, subject, toName, email, body, "", "");
                            stats = "S";
                        }
                        catch (Exception ex)
                        {
                            stats = "T";
                        }
                    }
                }
                catch (Exception ex) 
                {
                    stats = "T";
                }
            }
            else
            {
                stats = "0";
            }

            var lst = new
            {
                res = stats
            };
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdatePassword(string oldPass , string NewPass)
        {
            string stats = "";
            MembershipUser user;
            user = Membership.GetUser(Session["LoginID"].ToString());
            Boolean isSucess = user.ChangePassword(oldPass, NewPass);
           
            if (isSucess)
            {
                string[] arr = { ""};
                string svd = dm.SaveData("sp_Masters", "UpdNewUserStatus", Session["userID"].ToString(), arr);
                stats = "1";
            }
            else
            {
                stats = "0";
            }

            var lst = new
            {
                res = stats
            };
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProdNames(string text)
        {
            
            DataTable dt = dm.loadList("SelAutoComplete", "sp_B2B_Guest", text);

            List<AutoSearch> lst = new List<AutoSearch>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AutoSearch lstOrders = new AutoSearch();
                lstOrders.itmName = dt.Rows[i]["itm_Name"].ToString();
                lst.Add(lstOrders);
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public int GenerateOTP(string userID)
        {
            try
            {
                DataTable dt = dm.loadList("CheckDailyVerify", "sp_User", userID);
                if (dt.Rows.Count > 0)
                {
                    string VerStatus = dt.Rows[0]["VerStatus"].ToString();
                    string otp = dt.Rows[0]["otp"].ToString();


                    if (VerStatus.Equals("1") && !otp.Equals("0"))
                    {
                        //VERIFICATION PENDING
                        //SEND NEW MAIL

                        DataTable dtEmail = dm.loadList("GetDailyVerEmails", "sp_User", userID);
                        if (dtEmail.Rows.Count > 0)
                        {
                            string fromEmail = dtEmail.Rows[0]["FromMail"].ToString();
                            string fromName = dtEmail.Rows[0]["FromName"].ToString();
                            string subject = dtEmail.Rows[0]["sub"].ToString();
                            string toName = dtEmail.Rows[0]["ToName"].ToString();
                            string Tomail = dtEmail.Rows[0]["Tomail"].ToString();
                            string body = dtEmail.Rows[0]["htmlBody"].ToString();
                            body = body.Replace("{0}", otp);

                            try
                            {
                                dm.Execute(fromEmail, fromName, subject, toName, Tomail, body, "", "");
                            }
                            catch (Exception ex)
                            {
                                return 0;
                            }
                        }

                        return 1;
                    }
                    else if (VerStatus.Equals("1") )
                    {
                        //ALREADY PENDING VERIFICATION
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public JsonResult UpdDailyVer(string otp, string userID)
        {
            string stats = "";
            string platform = "WEB";

            string[] arr = { otp , platform };
            DataTable dt = dm.loadList("UpdDailyVerification", "sp_User", userID, arr);
            if (dt.Rows.Count > 0)
            {
                stats = dt.Rows[0]["res"].ToString();
            }

            var lst = new
            {
                res = stats
            };
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult OTP()
        {
            return View();
        }
        public ActionResult Terms()
        {
            return View();
        }
        public ActionResult Privacy()
        {
            return View();
        }
        public ActionResult BranchSelection()
        {         
                return View();
        }
        public ActionResult SelectOutletForUser()
        {
            HandleError handleError = new HandleError();

            DataTable dt = dm.loadList("GetOutletSelectionForUser", "sp_User", Session["userID"].ToString());
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

        [HttpPost]
        public JsonResult StoreCookiesInSession(string selectedCusID, string selectedCusName)
        {
            try
            {
                Session["CusID"] = selectedCusID;
                Session["cusName"] = selectedCusName;
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Handle any errors
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}