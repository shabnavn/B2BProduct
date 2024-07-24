using b2b_solution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using System.Collections;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace b2b_solution.Controllers
{
    [CheckSession]
    public class HomeController : Controller
    {
        DataModel dm = new DataModel();
        AppController app = new AppController();
		public ActionResult Index()
		{
			try
			{
				if (Session["userID"] != null)
				{
					string[] arr = { Session["userID"].ToString() };
					DataTable dtBestSellPrd = dm.loadList("SelBestSellingProds", "sp_B2B_Home", Session["CusID"].ToString(), arr);
					ViewBag.Products = dtBestSellPrd;
					return View();
				}
				else
				{
					return RedirectToAction("Login", "Landing");
				}
			}
			catch (Exception ex)
			{
				return RedirectToAction("Login", "Landing");
			}
		}

		public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Cart()
        {
           
            ViewBag.Message = "Cart";
            Session["CmpPrmID"] = null;
            return View();
        }

        //[HttpGet]
        //public ActionResult Search()
        //{
        //    SearchComplete sr = new SearchComplete();
        //    return Search(sr);
        //}

        [HttpGet]
        public ActionResult Search(SearchComplete searchComplete)
        {
            Session["catID"] = null;
            Session["sctID"] = null;
            Session["SpecialPrice"] = null;
            Category category = new Category();
            string JSONString = string.Empty;
            //JSONString = JsonConvert.SerializeObject(dt);
            var x = Json(JSONString);
            string catID = searchComplete.catID == null ? "0" : searchComplete.catID;
            string brdID = searchComplete.brdID == null ? "0" : searchComplete.brdID;
            string itmName = searchComplete.prdName == null ? "" : searchComplete.prdName;

            string specialPrice = searchComplete.specialPrice == true ? "1" : "0";
            Session["CurrentPage"] =  searchComplete.PageNum;
           
            string[] arr = { Session["CusID"].ToString(), catID, brdID, specialPrice, itmName, searchComplete.PageNum };
            DataTable dtBestSellPrd = dm.loadList("SelSearchProducts", "sp_B2B_Home", Session["userID"].ToString(), arr);
            ViewBag.Products = dtBestSellPrd;
            return View(searchComplete);
        }

        public ActionResult GetSubCats(string catID)
        {
            DataTable dtUom = dm.loadList("SelSubCatByID", "sp_B2B_Home", catID);
            ViewBag.SubCats = dtUom;
            return PartialView("SubCatPatial");
        }

        [HttpGet]
        public ActionResult GetLowerUOM(string id, string itmValues)
        {
            //string id = "1"; string itmValues = "5";
            UOMModel uOM = new UOMModel();
            string[] arr = { id };
            DataTable dtUom = dm.loadList("SelItemLowerUOM", "sp_B2B_Home", itmValues, arr);
            if (dtUom.Rows.Count > 0)
            {
                List<UOMModel> transaction_Lists = new List<UOMModel>();
                foreach (DataRow drUom in dtUom.Rows as IEnumerable)
                {
                    transaction_Lists.Add(new UOMModel { Text = drUom["uom_Name"].ToString(), Value = drUom["uom_ID"].ToString() });
                }
                var uomLow = new UomList();
                uomLow.ListUomModel = transaction_Lists;
                string[] arrItems = { Session["userID"].ToString() , id , dtUom.Rows[0]["uom_ID"].ToString() };
                DataTable dtBestSellPrd = dm.loadList("SelBestSellingProdByID", "sp_B2B_Home",  Session["CusID"].ToString(), arrItems);
                List<ItemRow> RowItems = new List<ItemRow>();
                string c = dtBestSellPrd.Rows[0]["crd_lowerUOM"].ToString();
                string d = dtBestSellPrd.Rows[0]["OfferPrice"].ToString();
                string e = dtBestSellPrd.Rows[0]["standardPrice"].ToString();
                string f = dtBestSellPrd.Rows[0]["crd_LowerQty"].ToString();
                RowItems.Add(new ItemRow
                {
                
                    id = id,
                    LowerUOM = dtBestSellPrd.Rows[0]["crd_lowerUOM"].ToString(),
                    LowerUOMOfferPrice = dtBestSellPrd.Rows[0]["OfferPrice"].ToString(),
                    LowerUOMStandardPrice = dtBestSellPrd.Rows[0]["standardPrice"].ToString(),
                    lowerQty = dtBestSellPrd.Rows[0]["crd_LowerQty"].ToString()
                }); ;

                var LowRow = new ItemList();
                LowRow.Rowitem = RowItems;


                return PartialView("LowerUOM", new ProductList { LowItem = LowRow, LowUOM = uomLow });
            }
            else
            {
                return null;
            }
         
           
        }

        [HttpGet]
        public ActionResult GetQLowerUOM(string id, string itmValues)
        {
            //string id = "1"; string itmValues = "5";
            UOMModel uOM = new UOMModel();
            string[] arr = { id };
            DataTable dtUom = dm.loadList("SelItemLowerUOM", "sp_B2B_Home", itmValues, arr);
            if (dtUom.Rows.Count > 0)
            {
                List<UOMModel> transaction_Lists = new List<UOMModel>();
                foreach (DataRow drUom in dtUom.Rows as IEnumerable)
                {
                    transaction_Lists.Add(new UOMModel { Text = drUom["uom_Name"].ToString(), Value = drUom["uom_ID"].ToString() });
                }
                var uomLow = new UomList();
                uomLow.ListUomModel = transaction_Lists;
                string[] arrItems = { Session["userID"].ToString(), id, dtUom.Rows[0]["uom_ID"].ToString() };
                DataTable dtBestSellPrd = dm.loadList("SelBestSellingProdByID", "sp_B2B_Home",  Session["CusID"].ToString(), arrItems);
                List<ItemRow> RowItems = new List<ItemRow>();
                string c = dtBestSellPrd.Rows[0]["crd_lowerUOM"].ToString();
                string d = dtBestSellPrd.Rows[0]["OfferPrice"].ToString();
                string e = dtBestSellPrd.Rows[0]["standardPrice"].ToString();
                string f = dtBestSellPrd.Rows[0]["crd_LowerQty"].ToString();
                RowItems.Add(new ItemRow
                {

                    id = id,
                    LowerUOM = dtBestSellPrd.Rows[0]["crd_lowerUOM"].ToString(),
                    LowerUOMOfferPrice = dtBestSellPrd.Rows[0]["OfferPrice"].ToString(),
                    LowerUOMStandardPrice = dtBestSellPrd.Rows[0]["standardPrice"].ToString(),
                    lowerQty = dtBestSellPrd.Rows[0]["crd_LowerQty"].ToString()
                }); ;

                var LowRow = new ItemList();
                LowRow.Rowitem = RowItems;

                return PartialView("../Promotion/QLowerUOM", new ProductList { LowItem = LowRow, LowUOM = uomLow });
            }
            else
            {
                return null;
            }
        }


        [HttpGet]
        public ActionResult GetALowerUOM(string id, string itmValues)
        {
            //string id = "1"; string itmValues = "5";
            UOMModel uOM = new UOMModel();
            string[] arr = { id };
            DataTable dtUom = dm.loadList("SelItemLowerUOM", "sp_B2B_Home", itmValues, arr);
            if (dtUom.Rows.Count > 0)
            {
                List<UOMModel> transaction_Lists = new List<UOMModel>();
                foreach (DataRow drUom in dtUom.Rows as IEnumerable)
                {
                    transaction_Lists.Add(new UOMModel { Text = drUom["uom_Name"].ToString(), Value = drUom["uom_ID"].ToString() });
                }
                var uomLow = new UomList();
                uomLow.ListUomModel = transaction_Lists;
                string[] arrItems = { Session["userID"].ToString(), id, dtUom.Rows[0]["uom_ID"].ToString() };
                DataTable dtBestSellPrd = dm.loadList("SelBestSellingProdByID", "sp_B2B_Home",  Session["CusID"].ToString(), arrItems);
                List<ItemRow> RowItems = new List<ItemRow>();
                string c = dtBestSellPrd.Rows[0]["crd_lowerUOM"].ToString();
                string d = dtBestSellPrd.Rows[0]["OfferPrice"].ToString();
                string e = dtBestSellPrd.Rows[0]["standardPrice"].ToString();
                string f = dtBestSellPrd.Rows[0]["crd_LowerQty"].ToString();
                RowItems.Add(new ItemRow
                {

                    id = id,
                    LowerUOM = dtBestSellPrd.Rows[0]["crd_lowerUOM"].ToString(),
                    LowerUOMOfferPrice = dtBestSellPrd.Rows[0]["OfferPrice"].ToString(),
                    LowerUOMStandardPrice = dtBestSellPrd.Rows[0]["standardPrice"].ToString(),
                    lowerQty = dtBestSellPrd.Rows[0]["crd_LowerQty"].ToString()
                }); ;

                var LowRow = new ItemList();
                LowRow.Rowitem = RowItems;


                return PartialView("../Promotion/ALowerUOM", new ProductList { LowItem = LowRow, LowUOM = uomLow });
            }
            else
            {
                return null;
            }


        }

        public ActionResult GetCartLowerUOM(string id, string itmValues)
        {
            //string id = "1"; string itmValues = "5";
            UOMModel uOM = new UOMModel();
            string[] arr = { id };
            DataTable dtUom = dm.loadList("SelItemLowerUOM", "sp_B2B_Home", itmValues, arr);
            if (dtUom.Rows.Count > 0)
            {
                List<UOMModel> transaction_Lists = new List<UOMModel>();
                foreach (DataRow drUom in dtUom.Rows as IEnumerable)
                {
                    transaction_Lists.Add(new UOMModel { Text = drUom["uom_Name"].ToString(), Value = drUom["uom_ID"].ToString() });
                }
                var uomLow = new UomList();
                uomLow.ListUomModel = transaction_Lists;
                string[] arrItems = { Session["userID"].ToString(), id };
                DataTable dtBestSellPrd = dm.loadList("SelBestSellingProdByID", "sp_B2B_Home",  Session["CusID"].ToString(), arrItems);
                List<ItemRow> RowItems = new List<ItemRow>();
                RowItems.Add(new ItemRow
                {
                    id = id,
                    LowerUOM = dtBestSellPrd.Rows[0]["crd_lowerUOM"].ToString(),
                    LowerUOMOfferPrice = dtBestSellPrd.Rows[0]["OfferPrice"].ToString(),
                    LowerUOMStandardPrice = dtBestSellPrd.Rows[0]["standardPrice"].ToString(),
                    lowerQty = dtBestSellPrd.Rows[0]["crd_LowerQty"].ToString()
                });

                var LowRow = new ItemList();
                LowRow.Rowitem = RowItems;


                return PartialView("LowerUOM", new ProductList { LowItem = LowRow, LowUOM = uomLow });
            }
            else
            {
                return null;
            }


        }


        [HttpGet]
        public ActionResult GetItemUOMPrice(string id, string itmValues)
        {
            string[] arr = { id ,  Session["CusID"].ToString() };
            DataTable dtItemPrice = dm.loadList("SelItemUomPrice", "sp_B2B_Home", itmValues, arr);


            ItemUomPrice itemUomPrice = new ItemUomPrice();
            string x = dtItemPrice.Rows[0]["standardPrice"].ToString();
            string y = dtItemPrice.Rows[0]["OfferPrice"].ToString();
            if (dtItemPrice.Rows.Count > 0)
            {
                itemUomPrice.StandardPrice = dtItemPrice.Rows[0]["standardPrice"].ToString();
                itemUomPrice.OfferPrice = dtItemPrice.Rows[0]["OfferPrice"].ToString();
            }
          
            return Json(itemUomPrice, JsonRequestBehavior.AllowGet);
        }
       
        [HttpGet]
        public ActionResult GetItemCartSubTotal(string id)
        {
            string[] arr = { Session["UserID"].ToString() , Session["CusID"].ToString() };
            DataTable dtItemPrice = dm.loadList("SelCartItemsByID", "sp_B2B_Home", id, arr);
            ItemSubTotal itemSubTotal = new ItemSubTotal();
            if (dtItemPrice.Rows.Count > 0)
            {
                itemSubTotal.GrandTotal = dtItemPrice.Rows[0]["Total"].ToString();
				itemSubTotal.VAT = dtItemPrice.Rows[0]["VAT"].ToString();
				itemSubTotal.Total_W_D = dtItemPrice.Rows[0]["SubTotal"].ToString();
			}

            return Json(itemSubTotal, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public void UpdateHUOMCart(string id, string itmValues)
        {
            string[] arr = { id, itmValues , Session["CusID"].ToString() };
            DataTable dtItemPrice = dm.loadList("UpdateCartHUOM", "sp_B2B_Home", Session["userID"].ToString(), arr);
        }

        public void UpdateLUOMCart(string id, string itmValues)
        {
            string[] arr = { id, itmValues , Session["CusID"].ToString() };
            DataTable dtItemPrice = dm.loadList("UpdateCartLUOM", "sp_B2B_Home", Session["userID"].ToString(), arr);
        }

        [HttpGet]
        public ActionResult InsCart(CartProps cartProps)
        {

            if (cartProps.HigherQty != null)
            {
                string cus = Session["CusID"].ToString();
                string[] arr = {  Session["CusID"].ToString(), cartProps.id, cartProps.HigherQty, cartProps.HigherUOM, cartProps.LowerQty, cartProps.LowerUOM };
                DataTable dtItemPrice = dm.loadList("InsCart", "sp_B2B_Home", Session["userID"].ToString(), arr);
            }

            return Json("Success", JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult SubCats(string id)
        {
            
            DataTable dtUom = dm.loadList("SelSubCatByID", "sp_B2B_Home", id);
            if (dtUom.Rows.Count > 0)
            {
                List<SubCats> transaction_Lists = new List<SubCats>();
                foreach (DataRow drUom in dtUom.Rows as IEnumerable)
                {
                    transaction_Lists.Add(new SubCats { name = drUom["sct_Name"].ToString(), id = drUom["sct_ID"].ToString() });
                }
                var uomLow = new SubCatList();
                uomLow.SCLists = transaction_Lists;

                SearchComplete search = new SearchComplete();
                search.catID = id;
                return PartialView("SubCatPatial", new SearchSubCats { SubCategories = uomLow, SearchRes = search });
            }
            else
            {
                return null;
            }


        }

        public ActionResult GetCart([DataSourceRequest] DataSourceRequest request)
        {
            string cus = Session["CusID"].ToString();
            string[] arr = { Session["CusID"].ToString() };
            DataTable dt = dm.loadList("SelCartItems" , "sp_B2B_Home", Session["userID"].ToString(),arr);

            List<ListCart> lst = new List<ListCart>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ListCart lstCart  = new ListCart();
                lstCart.itemID = dt.Rows[i]["itemID"].ToString();
                lstCart.ItemImage = dt.Rows[i]["itm_Image"].ToString();
                lstCart.itemName = dt.Rows[i]["itm_Name"].ToString();
                lstCart.HigherUOM = Int32.Parse( dt.Rows[i]["crd_HigherUOM"].ToString());
                lst.Add(lstCart);
            }
            DataSourceResult result = lst.ToDataSourceResult(request, p => new Models.ListCart
            {
                itemID = p.itemID,
                itemName = p.itemName,
                HigherUOM = p.HigherUOM,
                ItemImage = p.ItemImage

            });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelCartItem(string id)
        {
            string[] arr = { Session["userID"].ToString() , Session["CusID"].ToString() };
            DataTable dt = dm.loadList("DeleteCartItem", "sp_B2B_Home", id, arr);
            return RedirectToAction("Cart", "Home");
        }


        public ActionResult GetCartItemLowerUOM(string id, string itmValues)
        {
            //string id = "1"; string itmValues = "5";
            UOMModel uOM = new UOMModel();
            string[] arr = { id };
            DataTable dtUom = dm.loadList("SelItemLowerUOM", "sp_B2B_Home", itmValues, arr);
            if (dtUom.Rows.Count > 0)
            {
                List<UOMModel> transaction_Lists = new List<UOMModel>();
                foreach (DataRow drUom in dtUom.Rows as IEnumerable)
                {
                    transaction_Lists.Add(new UOMModel { Text = drUom["uom_Name"].ToString(), Value = drUom["uom_ID"].ToString() });
                }
                var uomLow = new UomList();
                uomLow.ListUomModel = transaction_Lists;
                string[] arrItems = { Session["userID"].ToString(), Session["CusID"].ToString() };
                DataTable dtBestSellPrd = dm.loadList("SelCartItemsByID", "sp_B2B_Home", id, arrItems);
                string uom ="0", price = "0";
                if (dtUom.Rows.Count > 0)
                {
                    if (Decimal.Parse( dtBestSellPrd.Rows[0]["LowerPrice"].ToString()) == 0)
                    {
                        string[] uomArr = { id,  Session["CusID"].ToString() };
                        DataTable dtItemPrice = dm.loadList("SelItemUomPrice", "sp_B2B_Home", dtUom.Rows[0]["uom_ID"].ToString(), uomArr);
                        if (dtItemPrice.Rows.Count > 0)
                        {
                            price = dtItemPrice.Rows[0]["OfferPrice"].ToString();
                            uom = dtUom.Rows[0]["uom_ID"].ToString();
                        }
                      
                    }else
                    {
                        price = dtBestSellPrd.Rows[0]["LowerPrice"].ToString();
                        uom = dtBestSellPrd.Rows[0]["crd_lowerUOM"].ToString();
                    }
                }
                List<ItemRow> RowItems = new List<ItemRow>();
                RowItems.Add(new ItemRow
                {
                    id = id,
                    LowerUOM = uom,
                    LowerUOMOfferPrice = price,
                    lowerQty = dtBestSellPrd.Rows[0]["crd_LowerQty"].ToString()
                });

                var LowRow = new ItemList();
                LowRow.Rowitem = RowItems;


                return PartialView("CartLowerUOM", new ProductList { LowItem = LowRow, LowUOM = uomLow });
            }
            else
            {
                return null;
            }


        }

        public ActionResult GetCartTotal()
        {
            string[] arr = {  Session["CusID"].ToString() };
            DataTable dtItemPrice = dm.loadList("SelCartTotal", "sp_B2B_Home", Session["UserID"].ToString() , arr);
            ItemSubTotal itemSubTotal = new ItemSubTotal();
            if (dtItemPrice.Rows.Count > 0)
            {
                itemSubTotal.Total_WO_D = dtItemPrice.Rows[0]["SubTotal_WO_D"].ToString();
                itemSubTotal.TotalDisc = dtItemPrice.Rows[0]["TotalDisc"].ToString();
                itemSubTotal.Total_W_D = dtItemPrice.Rows[0]["SubTotal_W_D"].ToString();
                itemSubTotal.VAT = dtItemPrice.Rows[0]["TotalVAT"].ToString();
                itemSubTotal.GrandTotal = dtItemPrice.Rows[0]["GrandTotal"].ToString();
                itemSubTotal.TotalCredit = dtItemPrice.Rows[0]["TotalCreditLimit"].ToString();
                itemSubTotal.AvailableCredit = dtItemPrice.Rows[0]["TotalAvailableLimit"].ToString();
                itemSubTotal.MinOrderValue = dtItemPrice.Rows[0]["MinOrderValue"].ToString();
                itemSubTotal.CreditDayStatus = dtItemPrice.Rows[0]["CreaditDays"].ToString();
            }
            return Json(itemSubTotal, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetCurrencyCode()
        {
            string[] arr = { Session["UserID"].ToString() };
            DataTable dtcurrency = dm.loadList("SelCurrencyCode", "sp_B2B_Home", Session["CusID"].ToString(), arr);
            Currency CurCode = new Currency();
            if (dtcurrency.Rows.Count > 0)
            {
                CurCode.CurrencyCode = dtcurrency.Rows[0]["cur_Code"].ToString();
                Session["Currency"] = dtcurrency.Rows[0]["cur_Code"].ToString();
            }
            return Json(CurCode, JsonRequestBehavior.AllowGet);
        }

        public void SetFilterSession(string value, string mode)
        {
            switch (mode)
            {
                case "CAT":
                    Session["catID"] = value == "" ? "0" : value;
                    Session["sctID"] = null;
                    break;
                case "SUBCAT":
                    Session["sctID"] = value == "" ? "0" : value; // Special Price
                    break;
                case "SP":
                    Session["SpecialPrice"] = value == "true" ? "1" : "0";
                    break ;
                }
        }
        public ActionResult GetSearchItems(string catID , string brdID , string itmName )
        {
            string cus = Session["CusID"].ToString();
            string[] arr = { Session["userID"].ToString(), catID, brdID, "0", "" , "1" };
            DataTable dtBestSellPrd = dm.loadList("SelSearchProducts", "sp_B2B_Home",  Session["CusID"].ToString(), arr);
            ViewBag.Products = dtBestSellPrd;
            var lst = new
            {
                res = "1"
            };
            return PartialView("Products");
        }

        public ActionResult BindFilteredItems()
        {
            return PartialView("Products");
        }

        public ActionResult CartCount()
        {
            string cus = Session["CusID"].ToString();
            string[] arr = { Session["CusID"].ToString()};
            DataTable dtCartCount = dm.loadList("SelCartCount", "sp_B2B_Home", Session["userID"].ToString(),arr);
            HandleError handle = new HandleError();
            if (dtCartCount.Rows.Count > 0)
            {
                handle.message = dtCartCount.Rows[0][0].ToString();
                ViewBag.CartCount = handle.message;
            }
            else
            {
                handle.message = "0";
                ViewBag.CartCount = handle.message;
            }
            return Json(handle , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ScrollSearch(SearchComplete searchComplete )
        {
            Session["catID"] = null;
            Session["sctID"] = null;
            Session["SpecialPrice"] = null;
            Category category = new Category();
            string JSONString = string.Empty;
            //JSONString = JsonConvert.SerializeObject(dt);
            var x = Json(JSONString);
            string catID = searchComplete.catID == null ? "0" : searchComplete.catID;
            string sct_ID = searchComplete.sctID == null ? "0" : searchComplete.sctID;
            string itmName = searchComplete.prdName == null ? "" : searchComplete.prdName;

            string specialPrice = searchComplete.specialPrice == true ? "1" : "0";

            DataTable dtBrands = dm.loadList("SelAllBrands", "sp_B2B_Home");
            List<Brands> listBrands = new List<Brands>();
            foreach (DataRow drBrands in dtBrands.Rows)
            {
                listBrands.Add(new Brands { brdname = drBrands["brd_Name"].ToString(), brdid = drBrands["brd_ID"].ToString(), remembercheck = false });
            }
            ViewBag.SearchBrands = searchComplete;

            var uomLow = new SearchBrandsList();
            uomLow.BrandsIDs = listBrands;
            searchComplete.brdIDs = uomLow;

            string brds = "itm_brd_ID";
            SearchBrandsList brandsList = searchComplete.brdIDs;
            if (brandsList != null)
            {
                foreach (var brand in brandsList.BrandsIDs)
                {
                    if (brand.remembercheck)
                    {
                        brds += brand.brdid + ",";
                    }
                }
            }

            string[] arr = { Session["userID"].ToString(), catID, sct_ID, specialPrice, itmName, searchComplete.PageNum };
            DataTable dtBestSellPrd = dm.loadList("SelSearchProducts", "sp_B2B_Home",  Session["CusID"].ToString(), arr);
            ViewBag.Products = dtBestSellPrd;
            return PartialView("Products");
            
        }

        public JsonResult GetCategories()
        {
            try
            {
                DataTable dt = dm.loadList("SelCats", "sp_B2B_Home");

                List<SelectListItem> list = new List<SelectListItem>();
                foreach (DataRow dr in dt.Rows)
                {
                    list.Add(new SelectListItem { Text = dr["Name"].ToString(), Value = dr["id"].ToString() });
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public ActionResult Brands()
        {
            return View();
        }

        public JsonResult GetBrands(string catID, string brdName)
        {
            try
            {
                string[] arr = { brdName };
                DataTable dtBrands = dm.loadList("SelCatBrands", "sp_B2B_Home" , catID , arr);
              
                List<SelectListItem> list = new List<SelectListItem>();
                foreach (DataRow dr in dtBrands.Rows)
                {
                    list.Add(new SelectListItem { Text = dr["brd_Name"].ToString(), Value = dr["brd_ID"].ToString() });
                }
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public JsonResult GetProdNames(string text)
        {
            string[] arr = { text };
            DataTable dt = dm.loadList("SelAutoComplete", "sp_B2B_Home",  Session["CusID"].ToString(), arr);

            List<AutoSearch> lst = new List<AutoSearch>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                AutoSearch lstOrders = new AutoSearch();
                lstOrders.itmName = dt.Rows[i]["itm_Name"].ToString();
                lst.Add(lstOrders);
            }
           
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Terms()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Privacy()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
		public JsonResult RemoveAllCart()
		{
			string[] arr = { Session["UserID"].ToString() };
			DataTable dt = dm.loadList("RemoveAllCart", "sp_B2B_Home",  Session["CusID"].ToString(), arr);

			List<AutoSearch> lst = new List<AutoSearch>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				AutoSearch lstOrders = new AutoSearch();
				lstOrders.itmName = dt.Rows[i]["Title"].ToString();
				lst.Add(lstOrders);
			}

			return Json(lst, JsonRequestBehavior.AllowGet);
		}
		
	}
}