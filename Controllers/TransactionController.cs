using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using b2b_solution.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using OfficeOpenXml;
using ProcessExcel;

namespace b2b_solution.Controllers
{
    [CheckSession]
    public class TransactionController : Controller
    {
        DataModel dm = new DataModel();
        // GET: Transaction
        public ActionResult Invoices(SearchComplete allOrders)
        {
            string Roles = Session["Roles"].ToString();
            if (Roles.Contains("AD") || Roles.Contains("FN"))
            {
                return View(allOrders);
            }
            else
            {
                return View("AccessDenied");
            }
            
        }

        public ActionResult Outstanding(SearchComplete allOrders)
        {
            string Roles = Session["Roles"].ToString();
            if (Roles.Contains("AD") || Roles.Contains("FN"))
            {
                return View(allOrders);
            }
            else
            {
                return View("AccessDenied");
            }
        }

        public ActionResult GetInvoices([DataSourceRequest] DataSourceRequest request, OrderList allOrders)
        {
            string[] arr = { allOrders.FromDate, allOrders.ToDate };
            DataTable dt = dm.loadList("SelInvoices", "sp_B2B_Orders",  Session["CusID"].ToString(), arr);

            List<ViewAllInvoices> lst = new List<ViewAllInvoices>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ViewAllInvoices lstOrders = new ViewAllInvoices();
                lstOrders.id = dt.Rows[i]["inv_ID"].ToString();
                lstOrders.Number = dt.Rows[i]["inv_Number"].ToString();
                lstOrders.Status = dt.Rows[i]["Status"].ToString();
                lstOrders.SubTotal = dt.Rows[i]["inv_SubTotal"].ToString();
                lstOrders.VAT = dt.Rows[i]["inv_Vat"].ToString();
                lstOrders.GradnTotal = dt.Rows[i]["inv_GrandTotal"].ToString();
                lstOrders.PayMode = dt.Rows[i]["inv_PaymentMode"].ToString();
                lstOrders.InvoicedOn = dt.Rows[i]["InvoicedOn"].ToString();

                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.ViewAllInvoices
            {
                id = p.id,
                Number = p.Number,
                Status = p.Status,
                SubTotal = p.SubTotal,
                VAT = p.VAT,
                GradnTotal = p.GradnTotal,
                PayMode = p.PayMode,
                InvoicedOn = p.InvoicedOn
            });
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult ViewInvoice(string id)
        {
            string x =  Session["CusID"].ToString();
            DataTable dtHeader = dm.loadList("SelInvoicesByID", "sp_B2B_Orders", id);
            ViewBag.HeaderInvoice = dtHeader;

            DataTable dt = dm.loadList("SelInvDetByID", "sp_B2B_Orders", id);
            ViewBag.CurrentInvoice = dt;
            Session["CurrentInvoice"] = dt;
            return View();
        }

        public ActionResult GetInvDetail([DataSourceRequest] DataSourceRequest request)
        {
            DataTable dt = (DataTable)Session["CurrentInvoice"];
            List<VieworderByID> lst = new List<VieworderByID>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VieworderByID lstOrders = new VieworderByID();
                lstOrders.id = dt.Rows[i]["ind_ID"].ToString();
                lstOrders.itmName = dt.Rows[i]["itm_Name"].ToString();
                lstOrders.itmCode = dt.Rows[i]["itm_Code"].ToString();
                lstOrders.itmImage = dt.Rows[i]["itm_Image"].ToString();
                lstOrders.HUOM = dt.Rows[i]["HUOM"].ToString();
                lstOrders.LUOM = dt.Rows[i]["LUOM"].ToString();
                lstOrders.HQty = dt.Rows[i]["ind_HQty"].ToString();
                lstOrders.LQty = dt.Rows[i]["ind_LQty"].ToString();
                lstOrders.HPrice = dt.Rows[i]["ind_HPrice"].ToString();
                lstOrders.Lprice = dt.Rows[i]["ind_LPrice"].ToString();
                lstOrders.SubTotal = dt.Rows[i]["ind_SubTotal"].ToString();
                lstOrders.VAT = dt.Rows[i]["ind_VAT"].ToString();
                lstOrders.GrandTotal = dt.Rows[i]["ind_GrandTotal"].ToString();
                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.VieworderByID
            {
                id = p.id,
                itmCode = p.itmCode,
                itmName = p.itmName,
                itmImage = p.itmImage,
                HUOM = p.HUOM,
                LUOM = p.LUOM,
                HQty = p.HQty,
                LQty = p.LQty,
                HPrice = p.HPrice,
                Lprice = p.Lprice,
                SubTotal = p.SubTotal,
                VAT = p.VAT,
                GrandTotal = p.GrandTotal
            });
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AR(SearchComplete allOrders)
        {
            string Roles = Session["Roles"].ToString();
            if (Roles.Contains("AD") || Roles.Contains("FN"))
            {
                return View(allOrders);
            }
            else
            {
                return View("AccessDenied");
            }
        }


