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
using Org.BouncyCastle.Ocsp;
using ProcessExcel;

namespace b2b_solution.Controllers
{
    [CheckSession]
    public class CustomerItemController : Controller
    {
        
        DataModel dm = new DataModel();

        public ActionResult PromotionHeader(SearchComplete allOrders)
        {
            return View(allOrders);
        }
        public ActionResult SpecialPriceDetail(SearchComplete allOrders)
        {
            return View(allOrders);
        }
        public ActionResult ItemList(SearchComplete allOrders)
        {
            return View(allOrders);
        }
        public JsonResult GetPromotion([DataSourceRequest] DataSourceRequest request, OrderList allOrders)
        {

            DataTable dt = dm.loadList("GetPromotionHeader", "sp_B2B_Promotions", Session["CusID"].ToString());
            List<PromotionGrid> lst = new List<PromotionGrid>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PromotionGrid lstOrders = new PromotionGrid();
                lstOrders.id= dt.Rows[i]["prm_ID"].ToString();
                lstOrders.Code = dt.Rows[i]["prm_Number"].ToString();
                lstOrders.Name = dt.Rows[i]["prm_Name"].ToString();
                lstOrders.Type = dt.Rows[i]["prt_Name"].ToString();
                lstOrders.Qualification = dt.Rows[i]["qlh_Name"].ToString();
                lstOrders.Assignment = dt.Rows[i]["ash_Name"].ToString();
                lstOrders.rcpId= dt.Rows[i]["rcp_ID"].ToString();
                lst.Add(lstOrders);

            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.PromotionGrid
            {
                id=p.id,
                Code = p.Code,
                Name = p.Name,
                Type = p.Type,
                Qualification = p.Qualification,
                Assignment = p.Assignment,
                rcpId=p.rcpId
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PromotionDetail(string id,string rcpId)
        {
            string[] arr = { Session["CusID"].ToString(), rcpId };
            DataTable dtHeader = dm.loadList("GetPromotionByID", "sp_B2B_Promotions", id,arr);
            ViewBag.HeaderOrder = dtHeader;

            DataTable dtRange = dm.loadList("GetPromotionRange", "sp_B2B_Promotions", id);
            Session["PromotionRange"] = dtRange;

            DataTable dtAssignment = dm.loadList("GetAssignment", "sp_B2B_Promotions", id);
            Session["Assignment"] = dtAssignment;

            DataTable dtQualification = dm.loadList("GetQualification", "sp_B2B_Promotions", id);
            Session["Qualification"] = dtQualification;



            return View();
        }
        public JsonResult GetPromotionRange([DataSourceRequest] DataSourceRequest request, OrderList allOrders)
        {

            DataTable dt = (DataTable)Session["PromotionRange"];
            List<PromotionRange> lst = new List<PromotionRange>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                PromotionRange lstOrders = new PromotionRange();

                lstOrders.MinVal = dt.Rows[i]["prr_MinValue"].ToString();
                lstOrders.MaxVal = dt.Rows[i]["prr_MaxValue"].ToString();
                lstOrders.Val = dt.Rows[i]["prr_Value"].ToString();
                lst.Add(lstOrders);

            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.PromotionRange
            {

                MinVal = p.MinVal,
                MaxVal = p.MaxVal,
                Val = p.Val
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQualification([DataSourceRequest] DataSourceRequest request, OrderList allOrders)
        {

            DataTable dt = (DataTable)Session["Qualification"];
            List<Qitems> lst = new List<Qitems>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Qitems lstOrders = new Qitems();

                lstOrders.Code = dt.Rows[i]["itm_Code"].ToString();
                lstOrders.itm_Name = dt.Rows[i]["itm_Name"].ToString();
                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.Qitems
            {
                Code = p.Code,
                itm_Name = p.itm_Name
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAssignment([DataSourceRequest] DataSourceRequest request, OrderList allOrders)
        {

            DataTable dt = (DataTable)Session["Assignment"];
            List<Aitems> lst = new List<Aitems>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Aitems lstOrders = new Aitems();

                lstOrders.Code = dt.Rows[i]["itm_Code"].ToString();
                lstOrders.itm_Name = dt.Rows[i]["itm_Name"].ToString();
                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.Aitems
            {
                Code = p.Code,
                itm_Name = p.itm_Name
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        


        public JsonResult GetSpecialPriceDetail([DataSourceRequest] DataSourceRequest request, OrderList allOrders)
        {

            DataTable dt = dm.loadList("GetSpecialPriceDetail", "sp_User", Session["CusID"].ToString());
            Session["SpecialPrice"] = dt;
            List<SpecialPriceDetailGrid> lst = new List<SpecialPriceDetailGrid>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SpecialPriceDetailGrid lstOrders = new SpecialPriceDetailGrid();

                lstOrders.Code = dt.Rows[i]["Code"].ToString();
                lstOrders.itm_Name = dt.Rows[i]["Name"].ToString();
                lstOrders.cat_Code = dt.Rows[i]["CategoryCode"].ToString();
                lstOrders.category = dt.Rows[i]["Category"].ToString();
                lstOrders.brand = dt.Rows[i]["Brand"].ToString();
                lstOrders.uom_Name = dt.Rows[i]["UOM"].ToString();
                lstOrders.OfferPrice = dt.Rows[i]["OfferPrice"].ToString();
                lstOrders.standardPrice = dt.Rows[i]["StandardPrice"].ToString();
                lstOrders.ReturnPrice = dt.Rows[i]["ReturnPrice"].ToString();
                lstOrders.VAT = dt.Rows[i]["VAT"].ToString();
                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.SpecialPriceDetailGrid
            {
                Code = p.Code,
                itm_Name = p.itm_Name,
                cat_Code = p.cat_Code,
                category = p.category,
                brand = p.brand,
                uom_Name = p.uom_Name,
                OfferPrice = p.OfferPrice,
                standardPrice = p.standardPrice,
                ReturnPrice = p.ReturnPrice,
                VAT = p.VAT
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemList([DataSourceRequest] DataSourceRequest request, OrderList allOrders)
        {

            DataTable dt = dm.loadList("GetItemList", "sp_User", Session["CusID"].ToString());
            Session["ItemList"] = dt;
            List<ItemListGrid> lst = new List<ItemListGrid>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ItemListGrid lstOrders = new ItemListGrid();

                lstOrders.Code = dt.Rows[i]["ItemCode"].ToString();
                lstOrders.itm_Name = dt.Rows[i]["Name"].ToString();
                lstOrders.category = dt.Rows[i]["Category"].ToString();
                lstOrders.uom_Name = dt.Rows[i]["UOM"].ToString();
                lstOrders.standardPrice = dt.Rows[i]["Std_Price"].ToString();
                lst.Add(lstOrders);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.ItemListGrid
            {
                Code = p.Code,
                itm_Name = p.itm_Name,
                category = p.category,
                uom_Name = p.uom_Name,
                OfferPrice = p.OfferPrice,
                standardPrice = p.standardPrice
            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DownloadOSIItemList()
        {
            DataTable dt = (DataTable)Session["ItemList"];


            BuildExcel excel = new BuildExcel();
            byte[] output = excel.SpreadSheetProcess(dt, "ItemList");

            return File(output, "application/excel", "ItemList" + DateTime.Now.ToString("yyyy-MMM-dd") + ".xlsx");

        }
        public ActionResult DownloadOSIItemListSP()
        {
            DataTable dt = (DataTable)Session["SpecialPrice"];


            BuildExcel excel = new BuildExcel();
            byte[] output = excel.SpreadSheetProcess(dt, "SpecialPrice");

            return File(output, "application/excel", "SpecialPrice" + DateTime.Now.ToString("yyyy-MMM-dd") + ".xlsx");

        }
    }
}