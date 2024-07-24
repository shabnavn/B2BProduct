using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActionResult = Microsoft.AspNetCore.Mvc.ActionResult;
using b2b_solution.Models;
using System.Data;
using Newtonsoft.Json;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using System.Web.Script.Serialization;
using System.IO;
using System.Xml;
using System.Web.Security;
using Microsoft.DotNet.PlatformAbstractions;
using System.Configuration;
using System.Windows.Input;

namespace b2b_solution.Controllers
{
    
    public class AppController : Controller
    {
        DataModel dm = new DataModel();
        ProcessJson pj = new ProcessJson();
        LandingController LC = new LandingController();
        // GET: App
        [HttpPost]
        public string  GetItems([FromForm] InParams inParams)
        {
            string catID = inParams.catID == null ? "0" : inParams.catID;
            string brd_ID = inParams.brd_ID == null ? "0" : inParams.brd_ID;
            string itmName = inParams.prdName == null ? "" : inParams.prdName;

            string specialPrice = inParams.specialPrice == true ? "1" : "0";

            string[] arr = { inParams.cusID, catID, brd_ID, specialPrice, itmName,inParams.PageNum };
            DataTable dtBestSellPrd = dm.loadList("SelSearchProducts", "sp_B2B_Home", inParams.userID, arr);

            string JSONString = string.Empty;
            try
            {
                if (dtBestSellPrd.Rows.Count > 0)
                {
                    JSONString = pj.ItemsJson(dtBestSellPrd , inParams);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string RecentSummary([FromForm] InParams inParams)
        {
           

            string[] arr = {  };
            DataTable RecentSum = dm.loadList("SelLastOrderSummary", "sp_B2B_Home", inParams.userID, arr);

            string JSONString = string.Empty;
            try
            {
                if (RecentSum.Rows.Count > 0)
                {
                    JSONString = pj.RecentSumJson(RecentSum, inParams);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string InsertCart([FromForm] cartParams inParams)
        {


            string[] arr = { inParams.cusID,inParams.itm_ID,inParams.HigherQty,inParams.HigherUOM,inParams.LowerQty,inParams.lowerUOM };
            DataTable InsCart = dm.loadList("InsCart", "sp_B2B_Home", inParams.userID, arr);

            string JSONString = string.Empty;
            try
            {
                if (InsCart.Rows.Count > 0)
                {
                    JSONString = pj.InsertCartJson(InsCart);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }


        public string InsertPromoCart([FromForm] PromocartParams inParams)
        {
            string JSONString = string.Empty;
            if (inParams.HigherQty != null)
            {
                string[] arr = { inParams.prdid, inParams.ashID, inParams.prmID,inParams.HigherQty, inParams.HigherUOM, inParams.LowerUOM,inParams.LowerQty,
            inParams.prrVal,inParams.cusID};
                DataTable InsCart = dm.loadList("InsPromoCart", "sp_B2B_Promotions", inParams.userID, arr);

                
                try
                {
                    if (InsCart.Rows.Count > 0)
                    {
                        JSONString = pj.InsPromoCartJson(InsCart);
                    }
                    else
                    {
                        JSONString = "NoDataRes";
                    }
                }
                catch (Exception ex)
                {
                    JSONString = "NoDataSQL";
                }
            }


            return JSONString;
        }

        public string SelAshId([FromForm] selAshid inParams)
        {



            DataTable CI = dm.loadList("SelPromoByID", "sp_B2B_Promotions", inParams.prmID);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelASHIDJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL" + ex.Message.ToString();
            }

            return JSONString;
        }


        public string UpdMessageResolve([FromForm] CUpdMsgParams inParams)
        {


            string[] arr = { inParams.userID};
            DataTable updmsg = dm.loadList("UpdMessageResolve", "sp_B2BApp", inParams.CspID, arr);

            string JSONString = string.Empty;
            try
            {
                if (updmsg.Rows.Count > 0)
                {
                    JSONString = pj.UpdMsgRslvJson(updmsg);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string InsEscalation([FromForm] CUpdMsgParams inParams)
        {


            string[] arr = { inParams.userID };
            DataTable updmsg = dm.loadList("InsEscalation", "sp_B2BApp", inParams.CspID, arr);

            string JSONString = string.Empty;
            try
            {
                if (updmsg.Rows.Count > 0)
                {
                    JSONString = pj.UpdMsgRslvJson(updmsg);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string InsSuppMessage([FromForm] InsSupMsgParams inParams)
        {
            string imagePath = "";

            var HttpReq = HttpContext.Request;

            try
            {
                var x = HttpReq.Files[0];
                var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/Support"), x.FileName);
                ViewBag.physicalPath = physicalPath;
                imagePath = "../UploadFiles/Support/" + x.FileName;
                // The files are not actually saved in this demo
                x.SaveAs(physicalPath);
            }
            catch (Exception ex)
            {

            }

            string[] arr = { inParams.message,imagePath, inParams.userID };
            DataTable updmsg = dm.loadList("InsSuppMessage", "sp_B2B_Support", inParams.CspID, arr);

            string JSONString = string.Empty;
            try
            {
                if (updmsg.Rows.Count > 0)
                {
                    JSONString = pj.UpdMsgRslvJson(updmsg);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string InsCusSupport([FromForm] InsCusSupParams inParams)
        {
            string imagePath = "";

            var HttpReq = HttpContext.Request;



            try
            {
                var x = HttpReq.Files[0];
                var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/Support"), x.FileName);
                ViewBag.physicalPath = physicalPath;
                imagePath = "../UploadFiles/Support/" + x.FileName;
                // The files are not actually saved in this demo
                x.SaveAs(physicalPath);
            }
            catch (Exception ex)
            {

            }


            string[] arr = { inParams.userID ,inParams.Title,inParams.message,inParams.reason, imagePath };
            DataTable updmsg = dm.loadList("InsCusSupport", "sp_B2B_Support", inParams.cus_ID, arr);

            string JSONString = string.Empty;
            try
            {
                if (updmsg.Rows.Count > 0)
                {
                    JSONString = pj.UpdMsgRslvJson(updmsg);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL"+ex.Message.ToString();
            }

            return JSONString;
        }

        public string InsNewAddress([FromForm] InsNewAddParams inParams)
        {
           
            
            string[] arr = {inParams.userID, inParams.BuldName, inParams.RoomNo, inParams.Street, inParams.LandMark, inParams.State };
            DataTable dt = dm.loadList("InsNewAddr", "sp_B2B_Profile", inParams.CusID.ToString(), arr);

            string JSONString = string.Empty;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    JSONString = pj.insAddJson(dt);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;





        }

        public string DelAddress([FromForm] DelAddParams inParams)
        {
            

            DataTable dt = dm.loadList("DelAddress", "sp_B2B_Profile",inParams.cuaID );

            string JSONString = string.Empty;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    JSONString = pj.insAddJson(dt);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;





        }





        public string RepOrder([FromForm] ReorderParams inParams)
        {


            string[] arr = { inParams.odd_ord_ID, inParams.cusID };
            DataTable RepeatOrder = dm.loadList("InsRepOrder", "sp_B2B_Orders", inParams.userID, arr);

            string JSONString = string.Empty;
            try
            {
                if (RepeatOrder.Rows.Count > 0)
                {
                    JSONString = pj.RepOrderJson(RepeatOrder);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string CartTotal([FromForm] CartTotalParams inParams)
        {


            string[] arr = {  inParams.cusID };
            DataTable CartTot = dm.loadList("SelCartTotal", "sp_B2B_Home", inParams.userID, arr);

            string JSONString = string.Empty;
            try
            {
                if (CartTot.Rows.Count > 0)
                {
                    JSONString = pj.CartTotalJson(CartTot);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string DelAllCart([FromForm] CartTotalParams inParams)
        {


            string[] arr = { inParams.userID };
            DataTable DelCart = dm.loadList("RemoveAllCart", "sp_B2B_Home", inParams.cusID, arr);

            string JSONString = string.Empty;
            try
            {
                if (DelCart.Rows.Count > 0)
                {
                    JSONString = pj.DelcatJson(DelCart);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string DeleteCartItem([FromForm] DeleteCartParams inParams)
        {


            string[] arr = { inParams.userID,inParams.cusID };
            DataTable DelCart = dm.loadList("DeleteCartItem", "sp_B2B_Home", inParams.itmID, arr);

            string JSONString = string.Empty;
            try
            {
                if (DelCart.Rows.Count > 0)
                {
                    JSONString = pj.DelCartJson(DelCart);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string CartCount([FromForm] CartCountParams inParams)
        {


            string cusId = inParams.cusID == null ? "0" : inParams.cusID;

            string[] arr = { cusId.ToString() };
            DataTable CC = dm.loadList("SelCartCount", "sp_B2B_Home", inParams.userID,arr);

            string JSONString = string.Empty;
            try
            {
                if (CC.Rows.Count > 0)
                {
                    JSONString = pj.CartCountJson(CC);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string SelCartItems([FromForm] CartTotalParams inParams)
        {
            string cusId = inParams.cusID == null ? "0" : inParams.cusID;

            string[] arr = { cusId.ToString() };
            DataTable CI = dm.loadList("SelCartItems", "sp_B2B_Home", inParams.userID,arr);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.CartItemsJson(CI, inParams);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string SelCustomerSupport([FromForm] ARInParams inParams)
        {


            string[] arr = { ""};
            DataTable CI = dm.loadList("SelCustomerSupport", "sp_B2B_Support", inParams.cusID,arr);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.CustomerSprtJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string SelSupportMsg([FromForm] SPMInParams inParams)
        {


            // string[] arr = { };
            DataTable CI = dm.loadList("SelSupportMessages", "sp_B2B_Support", inParams.CspID);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SprtMsgJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string SelCusSuppHeader([FromForm] SPMInParams inParams)
        {

            //string[] arr = { inParams.cusID };
            DataTable CI = dm.loadList("SelCusSuppHeader", "sp_B2B_Support", inParams.CspID);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.CusSupHdrJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL" ;
            }

            return JSONString;
        }

        public string SelBrand([FromForm] BrandInParams inParams)
        {

            string[] arr = { inParams.CatID };

            DataTable CI = dm.loadList("SelCatAllBrands", "sp_B2B_Home",inParams.cusID,arr);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelBrandJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string SelCategory()
        {


            
            DataTable CI = dm.loadList("SelCats", "sp_B2B_Home");

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelCatJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string Selbanners()
        {



            DataTable CI = dm.loadList("Selbanners", "sp_B2B_Home");

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelbannersJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string SelSubCategory([FromForm] SubCatParams inParams)
        {



            DataTable CI = dm.loadList("SelSubCategory", "sp_B2BApp", inParams.sct_cat_ID);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelSubCatJson(CI, inParams);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string RecentItems([FromForm] CartTotalParams inParams)
        {

            string[] arr = { inParams.userID};
            DataTable dtBestSellPrd = dm.loadList("SelBestSellingProds", "sp_B2B_Home", inParams.cusID, arr);

            string JSONString = string.Empty;
            try
            {
                if (dtBestSellPrd.Rows.Count > 0)
                {
                    JSONString = pj.RecentItemsJson(dtBestSellPrd, inParams);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string SelCountry()
        {



            DataTable SC = dm.loadList("SelStates", "sp_B2B_Home");

            string JSONString = string.Empty;
            try
            {
                if (SC.Rows.Count > 0)
                {
                    JSONString = pj.SelStatesjson(SC);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }


        public string PlaceOrder([FromForm] PlaceOrderInputs orderInputs)
        {
            string imagePath = "";
            string platform = "APP";

            var HttpReq = HttpContext.Request;

            

            try
            {
                var x = HttpReq.Files[0];
                var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/LPO"), x.FileName);
                ViewBag.physicalPath = physicalPath;
                imagePath = "../UploadFiles/LPO/" + x.FileName;
                // The files are not actually saved in this demo
                x.SaveAs(physicalPath);
            }
            catch (Exception ex)
            {

            }

            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw))
                {

                    writer.WriteStartDocument(true);
                    writer.WriteStartElement("r");
                    int c = 0;

                    string[] arr = { orderInputs.SubTotal, orderInputs.VAT, orderInputs.GrandTotal, orderInputs.ExpDate, orderInputs.Remarks, orderInputs.Emirate, orderInputs.BuildingName , orderInputs.RoomNo ,
                    orderInputs.Street, orderInputs.LandMark, orderInputs.LPO , imagePath,platform};
                    string[] arrName = { "SubTotal", "VAT", "GrandTotal", "DelDate", "Remarks", "Emirate", "BuildingName", "RoomNo", "Street", "LandMark", "LPO", "Attachment","Platform" };
                    createNode(arr, arrName, writer);

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                }

                string para3 = sw.ToString();

                string[] paras = { orderInputs.cusID, para3 };
                DataTable dt = dm.loadList("InsOrder", "sp_B2B_Orders",orderInputs.userID, paras);
                HandleError handleError = new HandleError();
                string JSONString = string.Empty;
                try
                {
                    if (dt.Rows.Count > 0)
                    {
                        JSONString = pj.InsertCartJson(dt);
                    }
                    else
                    {
                        JSONString = "NoDataRes";

                    }
                }
                catch (Exception ex)
                {
                    JSONString = "NoDataSQL";
                }

                return JSONString;
            }
            
        }
        private void createNode(string[] arr, string[] arrNames, XmlWriter writer)
        {
            writer.WriteStartElement("Values");
            for (int i = 0; i < arr.Length; i++)
            {
                writer.WriteStartElement(arrNames[i]);
                writer.WriteString(arr[i]);
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        public string SelAR([FromForm] ARInParams inParams)
        {



            string[] arr = { inParams.fromdate, inParams.todate };

            DataTable SelARH = dm.loadList("SelAr", "sp_B2B_Orders", inParams.cusID ,arr);

            string JSONString = string.Empty;
            try
            {
                if (SelARH.Rows.Count > 0)
                {
                    JSONString = pj.SelARJson(SelARH);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string SelARDetail([FromForm] ARDetailInParams inParams)
        {
            string[] arr = { };
            DataTable SelARD = dm.loadList("SelArDetail", "sp_B2B_Orders", inParams.arc_ID, arr);
            string JSONString = string.Empty;
            try
            {
                if (SelARD.Rows.Count > 0)
                {
                    JSONString = pj.SelARDetailJson(SelARD);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

       

        [HttpPost]
        public string Login([FromForm] Credential creds)
        {
            OutParams ot = new OutParams();
            LandingController LC = new LandingController();
            string JSONString = string.Empty;
            try
            {
               
                if (Membership.ValidateUser(creds.username, creds.password))
                {
                    //LOGIN SUCCESS
                    string[] arr = { creds.password };
                    DataTable dtUser = dm.loadList("AppLogin", "sp_User", creds.username,arr);
                   
                    JSONString = pj.SelLoginJson(dtUser);
                    LC.LoginAudit(dtUser.Rows[0]["userID"].ToString(), creds.username, "Success on B2B App");

                    
                }
                else
                {

                   
                    LC.LoginAudit("", creds.username, "invalid");
                   
                    JSONString = pj.Loginfail();


                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL" ;
            }
            return JSONString;
        }
        public string SelPromotions([FromForm] ARInParams inParams)
        {

            

            DataTable SelPromo = dm.loadList("SelPromotions", "sp_B2B_Promotions", inParams.cusID);

            string JSONString = string.Empty;
            try
            {
                if (SelPromo.Rows.Count > 0)
                {
                    JSONString = pj.SelPromoJson(SelPromo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string SelActFG_Promos([FromForm] FGParams inParams)
        {

            string[] arr = { inParams.userID, inParams.CmpPrmID };

            DataTable SelActFG = dm.loadList("SelActFG_Promos", "sp_B2B_Promotions", inParams.CusID, arr);

            string JSONString = string.Empty;
            try
            {
                if (SelActFG.Rows.Count > 0)
                {
                    JSONString = pj.SelActFGPromosJson(SelActFG);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string SelActPromos([FromForm] ActPromoParams inParams)
        {

            string[] arr = { inParams.userID };

            DataTable SelActPromo = dm.loadList("SelActPromos", "sp_B2B_Promotions", inParams.cusID, arr);

            string JSONString = string.Empty;
            try
            {
                if (SelActPromo.Rows.Count > 0)
                {
                    JSONString = pj.SelActPromosJson(SelActPromo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string SelRangeByQty([FromForm] SelRangeParams inParams)
        {

            string[] arr = { inParams.Range };

            DataTable SelRange = dm.loadList("SelRangeByQty", "sp_B2B_Promotions", inParams.PrmID, arr);

            string JSONString = string.Empty;
            try
            {
                if (SelRange.Rows.Count > 0)
                {
                    JSONString = pj.SelRangeByQtyJson(SelRange);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string SelfeaturedSubcats()
        {



            DataTable SelPromo = dm.loadList("SelSubCats", "sp_B2B_Home");

            string JSONString = string.Empty;
            try
            {
                if (SelPromo.Rows.Count > 0)
                {
                    JSONString = pj.SelfeaturedsubcatJson(SelPromo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string SelfeaturedCategory()
        {



            DataTable SelPromocat = dm.loadList("SelFeaturedCategories", "sp_B2B_Home");

            string JSONString = string.Empty;
            try
            {
                if (SelPromocat.Rows.Count > 0)
                {
                    JSONString = pj.SelfeaturedCategoryJson(SelPromocat);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string SelInvoice([FromForm] ARInParams inParams)
        {

            string[] arr = { inParams.fromdate, inParams.todate };

            DataTable SelInv = dm.loadList("SelInvoices", "sp_B2B_Orders", inParams.cusID,arr);

            string JSONString = string.Empty;
            try
            {
                if (SelInv.Rows.Count > 0)
                {
                    JSONString = pj.SelInvJson(SelInv);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string SelShipAddress([FromForm] CartCountParams inParams)
        {


            
            DataTable CI = dm.loadList("SelUserAddress", "sp_B2B_Profile", inParams.userID);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.ShipAddressJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL" + ex.Message.ToString();
            }

            return JSONString;
        }


        public string SelBillingAddress([FromForm] ARInParams inParams)
        {



            DataTable CI = dm.loadList("SelCus", "sp_B2B_Profile", inParams.cusID);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.BillAddressJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL" ;
            }

            return JSONString;
        }

        public string SelInvDetail([FromForm] InvDetParams inParams)
        {



            DataTable SelInvDet = dm.loadList("SelInvDetByID", "sp_B2B_Orders", inParams.InvID);

            string JSONString = string.Empty;
            try
            {
                if (SelInvDet.Rows.Count > 0)
                {
                    JSONString = pj.SelInvDetJson(SelInvDet);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string GetOrders([FromForm] GetOrderInParams inParams)
        {
            string[] arr = {inParams.FromDate,inParams.ToDate };
            DataTable SelORD = dm.loadList("SelOrders", "sp_B2B_Orders", inParams.cusID, arr);
            string JSONString = string.Empty;
            try
            {
                if (SelORD.Rows.Count > 0)
                {
                    JSONString = pj.GetOrderJson(SelORD);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string GetOrderDetail([FromForm] GetOrdDetailOutParams inParams)
        {
            string[] arr = { };
            DataTable SelORD = dm.loadList("SelOrderDetailbyID", "sp_B2B_Orders", inParams.OrdID);
            string JSONString = string.Empty;
            try
            {
                if (SelORD.Rows.Count > 0)
                {
                    JSONString = pj.GetOrderDetailJson(SelORD);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string SelPromoRange([FromForm] PromorangeIn inParams)
        {

            
            DataTable Prmrange = dm.loadList("SelPromoRange", "sp_B2B_Promotions", inParams.prmID);

            string JSONString = string.Empty;
            try
            {
                if (Prmrange.Rows.Count > 0)
                {
                    JSONString = pj.PromorangeJson(Prmrange);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string SelQualifiedGroup([FromForm] QualiGrpInParams inParams)
        {


            string[] arr = { inParams.userID, inParams.prmID};
            DataTable CI = dm.loadList("SelQualifiedItems", "sp_B2B_Promotions", inParams.cusID, arr);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.QualiGroupJson(CI, inParams);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string SelAssignedItem([FromForm] QualiGrpInParams inParams)
        {


            string[] arr = { inParams.userID, inParams.prmID };
            DataTable CI = dm.loadList("SelAssignedItems", "sp_B2B_Promotions", inParams.cusID, arr);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.AssignGroupJson(CI, inParams);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL"+ex.Message.ToString();
            }

            return JSONString;
        }
        public string OrderTracking([FromForm] GetOrdDetailOutParams inParams)
        {
            DataTable CI = dm.loadList("SelOrderLifeCycle", "sp_B2B_Orders", inParams.OrdID);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.ordtrackJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }


        public string PrmTotPcs([FromForm] PrmTotalInParams inParams)
        {
            string[] arr = { inParams.userID, inParams.prmID };
            DataTable CI = dm.loadList("SelTotalPcs", "sp_B2B_Promotions", inParams.cusID , arr);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelPrmTotalCount(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL" + ex.Message.ToString();
            }

            return JSONString;
        }

        public string PrmAssgnTot([FromForm] PrmTotalInParams inParams)
        {
            string[] arr = { inParams.userID, inParams.prmID };
            DataTable CI = dm.loadList("SelAssgTotCount", "sp_B2B_Promotions", inParams.cusID , arr);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelPrmAssgnQty(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string SelGenerateOtp([FromForm] Otp inParams)
        {
            LandingController LC = new LandingController();
            string JSONString = string.Empty;

            try
            {
                JSONString = pj.SelVerStatus(LC.GenerateOTP(inParams.userID).ToString());
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }
          
            return JSONString;

            
        }

        public string SelOtpProceed([FromForm] OtpProceed inParams)
        {


            string[] arr = { inParams.otp ,inParams.platform};
            DataTable CI = dm.loadList("UpdDailyVerification", "sp_User", inParams.userID,arr);
            string JSONString = string.Empty;

            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelProceed(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }

        public string SelAccountVerify([FromForm] verify inParams)
        {
            LandingController LC = new LandingController();          
            string JSONString = string.Empty;

            try
            {
                var result = LC.AccountSetup(inParams.email) as JsonResult;
                var ress = new JavaScriptSerializer().Serialize(result.Data);

                var dictionary = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(ress);
                var res = dictionary[dictionary.Keys.First()].ToString();
               

                if (res.Equals("S"))
                {
                    JSONString = pj.SelVerify("Y", "Verification e-mail has been sent, please check your mail");
                }
                else if(res.Equals("0"))
                {
                    JSONString = pj.SelVerify("N", "Couldnot find the account, please check the email");
                }
                else if(res.Equals("Y"))
                {
                    JSONString = pj.SelVerify("Y", "Already Verified, Kindly proceed to login");
                }
                else
                {
                    JSONString = pj.SelVerify("N", "We are facing some technical issues, please try again later");
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }

        public string ChangePassword([FromForm] changepass inParams)
        {
            ProfileController PC = new ProfileController();
            string JSONString = string.Empty;
            MembershipUser user;
            try
            {
                user = Membership.GetUser(inParams.uName);
                Boolean isSucess = user.ChangePassword(inParams.cPass, inParams.nPass);
                if (isSucess)
                {
                    JSONString = pj.SelchangePassStatus();
                }
                else
                {
                    JSONString = pj.SelchangePassfail();
                }

                
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;


        }
        public string SelCompanyBrochure()
        {
            DataTable CI = dm.loadList("CompanyBrochure", "sp_User");
            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelBrochure(CI);
                }
                else
                {
                    JSONString = "NoDataSQL";
                }
               
               
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;


        }
        public string SelOutStandingInvoices([FromForm] OutStandingInvoices inParams)
        {

          

            DataTable CI = dm.loadList("SelOutstandingInvoices", "sp_B2B_Orders", inParams.Cus_ID);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelOutStandingInv(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string SelExpDelDate([FromForm] ExpDelDate inParams)
        {



            DataTable CI = dm.loadList("SelExpDelDates", "sp_B2B_Orders", inParams.CusID);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelExpDelDateJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string Selcutoff([FromForm] cutoff inParams)
        {



            DataTable CI = dm.loadList("IsNullCustID", "sp_User", inParams.username);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelcutoffJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string SelReason()
        {



            DataTable CI = dm.loadList("SelReason", "sp_B2B_Home");

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelReasonJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string SelSearchItems([FromForm] Search inParams)
        {
            string itmName = inParams.text == null ? "" : inParams.text;
            string[] arr = { itmName };
            DataTable CI = dm.loadList("SelAutoComplete", "sp_B2B_Home", inParams.CusID,arr);

            string JSONString = string.Empty;
            try
            {
                if (CI.Rows.Count > 0)
                {
                    JSONString = pj.SelSearchItemsJson(CI);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }

        public string ActivateAccount([FromForm] changepass inParams)
        {
          
            string JSONString = string.Empty;
            MembershipUser user;
            try
            {
                user = Membership.GetUser(inParams.uName);
                
                Boolean isSucess = user.ChangePassword(inParams.cPass, inParams.nPass);
                if (isSucess)
                {
                    string[] arr = { "" };
                    string svd = dm.SaveData("sp_Masters", "UpdNewUserStatus", inParams.userID.ToString(), arr);
                    JSONString = pj.SelchangePassStatus();
                }
                else
                {
                    JSONString = pj.SelchangePassfail();
                }


            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;


        }
        public string SelOrderPDF([FromForm] OrdPdf inParams)
        {
            OrderController OR = new OrderController();
            string JSONString = string.Empty;

            try
            {
                var result = OR.GenerateOrderDetail(inParams.OrderID, inParams.OrderNum, inParams.cusID) as JsonResult;
                var ress = new JavaScriptSerializer().Serialize(result.Data);

                var dictionary = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(ress);
                var res = dictionary[dictionary.Keys.First()].ToString();

                JSONString = pj.SelOrderPDF(res.ToString());
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;


        }


        public string ForgotPassword([FromForm] forgotpass inParams)
        {
            LandingController LC = new LandingController();
            string JSONString = string.Empty;

            try
            {
                var result = LC.ResetPass(inParams.email) as JsonResult;
                var ress = new JavaScriptSerializer().Serialize(result.Data);

                var dictionary = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(ress);
                var res = dictionary[dictionary.Keys.First()].ToString();


                if (res.Equals("S"))
                {
                    JSONString = pj.SelForgotpass(res, "A Temporary password e-mail has been sent, please check your mail");
                }
                else if (res.Equals("0"))
                {
                    JSONString = pj.SelForgotpass(res, "Couldnot find the account, please check the email");
                }
                else
                {
                    JSONString = pj.SelForgotpass(res, "We are facing some technical issues, please try again later");
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        public string GetCustomerProfile([FromForm] CustomerProfileIn inParams)
        {
            
            string JSONString = string.Empty;
            string[] arr = { inParams.CusID };
            DataTable dtCusInfo = dm.loadList("SelCusProfiles", "sp_B2B_Profile", inParams.UserID,arr);
            

            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelCusProfile(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        public string GetCusSpecialPrice([FromForm] CustomerProfileIn inParams)
        {

            string JSONString = string.Empty;
            string[] arr = { inParams.CusID };
            DataTable dtCusInfo = dm.loadList("GetSpecialPriceDetail", "sp_User", inParams.CusID);


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelSpecialPriceDatail(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        public string GetUserList([FromForm] UserListIn inParams)
        {

            string JSONString = string.Empty;
            string[] arr = { inParams.CusID };
            DataTable dtCusInfo = dm.loadList("SelUserList", "sp_User", inParams.CusID);


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelUserList(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        public string GetDocType([FromForm] UserListIn inParams)
        {

            string JSONString = string.Empty;
            DataTable dtCusInfo = dm.loadList("GetDocType", "sp_B2B_CusDocument");


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelDocType(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        public string GetCusDocuments([FromForm] DocIn inParams)
        {

            string JSONString = string.Empty;
            DataTable dtCusInfo = dm.loadList("GetDetailsforApp", "sp_B2B_CusDocument",inParams.CusID);


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelDocuments(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        public string UploadDocument([FromForm] InsDoc inParams)
        {
            string imagePath = "";

            var HttpReq = HttpContext.Request;

            try
            {
                var x = HttpReq.Files[0];
                var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/Documents"), x.FileName);
                ViewBag.physicalPath = physicalPath;
                imagePath = "../UploadFiles/Documents/" + x.FileName;
                // The files are not actually saved in this demo
                x.SaveAs(physicalPath);
            }
            catch (Exception ex)
            {

            }

            string[] arr = {inParams.UploadedBy, inParams .TypeId, inParams.DocNo,inParams.ExpDate, imagePath, inParams.StartDate };
            DataTable dt = dm.loadList("InsCusDocfromApp", "sp_B2B_CusDocument", inParams.CusID, arr);

            string JSONString = string.Empty;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    JSONString = pj.InsDocOut(dt);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string UpdateDocument([FromForm] UpdateDoc inParams)
        {
            string imagePath = "";

            var HttpReq = HttpContext.Request;

            try
            {
                var x = HttpReq.Files[0];
                var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/Documents"), x.FileName);
                ViewBag.physicalPath = physicalPath;
                imagePath = "../UploadFiles/Documents/" + x.FileName;
                // The files are not actually saved in this demo
                x.SaveAs(physicalPath);
            }
            catch (Exception ex)
            {

            }

            string[] arr = { inParams.UploadedBy,inParams.DOcNo, inParams.ExpDate, imagePath, inParams.StartDate, inParams.Id };
            DataTable dt = dm.loadList("UpdateDoc", "sp_B2B_CusDocument", inParams.Id, arr);

            string JSONString = string.Empty;
            try
            {
                if (dt.Rows.Count > 0)
                {
                    JSONString = pj.UpdateCusDoc(dt);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;
        }
        public string RequestNewUser([FromForm] ReqUserIn crUser)
        {
            
            string JSONString = string.Empty;
            MembershipUser user = Membership.GetUser(crUser.email);

            if (user != null)
            {
                JSONString = "NoDataSQL";
            }
            else
            {
                string cusid, cshid;
                cshid = "0";
                cusid = "0";
                try
                {
                    DataTable dtHeader = new DataTable();
                    
                    if (crUser.type == "BR")
                    {

                        cshid = "0";
                        cusid = crUser.cusid.ToString();
                    }
                    else if (crUser.type == "HO")
                    {
                        
                        cshid = crUser.cshid.ToString();
                        cusid = "0";
                    }
                    

                    string password = ConfigurationManager.AppSettings.Get("defaultPass").ToString();
                    user = Membership.CreateUser(crUser.email, password);
                    string[] arr = { crUser.firstName, crUser.lastName, crUser.contactNum, crUser.email, crUser.createdby, cusid, crUser.roles, crUser.type, cshid };
                    DataTable dt = dm.loadList("InsUser", "sp_User", user.ProviderUserKey.ToString(), arr);

                    if (dt.Rows.Count > 0)
                    {
                        JSONString = pj.InsNewUser(dt);
                    }
                    else
                    {
                        JSONString = "NoDataRes";
                    }
                }
                catch (Exception ex)
                {
                    JSONString = "NoDataSQL";
                }
            }
            
            return JSONString;

        }
        public string GetCustomerListByHeader([FromForm] SelectCustomersIn inParams)
        {

            string JSONString = string.Empty;
            DataTable dtCusInfo = dm.loadList("GetCustomersByCshID", "sp_User", inParams.csh_ID);


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelCustomers(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        public string GetSwitchedCustomerInfo([FromForm] CustomerInfoIn inParams)
        {

            string JSONString = string.Empty;
            string[] arr = { inParams.CusID };

            DataTable dtCusInfo = dm.loadList("GetSwitchedCustomerInfo", "sp_User", inParams.UserID,arr);


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelSwitchedCustomerInfo(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        public string GetCustomerRoles([FromForm] UserListIn inParams)
        {

            string JSONString = string.Empty;
            DataTable dtCusInfo = dm.loadList("SelCusRoles", "sp_User");


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelCusRoles(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        public string GetCompanyInfo([FromForm] CompanyInfoIn inParams)
        {

            string JSONString = string.Empty;
            DataTable dtCusInfo = dm.loadList("SelComInfo", "sp_B2B_Home");


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelCompInfo(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }

        public string GetDisputeReqHeader([FromForm] DesputeIN inParams)
        {

            string JSONString = string.Empty;
            string[] arr = { inParams.CusID };
            DataTable dtCusInfo = dm.loadList("SelDisputeNote", "sp_B2B_MerchandisingServices", inParams.UserID, arr);


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelDisputeNoteRequestHeader(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        public string GetCreditNoteReqHeader([FromForm] DesputeIN inParams)
        {

            string JSONString = string.Empty;
            string[] arr = { inParams.CusID };
            DataTable dtCusInfo = dm.loadList("SelCreditNote", "sp_B2B_MerchandisingServices", inParams.UserID, arr);


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelCreditNoteRequestHeader(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        public string GetCusReqHeader([FromForm] DesputeIN inParams)
        {

            string JSONString = string.Empty;
            string[] arr = { inParams.CusID };
            DataTable dtCusInfo = dm.loadList("SelCustomerRequest", "sp_B2B_MerchandisingServices", inParams.UserID, arr);


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelCustomerRequestHeader(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        public string GetReturnReqHeader([FromForm] DesputeIN inParams)
        {

            string JSONString = string.Empty;
            string[] arr = { inParams.CusID };
            DataTable dtCusInfo = dm.loadList("GetReturnReq", "sp_B2B_MerchandisingServices", inParams.UserID, arr);


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelRetReqHeader(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }
        
        

        public string InsCreditNoteReq([FromForm] InsCRNHeader inputParams)
        {
            string JSONString = string.Empty;
            dm.TraceService("InsCreditNoteReq STARTED " + DateTime.Now.ToString());
            dm.TraceService("============================================");
            try
            {
                List<InsCRNDetail> itemData = JsonConvert.DeserializeObject<List<InsCRNDetail>>(inputParams.Detaildata);
                try
                {
                    string cusid = inputParams.cusid == null ? "PA" : inputParams.cusid;
                    string subtotal = inputParams.subtotal == null ? "0" : inputParams.subtotal;
                    string Amount = inputParams.Amount == null ? "0" : inputParams.Amount;
                    string usrid = inputParams.usrid == null ? "0" : inputParams.usrid;

                    string InputXml = "";
                    using (var sw = new StringWriter())
                    {
                        using (var writer = XmlWriter.Create(sw))
                        {

                            writer.WriteStartDocument(true);
                            writer.WriteStartElement("r");
                            int c = 0;
                            foreach (InsCRNDetail id in itemData)
                            {
                                string[] arr = { id.invid.ToString(), id.itmid.ToString(), id.huom.ToString(), id.hqty.ToString(), id.luom.ToString(), id.lqty.ToString(), id.amount.ToString(), id.rsnid.ToString() };
                                string[] arrName = { "InvoiceId", "ItemId", "HigherUOM", "HigherQty", "LowerUOM", "LowerQty", "Amount", "ReasonId" };
                                dm.createNode(arr, arrName, writer);
                            }

                            writer.WriteEndElement();
                            writer.WriteEndDocument();
                            writer.Close();
                        }
                        InputXml = sw.ToString();
                    }

                    try
                    {
                        string[] arr = { subtotal.ToString(), Amount.ToString(), usrid.ToString(), InputXml.ToString() };
                        DataTable dt = dm.loadList("InsCreditNoteRequest", "sp_B2B_MerchandisingServices", cusid.ToString(), arr);

                        try
                        {
                            if (dt.Rows.Count > 0)
                            {
                                JSONString = pj.InsertCNReq(dt);
                            }
                            else
                            {
                                JSONString = "NoDataRes";
                            }
                        }
                        catch (Exception ex)
                        {
                            JSONString = "NoDataSQL";
                        }
                    }
                    catch (Exception ex)
                    {
                        dm.TraceService(ex.Message.ToString());
                        JSONString = "NoDataSQL - " + ex.Message.ToString();
                    }

                }
                catch (Exception ex)
                {
                    dm.TraceService(ex.Message.ToString());
                    JSONString = "NoDataSQL - " + ex.Message.ToString();
                }
            }
            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "NoDataSQL - " + ex.Message.ToString();
            }
            dm.TraceService("InsCreditNoteReq ENDED " + DateTime.Now.ToString());
            dm.TraceService("========================================");
            return JSONString;
        }


        public string InsCustomerRequest([FromForm] InsCusReqParams inParams)
        {
            string imagePath = "";
            var HttpReq = HttpContext.Request;
            try
            {
                var x = HttpReq.Files[0];
                var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/CustomerRequest"), x.FileName);
                ViewBag.physicalPath = physicalPath;
                imagePath = "../UploadFiles/CustomerRequest/" + x.FileName;
                // The files are not actually saved in this demo
                x.SaveAs(physicalPath);
            }
            catch (Exception ex)
            {

            }
            string descr = inParams.descr == null ? "" : inParams.descr;
            string type = inParams.type == null ? "0" : inParams.type;
            string[] arr = { inParams.userID, descr, type, imagePath };
            DataTable updmsg = dm.loadList("InsNewRequest", "sp_B2B_MerchandisingServices", inParams.cus_ID, arr);

            string JSONString = string.Empty;
            try
            {
                if (updmsg.Rows.Count > 0)
                {
                    JSONString = pj.InsertCustRequest(updmsg);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL" + ex.Message.ToString();
            }

            return JSONString;
        }


        public string PostReturnRequest([FromForm] ScheduledReturnIn inputParams)
        {
            dm.TraceService("PostScheduledReturnRequest STARTED " + DateTime.Now.ToString());
            dm.TraceService("============================================");

            dm.TraceService("PostScheduledReturnRequest json:" + inputParams.JSONString);
            string JSONString = string.Empty;
            try
            {

                List<SRItemIDs> itemData = JsonConvert.DeserializeObject<List<SRItemIDs>>(inputParams.JSONString);
                try
                {

                    string InvoiceID = inputParams.InvoiceID == null ? "0" : inputParams.InvoiceID;
                    string UserID = inputParams.UserID == null ? "" : inputParams.UserID;
                    string SubTotal = inputParams.SubTotal == null ? "0" : inputParams.SubTotal;
                    string Vat = inputParams.Vat == null ? "" : inputParams.Vat;
                    string Total = inputParams.Total == null ? "" : inputParams.Total;
                    string type = inputParams.type == null ? "" : inputParams.type;
                    string InputXml = "";
                    string img = "";

                    dm.TraceService("PostScheduledReturnRequest InputXml:" + InputXml);
                    try
                    {
                        using (var sw = new StringWriter())
                        {
                            using (var writer = XmlWriter.Create(sw))
                            {

                                writer.WriteStartDocument(true);
                                writer.WriteStartElement("r");
                                int c = 0;
                                foreach (SRItemIDs id in itemData)
                                {
                                    string[] arr = { id.prdID.ToString(), id.HigherUOM.ToString(), id.HigherQty.ToString(), id.LowerUOM.ToString(), id.LowerQty.ToString(),id.HigherPrice.ToString(),
                                id.LowerPrice.ToString(),id.LineTotal.ToString(),id.Vat.ToString(),id.GrandTotal.ToString()};
                                    string[] arrName = { "ItemId", "HigherUOM", "HigherQty", "LowerUOM", "LowerQty", "HigherPrice", "LowerPrice", "LineTotal", "Vat", "GrandTotal" };
                                    dm.createNode(arr, arrName, writer);
                                }

                                writer.WriteEndElement();
                                writer.WriteEndDocument();
                                writer.Close();
                            }
                            InputXml = sw.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        dm.TraceService(ex.Message.ToString());
                        JSONString = "Issue while creating Xml - " + ex.Message.ToString();
                    }



                    


                    try
                    {
                        string[] arr = { InputXml.ToString(), InvoiceID.ToString(), UserID.ToString(), type, SubTotal.ToString(), Vat.ToString(), Total.ToString()};

                        DataTable dtDN = dm.loadList("InsScheduledReturn", "sp_B2B_MerchandisingServices", inputParams.CusID.ToString(), arr);
                        if (dtDN.Rows.Count > 0)
                        {
                            JSONString = pj.InsReturnRequest(dtDN);
                        }
                        else
                        {
                            JSONString = "NoDataRes";
                            dm.TraceService("NoDataRes");
                        }


                    }
                    catch (Exception ex)
                    {
                        dm.TraceService(ex.Message.ToString());
                        JSONString = "NoDataSQL - " + ex.Message.ToString();
                    }

                }
                catch (Exception ex)
                {
                    dm.TraceService(ex.Message.ToString());
                    JSONString = "NoDataSQL - " + ex.Message.ToString();
                }
            }
            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "NoDataSQL - " + ex.Message.ToString();
            }
            dm.TraceService("PostScheduledReturnRequest ENDED " + DateTime.Now.ToString());
            dm.TraceService("========================================");
            return JSONString;
        }

        public string PostDisputeNoteRequest([FromForm] DisputeNoteReqIn inputParams)
        {
            dm.TraceService("PostScheduledReturnRequest STARTED " + DateTime.Now.ToString());
            dm.TraceService("============================================");
            string JSONString = string.Empty;

            try
            {
                string imagePath = "";
                var HttpReq = HttpContext.Request;
                try
                {
                    var x = HttpReq.Files[0];
                    var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/DisputeRequest"), x.FileName);
                    ViewBag.physicalPath = physicalPath;
                    imagePath = "../UploadFiles/DisputeRequest/" + x.FileName;
                    // The files are not actually saved in this demo
                    x.SaveAs(physicalPath);
                }
                catch (Exception ex)
                {

                }


                try
                {
                    string Amount = inputParams.Amount == null ? "0" : inputParams.Amount;
                    string UserID = inputParams.UserID == null ? "PA" : inputParams.UserID;
                    string cusID = inputParams.CusID == null ? "0" : inputParams.CusID;
                    string type = inputParams.type == null ? "" : inputParams.type;
                    string date = inputParams.date == null ? "" : inputParams.date;
                    string Remark = inputParams.Remark == null ? "" : inputParams.Remark;
                    string OtherInfo = inputParams.OtherInfo == null ? "0" : inputParams.OtherInfo;
                    string InputXml = "";
                    if (inputParams.JSONString != null)
                    {

                        List<InvoiceIDs> itemData = JsonConvert.DeserializeObject<List<InvoiceIDs>>(inputParams.JSONString);
                        using (var sw = new StringWriter())
                        {
                            using (var writer = XmlWriter.Create(sw))
                            {

                                writer.WriteStartDocument(true);
                                writer.WriteStartElement("r");
                                int c = 0;
                                foreach (InvoiceIDs id in itemData)
                                {
                                    string[] arr = { id.oidID.ToString(), id.Balance.ToString() };
                                    string[] arrName = { "oid_ID", "balance" };
                                    dm.createNode(arr, arrName, writer);
                                }

                                writer.WriteEndElement();
                                writer.WriteEndDocument();
                                writer.Close();
                            }
                            InputXml = sw.ToString();
                        }
                    }
                    try
                    {


                        string[] arr = {  UserID.ToString(), Remark.ToString(), OtherInfo.ToString(),
                            Amount.ToString(),type.ToString(),date.ToString(),  InputXml.ToString(),imagePath };

                        DataTable dtDN = dm.loadList("InsDisputeNoteRequest", "sp_B2B_MerchandisingServices", cusID.ToString(), arr);
                        if (dtDN.Rows.Count > 0)
                        {
                            List<DisputeNoteReqOut> listDn = new List<DisputeNoteReqOut>();
                            foreach (DataRow dr in dtDN.Rows)
                            {
                                listDn.Add(new DisputeNoteReqOut
                                {
                                    Res = dr["Res"].ToString(),
                                    Message = dr["Message"].ToString(),

                                    TransID = dr["TransID"].ToString()


                                });
                            }
                            JSONString = JsonConvert.SerializeObject(new
                            {
                                result = listDn
                            });

                            return JSONString;
                        }
                        else
                        {
                            JSONString = "NoDataRes";
                            dm.TraceService("NoDataRes");
                        }


                    }
                    catch (Exception ex)
                    {
                        dm.TraceService(ex.Message.ToString());
                        JSONString = "NoDataSQL - " + ex.Message.ToString();
                    }

                }
                catch (Exception ex)
                {
                    dm.TraceService(ex.Message.ToString());
                    JSONString = "NoDataSQL - " + ex.Message.ToString();
                }


            }
            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "NoDataSQL - " + ex.Message.ToString();
            }
            dm.TraceService("PostScheduledReturnRequest ENDED " + DateTime.Now.ToString());
            dm.TraceService("========================================");
            return JSONString;
        }

        public string PosDisputeImage([FromForm] DisputeImageIn inputParams)
        {
            dm.TraceService("PosDisputeImage STARTED ");
            dm.TraceService("==============================");
            string JSONString = string.Empty;
            try
            {


                string TransID = inputParams.TransID == null ? "0" : inputParams.TransID;



                dm.TraceService("Value for Transaction :" + TransID.ToString());


                try
                {
                    var HttpReq = HttpContext.Request;
                    try
                    {


                        HttpPostedFileBase[] imageFiles = new HttpPostedFileBase[HttpReq.Files.Count];
                        dm.TraceService("file Received in Httpreq" + imageFiles.Length.ToString());
                        var folderName = DateTime.Now.ToString("ddMMyyyy");

                        var physicalPath = Server.MapPath("../../UploadFiles/DisputeReq");
                        dm.TraceService("Physical Path Generated" + physicalPath.ToString());
                        if (!Directory.Exists(physicalPath))
                        {
                            Directory.CreateDirectory(physicalPath);
                            dm.TraceService("Directory Created");
                        }
                        string image = "";
                        var imagePath = physicalPath + "/";
                        if (!Directory.Exists(imagePath))
                        {
                            Directory.CreateDirectory(imagePath);
                            dm.TraceService("Directory for Image Path Created");
                        }
                        string OutImages = "";
                        for (int y = 0; y < HttpReq.Files.Count; y++)
                        {

                            dm.TraceService("Loop Started" + y.ToString());
                            imageFiles[y] = HttpReq.Files[y];
                            long fileSize = imageFiles[y].ContentLength;
                            string REcimage = (DateTime.Now.ToString("HHmmss") + imageFiles[y].FileName);
                            image = imagePath + "/" + (DateTime.Now.ToString("HHmmss") + imageFiles[y].FileName);
                            imageFiles[y].SaveAs(image);
                            if (y == 0 && fileSize > 0)
                            {
                                OutImages = "../UploadFiles/DisputeReq/" + REcimage;

                            }
                            else if (y != 0 && fileSize > 0)
                            {
                                OutImages += "," + "../UploadFiles/DisputeReq/" + REcimage;
                            }
                            else if (fileSize == 0)
                            {
                                OutImages = "";
                            }
                            dm.TraceService("ImagePath" + OutImages);
                            dm.TraceService("ImageFile" + imageFiles[y].FileName.ToString());
                            dm.TraceService("Loop Ended" + y.ToString());
                        }
                        string[] ar = { OutImages };
                        DataTable dtDN = dm.loadList("InsDisputeImage", "sp_DisputeNoteRequest", TransID, ar);
                        if (dtDN.Rows.Count > 0)
                        {
                            List<DisputeImageOut> listDn = new List<DisputeImageOut>();
                            foreach (DataRow dr in dtDN.Rows)
                            {
                                listDn.Add(new DisputeImageOut
                                {
                                    Res = dr["Res"].ToString(),
                                    Message = dr["Message"].ToString(),



                                });
                            }
                            JSONString = JsonConvert.SerializeObject(new
                            {
                                result = listDn
                            });

                            return JSONString;
                        }
                        else
                        {
                            JSONString = "NoDataRes";
                            dm.TraceService("NoDataRes");
                        }

                    }
                    catch (Exception ex)
                    {
                        dm.TraceService(ex.Message.ToString());
                        JSONString = "NoDataSQL - " + ex.Message.ToString();
                    }


                }
                catch (Exception ex)
                {
                    dm.TraceService(ex.Message.ToString());
                    JSONString = "NoDataSQL - " + ex.Message.ToString();
                }
            }
            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "NoDataSQL - " + ex.Message.ToString();
            }

            dm.TraceService("PosDisputeImage ENDED ");
            dm.TraceService("==========================");

            return JSONString;
        }
        public string GetOutstandingInvoices([FromForm] OutstandingINvIn inputParams)
        {
            dm.TraceService("GetOutstandingInvoices STARTED " + DateTime.Now.ToString());
            dm.TraceService("======================================");

            string CusID = inputParams.CusID == null ? "0" : inputParams.CusID;
            string userID = inputParams.UserID == null ? "0" : inputParams.UserID;
            string fromdate= inputParams.fromDate == null ? "0" : inputParams.fromDate;
            string todate = inputParams.todate == null ? "0" : inputParams.todate;
            string JSONString = string.Empty;
            string[] arr = { userID.ToString() , fromdate ,todate};
            DataTable dtreturn = dm.loadList("SelOutstandingInvoiceData", "sp_B2B_MerchandisingServices", CusID.ToString(), arr);



            try
            {
                if (dtreturn.Rows.Count > 0)
                {
                    List<OutstandingINvOut> listHeader = new List<OutstandingINvOut>();
                    foreach (DataRow dr in dtreturn.Rows)
                    {
                        listHeader.Add(new OutstandingINvOut
                        {


                            oid_ID = dr["osi_ID"].ToString(),
                            InvoiceNumber = dr["InvoiceID"].ToString(),
                            InvoiceBalance = dr["InvoiceBalance"].ToString(),
                            InvoicedOn = dr["InvoicedOn"].ToString()

                        });
                    }

                    JSONString = JsonConvert.SerializeObject(new
                    {
                        result = listHeader
                    });

                    return JSONString;
                }
                else
                {
                    dm.TraceService("NoDataRes");
                    JSONString = "NoDataRes";
                }
            }


            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "GetOutstandingInvoices - " + ex.Message.ToString();
            }
            dm.TraceService("GetOutstandingInvoices ENDED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            return JSONString;
        }

        public string GetScheduleRtnDetailData([FromForm] SRRequestDetailIn inputParams)
        {
            dm.TraceService("GetScheduleRtnHeaderData STARTED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            string JSONString = string.Empty;

            string rotID = inputParams.RequestID == null ? "0" : inputParams.RequestID;

            DataTable dtreturn = dm.loadList("SelPendingReturnRequestDetailData", "sp_B2B_MerchandisingServices", rotID.ToString());



            try
            {
                if (dtreturn.Rows.Count > 0)
                {
                    List<GetPendingRtnRequestDetail> listHeader = new List<GetPendingRtnRequestDetail>();
                    foreach (DataRow drDetails in dtreturn.Rows)
                    {
                        listHeader.Add(new GetPendingRtnRequestDetail
                        {
                            prd_ID = drDetails["rrd_prd_ID"].ToString(),
                            HUOM = drDetails["rrd_HUOM"].ToString(),
                            HQty = drDetails["rrd_HQty"].ToString(),
                            LUOM = drDetails["rrd_LUOM"].ToString(),
                            LQty = drDetails["rrd_LQty"].ToString(),

                            prd_Name = drDetails["itm_Name"].ToString(),
                            prd_cat_id = drDetails["itm_cat_ID"].ToString(),
                            prd_sub_ID = drDetails["itm_sct_ID"].ToString(),
                            prd_brd_ID = drDetails["itm_brd_ID"].ToString(),
                            prd_Image = drDetails["itm_Image"].ToString(),
                            InvoiceNumber = drDetails["inv_InvoiceID"].ToString(),
                            inv_ID = drDetails["rrh_inv_ID"].ToString(),
                            prd_code = drDetails["itm_Code"].ToString(),
                            Image = drDetails["Image"].ToString(),
                            Vat = drDetails["rrd_Vat"].ToString(),
                            GrandTotal = drDetails["rrd_GrandTotal"].ToString(),
                            HigherPrice = drDetails["rrd_HigherPrice"].ToString(),
                            LowerPrice = drDetails["rrd_LowerPrice"].ToString(),




                        });
                    }

                    JSONString = JsonConvert.SerializeObject(new
                    {
                        result = listHeader
                    });

                    return JSONString;
                }
                else
                {
                    dm.TraceService("NoDataRes");
                    JSONString = "NoDataRes";
                }
            }


            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "GetScheduleRtnHeaderData - " + ex.Message.ToString();
            }
            dm.TraceService("GetScheduleRtnHeaderData ENDED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            return JSONString;
        }

        public string GetCusRequestTypes([FromForm] CusReqTypeIn inputParams)
        {
            dm.TraceService("GetCusRequestTypes STARTED " + DateTime.Now.ToString());
            dm.TraceService("======================================");

            
            string JSONString = string.Empty;
            
            DataTable dtreturn = dm.loadList("SelNewRequestType", "sp_B2B_MerchandisingServices");
            try
            {
                if (dtreturn.Rows.Count > 0)
                {
                    List<CusReqTypeOut> listHeader = new List<CusReqTypeOut>();
                    foreach (DataRow dr in dtreturn.Rows)
                    {
                        listHeader.Add(new CusReqTypeOut
                        {
                            rqtID = dr["rqt_ID"].ToString(),
                            ReqName = dr["rqt_Name"].ToString()
                        });
                    }
                    JSONString = JsonConvert.SerializeObject(new
                    {
                        result = listHeader
                    });
                    return JSONString;
                }
                else
                {
                    dm.TraceService("NoDataRes");
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "GetCusRequestTypes - " + ex.Message.ToString();
            }
            dm.TraceService("GetCusRequestTypes ENDED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            return JSONString;
        }

        public string GetReasons([FromForm] CusReqTypeIn inputParams)
        {
            dm.TraceService("GetCusRequestTypes STARTED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            
            string JSONString = string.Empty;           
            DataTable dtreturn = dm.loadList("SelReasons", "sp_B2B_MerchandisingServices");
            try
            {
                if (dtreturn.Rows.Count > 0)
                {
                    List<ReasonOut> listHeader = new List<ReasonOut>();
                    foreach (DataRow dr in dtreturn.Rows)
                    {
                        listHeader.Add(new ReasonOut
                        {
                            rsnID = dr["rsn_ID"].ToString(),
                            reason = dr["rsn_Name"].ToString()
                        });
                    }
                    JSONString = JsonConvert.SerializeObject(new
                    {
                        result = listHeader
                    });
                    return JSONString;
                }
                else
                {
                    dm.TraceService("NoDataRes");
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "GetCusRequestTypes - " + ex.Message.ToString();
            }
            dm.TraceService("GetCusRequestTypes ENDED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            return JSONString;
        }

        public string InsCreditNoteImage([FromForm] CreditNoteImageIn inputParams)
        {
            dm.TraceService("InsCreditNoteImage STARTED ");
            dm.TraceService("==============================");
            string JSONString = string.Empty;
            try
            {
                string TransID = inputParams.ReqID == null ? "0" : inputParams.ReqID;
                string prdID = inputParams.prdID == null ? "0" : inputParams.prdID;

                dm.TraceService("Value for Transaction :" + TransID.ToString());
                dm.TraceService("Value for Prd:" + prdID.ToString());
                try
                {
                    string imagePath = "";
                    var HttpReq = HttpContext.Request;
                    try
                    {
                        var x = HttpReq.Files[0];
                        var physicalPath = Path.Combine(Server.MapPath("~/UploadFiles/CreditNoteImage"), x.FileName);
                        ViewBag.physicalPath = physicalPath;
                        imagePath = "../UploadFiles/CreditNoteImage/" + x.FileName;
                        // The files are not actually saved in this demo
                        x.SaveAs(physicalPath);
                    }
                    catch (Exception ex)
                    {

                    }

                    string[] ar = { imagePath, prdID };
                    DataTable dtDN = dm.loadList("InsCreditNoteImage", "sp_B2B_MerchandisingServices", TransID, ar);
                    if (dtDN.Rows.Count > 0)
                    {
                        List<CreditNoteImageOut> listDn = new List<CreditNoteImageOut>();
                        foreach (DataRow dr in dtDN.Rows)
                        {
                            listDn.Add(new CreditNoteImageOut
                            {
                                Res = dr["Res"].ToString(),
                                Message = dr["Message"].ToString()

                            });
                        }
                        JSONString = JsonConvert.SerializeObject(new
                        {
                            result = listDn
                        });

                        return JSONString;
                    }
                    else
                    {
                        JSONString = "NoDataRes";
                        dm.TraceService("NoDataRes");
                    }

                }
                catch (Exception ex)
                {
                    dm.TraceService(ex.Message.ToString());
                    JSONString = "NoDataSQL - " + ex.Message.ToString();
                }


            }
            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "NoDataSQL - " + ex.Message.ToString();
            }

            dm.TraceService("InsCreditNoteImage ENDED ");
            dm.TraceService("==========================");

            return JSONString;
        }
        public string GetDisputeReqDetailData([FromForm] PendingDisputeReqDetailIn inputParams)
        {
            dm.TraceService("GetCompletedDisputeReqDetailData STARTED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            string JSONString = string.Empty;

            string rotID = inputParams.drhID == null ? "0" : inputParams.drhID;
            string url = ConfigurationManager.AppSettings.Get("BackendUrl");

            DataTable dtreturn = dm.loadList("SelCompletedDisputeReqDetailData", "sp_B2B_MerchandisingServices", rotID.ToString());



            try
            {
                if (dtreturn.Rows.Count > 0)
                {
                    List<PendingDisputeReqDetailOut> listHeader = new List<PendingDisputeReqDetailOut>();
                    foreach (DataRow drDetails in dtreturn.Rows)
                    {
                        string imag = "";
                        string img = drDetails["drh_Image"].ToString();
                        if (img != "")
                        {
                            string[] ar = (drDetails["drh_Image"].ToString().Replace("../", "")).Split(',');

                            for (int i = 0; i < ar.Length; i++)
                            {
                                if (i > 0)
                                {
                                    imag = imag + "," + url + ar[i];
                                }
                                else
                                {
                                    imag = url + ar[i];
                                }
                            }

                        }
                        listHeader.Add(new PendingDisputeReqDetailOut
                        {
                            drhID = drDetails["drh_ID"].ToString(),
                            Type = drDetails["drh_DisputeType"].ToString(),
                            OtherInfo = drDetails["drh_OtherInfo"].ToString(),
                            Comment = drDetails["drh_ResponseRemark"].ToString(),
                            Amount = drDetails["drd_InvoiceBalance"].ToString(),
                            InvCount = drDetails["drd_oid_ID"].ToString(),
                            Image= imag



                        });
                    }

                    JSONString = JsonConvert.SerializeObject(new
                    {
                        result = listHeader
                    });

                    return JSONString;
                }
                else
                {
                    dm.TraceService("NoDataRes");
                    JSONString = "NoDataRes";
                }
            }



            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "GetCompletedDisputeReqDetailData - " + ex.Message.ToString();
            }
            dm.TraceService("GetCompletedDisputeReqDetailData ENDED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            return JSONString;
        }


        public string GetOutstandingInvoiceForNewDisputeHeader([FromForm] OutInvforDispIn inputParams)
        {
            dm.TraceService("GetOutstandingInvoices STARTED " + DateTime.Now.ToString());
            dm.TraceService("======================================");

            string CusID = inputParams.drhID == null ? "0" : inputParams.drhID;
            string JSONString = string.Empty;
            DataTable dtreturn = dm.loadList("SelOInvforDisputeReq", "sp_B2B_MerchandisingServices", CusID.ToString());



            try
            {
                if (dtreturn.Rows.Count > 0)
                {
                    List<OutstandingINvOut> listHeader = new List<OutstandingINvOut>();
                    foreach (DataRow dr in dtreturn.Rows)
                    {
                        listHeader.Add(new OutstandingINvOut
                        {


                            oid_ID = dr["osi_ID"].ToString(),
                            InvoiceNumber = dr["InvoiceID"].ToString(),
                            InvoicedOn = dr["InvoicedOn"].ToString(),
                            InvoiceBalance = dr["InvoiceBalance"].ToString(),


                        });
                    }

                    JSONString = JsonConvert.SerializeObject(new
                    {
                        result = listHeader
                    });

                    return JSONString;
                }
                else
                {
                    dm.TraceService("NoDataRes");
                    JSONString = "NoDataRes";
                }
            }


            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "GetOutstandingInvoices - " + ex.Message.ToString();
            }
            dm.TraceService("GetOutstandingInvoices ENDED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            return JSONString;
        }

        public string GetCNRDetailData([FromForm] GetCNRDetailDataIn inputParams)
        {
            dm.TraceService("GetCNRDetailData STARTED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            string JSONString = string.Empty;

            string cnhID = inputParams.cnhID == null ? "0" : inputParams.cnhID;
            string url = ConfigurationManager.AppSettings.Get("BackendUrl");

            DataTable dtreturn = dm.loadList("GetCNRDetailData", "sp_B2B_MerchandisingServices", cnhID.ToString());



            try
            {
                if (dtreturn.Rows.Count > 0)
                {
                    List<GetCNRDetailDataOut> listHeader = new List<GetCNRDetailDataOut>();
                    foreach (DataRow drDetails in dtreturn.Rows)
                    {
                        string imag = "";
                        string img = drDetails["crd_Image"].ToString();
                        if (img != "")
                        {
                            string[] ar = (drDetails["crd_Image"].ToString().Replace("../", "")).Split(',');

                            for (int i = 0; i < ar.Length; i++)
                            {
                                if (i > 0)
                                {
                                    imag = imag + "," + url + ar[i];
                                }
                                else
                                {
                                    imag = url + ar[i];
                                }
                            }

                        }

                        string imag2 = "";
                        string img2 = drDetails["itm_Image"].ToString();
                        if (img2 != "")
                        {
                            string[] ar = (drDetails["itm_Image"].ToString().Replace("../", "")).Split(',');

                            for (int i = 0; i < ar.Length; i++)
                            {
                                if (i > 0)
                                {
                                    imag2 = imag2 + "," + url + ar[i];
                                }
                                else
                                {
                                    imag2 = url + ar[i];
                                }
                            }

                        }
                        listHeader.Add(new GetCNRDetailDataOut
                        {
                            cndID = drDetails["cnd_ID"].ToString(),
                            Date = drDetails["CreatedDate"].ToString(),                            
                            itmCode = drDetails["itm_Code"].ToString(),
                            itmName = drDetails["itm_Name"].ToString(),
                            itmImg = imag2,
                            HUom = drDetails["crd_HUOM"].ToString(),
                            HQty = drDetails["crd_HQty"].ToString(),
                            LUom = drDetails["crd_LUOM"].ToString(),
                            LQty = drDetails["crd_LQty"].ToString(),
                            InvNum = drDetails["inv_Number"].ToString(),
                            Reason = drDetails["rsn_Name"].ToString(),
                            Image = imag ,
                            amount= drDetails["amount"].ToString()


                        });
                    }

                    JSONString = JsonConvert.SerializeObject(new
                    {
                        result = listHeader
                    });

                    return JSONString;
                }
                else
                {
                    dm.TraceService("NoDataRes");
                    JSONString = "NoDataRes";
                }
            }



            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "GetCNRDetailData - " + ex.Message.ToString();
            }
            dm.TraceService("GetCNRDetailData ENDED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            return JSONString;
        }
        public string GetCusReqDetail([FromForm] GetCusReqDetailIn inputParams)
        {
            dm.TraceService("GetCusReqDetail STARTED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            string JSONString = string.Empty;

            string reqID = inputParams.reqID == null ? "0" : inputParams.reqID;
            string url = ConfigurationManager.AppSettings.Get("BackendUrl");

            DataTable dtreturn = dm.loadList("SelCustomerReqDetail", "sp_B2B_MerchandisingServices", reqID.ToString());
            try
            {
                if (dtreturn.Rows.Count > 0)
                {
                    List<GetCusReqDetailOut> listHeader = new List<GetCusReqDetailOut>();
                    foreach (DataRow drDetails in dtreturn.Rows)
                    {
                        string imag = "";
                        string img = drDetails["rei_Image"].ToString();
                        if (img != "")
                        {
                            string[] ar = (drDetails["rei_Image"].ToString().Replace("../", "")).Split(',');

                            for (int i = 0; i < ar.Length; i++)
                            {
                                if (i > 0)
                                {
                                    imag = imag + "," + url + ar[i];
                                }
                                else
                                {
                                    imag = url + ar[i];
                                }
                            }
                        }
                        listHeader.Add(new GetCusReqDetailOut
                        {
                            reqID = drDetails["req_ID"].ToString(),
                            description = drDetails["req_Description"].ToString(),
                            type = drDetails["rqt_Name"].ToString(),
                            reqImg = imag


                        });
                    }

                    JSONString = JsonConvert.SerializeObject(new
                    {
                        result = listHeader
                    });

                    return JSONString;
                }
                else
                {
                    dm.TraceService("NoDataRes");
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "GetCusReqDetail - " + ex.Message.ToString();
            }
            dm.TraceService("GetCusReqDetail ENDED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            return JSONString;
        }

        public string GetCreditNoteReqHeaderByID([FromForm] GetCNRDetailDataIn inParams)
        {

            string JSONString = string.Empty;
            string cnhID = inParams.cnhID == null ? "0" : inParams.cnhID;

            DataTable dtCusInfo = dm.loadList("GetCNRHeaderByID", "sp_B2B_MerchandisingServices", cnhID.ToString());


            try
            {
                if (dtCusInfo.Rows.Count > 0)
                {
                    JSONString = pj.SelCreditNoteRequestHeaderByID(dtCusInfo);
                }
                else
                {
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                JSONString = "NoDataSQL";
            }

            return JSONString;

        }

        public string GetProducts([FromForm] GetProductsIn inputParams)
        {
            dm.TraceService("GetProducts STARTED " + DateTime.Now.ToString());
            dm.TraceService("======================================");

            string JSONString = string.Empty;
            DataTable dtreturn = dm.loadList("SelProducts", "sp_B2B_MerchandisingServices");
            try
            {
                if (dtreturn.Rows.Count > 0)
                {
                    List<GetProductsOut> listHeader = new List<GetProductsOut>();
                    foreach (DataRow dr in dtreturn.Rows)
                    {
                        listHeader.Add(new GetProductsOut
                        {
                            prdID = dr["itm_ID"].ToString(),
                            prdName = dr["itm_Name"].ToString()
                        });
                    }
                    JSONString = JsonConvert.SerializeObject(new
                    {
                        result = listHeader
                    });
                    return JSONString;
                }
                else
                {
                    dm.TraceService("NoDataRes");
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "GetProducts - " + ex.Message.ToString();
            }
            dm.TraceService("GetProducts ENDED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            return JSONString;
        }

        public string GetInvoiceAlongProducts([FromForm] GetInvProdIn inputParams)
        {
            dm.TraceService("GetInvoiceAlongProducts STARTED " + DateTime.Now.ToString());
            dm.TraceService("======================================");

            string JSONString = string.Empty;
            string prdID = inputParams.prdID == null ? "0" : inputParams.prdID;
            DataTable dtreturn = dm.loadList("SelInvoicewithProducts", "sp_B2B_MerchandisingServices", prdID.ToString() );
            try
            {
                if (dtreturn.Rows.Count > 0)
                {
                    List<GetInvProdOut> listHeader = new List<GetInvProdOut>();
                    foreach (DataRow dr in dtreturn.Rows)
                    {
                        listHeader.Add(new GetInvProdOut
                        {
                            invID = dr["inv_ID"].ToString(),
                            invNum = dr["inv_Number"].ToString()
                        });
                    }
                    JSONString = JsonConvert.SerializeObject(new
                    {
                        result = listHeader
                    });
                    return JSONString;
                }
                else
                {
                    dm.TraceService("NoDataRes");
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "GetInvoiceAlongProducts - " + ex.Message.ToString();
            }
            dm.TraceService("GetInvoiceAlongProducts ENDED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            return JSONString;
        }

        public string GetInvoiceAlongProductDetail([FromForm] GetInvProdDetIn inputParams)
        {
            dm.TraceService("GetInvoiceAlongProductDetail STARTED " + DateTime.Now.ToString());
            dm.TraceService("======================================");

            string JSONString = string.Empty;
            string prdID = inputParams.prdID == null ? "0" : inputParams.prdID;
            string invID = inputParams.invID == null ? "0" : inputParams.invID;

            string[] arr = { invID.ToString() };
            DataTable dtreturn = dm.loadList("SelInvoicewithProdDetail", "sp_B2B_MerchandisingServices", prdID.ToString(), arr);
            try
            {
                if (dtreturn.Rows.Count > 0)
                {
                    List<GetInvProdDetOut> listHeader = new List<GetInvProdDetOut>();
                    foreach (DataRow dr in dtreturn.Rows)
                    {
                        listHeader.Add(new GetInvProdDetOut
                        {
                            invID = dr["ind_inv_ID"].ToString(),
                            itmCode = dr["itm_Code"].ToString(),
                            itmName = dr["itm_Name"].ToString(),
                            HQty = dr["ind_HQty"].ToString(),
                            HUom = dr["ind_HUom"].ToString(),
                            LQty = dr["ind_LQty"].ToString(),
                            LUom = dr["ind_LUOM"].ToString(),                            
                            SubTotal = dr["ind_SubTotal"].ToString(),
                            VAT = dr["ind_VAT"].ToString(),
                            GrandTotal = dr["ind_GrandTotal"].ToString(),
                            huomid= dr["humoid"].ToString(),
                            luomid= dr["luomid"].ToString()

                        });
                    }
                    JSONString = JsonConvert.SerializeObject(new
                    {
                        result = listHeader
                    });
                    return JSONString;
                }
                else
                {
                    dm.TraceService("NoDataRes");
                    JSONString = "NoDataRes";
                }
            }
            catch (Exception ex)
            {
                dm.TraceService(ex.Message.ToString());
                JSONString = "GetInvoiceAlongProductDetail - " + ex.Message.ToString();
            }
            dm.TraceService("GetInvoiceAlongProductDetail ENDED " + DateTime.Now.ToString());
            dm.TraceService("======================================");
            return JSONString;
        }



    }
}