        public ActionResult GetAR([DataSourceRequest] DataSourceRequest request, OrderList allOrders)
        {
            string[] arr = { allOrders.FromDate, allOrders.ToDate };
            DataTable dt = dm.loadList("SelAr", "sp_B2B_Orders",  Session["CusID"].ToString(), arr);

            List<ViewAllAR> lst = new List<ViewAllAR>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ViewAllAR lstOrders = new ViewAllAR();
                lstOrders.id = dt.Rows[i]["arc_ID"].ToString();
                lstOrders.Number = dt.Rows[i]["arc_Code"].ToString();
                lstOrders.PayMode = dt.Rows[i]["arc_PaymentMode"].ToString();
                lstOrders.SubTotal = dt.Rows[i]["arc_SubTotal"].ToString();
                lstOrders.VAT = dt.Rows[i]["arc_Vat"].ToString();
                lstOrders.GradnTotal = dt.Rows[i]["arc_GrandTotal"].ToString();
                lstOrders.ChequeDate = dt.Rows[i]["ChequeDate"].ToString();
                lstOrders.ChequeImg = dt.Rows[i]["arc_ChequeImg"].ToString();
                lstOrders.ChequeNo = dt.Rows[i]["arc_ChequeNo"].ToString();
                lstOrders.bank = dt.Rows[i]["bank_Name"].ToString();
                lstOrders.CreatedDate = dt.Rows[i]["CreatedDate"].ToString();
                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.ViewAllAR
            {
                id = p.id,
                Number = p.Number,
                bank = p.bank,
                ChequeNo = p.ChequeNo,
                ChequeImg = p.ChequeImg,
                ChequeDate = p.ChequeDate,
                SubTotal = p.SubTotal,
                VAT = p.VAT,
                GradnTotal = p.GradnTotal,
                PayMode = p.PayMode,
                CreatedDate = p.CreatedDate
            });
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult ViewAR(string id)
        {
            string x =  Session["CusID"].ToString();
            DataTable dtHeader = dm.loadList("SelArByID", "sp_B2B_Orders", id);
            ViewBag.HeaderAR = dtHeader;

            DataTable dt = dm.loadList("SelArDetail", "sp_B2B_Orders", id);
            ViewBag.CurrentAR = dt;
            Session["CurrentAR"] = dt;
            return View();
        }

        public ActionResult GetARDetail([DataSourceRequest] DataSourceRequest request)
        {
            DataTable dt = (DataTable)Session["CurrentAR"];
            List<ARDetail> lst = new List<ARDetail>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ARDetail lstOrders = new ARDetail();
                lstOrders.Number = dt.Rows[i]["inv_Number"].ToString();
                lstOrders.InvAmount = dt.Rows[i]["inv_GrandTotal"].ToString();
                lstOrders.PaidAmount = dt.Rows[i]["ard_PaidAmount"].ToString();
                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.ARDetail
            {
                Number = p.Number,
                InvAmount = p.InvAmount,
                PaidAmount = p.PaidAmount
            });
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetOSI([DataSourceRequest] DataSourceRequest request, OrderList allOrders)
        {
            //string[] arr = { allOrders.FromDate , allOrders.ToDate };
            DataTable dt = dm.loadList("SelOutstandingInvoices", "sp_B2B_Orders",  Session["CusID"].ToString() );
            if (dt != null)
            {
                Session["dtOSI"] = dt;
            }
            List<Outstanding> lst = new List<Outstanding>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Outstanding lstOrders = new Outstanding();

                lstOrders.InvoiceID = dt.Rows[i]["InvoiceID"].ToString();
                lstOrders.PendingAmount = dt.Rows[i]["PendingAmount"].ToString();
                lstOrders.cshName = dt.Rows[i]["Name"].ToString();
                lstOrders.InvoiceAmount = dt.Rows[i]["InvoiceAmount"].ToString();
                lstOrders.InvoicedOn = dt.Rows[i]["InvoicedOn"].ToString();

                lst.Add(lstOrders);
            }

            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.Outstanding
            {
                InvoicedOn = p.InvoicedOn,
                InvoiceAmount = p.InvoiceAmount,
                cshName = p.cshName,
                InvoiceID = p.InvoiceID,
                PendingAmount = p.PendingAmount
            });
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult DownloadOSI()
        {
            DataTable dt = (DataTable)Session["dtOSI"];

            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            //using (var excelPackage = new ExcelPackage())
            //{
            //    var worksheet = excelPackage.Workbook.Worksheets.Add("Outstanding");
            //    worksheet.Cells["A1"].LoadFromDataTable(dt, true);

            //    var memoryStream = new MemoryStream();
            //    excelPackage.SaveAs(memoryStream);
            //    memoryStream.Position = 0;

            //    return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Outstanding_Invoices_"+DateTime.Now.ToString("yyyy-MMM-dd")+".xlsx");
            //}

            BuildExcel excel = new BuildExcel(); 
            byte[] output =   excel.SpreadSheetProcess(dt, "Invoices");

            return File(output, "application/excel", "Outstanding_Invoices_" + DateTime.Now.ToString("yyyy-MMM-dd") + ".xlsx");

        }

    }
}