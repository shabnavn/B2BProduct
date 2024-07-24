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
using System.Web.Razor.Generator;
using Kendo.Mvc.UI.Html;
using Microsoft.AspNetCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace b2b_solution.Controllers
{

    [CheckSession]
    public class OrderTemplateController : Controller
	{
		// GET: OrderTemplate
		DataModel dm = new DataModel();


		public ActionResult OrderTemplate()
		{

			return View();
		}
		public ActionResult AddTemplate()
		{
            DataTable dt = new DataTable();
            dt.Columns.Add("seq", typeof(string));
            dt.Columns.Add("itmID", typeof(string));
            dt.Columns.Add("itmName", typeof(string));
            dt.Columns.Add("HUomName", typeof(string));
            dt.Columns.Add("HUom", typeof(string));
            dt.Columns.Add("HQty", typeof(string));
            dt.Columns.Add("LUomName", typeof(string));
            dt.Columns.Add("LUom", typeof(string));
            dt.Columns.Add("LQty", typeof(string));
            Session["dtItems"] = dt;

            return View();
		}

        public ActionResult ViewTemplate(string id)
        {

            string x = Session["CusID"].ToString();
            DataTable dtHeader = dm.loadList("SelTemplatebyID", "sp_OrderTemplate", id);
            ViewBag.HeaderTemplate = dtHeader;

            DataTable dt = dm.loadList("SelTemplateDetailbyID", "sp_OrderTemplate", id);
            ViewBag.CurrentTemplate = dt;
            Session["CurrentTemplate"] = dt;

            DataTable dtSession = dm.loadList("SelSessionItems", "sp_OrderTemplate", id);
            Session["dtItems"] = dtSession;


            return View();
        }

        public JsonResult GetUom(string itmID, string uomName)
		{

            try
            {
                string[] arr = { uomName == null ? "" : uomName };

                DataTable dt = dm.loadList("SelCusItemHUOM", "sp_Web_SalesOrder", itmID, arr);
                List<OrderTemplateUOM> lst = new List<OrderTemplateUOM>();

                foreach (DataRow dr in dt.Rows)
                {
                    OrderTemplateUOM lstOrders = new OrderTemplateUOM();
                    lstOrders.uomid = dr["uom_ID"].ToString();
                    lstOrders.uomname = dr["uom_Name"].ToString();
                    lst.Add(lstOrders);
                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public JsonResult GetLUom(string itmID, string uomName, string H_uom_ID)
        {

            try
            {

                string[] arr = { uomName == null ? "" : uomName };

                DataTable dt = dm.loadList("SelCusItemLUOM", "sp_Web_SalesOrder", itmID, arr);
                List<OrderTemplateUOM> lst = new List<OrderTemplateUOM>();

                foreach (DataRow dr in dt.Rows)
                {
                    OrderTemplateUOM lstOrders = new OrderTemplateUOM();
                    lstOrders.uomid = dr["uom_ID"].ToString();
                    lstOrders.uomname = dr["uom_Name"].ToString();
                    lst.Add(lstOrders);
                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public JsonResult GetItems()
        {
            try
            {
                List<TemplateItems> lst = new List<TemplateItems>();
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



                    string[] arr = { sw.ToString() };
                    DataTable dtItems = dm.loadList("SelCusItems", "sp_Web_SalesOrder", Session["cusID"].ToString(), arr);
                    foreach (DataRow dr in dtItems.Rows)
                    {
                        TemplateItems lstOrders = new TemplateItems();
                        lstOrders.itmid = dr["itm_ID"].ToString();
                        lstOrders.itmcode = dr["itm_Code"].ToString();
                        lstOrders.itmname = dr["itm_Name"].ToString();
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
        public JsonResult InsItem(TemplateItems itm)
		{
			HandleError handleError = new HandleError();

			try
			{
                string lqty, luom;

                if (itm.luomid == null)
                {
                    luom = "";
                    lqty = "";
                }
                else
                {
                    luom = itm.luomid;
                    lqty = itm.itmlqty;

                }

                string[] arr = { itm.itmid,  itm.itmhqty, itm.huomid, lqty.ToString(), luom.ToString()  };
                //otd_oth_ID, otd_itm_ID, otd_HQty, otd_HUOM,otd_LQty, otd_LUOM, CreatedDate, CreatedBy
                DataTable dt = dm.loadList("InsItem", "sp_OrderTemplate", itm.ID, arr);
				if (dt.Rows.Count > 0)
				{
					handleError.mode = Int32.Parse(dt.Rows[0]["res"].ToString());
				}
				else
				{
					handleError.mode = -2;
				}
			}
			catch (Exception ex)
			{
				handleError.mode = -2;
			}


			return Json(handleError, JsonRequestBehavior.AllowGet);
		}

		public JsonResult InsNewTemplate(OrderTemplate temp)
		{
			HandleError handleError = new HandleError();
			string user = Session["UserID"].ToString();
			string[] arr = { Session["CusID"].ToString(), temp.TempName };
			DataTable dt = dm.loadList("InsTemphead", "sp_OrderTemplate", user, arr);
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
					else if (resID < 0)
					{
						handleError.mode = -2;
						handleError.message = "failure";
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

			return Json(handleError, JsonRequestBehavior.AllowGet);
		}

		
		public JsonResult InsNewTempItem(string tempname, List<GridItems> items)
		{
			HandleError handleError = new HandleError();

			try
			{
				string user = Session["UserID"].ToString();
				string cusID = Session["CusID"].ToString();
				string[] arr1 = { cusID, tempname };

				DataTable dtt = dm.loadList("InsNewTemp", "sp_OrderTemplate", user, arr1);
				if (dtt.Rows.Count > 0)

				{
					int resID = Int32.Parse(dtt.Rows[0]["Res"].ToString());
                    int oth_id = Int32.Parse(dtt.Rows[0]["oth_ID"].ToString());

                    if (resID > 0)
					{
						foreach (var item in items)
						{
                            string lqty, luom;

                            if (item.luomid == null)
                            {
                                luom = "";
                                lqty = "";
                            }
                            else
                            {
                                luom = item.luomid;
                                lqty = item.lqty;

                            }
                            string[] arr2 = { oth_id.ToString(), item.itemid, item.hqty,item.huomid,lqty.ToString(),luom.ToString() };
							DataTable dt = dm.loadList("InsNewTempItem", "sp_OrderTemplate", user, arr2);

							if (dt.Rows.Count > 0)
							{
								handleError.mode = Int32.Parse(dt.Rows[0]["res"].ToString());
							}
							else
							{
								handleError.mode = -2;
							}
						}
					}
					else
					{
						handleError.mode = Int32.Parse(dtt.Rows[0]["res"].ToString());

					}
				}
			}
			catch (Exception ex)
			{
				handleError.mode = -2;
				
			}

			return Json(handleError, JsonRequestBehavior.AllowGet);
		}

        public ActionResult GetOrdItems([DataSourceRequest] DataSourceRequest request)
        {


            //DataTable dt = dm.loadList("SelOrders", "sp_B2B_Orders", Session["CusID"].ToString() ,  mode );

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

        public JsonResult GetOrderTemplate([DataSourceRequest] DataSourceRequest request)
		{

			DataTable dt = dm.loadList("SelTemplates", "sp_OrderTemplate", Session["CusID"].ToString());

			List<OrderTemplate> lst = new List<OrderTemplate>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				OrderTemplate lsttemp = new OrderTemplate();
				lsttemp.ID = dt.Rows[i]["oth_ID"].ToString();
				lsttemp.TempCode = dt.Rows[i]["oth_templateCode"].ToString();
				lsttemp.TempName = dt.Rows[i]["oth_templateName"].ToString();

				lst.Add(lsttemp);
			}
			DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.OrderTemplate
			{
				ID = p.ID,
				TempCode = p.TempCode,
				TempName = p.TempName

			});
			return Json(result, JsonRequestBehavior.AllowGet);
		}
	
		public ActionResult GetTemplateDetail([DataSourceRequest] DataSourceRequest request)
		{

			DataTable dt = (DataTable)Session["CurrentTemplate"];

			List<TemplateItems> lst = new List<TemplateItems>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				TemplateItems lsttemp = new TemplateItems();
				lsttemp.ID = dt.Rows[i]["otd_ID"].ToString();
				lsttemp.itmid = dt.Rows[i]["otd_itm_ID"].ToString();
				lsttemp.itmname = dt.Rows[i]["itm_Name"].ToString();
				lsttemp.itmhqty = dt.Rows[i]["otd_HQty"].ToString();
				lsttemp.itmhuom = dt.Rows[i]["HUOM"].ToString();
                lsttemp.itmlqty = (dt.Rows[i]["otd_LQty"].ToString() == "0") ? "" : dt.Rows[i]["otd_LQty"].ToString();
                lsttemp.itmluom = dt.Rows[i]["LUOM"].ToString();
                lsttemp.luomid = dt.Rows[i]["luomid"].ToString();
                lsttemp.huomid = dt.Rows[i]["huomid"].ToString();

                lst.Add(lsttemp);
			}
			DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.TemplateItems
			{
				ID = p.ID,
                itmid=p.itmid,
				itmcode = p.itmcode,
				itmname = p.itmname,
				itmhqty = p.itmhqty,
				itmhuom = p.itmhuom,
                itmlqty = p.itmlqty,
                itmluom = p.itmluom,
                huomid= p.huomid,
                luomid=p.luomid



            });
			return Json(result, JsonRequestBehavior.AllowGet);

		}

		public JsonResult GenerateOrder(TemplateItems itm)
		{

			HandleError handleError = new HandleError();
			string user = Session["UserID"].ToString();
			string[] arr = { Session["CusID"].ToString() };
			DataTable dt = dm.loadList("InsCart", "sp_OrderTemplate", user, arr);

			try
			{
				if (dt.Rows.Count > 0)
				{
					int lrw = dt.Rows.Count;
					string dtHeader = dt.Rows[lrw - 1]["crt_ID"].ToString();
					DataTable dtbl = (DataTable)Session["CurrentTemplate"];
					for (int i = 0; i < dtbl.Rows.Count; i++)
					{

						string[] paras = { dtbl.Rows[i]["otd_itm_id"].ToString(), dtbl.Rows[i]["otd_HQty"].ToString(), dtbl.Rows[i]["otd_LQty"].ToString(), dtbl.Rows[i]["huomid"].ToString(),  dtbl.Rows[i]["luomid"].ToString() };
                        //I.itm_Name,otd_ID,otd_itm_ID,itm_Name,otd_HQty,otd_LQty,HU.uom_Name As HUOM,LU.uom_Name As LUOM,HU.uom_ID as huomid,LU.uom_ID as luomid
                        DataTable dtt = dm.loadList("InsCartDetail", "sp_OrderTemplate", dtHeader, paras);
						if (dtt.Rows.Count > 0)
						{
							int resID = Int32.Parse(dtt.Rows[0]["Res"].ToString());
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
				}
				else
				{
					handleError.mode = 0;
					handleError.message = "failure";
				}


			}
			catch (Exception e)
			{
				handleError.mode = 0;
				handleError.message = "failure";
			}

			return Json(handleError, JsonRequestBehavior.AllowGet);

		}
    
        public JsonResult AddItem(string itmID, int? H_uom_ID = 0, int? H_Qty = 0, int? L_uom_ID = 0, int? L_Qty = 0, int? CallMode = 0, int? SeqNo = 0)
        {
            try
            {
                DataTable dt = (DataTable)Session["dtItems"];

                int x = dt.Rows.Count;
                if (CallMode == 0)
                {
                    dt.Rows.Add(x + 1, itmID,"", "", H_uom_ID, H_Qty, "", L_uom_ID, L_Qty);
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
                    dt.Rows.Add(SeqNo, itmID, "", "", H_uom_ID, H_Qty, L_uom_ID, "", L_Qty);
                }

                string[] arr = { dm.BuildXML(dt).ToString() };
                DataSet dtItems = dm.loadListDS("SelItemDetails", "sp_Web_SalesOrder", Session["cusID"].ToString(), arr);
                Session["dtItems"] = dtItems.Tables[0];

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
        public JsonResult DeleteItemFromDetail(string itmID,string ID)
        {
            HandleError handleError = new HandleError();
            try
            {
                string[] arr = { ID.ToString() };
                DataTable dt = dm.loadList("DeleteItem", "sp_OrderTemplate", itmID, arr);

                if (dt.Rows.Count > 0)
                {
                    handleError.mode = Int32.Parse(dt.Rows[0]["res"].ToString());
                }
                else
                {
                    handleError.mode = -2;
                }
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

            return Json(handleError, JsonRequestBehavior.AllowGet);

        }
        public JsonResult DeleteTemplate( string ID)
        {
            HandleError handleError = new HandleError();
            try
            {
                
                DataTable dt = dm.loadList("DeleteTemplate", "sp_OrderTemplate", ID.ToString() );

                if (dt.Rows.Count > 0)
                {
                    DataTable dtDetail = dm.loadList("DeleteTemplateDetail", "sp_OrderTemplate", ID.ToString());
                    if (dtDetail.Rows.Count > 0)
                    {
                        handleError.mode = Int32.Parse(dt.Rows[0]["res"].ToString());

                    }
                    else
                    {
                        handleError.mode = -2;

                    }
                }
                else
                {
                    handleError.mode = -2;
                }
            }
            catch (Exception ex)
            {
                var lst = new
                {
                    res = "1",
                    err = ex.Message.ToString()
                };

                // Log the error
                Trace.TraceError("Error while deleting item - itmID: {0}, Error: {1}", ID, ex);
                return Json(lst, JsonRequestBehavior.AllowGet);
            }

            return Json(handleError, JsonRequestBehavior.AllowGet);

        }


        
    }
}