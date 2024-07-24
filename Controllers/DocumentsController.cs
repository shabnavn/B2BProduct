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
    public class DocumentsController : Controller
    {
        DataModel dm = new DataModel();
        public ActionResult NewKYCDocument()
        {
            return View();
        }


        public ActionResult ViewCustomerDocuments(SearchComplete allOrders)
        {
            return View(allOrders);
        }
        public ActionResult EditDocument(string id)
        {
            DataTable dt = dm.loadList("CusDocDetail", "sp_B2B_CusDocument", id);
            ViewBag.DocumentDetail = dt;
            return View();
        }

        public ActionResult GetDocuments([DataSourceRequest] DataSourceRequest request, OrderList allOrders)
        {
            
            DataTable dt = dm.loadList("GetDetails", "sp_B2B_CusDocument", Session["CusID"].ToString());

            List<ViewAllCusDocuments> lst = new List<ViewAllCusDocuments>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ViewAllCusDocuments lstOrders = new ViewAllCusDocuments();
                lstOrders.id = dt.Rows[i]["csd_ID"].ToString();
                lstOrders.Type = dt.Rows[i]["cdt_Type"].ToString();
                lstOrders.Status = dt.Rows[i]["Status"].ToString();
                lstOrders.DocNo = dt.Rows[i]["csd_docNo"].ToString();
                lstOrders.Image = dt.Rows[i]["csd_docPath"].ToString();
                lstOrders.CreatedDate = dt.Rows[i]["CreatedDate"].ToString();
                lstOrders.CreatedBy = dt.Rows[i]["name"].ToString();
                lstOrders.StartDate = dt.Rows[i]["StartDate"].ToString();
                lstOrders.ExpiryDate = dt.Rows[i]["ExpiryDate"].ToString();
                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.ViewAllCusDocuments
            {
                id = p.id,
                Type = p.Type,
                Status = p.Status,
                DocNo = p.DocNo,
                Image = p.Image,
                CreatedDate = p.CreatedDate,
                CreatedBy = p.CreatedBy,
                ExpiryDate = p.ExpiryDate,
                StartDate = p.StartDate
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
                    var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/Documents"), fileName);
                    ViewBag.physicalPath = physicalPath;
                    Session["SupportImage"] = "../UploadFiles/Documents/" + fileName;
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
                    var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/Documents"), fileName);

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
        public ActionResult NewUpload(DocUploadInputs supportInputs)
        {
            string imagePath = "";
            string[] paras;
            try
            {
                imagePath = Session["SupportImage"].ToString();
                Session["SupportImage"] = null;
            }
            catch (Exception ex)
            {
                Session["SupportImage"] = null;
            }
            string ExpDt, StDt;

            if (supportInputs.ExpDate == null)
            {
                 ExpDt = "";
                 StDt = "";
            }else
            {
                ExpDt = supportInputs.ExpDate;
                StDt = supportInputs.StartDate;

            }
           
            paras = new string[] { Session["UserID"].ToString(), supportInputs.DocType, supportInputs.DocNo, ExpDt.ToString(), imagePath, StDt.ToString() };
            DataTable dt = dm.loadList("InsCusDoc", "sp_B2B_CusDocument", Session["CusID"].ToString(), paras);
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
        public ActionResult UpdateDocument(DocUploadInputs supportInputs)
        {
            string imagePath = "";
            string[] paras;
            try
            {
                imagePath = Session["SupportImage"].ToString();
                Session["SupportImage"] = null;
            }
            catch (Exception ex)
            {
                Session["SupportImage"] = null;
            }
            string ExpDt, StDt;

            if (supportInputs.ExpDate == null)
            {
                ExpDt = "";
                StDt = "";
            }
            else
            {
                ExpDt = supportInputs.ExpDate;
                StDt = supportInputs.StartDate;

            }
            paras = new string[] { Session["UserID"].ToString(), supportInputs.DocNo, ExpDt, imagePath, StDt, supportInputs.id };
            DataTable dt = dm.loadList("UpdateDoc", "sp_B2B_CusDocument", Session["CusID"].ToString(), paras);
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
        public JsonResult GetDocType()
        {

            DataTable dt = dm.loadList("GetDocType", "sp_B2B_CusDocument");

            List<DocType> lst = new List<DocType>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DocType lstOrders = new DocType();
                lstOrders.DocId = dt.Rows[i]["cdt_ID"].ToString();
                lstOrders.DocumentType = dt.Rows[i]["cdt_Type"].ToString();
                lstOrders.IsExpiryDate = dt.Rows[i]["IsExpiryDate"].ToString();

                lst.Add(lstOrders);
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIsExpiry(string Id)
        {
            int result = 0;
            DataTable dt = dm.loadList("GetExpiry", "sp_B2B_CusDocument", Id);

            List<DocType> lst = new List<DocType>();

            DocType lstOrders = new DocType();
            lstOrders.IsExpiryDate = dt.Rows[0]["IsExpiryDate"].ToString();

            return Json(lstOrders, JsonRequestBehavior.AllowGet);
        }
    }
}