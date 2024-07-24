using b2b_solution.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Data;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Configuration;
using PDF_Export;
using System.Windows.Controls.Primitives;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Telerik.Windows.Documents.Flow.Model;

namespace b2b_solution.Controllers
{
    [CheckSession]
    public class OrderController : Controller
    {
        DataModel dm = new DataModel();

        // GET: Order
        public ActionResult CheckOut()
        {
            Session["LPOImage"] = null;
            return View();
        }

        public ActionResult SuccessOrder(string OrderNo , string id)
        {
            
            ViewBag.OrderNo = OrderNo;
            ViewBag.OrderID = id;

            return View();
        }

        public ActionResult Async_Save(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {

          
            // The Name of the Upload component is "files"
            if (files != null)
            {
               
                foreach (var file in files)
                {

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                        dm.TraceService(fileName );
                        var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/LPO"), fileName);
                    ViewBag.physicalPath = physicalPath;
                    Session["LPOImage"] = "../UploadFiles/LPO/" + fileName;
                    file.SaveAs(physicalPath);
                        dm.TraceService( "File Saved" + DateTime.Now);
                    }
                }
            }
            catch(Exception ex){
                dm.TraceService(ex.Message.ToString() +"_" + DateTime.Now);
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
                    var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        // System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult PlaceOrder(OrderInputs orderInputs)
        {
            string imagePath = "";
            string platform = "WEB";
            try
            {
                imagePath = Session["LPOImage"].ToString();
                Session["LPOImage"] = null;
            }
            catch (Exception ex)
            {
                Session["LPOImage"] = null;
            }


            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw))
                {

                    writer.WriteStartDocument(true);
                    writer.WriteStartElement("r");
                    int c = 0;

                    string[] arr = { orderInputs.SubTotal, orderInputs.VAT, orderInputs.GrandTotal, orderInputs.ExpDate, orderInputs.Remarks, orderInputs.Emirate, orderInputs.BuildingName , orderInputs.RoomNo ,
                    orderInputs.Street, orderInputs.LandMark, orderInputs.LPO , imagePath , orderInputs.SubTotal_WO_D , orderInputs.TotalDisc,platform};
                    string[] arrName = { "SubTotal", "VAT", "GrandTotal", "DelDate", "Remarks", "Emirate", "BuildingName", "RoomNo", "Street", "LandMark", "LPO", "Attachment" , "SubTotal_WO_D", "TotalDisc", "Platform" };
                    createNode(arr , arrName ,  writer);

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                }

                string para3 = sw.ToString();

                string[] paras = { Session["CusID"].ToString(), para3 };
                DataTable dt = dm.loadList("InsOrder", "sp_B2B_Orders", Session["UserID"].ToString()  , paras);
                HandleError handleError = new HandleError(); 
                if (dt.Rows.Count > 0)
                {
                    string OrderNo = dt.Rows[0]["Res"].ToString();
                    string OrderID = dt.Rows[0]["Descr"].ToString();
                    int mode = Int32.Parse( dt.Rows[0]["Title"].ToString());
                    if (mode == 1)
                    {
                        handleError.message = OrderNo;
                        handleError.mode = mode;
                        handleError.desc = OrderID;
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
        }

        public ActionResult AllOrders(SearchComplete allOrders)
        {
			string[] paras = { Session["CusID"].ToString() };
			DataTable dt = dm.loadList("SelDealerType", "sp_B2B_Orders", Session["UserID"].ToString(), paras);
			ViewBag.DealerType = dt;
			return View(allOrders);

		}
		public JsonResult GetSubDealers()
		{

			DataTable dt = dm.loadList("GetSubDealers", "sp_B2B_Orders", Session["CusID"].ToString());
            List<Dealers> lst = new List<Dealers>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
				Dealers lstOrders = new Dealers();
				lstOrders.cusID = dt.Rows[i]["cus_ID"].ToString();
				lstOrders.CusName = dt.Rows[i]["cus_Name"].ToString();

				lst.Add(lstOrders);
			}

			return Json(lst, JsonRequestBehavior.AllowGet);
			
        }

        private void createNode(string[] arr, string[] arrNames, XmlWriter writer)
        {
            writer.WriteStartElement("Values");
            for(int i = 0; i < arr.Length; i++)
            {
                writer.WriteStartElement(arrNames[i]);
                writer.WriteString(arr[i]);
                writer.WriteEndElement();
            }
          writer.WriteEndElement();
        }


        public ActionResult GetOrders([DataSourceRequest] DataSourceRequest request , OrderList allOrders)
        {
            

           
            string[] arr = { allOrders.FromDate, allOrders.ToDate, allOrders.SubDealer == null ? "0" : allOrders.SubDealer };

			DataTable dt = dm.loadList("SelOrders", "sp_B2B_Orders", Session["CusID"].ToString(), arr);
            List<ViewAllOrder> lst = new List<ViewAllOrder>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ViewAllOrder lstOrders = new ViewAllOrder();
                lstOrders.id = dt.Rows[i]["ord_ID"].ToString();
                lstOrders.OrderNo = dt.Rows[i]["ord_Number"].ToString();
                lstOrders.Status = dt.Rows[i]["Status"].ToString();
                lstOrders.SubTotal = dt.Rows[i]["ord_SubTotal"].ToString();
                lstOrders.VAT = dt.Rows[i]["ord_Vat"].ToString();
                lstOrders.GradnTotal = dt.Rows[i]["ord_GrandTotal"].ToString();
                lstOrders.CreatedDate = dt.Rows[i]["CreatedDate"].ToString();
                lstOrders.CreatedBy = dt.Rows[i]["CreatedBy"].ToString();
                lstOrders.ExpectedDate = dt.Rows[i]["ExpDate"].ToString();
                lstOrders.Attachment = dt.Rows[i]["ord_LPO_Attachment"].ToString();
                lstOrders.LPO = dt.Rows[i]["ord_LPO"].ToString();
                lstOrders.Total = dt.Rows[i]["ord_SubTotal_WO_Discount"].ToString();
                lstOrders.Discount = dt.Rows[i]["ord_TotalDiscount"].ToString();
                lstOrders.DealerCode= dt.Rows[i]["cus_Code"].ToString();
                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.ViewAllOrder
            {
                id = p.id,
                OrderNo = p.OrderNo,
                Status = p.Status,
                SubTotal = p.SubTotal,
                VAT = p.VAT,
                GradnTotal = p.GradnTotal,
                CreatedDate = p.CreatedDate,
                CreatedBy = p.CreatedBy,
                LPO = p.LPO,
                Attachment = p.Attachment,
                ExpectedDate = p.ExpectedDate,
                Discount = p.Discount,
                Total = p.Total,
                DealerCode= p.DealerCode,
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewOrder(string id)
        {
            string x =  Session["CusID"].ToString();
            DataTable dtHeader = dm.loadList("SelOrdersbyID", "sp_B2B_Orders", id);
            ViewBag.HeaderOrder = dtHeader;
           
            DataTable dt = dm.loadList("SelOrderDetailbyID", "sp_B2B_Orders" , id);
            ViewBag.CurrentOrder = dt;
            Session["CurrentOrder"] = dt;

            DataTable dtHist = dm.loadList("SelOrderLifeCycle", "sp_B2B_Orders", id);
            ViewBag.TrackOrder = dtHist;

            return View();
        }

        public ActionResult GetOrderDetail([DataSourceRequest] DataSourceRequest request)
        {
            
            DataTable dt = (DataTable)Session["CurrentOrder"];

            List<VieworderByID> lst = new List<VieworderByID>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                VieworderByID lstOrders = new VieworderByID();
                lstOrders.id = dt.Rows[i]["odd_ID"].ToString();
                lstOrders.itmName = dt.Rows[i]["itm_Name"].ToString();
                lstOrders.itmCode = dt.Rows[i]["itm_Code"].ToString();
                lstOrders.itmImage = dt.Rows[i]["itm_Image"].ToString();
                lstOrders.HUOM = dt.Rows[i]["HigherUom"].ToString();
                lstOrders.LUOM = dt.Rows[i]["LowerUOM"].ToString();
                lstOrders.HQty = dt.Rows[i]["odd_HigherQty"].ToString();
                lstOrders.LQty = dt.Rows[i]["odd_LowerQty"].ToString();
                lstOrders.HPrice = dt.Rows[i]["odd_HigherUOMSellPrice"].ToString();
                lstOrders.Lprice = dt.Rows[i]["odd_LowerUOMSellPrice"].ToString();
                lstOrders.SubTotal = dt.Rows[i]["odd_SubTotal"].ToString();
                lstOrders.VAT = dt.Rows[i]["odd_VAT"].ToString();
                lstOrders.GrandTotal = dt.Rows[i]["odd_GrandTotal"].ToString();
                lstOrders.TransType = dt.Rows[i]["odd_TransType"].ToString();
                lstOrders.Total = dt.Rows[i]["odd_SubTotalWDiscount"].ToString();
                lstOrders.Discount = dt.Rows[i]["odd_Discount"].ToString();

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
                GrandTotal = p.GrandTotal,
                TransType = p.TransType,
                Discount = p.Discount,
                Total = p.Total
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TrackHistory(string id)
        {
            DataTable dt = dm.loadList("SelOrderLifeCycle", "sp_B2B_Orders", id);
            ViewBag.TrackOrder = dt;
            return PartialView("OrderFlow");
        }

		public ActionResult getPrevOrders(string ordID)
		{
			HandleError handleError = new HandleError();
			string user = Session["UserID"].ToString();
			string x = Session["CusID"].ToString();
			if (Session["UserID"] == null)
			{
				handleError.mode = -1; 
				handleError.message = "User session expired"; 
			}
			else
			{
				string[] arr = { ordID, x };
				DataTable dt = dm.loadList("InsRepOrder", "sp_B2B_Orders", user, arr);

				try
				{
					if (dt.Rows.Count > 0)
					{
						int resID = Int32.Parse(dt.Rows[0]["Res"].ToString());
						if (resID > 0)
						{
							handleError.mode = 1;
							handleError.message = "success";
						}
						else
						{
							handleError.mode = 0;
							handleError.message = "failure";
						}
					}
				}
				catch (Exception e)
				{
					handleError.mode = 0;
					handleError.message = "failure";
				}
			}
			return Json(handleError, JsonRequestBehavior.AllowGet);
		}


		public JsonResult GetExpDates()
        {
           
            DataTable dt = dm.loadList("SelExpDelDates", "sp_B2B_Orders",  Session["CusID"].ToString());

            List<DelDates> lst = new List<DelDates>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DelDates lstOrders = new DelDates();
                lstOrders.Dates = dt.Rows[i]["NextDay"].ToString();

                lst.Add(lstOrders);
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerateOrderDetail(string OrderID , string OrderNum , string cusID )
        {
            try
            {
                string filePath = "", JsonPath = "", folderPath = "";
                string fileName = "/" + OrderNum + ".pdf";
                string downPath = "/UploadFiles/downloads/" + cusID;
                string DownloadURL = downPath + fileName;
              
               
                if (!System.IO.File.Exists(folderPath + fileName))
                {
					var s = Server.MapPath("/UploadFiles/ReportTemplate/license.key");
					Stimulsoft.Base.StiLicense.LoadFromFile(s);
					var report = new StiReport();
					var path = Server.MapPath("/UploadFiles/ReportTemplate/B2BOrders.mrt");

					report.Load(path);
                    report["@Para2"] = OrderID;

					string url = ConfigurationManager.AppSettings.Get("DIGITS-B2B");
					((StiSqlDatabase)report.Dictionary.Databases["B2BReport"]).ConnectionString = url;
					StiOptions.Export.Pdf.AllowImportSystemLibraries = true;


					var tempPdfPath = Server.MapPath(DownloadURL);
					MemoryStream ms = new MemoryStream();
					report.Render();
					report.ExportDocument(StiExportFormat.Pdf, ms);
					System.IO.File.WriteAllBytes(tempPdfPath, ms.ToArray());
				}
                var res = new
                {
                    url = DownloadURL
                };

                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var res = new
                {
                    url = ""
                };
                return Json(res, JsonRequestBehavior.AllowGet);
            }
        }
	}
}