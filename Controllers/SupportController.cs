using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using b2b_solution.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace b2b_solution.Controllers
{
    [CheckSession]
    public class SupportController : Controller
    {
        DataModel dm = new DataModel(); 
        // GET: Support
        public ActionResult NewSupport()
        {
            return View();
        }

        public ActionResult ViewSupports(SearchComplete allOrders)
        {
            return View(allOrders);
        }

        public ActionResult SupportDetail(string id)
        {
            DataTable dt = dm.loadList("SelCusSuppHeader", "sp_B2B_Support", id);
            DataTable dtMessages = dm.loadList("SelSupportMessages", "sp_B2B_Support", id);
            ViewBag.SupportDetail = dt;
            ViewBag.SupportDetailMessages = dtMessages;
            return View();
        }

        public ActionResult GetSupports([DataSourceRequest] DataSourceRequest request, OrderList allOrders)
        {
            string[] arr = { allOrders.FromDate, allOrders.ToDate };
            DataTable dt = dm.loadList("SelCustomerSupport", "sp_B2B_Support",  Session["CusID"].ToString(), arr);

            List<ViewAllSupports> lst = new List<ViewAllSupports>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ViewAllSupports lstOrders = new ViewAllSupports();
                lstOrders.id = dt.Rows[i]["csp_ID"].ToString();
                lstOrders.Number = dt.Rows[i]["csp_Number"].ToString();
                lstOrders.Status = dt.Rows[i]["stat_Name"].ToString();
                lstOrders.Title = dt.Rows[i]["csp_Title"].ToString();
                lstOrders.Message = dt.Rows[i]["csp_Message"].ToString();
                lstOrders.Image = dt.Rows[i]["csp_Image"].ToString();
                lstOrders.CreatedDate = dt.Rows[i]["CreatedDate"].ToString();
                lstOrders.CreatedBy = dt.Rows[i]["name"].ToString();
                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.ViewAllSupports
            {
                id = p.id,
                Number = p.Number,
                Status = p.Status,
                Title = p.Title,
                Message = p.Message,
                Image = p.Image,
                CreatedDate = p.CreatedDate,
                CreatedBy = p.CreatedBy
            });
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult Async_Save(IEnumerable<HttpPostedFileBase> files)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/Support"), fileName);
                    ViewBag.physicalPath = physicalPath;
                    Session["SupportImage"] = "../UploadFiles/Support/" + fileName;
                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Async_Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/Support"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                        Session["SupportImage"] = null;
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult NewRequest(SupportInputs supportInputs)
        {
            string imagePath = "";
            try
            {
                imagePath = Session["SupportImage"].ToString();
                Session["SupportImage"] = null;
            }
            catch (Exception ex)
            {
                Session["SupportImage"] = null;
            }

                string[] paras = { Session["UserID"].ToString(), supportInputs.Title , supportInputs.Message , supportInputs.Reason , imagePath };
                DataTable dt = dm.loadList("InsCusSupport", "sp_B2B_Support",  Session["CusID"].ToString(), paras);
                HandleError handleError = new HandleError();
                if (dt.Rows.Count > 0)
                {
                    string OrderNo = dt.Rows[0]["seqNum"].ToString();
                    int mode = Int32.Parse(dt.Rows[0]["res"].ToString());
                    if (mode == 1)
                    {
                        handleError.message = OrderNo;
                        handleError.mode = mode;
                        ViewBag.OrderID = OrderNo;
                        return Json(handleError, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        handleError.message = OrderNo;
                        handleError.mode = mode;
                        return Json(handleError, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {

                    handleError.message = "";
                    handleError.mode = 0;
                    return Json(handleError, JsonRequestBehavior.AllowGet);
                }
        }

        public ActionResult SendReply(SupportInputs supportInputs)
        {
            string imagePath = "";
            try
            {
                imagePath = Session["SupportImage"].ToString();
                Session["SupportImage"] = null;
            }
            catch (Exception ex)
            {
                Session["SupportImage"] = null;
            }

            string[] paras = { supportInputs.Message, imagePath , Session["UserID"].ToString() };
            DataTable dt = dm.loadList("InsSuppMessage", "sp_B2B_Support", supportInputs.id, paras);
            HandleError handleError = new HandleError();
            if (dt.Rows.Count > 0)
            {
                int mode = Int32.Parse(dt.Rows[0]["res"].ToString());
                if (mode == 1)
                {
                    handleError.mode = mode;
                    return Json(handleError, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    handleError.mode = mode;
                    return Json(handleError, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                handleError.mode = 0;
                return Json(handleError, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Resolve(SupportInputs supportInputs)
        {

            string[] paras = { Session["UserID"].ToString() };
            DataTable dt = dm.loadList("UpdMessageResolve", "sp_B2B_Support", supportInputs.id, paras);
            HandleError handleError = new HandleError();
            if (dt.Rows.Count > 0)
            {
                int mode = Int32.Parse(dt.Rows[0]["res"].ToString());
                if (mode == 1)
                {
                    handleError.mode = mode;
                    return Json(handleError, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    handleError.mode = mode;
                    return Json(handleError, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                handleError.mode = 0;
                return Json(handleError, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Escalation(SupportInputs supportInputs)
        {

            string[] paras = { Session["UserID"].ToString() };
            DataTable dt = dm.loadList("InsEscalation", "sp_B2B_Support", supportInputs.id, paras);
            HandleError handleError = new HandleError();
            if (dt.Rows.Count > 0)
            {
                int mode = Int32.Parse(dt.Rows[0]["res"].ToString());
                if (mode == 1)
                {
                    handleError.mode = mode;
                    return Json(handleError, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    handleError.mode = mode;
                    return Json(handleError, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                handleError.mode = 0;
                return Json(handleError, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetReasons()
        {

            DataTable dt = dm.loadList("SelSupportReason", "sp_B2B_Support");

            List<Reason> lst = new List<Reason>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Reason lstOrders = new Reason();
                lstOrders.ReasonCode = dt.Rows[i]["rsn_ID"].ToString();
                lstOrders.ReasonName = dt.Rows[i]["rsn_Name"].ToString();

                lst.Add(lstOrders);
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }
    }
}