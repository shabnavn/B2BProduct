using b2b_solution.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Org.BouncyCastle.Asn1.Ocsp;
using PDF_Export;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace b2b_solution.Controllers
{


	[CheckSession]
	public class GridOrderController : Controller
    {
		
		DataModel dm = new DataModel();

		public ActionResult NewOrder()
		
		{
			if (Session["CusID"] == null)
			{
				return RedirectToAction("Login", "Landing");
			}
			else
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("seq", typeof(string));
				dt.Columns.Add("itmID", typeof(string));
				dt.Columns.Add("itmCode", typeof(string));
				dt.Columns.Add("itmName", typeof(string));
				dt.Columns.Add("HUomName", typeof(string));
				dt.Columns.Add("HUom", typeof(string));
				dt.Columns.Add("HQty", typeof(string));
				dt.Columns.Add("LUom", typeof(string));
				dt.Columns.Add("LUomName", typeof(string));
				dt.Columns.Add("LQty", typeof(string));
				dt.Columns.Add("HPrice", typeof(string));
				dt.Columns.Add("LPrice", typeof(string));
				dt.Columns.Add("TotalPrice", typeof(string));
				dt.Columns.Add("Discount", typeof(string));
				dt.Columns.Add("SubTotal", typeof(string));
				dt.Columns.Add("VAT", typeof(string));
				dt.Columns.Add("GrandTotal", typeof(string));
				Session["dtItems"] = dt;
                Session["NewDtItems"] = dt;
                return View();
			}
				
		}


		
	public JsonResult getLUOM(string itmID, string uomName, string H_uom_ID)
		{
			try
			{
				
				string[] arr = { uomName == null ? "" : uomName, H_uom_ID };

				DataTable dt = dm.loadList("SelCusItemLUOM", "sp_Web_SalesOrder", itmID, arr);
				List<UOM> lst = new List<UOM>();

				foreach (DataRow dr in dt.Rows)
				{
					UOM lstOrders = new UOM();
					lstOrders.uomID = dr["uom_ID"].ToString();
					lstOrders.uomName = dr["uom_Name"].ToString();
					lstOrders.UPC = dr["pru_UPC"].ToString();
					lst.Add(lstOrders);
				}
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return null;
			}

		}

		
		public JsonResult getItmPrice( string itmID, string uomID)
		{
			try
			{
				string cusID = Session["cusID"].ToString();

				string[] arr = { itmID, uomID };

				DataTable dt = dm.loadList("SelItemPriceByUOM", "sp_Web_SalesOrder", cusID, arr);
				List<ItemPrice> lst = new List<ItemPrice>();
				if (dt.Rows.Count > 0)
				{
					foreach (DataRow dr in dt.Rows)
					{
						ItemPrice lstOrders = new ItemPrice();
						lstOrders.OfferPrice = dr["OfferPrice"].ToString();
						lstOrders.StandardPrice = dr["standardPrice"].ToString();
						lst.Add(lstOrders);
					}
				}
				else
				{
					ItemPrice lstOrders = new ItemPrice();
					lstOrders.OfferPrice = "0.00";
					lstOrders.StandardPrice = "0.00";
					lst.Add(lstOrders);
				}
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return null;
			}

		}

		
		public JsonResult CalcItemTotal(string H_price, string L_price, string h_Qty, string l_Qty)
				{
			try
			{
				double hPrice = 0, LPrice = 0, hQty = 0, lQty = 0;
				try
				{
					hPrice = float.Parse(H_price);
					LPrice = float.Parse(L_price);
					hQty = int.Parse(h_Qty);
					lQty = int.Parse(l_Qty);
				}
				catch
				{

				}


				double dectotal = float.Parse(((hPrice * hQty) + (LPrice * lQty)).ToString()) * (1.00);

                dectotal = Math.Round(dectotal, 2);

				var total = dectotal.ToString("F2");

				var lst = new
				{
					total = total
				};

				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return null;
			}

		}

		public JsonResult GetItems( )
		{
			try
			{
				List<Items> lst = new List<Items>();
				DataTable dt = (DataTable)Session["dtItems"];

				using (var sw = new StringWriter())
				{
					using (var writer = XmlWriter.Create(sw))
					{
						writer.WriteStartDocument(true);
						writer.WriteStartElement("r");
						string[] arrName = new string[1];
						string[] arrVals = new string[1];

						foreach (DataRow dr in dt.Rows)
						{
							arrName[0] = "itmID";
							arrVals[0] = dr["itmID"].ToString();
							dm.createNode(arrVals, arrName, writer);
						}
						writer.WriteEndElement();
						writer.WriteEndDocument();
						writer.Close();
					}



					string[] arr = {  sw.ToString() };
					DataTable dtItems = dm.loadList("SelCusItems", "sp_Web_SalesOrder", Session["cusID"].ToString(), arr);
					foreach (DataRow dr in dtItems.Rows)
					{
						Items lstOrders = new Items();
						lstOrders.itmID = dr["itm_ID"].ToString();
						lstOrders.itmCode = dr["itm_Code"].ToString();
						lstOrders.itmName = dr["itm_Name"].ToString();
						lst.Add(lstOrders);
					}
				}
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		
		public JsonResult AddItem(string itmID, int? H_uom_ID = 0, int? H_Qty = 0, int? L_uom_ID = 0, int? L_Qty = 0, int? CallMode = 0, int? SeqNo = 0)
		{
			try
			{
				DataTable dt = (DataTable)Session["dtItems"];
				
				int x = dt.Rows.Count;
				if (CallMode == 0)
				{
					dt.Rows.Add(x + 1, itmID, "", "", "", H_uom_ID, H_Qty, L_uom_ID, "", L_Qty, 0, 0, 0, 0, 0, 0, 0);
				}
				else
				{
					for (int i = dt.Rows.Count - 1; i >= 0; i--)
					{
						DataRow dr = dt.Rows[i];
						if (dr["seq"].ToString() == SeqNo.ToString())
						{
							dr.Delete();
						}
					}
					dt.AcceptChanges();
					dt.Rows.Add(SeqNo, itmID, "", "", "", H_uom_ID, H_Qty, L_uom_ID, "", L_Qty, 0, 0, 0, 0, 0, 0, 0);
				}

				string[] arr = { dm.BuildXML(dt).ToString() };
				DataSet dtItems = dm.loadListDS("SelItemDetails", "sp_Web_SalesOrder", Session["cusID"].ToString(), arr);
				Session["dtItems"] = dtItems.Tables[0];
				Session["dtSummary"] = dtItems.Tables[1];

				var lst = new
				{
					res = "0"
				};
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				var lst = new
				{
					res = "1",
					err = ex.Message.ToString()
				};
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
		}


		


public JsonResult DeleteItem(string itmID)
	{
		try
		{
			DataTable dt = (DataTable)Session["dtItems"];

			for (int i = dt.Rows.Count - 1; i >= 0; i--)
			{
				DataRow dr = dt.Rows[i];
				if (dr["itmID"].ToString() == itmID.ToString())
				{
					dr.Delete();
				}
			}
			dt.AcceptChanges();

			string[] arr = { dm.BuildXML(dt).ToString() };
			DataSet dtItems = dm.loadListDS("SelItemDetails", "sp_Web_SalesOrder", Session["cusID"].ToString(), arr);
			Session["dtItems"] = dtItems.Tables[0];

			Session["dtSummary"] = dtItems.Tables[1];

			var lst = new
			{
				res = "0"
			};

			// Log successful deletion
			Trace.TraceInformation("Item deleted - itmID: {0}", itmID);

			return Json(lst, JsonRequestBehavior.AllowGet);
		}
		catch (Exception ex)
		{
			var lst = new
			{
				res = "1",
				err = ex.Message.ToString()
			};

			// Log the error
			Trace.TraceError("Error while deleting item - itmID: {0}, Error: {1}", itmID, ex);

			return Json(lst, JsonRequestBehavior.AllowGet);
		}
	}



	public ActionResult ShowList()
		{
			return PartialView("ListOrder");
		}

		
		public ActionResult ShowFreeGood(int? csID = 0)
		{
			return PartialView("FreeGood", csID);
		}

		
		public ActionResult GetOrdItems([DataSourceRequest] DataSourceRequest request)
		{
			

			//DataTable dt = dm.loadList("SelOrders", "sp_B2B_Orders", Session["CusID"].ToString() );

			DataTable dt = (DataTable)Session["dtItems"];

			List<Items> lst = new List<Items>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				Items lstOrders = new Items();

				lstOrders.SeqNo = dt.Rows[i]["seq"].ToString();
				lstOrders.itmID = dt.Rows[i]["itmID"].ToString();
				lstOrders.itmName = dt.Rows[i]["itmName"].ToString();
				lstOrders.itmCode = dt.Rows[i]["itmCode"].ToString();
				lstOrders.H_UOM = dt.Rows[i]["HUom"].ToString();
				lstOrders.H_Qty = dt.Rows[i]["HQty"].ToString();
				lstOrders.H_Price = dt.Rows[i]["HPrice"].ToString();
				lstOrders.L_UOM = dt.Rows[i]["LUom"].ToString();
				lstOrders.L_Qty = dt.Rows[i]["LQty"].ToString();
				lstOrders.L_Price = dt.Rows[i]["LPrice"].ToString();
				lstOrders.Total = dt.Rows[i]["TotalPrice"].ToString();
				lstOrders.HuomName = dt.Rows[i]["HUomName"].ToString();
				lstOrders.LuomName = dt.Rows[i]["LUomName"].ToString();
				lstOrders.SubTotal = dt.Rows[i]["SubTotal"].ToString();
				lstOrders.Discount = dt.Rows[i]["Discount"].ToString();
				lstOrders.VAT = dt.Rows[i]["VAT"].ToString();
				lstOrders.GrandTotal = dt.Rows[i]["GrandTotal"].ToString();
				lst.Add(lstOrders);
			}
			DataSourceResult result = lst.ToDataSourceResult(request, p => new Items
			{
				itmID = p.itmID,
				itmCode = p.itmCode,
				itmName = p.itmName,
				H_UOM = p.H_UOM,
				H_Qty = p.H_Qty,
				H_Price = p.H_Price,
				L_Price = p.L_Price,
				L_Qty = p.L_Qty,
				L_UOM = p.L_UOM,
				Total = p.Total,
				HuomName = p.HuomName,
				LuomName = p.LuomName,
				Discount = p.Discount,
				SubTotal = p.SubTotal,
				VAT = p.VAT,
				GrandTotal = p.GrandTotal,
				SeqNo = p.SeqNo
			});
			return Json(result, JsonRequestBehavior.AllowGet);
		}
		
		public JsonResult getCusCrTerms()
		{
			try
			{


				DataTable dt = dm.loadList("SelCusCrLimits", "sp_Web_SalesOrder", Session["cusID"].ToString());
				CusCrTerms lstOrders = new CusCrTerms();
				if (dt.Rows.Count > 0)
				{
					foreach (DataRow dr in dt.Rows)
					{
						lstOrders.TotalCr = dr["cus_TotalCreditLimit"].ToString();
						lstOrders.cusType = dr["cus_Type"].ToString();
						lstOrders.UsedCr = dr["cus_UsedCreditLimit"].ToString();
						lstOrders.crDays = dr["cus_CreditDays"].ToString();
						lstOrders.AvlCr = dr["cus_AvailableCreditLimit"].ToString();
					}
				}
				else
				{
					lstOrders.TotalCr = "";
					lstOrders.cusType = "";
					lstOrders.UsedCr = "";
					lstOrders.crDays = "";
					lstOrders.AvlCr = "";
				}
				return Json(lstOrders, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return null;
			}

		}
		
		public JsonResult getUOM(string itmID, string uomName)
		{
			try
			{
				string[] arr = { uomName == null ? "" : uomName };

				DataTable dt = dm.loadList("SelCusItemHUOM", "sp_Web_SalesOrder", itmID, arr);
				List<UOM> lst = new List<UOM>();

				foreach (DataRow dr in dt.Rows)
				{
					UOM lstOrders = new UOM();
					lstOrders.uomID = dr["uom_ID"].ToString();
					lstOrders.uomName = dr["uom_Name"].ToString();
					lstOrders.UPC = dr["pru_UPC"].ToString();
					lst.Add(lstOrders);
				}
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return null;
			}

		}
		
		public JsonResult getGridH_UOM(string itmID, string uomName)
		{
			try
			{
				string[] arr = { uomName == null ? "" : uomName };

				DataTable dt = dm.loadList("SelCusItemHUOM", "sp_Web_SalesOrder", itmID, arr);
				List<H_UOM> lst = new List<H_UOM>();

				foreach (DataRow dr in dt.Rows)
				{
					H_UOM lstOrders = new H_UOM();
					lstOrders.uomID = dr["uom_ID"].ToString();
					lstOrders.HuomName = dr["uom_Name"].ToString();
					lstOrders.UPC = dr["pru_UPC"].ToString();
					lst.Add(lstOrders);
				}
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return null;
			}

		}

		public JsonResult getSummary()
		{
			try
			{


				DataTable dt = (DataTable)Session["dtSummary"];

				ordSummary ord = new ordSummary();
				foreach (DataRow dr in dt.Rows)
				{
					ord.Total = dr["TotalPrice"].ToString();
					ord.Discount = dr["Discount"].ToString();
					ord.SubTotal = dr["SubTotal"].ToString();
					ord.VAT = dr["VAT"].ToString();
					ord.GrandTotal = dr["GrandTotal"].ToString();
					
				}

				Session["OrdSummary"] = ord;


				return Json(ord, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return null;
			}

		}

		public JsonResult ProceedOrder(OrderInput input)
		{
			
			try
			{
				ordSummary ordSummary = (ordSummary)Session["OrdSummary"];
				DataTable dtItems = (DataTable)Session["dtItems"];
				DataTable dt_fg_Items = (DataTable)Session["dt_fg_Items"];
				string x = Session["UserID"].ToString();
				string imagePath = "";

				try
				{
					if (Session["GridLPO"] != null)
					{
						imagePath = Session["GridLPO"].ToString();
						Session["GridLPO"] = null;
					}
					else
					{
						Session["GridLPO"] = null;
					}
					
				}
				catch (Exception ex)
				{
					Session["GridLPO"] = null;
				}



				string[] arr = { input.remarks == null ? "" : input.remarks,  Session["UserID"].ToString(), ordSummary.Total , ordSummary.Discount , ordSummary.SubTotal , ordSummary.VAT , ordSummary.GrandTotal , // Para8
                 input.expDelDate ,input.DelSlot == null ? "" : input.DelSlot, dm.BuildXML(dtItems).ToString() ,  dm.BuildXML(dt_fg_Items).ToString() ,
					input.LPO==null? "":input.LPO, imagePath}; //para14
				DataTable dt = dm.loadList("InsOrderByCus", "sp_Web_SalesOrder", Session["cusID"].ToString(), arr);

				if (dt.Rows.Count > 0)
				{
					if (dt.Rows[0]["Res"].ToString() == "1")
					{

						Session["cusID"] = input.cusID;
						Session["OrderID"] = dt.Rows[0]["Title"].ToString();
						Session["ordID"] = dt.Rows[0]["ordID"].ToString();
					}

					var lst = new
					{
						res = dt.Rows[0]["Res"].ToString(),
						Title = dt.Rows[0]["Title"].ToString(),
						Message = dt.Rows[0]["Descr"].ToString()
					};
					return Json(lst, JsonRequestBehavior.AllowGet);
				}
				else
				{
					var lst = new
					{
						res = -1,
						Title = "Technical Exception",
						Message = "Please try again later"
					};
					return Json(lst, JsonRequestBehavior.AllowGet);
				}

			}
			catch (Exception ex)
			{
				var lst = new
				{
					res = -1,
					Title = "Technical Exception",
					Message = ex.Message.ToString()
				};
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
		}

		
		public JsonResult getItemCount()
		{
			try
			{
				
				DataTable dt = (DataTable)Session["dtItems"];
				string[] arr = { dm.BuildXML(dt).ToString(), "0" };
				DataTable dtheader = dm.loadList("SelFreeGoodPromos", "sp_Web_SalesOrder", Session["cusID"].ToString(), arr);
				Session["prmID"] = "0";
				if (dtheader.Rows.Count > 0)
				{
					Session["prmID"] = dtheader.Rows[0]["prm_ID"].ToString();
				}

				Session["dt_fg_prms"] = dtheader;
				Session["CompletedPrmIDs"] = "";
				Session["dt_fg_Items"] = null;

				if (dt.Rows.Count > 0)
				{
					var lst = new
					{
						res = 1,
						PrmCount = dtheader.Rows.Count.ToString()
					};
					return Json(lst, JsonRequestBehavior.AllowGet);
				}
				else
				{
					var lst = new
					{
						res = 0,
						PrmCount = "0"
					};
					return Json(lst, JsonRequestBehavior.AllowGet);
				}
			}
			catch (Exception ex)
			{
				var lst = new
				{
					res = -1,
					PrmCount = "0"
				};
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
		}



		
		public JsonResult FindNextPromo(List<SelectedItems> values)
		{
			DataTable dt_fg_Items = (DataTable)Session["dt_fg_Items"];
			if (dt_fg_Items == null)
			{
				dt_fg_Items = new DataTable();
				dt_fg_Items.Columns.Add("itmID", typeof(string));
				dt_fg_Items.Columns.Add("Qty", typeof(string));
				dt_fg_Items.Columns.Add("prmID", typeof(string));
			}
			string prmID = Session["prmID"].ToString();
			foreach (SelectedItems itms in values)
			{
				dt_fg_Items.Rows.Add(itms.itmID, itms.Qty, prmID);
			}

			Session["dt_fg_Items"] = dt_fg_Items;

			Session["CompletedPrmIDs"] = Session["CompletedPrmIDs"] + prmID + "-";
			string[] AllprmIds = (string[])Session["prmIDArr"];
			string[] RemoveIds = (string[])Session["prmIDArr"];

			string compPrmIDs = Session["CompletedPrmIDs"].ToString();
			string[] cmpIDs = compPrmIDs.Split('-');

			for (int i = 0; i < AllprmIds.Length; i++)
			{
				for (int j = 0; j < cmpIDs.Length; j++)
				{
					if (AllprmIds[i].ToString() == cmpIDs[j].ToString())
					{
						RemoveIds = RemoveIds.Where(val => val != AllprmIds[i]).ToArray();
					}
				}
			}
			string nextEnable = "1";
			if (RemoveIds.Length <= 1)
			{
				nextEnable = "0";
			}
			string mode = "";
			if (RemoveIds.Length > 0)
			{
				Session["prmID"] = RemoveIds[0].ToString();
				mode = "P";
			}
			else
			{
				Session["CompletedPrmIDs"] = "";
				mode = "O";
			}


			var lst = new
			{
				mode = mode,
				nextEnable = nextEnable
			};

			return Json(lst, JsonRequestBehavior.AllowGet);
		}

		public void ProcessPDF( string OrderID, string ordID)
		{

			try
			{
				ProcessPDF pDF = new ProcessPDF();
				PDFDocument pDFDocument = new PDFDocument();
				
				PDFResponse pdfRes = new PDFResponse();
				for (int d = 0; d < 1; d++) //Its the number of orders.If there is only 1 order, then no need of this loop
				{
					string filePath = Server.MapPath("../Content/Templates/Order.docx"); //TEMPLATE PATH
					string[] arr = { OrderID };
					DataTable dtHeader = dm.loadList("SelCusOrderHeader", "sp_Web_SalesOrder", Session["cusID"].ToString(), arr);
					string[] replaceTexts = { "{cus#}", "{cusName}", "{TRN}", "{Address}", "{SiteName}", "{SiteCode}", "{OrderID}", "{OrderDate}" };
					string[,] headerColumns = new string[replaceTexts.Length, 2];

					int i = 0;
					foreach (DataColumn dc in dtHeader.Columns)
					{
						headerColumns[i, 0] = replaceTexts[i]; //Strings need to be replaced in the excel
						headerColumns[i, 1] = dc.ColumnName;
						i++;
					}

					DataSet dataSet = dm.loadListDS("SelCusOrderDetail", "sp_Web_SalesOrder", ordID, arr); //ALL THE TABLES NEED TO BE THERE IN PDF SHOULD BE RETRIEVED AS A DATASET

					string[] dtHeading = { "Order Details" }; //THIS IS THE HEADINGS FOR EACH TABLES

					List<TableParams> tp = new List<TableParams>(); //THIS IS THE CLASS INSIDE THE PDF PROCESS DLL

					string JsonPath = Server.MapPath("../pdfColumns.json"); //EACH PDF WILL HAVE DIFFERENT JSON FILES AND IT SHOULD BE IN THE CORRECT ORDER
					foreach (DataTable dt in dataSet.Tables)
					{
						TableParams jsonParse = new TableParams();
						jsonParse = pDF.LoadJson(JsonPath, "Columns"); // Second Parameter is the name of the Object inside the Json File
						tp.Add(jsonParse);
					}


					pdfRes = pDF.PDFCall(filePath, dtHeader, headerColumns, dataSet, dtHeading, tp, pDFDocument);
					pDFDocument.doc = pdfRes.pdfDoc; //Need to add the reference to the Telerik.Windows.documents.flow

				}



				Session["OrderPDF"] = pdfRes;


			}
			catch (Exception ex)
			{

			}

		}
		public JsonResult getPrevOrders(string ordID)
		{
			string cusID = Session["cusID"].ToString();
			try
			{
				string[] arr = { ordID == null ? "" : ordID };

				DataTable dt = dm.loadList("SelPrevOrders", "sp_Web_SalesOrder", cusID, arr);
				List<PrevOrders> lst = new List<PrevOrders>();

				foreach (DataRow dr in dt.Rows)
				{
					PrevOrders lstOrders = new PrevOrders();
					lstOrders.OrdNumber = dr["OrderID"].ToString();
					lstOrders.ordID = dr["ord_ID"].ToString();
					lstOrders.CreatedOn = dr["CreatedDate"].ToString();
					lst.Add(lstOrders);
				}
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public JsonResult updateRepeatOrder(string ordID)
		{
			//DataTable dtSessItems = (DataTable)Session["dtItems"];
			DataTable dtSessItems = new DataTable();

            if (dtSessItems.Rows.Count == 0)
			{
				DataTable dt = dm.loadList("SelRepeatOrderByID", "sp_Web_SalesOrder", ordID);

			
				string[] arr = { dm.BuildXML(dt).ToString() };

				DataSet dtItems = dm.loadListDS("SelItemDetails", "sp_Web_SalesOrder", Session["CusID"].ToString(), arr);
				Session["dtItems"] = dtItems.Tables[0];
				Session["dtSummary"] = dtItems.Tables[1];

				var lst = new
				{
					
					res = "0"

				};
				
			
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
			else
			{
				var lst = new
				{
					res = "1"
				};
				return Json(lst, JsonRequestBehavior.AllowGet);
			}
		}


		public ActionResult Async_Save(IEnumerable<HttpPostedFileBase> files)
		{
			// The Name of the Upload component is "files"
			if (files != null)
			{
				foreach (var file in files)
				{
					try
					{
						
						var fileName = Path.GetFileName(file.FileName);
						var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/GridOrderLPO"), fileName);
						ViewBag.physicalPath = physicalPath;
						Session["GridLPO"] = "../UploadFiles/GridOrderLPO" + fileName;
						// The files are not actually saved in this demo
						file.SaveAs(physicalPath);
					}
					catch(Exception ex)
					{

					}

					
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
					var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/GridOrderLPO"), fileName);

					// TODO: Verify user permissions

					if (System.IO.File.Exists(physicalPath))
					{
						// The files are not actually removed in this demo
						System.IO.File.Delete(physicalPath);
						Session["GridLPO"] = null;
					}
				}
			}

			// Return an empty string to signify success
			return Content("");
		}

		[HttpPost]
		public ActionResult ClearDtItems()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("seq", typeof(string));
            dt.Columns.Add("itmID", typeof(string));
            dt.Columns.Add("itmCode", typeof(string));
            dt.Columns.Add("itmName", typeof(string));
            dt.Columns.Add("HUomName", typeof(string));
            dt.Columns.Add("HUom", typeof(string));
            dt.Columns.Add("HQty", typeof(string));
            dt.Columns.Add("LUom", typeof(string));
            dt.Columns.Add("LUomName", typeof(string));
            dt.Columns.Add("LQty", typeof(string));
            dt.Columns.Add("HPrice", typeof(string));
            dt.Columns.Add("LPrice", typeof(string));
            dt.Columns.Add("TotalPrice", typeof(string));
            dt.Columns.Add("Discount", typeof(string));
            dt.Columns.Add("SubTotal", typeof(string));
            dt.Columns.Add("VAT", typeof(string));
            dt.Columns.Add("GrandTotal", typeof(string));
            Session["dtItems"] = dt;

            return Json(new { success = true });
		}



	}
}