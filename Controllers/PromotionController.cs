using b2b_solution.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace b2b_solution.Controllers
{
    [CheckSession]
    public class PromotionController : Controller
    {
        DataModel dm = new DataModel();
        // GET: Promotion
        public ActionResult Detail(string id)
        {
            ViewBag.PrmID = id;
            return View();
        }

        public ActionResult AmountDetail(string id)
        {
            ViewBag.PrmID = id;
            return View();
        }

        public ActionResult AllPromotions()
        {
           
            return View();
        }

        public ActionResult GetTotalCount(string prmID)
        {
            HandleError handle = new HandleError();
            string[] arr = { Session["userID"].ToString(), prmID };
            DataTable dtCartCount = dm.loadList("SelTotalPcs", "sp_B2B_Promotions", Session["CusID"].ToString() , arr);
            if (dtCartCount.Rows.Count > 0)
            {
                handle.message = dtCartCount.Rows[0]["TotalPcs"].ToString();
                handle.desc = dtCartCount.Rows[0]["ElgQty"].ToString();
                Session["prrVal"] = handle.desc;
               
            }
            else
            {
                handle.message = "0";
                handle.desc = "0";
                Session["prrVal"] = handle.desc;
            }
            return Json(handle, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTotalAssgCount(string prmID)
        {
            HandleError handle = new HandleError();
            string[] arr = { Session["userID"].ToString(), prmID };
            DataTable dtCartCount = dm.loadList("SelAssgTotCount", "sp_B2B_Promotions", Session["CusID"].ToString(), arr);
            if (dtCartCount.Rows.Count > 0)
            {
                handle.message = dtCartCount.Rows[0]["TotalPcs"].ToString();
                Session["totAsgCnt"] = handle.message;
            }
            else
            {
                handle.message = "0";
                Session["totAsgCnt"] = handle.message;
            }
            return Json(handle, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult InsPromoCart(PromoCart cartProps)
        {
            string res = "";
            if (cartProps.HigherQty != null)
            { 
                string[] arr = {  cartProps.id, cartProps.ashID , cartProps.prmID ,  cartProps.HigherQty, cartProps.HigherUOM, cartProps.LowerUOM ,  
                    cartProps.LowerQty , Session["prrVal"].ToString() , Session["CusID"].ToString()  };
                DataTable dtItemPrice = dm.loadList("InsPromoCart", "sp_B2B_Promotions", Session["userID"].ToString(), arr);
                if (dtItemPrice.Rows.Count > 0)
                {
                    res = dtItemPrice.Rows[0]["Res"].ToString();
                }
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public void UpdateLUOMPromoCart(PromoCart cartProps)
        {
            string[] arr = { Session["userID"].ToString()   , cartProps.LowerUOM, cartProps.prmID , cartProps.ashID };
            DataTable dtItemPrice = dm.loadList("UpdatePromoCartLUOM", "sp_B2B_Promotions", cartProps.id, arr);
        }

        [HttpGet]
        public ActionResult SelRangeQty(string prmID, string qty)
        {
            RangeRes handle = new RangeRes(); 
            string res = "";
            string[] arr = { qty };
            DataTable dtItemPrice = dm.loadList("SelRangeByQty", "sp_B2B_Promotions", prmID, arr);
            if (dtItemPrice.Rows.Count > 0)
            {
                handle.prrID = dtItemPrice.Rows[0]["prr_ID"].ToString();
                handle.Count = dtItemPrice.Rows[0]["prr_Value"].ToString();
                Session["prrVal"] = handle.Count;
            }
            else
            {
                handle.prrID = "0";
                handle.Count = "0";
                Session["prrVal"] = handle.Count;
            }

            return Json(handle, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CartPromo()
        {
            if (Session["CmpPrmID"] == null)
            {
                Session["CmpPrmID"] = "0";
            }

            if (Session["CmpPrmID"].ToString().Equals("-1"))
            {
                return View("../Order/Checkout");
            }
            else
            {
                string[] arr = { Session["userID"].ToString(),  Session["CmpPrmID"].ToString() };
                DataTable dtFG_Promo = dm.loadList("SelActFG_Promos", "sp_B2B_Promotions", Session["CusID"].ToString() , arr);
                if (dtFG_Promo.Rows.Count > 0)
                {
                    ViewBag.prmID = dtFG_Promo.Rows[0]["prm_ID"].ToString();
                    Session["CmpPrmID"] += "," + ViewBag.prmID;
                    return View();
                }
                else
                {
                    Session["CmpPrmID"] = "-1";
                    return View("../Order/Checkout");
                }
            }
        }

        [HttpGet]
        public ActionResult DelPromoCart(string prmID, string ashID)
        {
            RangeRes handle = new RangeRes();
            string res = "";
            string[] arr = { prmID , ashID };
            DataTable dtItemPrice = dm.loadList("DelPromoCart", "sp_B2B_Promotions", Session["userID"].ToString() , arr);
            handle.prrID = "0";
            return Json(handle, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckItemDiscount(string prmID, string itmID)
        {
            RangeRes handle = new RangeRes();
            string res = "";
            string[] arr = { Session["userID"].ToString() , prmID, itmID };
            DataTable dtItemPrice = dm.loadList("SelAmountPromobyItemID", "sp_B2B_Promotions", Session["CusID"].ToString(), arr);
            if (dtItemPrice.Rows.Count  > 0)
            {
                handle.prrID = dtItemPrice.Rows[0]["prr_Value"].ToString();
            }
            
            return Json(handle, JsonRequestBehavior.AllowGet);


        }
    }
}