using b2b_solution.Models;
using Kendo.Mvc.Infrastructure.Implementation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace b2b_solution.Controllers
{
    public class CreateUserController : Controller
    {
        DataModel dm = new DataModel();
        LandingController lc = new LandingController();
        [HttpPost]

        public JsonResult NewUser(List<NewUser> newUser)
        {
            dm.TraceService("NewUser - The function called");
            JsonResult res = new JsonResult();
            foreach (var data in newUser)
            {
                DataTable dtz = dm.loadList("SelCusInfo", "sp_User", data.CusCode);
                if (dtz.Rows.Count > 0)
                {
                    dm.TraceService("New User Creation request");
                    dm.TraceService(data.Email);
                    MembershipUser user = Membership.GetUser(data.Email);

                    if (user == null)
                    {
                        try
                        {
                            string password = ConfigurationManager.AppSettings.Get("defaultPass").ToString();
                            user = Membership.CreateUser(data.Email, password);


                            user.IsApproved = true;

                            string[] arr = { data.FirstName, data.LastName == null ? "" : data.LastName, data.MobileNumber == null ? "" : data.MobileNumber, data.Email, data.CusCode };
                            DataTable dt = dm.loadList("InsInitialUser", "sp_User", user.ProviderUserKey.ToString(), arr);
                            if (dt.Rows.Count > 0)
                            {
                                dm.TraceService("User Creation successfull");
                                res = lc.AccountSetup(data.Email);
                            }
                            else
                            {
                                dm.TraceService("Customer Not found");
                                res.Data = "Customer Not found" + data.CusCode;
                                Membership.DeleteUser(data.Email);
                            }

                        }
                        catch (Exception ex)
                        {
                            dm.TraceService("Failed creation" + ex.Message.ToString());
                            Membership.DeleteUser(data.Email);
                            res.Data = ex.Message;
                        }
                    }
                    else
                    {

                        DataTable dt = dm.loadList("CheckEmail", "sp_User", data.Email);
                        if (dt.Rows.Count > 0)
                        {
                            res.Data = "Already";
                            dm.TraceService("user already existing");
                        }
                        else
                        {
                            dm.TraceService("Customer not found issue reporting");
                            Membership.DeleteUser(data.Email);
                            NewUser(newUser);
                        }
                    }
                }
                else
                {
                    res.Data = "Customer Not Found - " + data.CusCode + "====" + data.Email;
                    dm.TraceService("Customer Not Found");
                }


            }
            return res;
        }
        public class EmailParams
        {
            public string fromEmail { get; set; }
			public string fromName { get; set; }
			public string Subject { get; set; }
			public string ToName { get; set; }
			public string ToEmail { get; set; }
			public string body { get; set; }
		}

		public string SendEmail(EmailParams emailParas)
		{
			try
			{
				dm.TraceService("=============Mial Sending Started for " + emailParas.ToEmail + "==================");

			
				MailAddress fromAddress = new MailAddress(emailParas.fromEmail, emailParas.fromName);
				MailAddress toAddress = new MailAddress(emailParas.ToEmail, emailParas.ToName);

				MailMessage message = new MailMessage(fromAddress, toAddress);
                message.Subject = emailParas.Subject;
                message.Body = emailParas.body;


				string smptClient = ConfigurationManager.AppSettings.Get("smtpClinet").ToString();  
				string smtpUsername = ConfigurationManager.AppSettings.Get("smtpUsername").ToString();
				string smptPassword = ConfigurationManager.AppSettings.Get("smtpPassword").ToString();
				int smtpPort = Int32.Parse( ConfigurationManager.AppSettings.Get("smtpPort").ToString());
                bool ssl = bool.Parse(ConfigurationManager.AppSettings.Get("smtpSSL").ToString());

				SmtpClient client = new SmtpClient(smptClient, smtpPort);
				client.Credentials = new NetworkCredential(smtpUsername, smptPassword);
				client.EnableSsl = ssl;

				dm.TraceService("Mail has been triggered");

				try
				{

					client.Send(message);
					dm.TraceService("=============Mail sent to" + emailParas.ToEmail + "==================");
					return "Success";
				}
				catch (Exception ex)
				{
                    string innerException = ex.InnerException != null ? ex.InnerException.ToString() : " No Inner Exception";
					dm.TraceService("=============Mail sent Exception for " + emailParas.ToEmail + "==================" + ex.Message.ToString() + "==========" + innerException);
                    return "Exception";
				}
			}
			catch (Exception ex)
			{
				string innerException = ex.InnerException != null ? ex.InnerException.ToString() : " No Inner Exception";
				dm.TraceService("=============Mail sent Exception for " + emailParas.ToEmail + "==================" + ex.Message.ToString() + "==========" + innerException);
				return "Exception";
			}
		}
	}
}