using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;

namespace b2b_solution.Models
{
    public class ProcessJson
    {
        DataModel dm = new DataModel();

        string Image = ConfigurationManager.AppSettings.Get("URL");
        string Images = "https://digitsb2b.dev-ts.online/";
        //JSON TO RETURN MULTIPLE OBJECTS
        public string ItemsJson(DataTable dt, InParams inParams)
        {
            List<GetItem> listItems = new List<GetItem>();
            foreach (DataRow dr in dt.Rows)
            {
                List<ItemUOM> listUOMS = new List<ItemUOM>();
                string[] arr = { inParams.cusID };
                DataTable dtUOM = dm.loadList("SelItemUOM", "sp_B2B_Home", dr["itm_ID"].ToString(), arr);
                foreach (DataRow drDetails in dtUOM.Rows)
                {
                    listUOMS.Add(new ItemUOM
                    {
                        IsDefault = drDetails["itu_IsDefault"].ToString(),
                        uomName = drDetails["uom_Name"].ToString(),
                        uomID = drDetails["itu_uom_ID"].ToString(),
                        UPC = drDetails["itu_UPC"].ToString(),
                        Offerprice = drDetails["OfferPrice"].ToString(),
                        StandardPrice = drDetails["standardPrice"].ToString()
                    });
                }

                listItems.Add(new GetItem
                {
                    Category = dr["cat_Name"].ToString(),
                    CatID = dr["cat_ID"].ToString(),
                    Hqty = dr["crd_HigherQty"].ToString(),
                    HUOM = dr["crd_HigherUOM"].ToString(),
                    itmID = dr["itm_ID"].ToString(),
                    itmImage = dr["itm_Image"].ToString(),
                    itmName = dr["itm_Name"].ToString(),
                    Lqty = dr["crd_LowerQty"].ToString(),
                    LUOM = dr["crd_lowerUOM"].ToString(),
                    sctName = dr["sct_Name"].ToString(),
                    UOM = listUOMS,
                    cat_ArabicName = dr["cat_ArabicName"].ToString(),
                    itm_ArabicName = dr["itm_ArabicName"].ToString(),
                    sct_ArabicName = dr["sct_ArabicName"].ToString(),
                    itmCode = dr["itm_Code"].ToString(),
                    FreqOrder = dr["FreqOrder"].ToString(),
                    itm_Type = dr["itm_Type"].ToString(),
                    brd_Name = dr["brd_Name"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        //JSON TO RETURN ONLY SINGLE OBJECTS
        //YOU CAN CHOOSE YOUR OWN CLASSES BASED ON THE SERVICES
        public string CartJson(DataTable dt, InParams inParams)
        {
            List<GetItem> listItems = new List<GetItem>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new GetItem
                {
                    Category = dr["cat_Name"].ToString(),
                    CatID = dr["cat_ID"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelBrandJson(DataTable dt)
        {
            List<brandParams> listItems = new List<brandParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new brandParams
                {
                    brd_ID = dr["brd_ID"].ToString(),
                    brd_Name = dr["brd_Name"].ToString(),
                    brd_Image = dr["brd_Image"].ToString(),                  
                    brd_Code = dr["brd_Code"].ToString(),
                    brd_ArabicName = dr["brd_ArabicName"].ToString()

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelCatJson(DataTable dt)
        {
            List<categoryParams> listItems = new List<categoryParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new categoryParams
                {
                    id = dr["id"].ToString(),
                    Name = dr["Name"].ToString(),
                    Img = dr["Img"].ToString(),
                    ArabicName = dr["cat_ArabicName"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelbannersJson(DataTable dt)
        {
            List<BannerParams> listItems = new List<BannerParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new BannerParams
                {
                    Heading = dr["bnr_Heading"].ToString(),
                    image = dr["bnr_Image"].ToString(),
                    ShortMessage = dr["bnr_ShortMessage"].ToString(),
                    ArabicHeading = dr["bnr_HeadingArabic"].ToString(),
                    ArabicShortMessage = dr["bnr_ShortMessageArabic"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }




        public string RecentSumJson(DataTable dt, InParams inParams)
        {
            List<OrdParams> listItems = new List<OrdParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new OrdParams
                {
                    OrdID = dr["ord_ID"].ToString(),
                    OrdGrandTotal = dr["ord_GrandTotal"].ToString(),
                    OrdDate = dr["ord_Date"].ToString(),
                    Orditm_Count = dr["itm_Count"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelARJson(DataTable dt)
        {
            List<ARParams> listItems = new List<ARParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new ARParams
                {
                    arc_ID = dr["arc_ID"].ToString(),
                    arc_Code = dr["arc_Code"].ToString(),
                    arc_PaymentMode = dr["arc_PaymentMode"].ToString(),
                    ChequeDate = dr["ChequeDate"].ToString(),
                    arc_ChequeImg = dr["arc_ChequeImg"].ToString(),
                    arc_ChequeNo = dr["arc_ChequeNo"].ToString(),
                    arc_SubTotal = dr["arc_SubTotal"].ToString(),
                    arc_Vat = dr["arc_Vat"].ToString(),
                    arc_GrandTotal = dr["arc_GrandTotal"].ToString(),
                    bank_Name = dr["bank_Name"].ToString(),
                    CreatedDate = dr["CreatedDate"].ToString(),
                    arc_PaymentMode_Arabic = dr["arc_PaymentMode_Arabic"].ToString(),
                    bank_Name_Arabic = dr["bank_Name_Arabic"].ToString(),

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelARDetailJson(DataTable dt)
        {
            List<ARDOutParams> listItems = new List<ARDOutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new ARDOutParams
                {
                    inv_Number = dr["inv_Number"].ToString(),
                    inv_GrandTotal = dr["inv_GrandTotal"].ToString(),
                    ard_PaidAmount = dr["ard_PaidAmount"].ToString(),
                    inv_Date = dr["inv_Date"].ToString(),
                    inv_SubTotal = dr["inv_SubTotal"].ToString(),



                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string GetOrderJson(DataTable dt)
        {
            List<GetAllOrder> listItems = new List<GetAllOrder>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new GetAllOrder
                {

                    id = dr["ord_ID"].ToString(),
                    OrderNo = dr["ord_Number"].ToString(),
                    Status = dr["Status"].ToString(),
                    SubTotal = dr["ord_SubTotal"].ToString(),
                    VAT = dr["ord_Vat"].ToString(),
                    GradndTotal = dr["ord_GrandTotal"].ToString(),
                    CreatedDate = dr["CreatedDate"].ToString(),
                    CreatedBy = dr["CreatedBy"].ToString(),
                    ExpectedDate = dr["ExpDate"].ToString(),
                    Attachment = dr["ord_LPO_Attachment"].ToString(),
                    LPO = dr["ord_LPO"].ToString(),
                    Total = dr["ord_SubTotal_WO_Discount"].ToString(),
                    Discount = dr["ord_TotalDiscount"].ToString(),
                    Counts = dr["Counts"].ToString(),
                    ArabicStatus = dr["ArabicStatus"].ToString(),




                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string GetOrderDetailJson(DataTable dt)
        {
            List<GetorderByID> listItems = new List<GetorderByID>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new GetorderByID
                {

                    id = dr["odd_ID"].ToString(),
                    itmName = dr["itm_Name"].ToString(),
                    itmCode = dr["itm_Code"].ToString(),
                    itmImage = dr["itm_Image"].ToString(),
                    HUOM = dr["HigherUom"].ToString(),
                    LUOM = dr["LowerUOM"].ToString(),
                    HQty = dr["odd_HigherQty"].ToString(),
                    LQty = dr["odd_LowerQty"].ToString(),
                    HPrice = dr["odd_HigherUOMSellPrice"].ToString(),
                    Lprice = dr["odd_LowerUOMSellPrice"].ToString(),
                    SubTotal = dr["odd_SubTotal"].ToString(),
                    VAT = dr["odd_VAT"].ToString(),
                    GrandTotal = dr["odd_GrandTotal"].ToString(),
                    TransType = dr["odd_TransType"].ToString(),
                    Total = dr["odd_SubTotalWDiscount"].ToString(),
                    Discount = dr["odd_Discount"].ToString(),
                    itm_ArabicName = dr["itm_ArabicName"].ToString(),


                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string InsertCartJson(DataTable dt)
        {
            List<OutParams> listItems = new List<OutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new OutParams
                {
                    Res = dr["Res"].ToString(),
                    Title = dr["Title"].ToString(),
                    Descr = dr["Descr"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }

        public string InsPromoCartJson(DataTable dt)
        {
            List<insPromocartOutParams> listItems = new List<insPromocartOutParams>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new insPromocartOutParams
                {
                    Res = dr["Res"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }

       public string SelDocType(DataTable dt)
       {
            List<DocTypeOut> listItems = new List<DocTypeOut>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new DocTypeOut
                {
                    TypeID = dr["cdt_ID"].ToString(),
                    Type = dr["cdt_Type"].ToString(),
                    IsExpiry = dr["IsExpiryDate"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
       }
        public string SelCusRoles(DataTable dt)
        {
            List<CustRoles> listItems = new List<CustRoles>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new CustRoles
                {
                    rolName = dr["crl_Name"].ToString(),
                    rolID = dr["crl_ID"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }

        public string SelASHIDJson(DataTable dt)
        {
            List<selAshidout> listItems = new List<selAshidout>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new selAshidout
                {
                    ashID = dr["prm_ash_ID"].ToString(),
                   
                   



                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string UpdMsgRslvJson(DataTable dt)
        {
            List<updmsgrslParams> listItems = new List<updmsgrslParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new updmsgrslParams
                {
                    res = dr["Res"].ToString(),
                    status = dr["status"].ToString(),
                    remarks = dr["remarks"].ToString(),
                    seqNum = dr["seqNum"].ToString(),

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
       
        public string ShipAddressJson(DataTable dt)
        {
            List<ShipAddOutParams> listItems = new List<ShipAddOutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new ShipAddOutParams
                {
                    cua_ID= dr["cua_ID"].ToString(),
                    cua_BuldgName = dr["cua_BuldgName"].ToString(),
                    cua_LandMark= dr["cua_LandMark"].ToString(),
                    cua_RoomNo= dr["cua_RoomNo"].ToString(),
                    cua_Street= dr["cua_Street"].ToString(),
                    sta_ID= dr["sta_ID"].ToString(),
                    sta_Name= dr["sta_Name"].ToString(),
                    cua_IsDefault= dr["cua_IsDefault"].ToString(),


                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }


        public string BillAddressJson(DataTable dt)
        {
            List<BillAddOutParams> listItems = new List<BillAddOutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new BillAddOutParams
                {
                    cus_BuildingName = dr["cus_BuildingName"].ToString(),
                    cus_LandMark = dr["cus_LandMark"].ToString(),
                    cus_RoomNo = dr["cus_RoomNo"].ToString(),
                    cus_Street = dr["cus_Street"].ToString(),
                    sta_Name = dr["sta_Name"].ToString(),
                    


                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }


        public string insAddJson(DataTable dt)
        {
            List<insAddressOutParams> listItems = new List<insAddressOutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new insAddressOutParams
                {
                    Res = dr["Res"].ToString(),
                    Title = dr["Title"].ToString(),
                    


                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string RepOrderJson(DataTable dt)
        {
            List<OutParams> listItems = new List<OutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new OutParams
                {
                    Res = dr["Res"].ToString(),
                    Title = dr["Title"].ToString(),
                    Descr = dr["Descr"].ToString(),

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string DelcatJson(DataTable dt)
        {
            List<OutParams> listItems = new List<OutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new OutParams
                {

                    Title = dr["Title"].ToString(),
                    Descr = dr["Descr"].ToString(),

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string CartTotalJson(DataTable dt)
        {
            List<carttotParams> listItems = new List<carttotParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new carttotParams
                {

                    SubTotal = dr["SubTotal_W_D"].ToString(),
                    VAT = dr["TotalVAT"].ToString(),
                    GrandTotal = dr["GrandTotal"].ToString(),
                    TotalCreditLimit = dr["TotalCreditLimit"].ToString(),
                    TotalAvailableLimit = dr["TotalAvailableLimit"].ToString(),
                    MinOrderValue = dr["MinOrderValue"].ToString(),
                    CreaditDays = dr["CreaditDays"].ToString(),
                    Discount = dr["TotalDisc"].ToString()

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelSubCatJson(DataTable dt, SubCatParams inParams)
        {
            List<SubCatParams> listItems = new List<SubCatParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new SubCatParams
                {

                    sct_ID = dr["sct_ID"].ToString(),
                    sct_Name = dr["sct_Name"].ToString(),
                    sct_Code = dr["sct_Code"].ToString(),
                    sct_Image = dr["sct_Image"].ToString(),
                    sct_ArabicName = dr["sct_ArabicName"].ToString(),
                    

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string DelCartJson(DataTable dt)
        {
            List<OutParams> listItems = new List<OutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new OutParams
                {
                    Res = dr["Res"].ToString(),
                    Title = dr["Title"].ToString(),
                    Descr = dr["Descr"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }


        public string CartCountJson(DataTable dt)
        {
            List<CartCountoutParams> listItems = new List<CartCountoutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new CartCountoutParams
                {

                    count = dr["count"].ToString(),


                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string PromorangeJson(DataTable dt)
        {
            List<PromorangeoutParams> listItems = new List<PromorangeoutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new PromorangeoutParams
                {

                    count = dr["prr_Value"].ToString(),
                    prrID = dr["prr_ID"].ToString(),
                    min = dr["prr_MinValue"].ToString(),
                    max = dr["prr_MaxValue"].ToString(),
                    desc1 = dr["desc1"].ToString(),
                    desc2 = dr["desc2"].ToString(),
                    ArabicDesc1 = dr["prt_ArabicDesc1"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string QualiGrpJson(DataTable dt)
        {
            List<PromorangeoutParams> listItems = new List<PromorangeoutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new PromorangeoutParams
                {

                    count = dr["prr_Value"].ToString(),
                    prrID = dr["prr_ID"].ToString(),
                    min = dr["prr_MinValue"].ToString(),
                    max = dr["prr_MaxValue"].ToString(),
                    desc1 = dr["desc1"].ToString(),
                    desc2 = dr["desc2"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string CartItemsJson(DataTable dt, CartTotalParams inParams)
        {
            List<cartitemParams> listItems = new List<cartitemParams>();
            foreach (DataRow dr in dt.Rows)
            {
                List<ItemUOM> listUOMS = new List<ItemUOM>();
                string[] arr = { inParams.cusID };
                DataTable dtUOM = dm.loadList("SelItemUOM", "sp_B2B_Home", dr["itm_ID"].ToString(), arr);
                foreach (DataRow drDetails in dtUOM.Rows)
                {
                    listUOMS.Add(new ItemUOM
                    {
                        IsDefault = drDetails["itu_IsDefault"].ToString(),
                        uomName = drDetails["uom_Name"].ToString(),
                        uomID = drDetails["itu_uom_ID"].ToString(),
                        UPC = drDetails["itu_UPC"].ToString(),
                        Offerprice = drDetails["OfferPrice"].ToString(),
                        StandardPrice = drDetails["standardPrice"].ToString()
                       
                    });
                }


                listItems.Add(new cartitemParams
                {


                    itm_ID = dr["itm_ID"].ToString(),
                    itm_Name = dr["itm_Name"].ToString(),
                    itm_Image = dr["itm_Image"].ToString(),
                    sct_Name = dr["sct_Name"].ToString(),
                    cat_Name = dr["cat_Name"].ToString(),
                    OfferPrice = dr["OfferPrice"].ToString(),
                    crd_HigherQty = dr["crd_HigherQty"].ToString(),
                    crd_HigherUOM = dr["crd_HigherUOM"].ToString(),
                    crd_LowerQty = dr["crd_LowerQty"].ToString(),
                    crd_lowerUOM = dr["crd_lowerUOM"].ToString(),
                    LowerPrice = dr["LowerPrice"].ToString(),
                    Total = dr["GrandTotal"].ToString(),
                    TotalDisc = dr["TotalDisc"].ToString(),
                    UOM = listUOMS,
                    itm_ArabicName = dr["itm_ArabicName"].ToString(),
                    sct_ArabicName = dr["sct_ArabicName"].ToString(),
                    cat_ArabicName = dr["cat_ArabicName"].ToString(),
                     itmCode = dr["itm_Code"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string QualiGroupJson(DataTable dt, QualiGrpInParams inParams)
        {
            List<cartitemParams> listItems = new List<cartitemParams>();
            foreach (DataRow dr in dt.Rows)
            {
                List<ItemUOM> listUOMS = new List<ItemUOM>();
                string[] arr = { inParams.cusID };
                DataTable dtUOM = dm.loadList("SelItemUOM", "sp_B2B_Home", dr["itm_ID"].ToString(), arr);
                foreach (DataRow drDetails in dtUOM.Rows)
                {
                    listUOMS.Add(new ItemUOM
                    {
                        IsDefault = drDetails["itu_IsDefault"].ToString(),
                        uomName = drDetails["uom_Name"].ToString(),
                        uomID = drDetails["itu_uom_ID"].ToString(),
                        UPC = drDetails["itu_UPC"].ToString(),
                        Offerprice = drDetails["OfferPrice"].ToString(),
                        StandardPrice = drDetails["standardPrice"].ToString()
                    });
                }


                listItems.Add(new cartitemParams
                {


                    itm_ID = dr["itm_ID"].ToString(),
                    itm_Name = dr["itm_Name"].ToString(),
                    itm_Image = dr["itm_Image"].ToString(),
                    sct_Name = dr["sct_Name"].ToString(),
                    cat_Name = dr["cat_Name"].ToString(),
                    OfferPrice = dr["OfferPrice"].ToString(),
                    crd_HigherQty = dr["crd_HigherQty"].ToString(),
                    crd_HigherUOM = dr["crd_HigherUOM"].ToString(),
                    crd_LowerQty = dr["crd_LowerQty"].ToString(),
                    crd_lowerUOM = dr["crd_lowerUOM"].ToString(),
                    //LowerPrice = dr["LowerPrice"].ToString(),
                    // Total = dr["GrandTotal"].ToString(),
                    HUPC = dr["HUPC"].ToString(),
                    LUPC = dr["LUPC"].ToString(),
                    UOM = listUOMS,
                    cat_ArabicName = dr["cat_ArabicName"].ToString(),
                    itm_ArabicName = dr["itm_ArabicName"].ToString(),
                    sct_ArabicName = dr["sct_ArabicName"].ToString()

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string AssignGroupJson(DataTable dt, QualiGrpInParams inParams)
        {
            List<AssignitemParams> listItems = new List<AssignitemParams>();
            foreach (DataRow dr in dt.Rows)
            {
                List<ItemUOM> listUOMS = new List<ItemUOM>();
                string[] arr = { inParams.cusID };
                DataTable dtUOM = dm.loadList("SelItemUOM", "sp_B2B_Home", dr["itm_ID"].ToString(), arr);
                foreach (DataRow drDetails in dtUOM.Rows)
                {
                    listUOMS.Add(new ItemUOM
                    {
                        IsDefault = drDetails["itu_IsDefault"].ToString(),
                        uomName = drDetails["uom_Name"].ToString(),
                        uomID = drDetails["itu_uom_ID"].ToString(),
                        UPC = drDetails["itu_UPC"].ToString(),
                        Offerprice = drDetails["OfferPrice"].ToString(),
                        StandardPrice = drDetails["standardPrice"].ToString()
                    });
                }


                listItems.Add(new AssignitemParams
                {


                    itm_ID = dr["itm_ID"].ToString(),
                    itm_Name = dr["itm_Name"].ToString(),
                    itm_Image = dr["itm_Image"].ToString(),
                    sct_Name = dr["sct_Name"].ToString(),
                    cat_Name = dr["cat_Name"].ToString(),
                    OfferPrice = dr["OfferPrice"].ToString(),
                    crd_HigherQty = dr["crd_HigherQty"].ToString(),
                    crd_HigherUOM = dr["crd_HigherUOM"].ToString(),
                    crd_LowerQty = dr["crd_LowerQty"].ToString(),
                    crd_lowerUOM = dr["crd_lowerUOM"].ToString(),
                    //LowerPrice = dr["LowerPrice"].ToString(),
                    // Total = dr["GrandTotal"].ToString(),
                   
                    UOM = listUOMS,
                    cat_ArabicName = dr["cat_ArabicName"].ToString(),
                    itm_ArabicName = dr["itm_ArabicName"].ToString(),
                    sct_ArabicName = dr["sct_ArabicName"].ToString()

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }


        public string RecentItemsJson(DataTable dt, CartTotalParams inParams)
        {
            List<RecentItem> listItems = new List<RecentItem>();
            foreach (DataRow dr in dt.Rows)
            {
                List<ItemUOM> listUOMS = new List<ItemUOM>();
                string[] arr = { inParams.cusID };
                DataTable dtUOM = dm.loadList("SelItemUOM", "sp_B2B_Home", dr["itm_ID"].ToString(), arr);
                foreach (DataRow drDetails in dtUOM.Rows)
                {
                    listUOMS.Add(new ItemUOM
                    {
                        IsDefault = drDetails["itu_IsDefault"].ToString(),
                        uomName = drDetails["uom_Name"].ToString(),
                        uomID = drDetails["itu_uom_ID"].ToString(),
                        UPC = drDetails["itu_UPC"].ToString(),
                        Offerprice = drDetails["OfferPrice"].ToString(),
                        StandardPrice = drDetails["standardPrice"].ToString()
                    });
                }

                listItems.Add(new RecentItem
                {
                    catName = dr["cat_Name"].ToString(),
                    catID = dr["cat_ID"].ToString(),
                    CHQty = dr["crd_HigherQty"].ToString(),
                    CHUOM = dr["crd_HigherUOM"].ToString(),
                    itmID = dr["itm_ID"].ToString(),
                    itmImage = dr["itm_Image"].ToString(),
                    itmName = dr["itm_Name"].ToString(),
                    itmCount = dr["itmCount"].ToString(),
                    CLQty = dr["crd_LowerQty"].ToString(),
                    CLUOM = dr["crd_lowerUOM"].ToString(),
                    sctName = dr["sct_Name"].ToString(),
                    UOM = listUOMS,
                    itm_ArabicName = dr["itm_ArabicName"].ToString(),
                    sct_ArabicName = dr["sct_ArabicName"].ToString(),
                    cat_ArabicName = dr["cat_ArabicName"].ToString(),
                    itmCode = dr["itm_Code"].ToString(),
                    FreqOrder = dr["FreqOrder"].ToString(),
                    itm_Type = dr["itm_Type"].ToString(),
                    brd_Name = dr["brd_Name"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelStatesjson(DataTable dt)
        {
            List<StateParams> listItems = new List<StateParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new StateParams
                {
                    StateID = dr["sta_ID"].ToString(),
                    State = dr["sta_Name"].ToString(),
                    StateArabic = dr["sta_ArabicName"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string ordtrackJson(DataTable dt)
        {
            List<ordtrackoutparams> listItems = new List<ordtrackoutparams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new ordtrackoutparams
                {
                    CreatedDate = dr["CreatedDate"].ToString(),
                    olc_Remarks = dr["olc_Remarks"].ToString(),
                    olc_Status = dr["olc_Status"].ToString(),
                    olc_ArabicRemarks = dr["olc_ArabicRemarks"].ToString(),
                    olc_ArabicStatus = dr["olc_ArabicStatus"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string CustomerSprtJson(DataTable dt)
        {
            List<CSPOutParams> listItems = new List<CSPOutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new CSPOutParams
                {


                    csp_ID = dr["csp_ID"].ToString(),
                    csp_Title = dr["csp_Title"].ToString(),
                    csp_Message = dr["csp_Message"].ToString(),
                    csp_Image =  dr["csp_Image"].ToString(),
                    csp_Reason = dr["csp_Reason"].ToString(),
                    csp_Number = dr["csp_Number"].ToString(),
                    CreatedDate = dr["CreatedDate"].ToString(),
                    csp_cus_ID = dr["csp_cus_ID"].ToString(),
                    stat_Name = dr["stat_Name"].ToString(),
                    stat_ArabicName = dr["stat_ArabicName"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SprtMsgJson(DataTable dt)
        {
            List<SPMOutParams> listItems = new List<SPMOutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new SPMOutParams
                {

                    csd_Message = dr["csd_Message"].ToString(),
                    csd_Image = dr["csd_Image"].ToString(),
                    Mode = dr["Mode"].ToString(),
                    CreatedBy = dr["CreatedBy"].ToString(),
                    CreatedDate = dr["CreatedDate"].ToString(),
                    csp_ID = dr["csp_ID"].ToString(),
                    EnableEscalate = dr["EnableEscalate"].ToString(),
                    CreatedByArabic = dr["CreatedByArabic"].ToString()
                }); ;
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string CusSupHdrJson(DataTable dt)
        {
            List<CSPHOutParams> listItems = new List<CSPHOutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new CSPHOutParams
                {


                    csp_ID = dr["csp_ID"].ToString(),
                    csp_Title = dr["csp_Title"].ToString(),
                    csp_Message = dr["csp_Message"].ToString(),
                    csp_Image = dr["csp_Image"].ToString(),
                    csp_Reason = dr["csp_Reason"].ToString(),
                    csp_Number = dr["csp_Number"].ToString(),
                    CreatedDate = dr["CreatedDate"].ToString(),
                    csp_cus_ID = dr["csp_cus_ID"].ToString(),
                    stat_Name = dr["stat_Name"].ToString(),
                    csp_CurrentLevel = dr["csp_CurrentLevel"].ToString(),
                    

                    EnableEscalate = dr["EnableEscalate"].ToString(),
                    stat = dr["stat"].ToString()
                   
                    
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelLoginJson(DataTable dt)
        {
            List<UserLogin> listItems = new List<UserLogin>();
            foreach (DataRow dr in dt.Rows)
            {
               
                listItems.Add(new UserLogin
                {
                    Title = dr["Title"].ToString(),
                    Descr = dr["Descr"].ToString(),
                    userID = dr["userID"].ToString(),
                    firstName = dr["firstName"].ToString(),
                    lastName = dr["lastName"].ToString(),
                    FirstnameArbic = dr["FirstnameArbic"].ToString(),
                    LastnameArabic = dr["LastnameArabic"].ToString(),
                    cus_ID = dr["cus_ID"].ToString(),
                    MobileNumber = dr["MobileNumber"].ToString(),
                    cus_Name = dr["cus_Name"].ToString(),
                    cus_Code = dr["cus_Code"].ToString(),
                    cus_Currency = dr["cus_Currency"].ToString(),
                    cus_CountryCode = dr["cus_CountryCode"].ToString(),
                    cus_VAT = dr["cus_VAT"].ToString(),
                    cus_MobileNumber = dr["cus_MobileNumber"].ToString(),
                    email = dr["email"].ToString(),
                    cus_AvailableCredit = dr["cus_AvailableCredit"].ToString(),
                    cus_TotalCredit = dr["cus_TotalCredit"].ToString(),             
                    Roles = dr["Roles"].ToString(),
                    csh_Name = dr["csh_Name"].ToString(),
                    isverified = dr["isVerified"].ToString(),
                    Newuser = dr["NewUser"].ToString(),
                    Type = dr["type"].ToString(),
                    csh_ID = dr["csh_ID"].ToString()
                });
            }
           
        
            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelCusProfile(DataTable dt)                             ///NEW
        {
            List<CustomerProfileOut> listItems = new List<CustomerProfileOut>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new CustomerProfileOut
                {
                    CusName= dr["cus_Name"].ToString(),
                    CshName= dr["csh_Name"].ToString(),
                    UserName = dr["UserName"].ToString(),
                    Mobile = dr["MobileNumber"].ToString(),
                    Email = dr["Email"].ToString(),
                    CusCode = dr["cus_Code"].ToString(),
                    Roles = dr["Roles"].ToString(),
                    Currency = dr["cus_Currency"].ToString(),
                    TotCreditLimit = dr["cus_TotalCredit"].ToString(),
                    CreditDays = dr["cus_CreditDays"].ToString(),
                    UsedCreditLimits = dr["cus_UsedCredit"].ToString(),
                    AvailableCreditLimit = dr["cus_AvailableCredit"].ToString(),
                    CusArabicName = dr["cus_ArabicName"].ToString()

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelSpecialPriceDatail(DataTable dt)                             ///NEW
        {
            List<SpecialPriceDetailOut> listItems = new List<SpecialPriceDetailOut>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new SpecialPriceDetailOut
                {
                Code = dr["Code"].ToString(),
                itm_Name = dr["Name"].ToString(),
                cat_Code = dr["CategoryCode"].ToString(),
                category = dr["Category"].ToString(),
                brand = dr["Brand"].ToString(),
                uom_Name = dr["UOM"].ToString(),
                OfferPrice = dr["OfferPrice"].ToString(),
                standardPrice = dr["StandardPrice"].ToString(),
                ReturnPrice = dr["ReturnPrice"].ToString(),
                VAT = dr["VAT"].ToString()
                });

            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelUserList(DataTable dt)                             ///NEW
        {
            List<UserlistOut> listItems = new List<UserlistOut>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new UserlistOut
                {
                    Name = dr["UserName"].ToString(),
                    MobileNumber = dr["MobileNumber"].ToString(),
                    Email = dr["Email"].ToString(),
                    Status = dr["UserStatus"].ToString(),
                    ApprStatus = dr["ApprStatus"].ToString(),
                    type = dr["Type"].ToString(),
                    OutletName= dr["Customer"].ToString(),
                    CreatedOn= dr["CreatedOn"].ToString(),
                    Roles= dr["Roles"].ToString(),

                });

            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelDocuments(DataTable dt)
        {
            List<DocumetsOut> listItems = new List<DocumetsOut>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new DocumetsOut
                {
                    Id = dr["csd_ID"].ToString(),
                    DocNo = dr["csd_docNo"].ToString(),
                    Type = dr["cdt_Type"].ToString(),
                    StartDate = dr["StartDate"].ToString(),
                    ExpDate = dr["ExpiryDate"].ToString(),
                    UploadBy = dr["name"].ToString(),
                    UploadDate = dr["CreatedDate"].ToString(),
                    Status = dr["Status"].ToString(),
                    Document = dr["csd_docPath"].ToString(),


                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }
        public string InsDocOut(DataTable dt)
        {
            List<InsDocOut> listItems = new List<InsDocOut>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new InsDocOut
                {
                    Res = dr["Res"].ToString(),
                    Status = dr["status"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string UpdateCusDoc(DataTable dt)
        {
            List<UpdateDocOut> listItems = new List<UpdateDocOut>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new UpdateDocOut
                {
                    Res = dr["Res"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string InsNewUser(DataTable dt) //New
        {
            List<ReqUserOut> listItems = new List<ReqUserOut>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new ReqUserOut
                {
                    Res = dr["Res"].ToString(),
                    Status = dr["status"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelCustomers(DataTable dt)                 //New
        {
            List<SelectCustomersOut> listItems = new List<SelectCustomersOut>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new SelectCustomersOut
                {
                    CusID = dr["cus_ID"].ToString(),
                    CusName = dr["Cus_Name"].ToString()


                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }
        public string SelSwitchedCustomerInfo(DataTable dt)                 //New
        {
            List<CustomerInfoOut> listItems = new List<CustomerInfoOut>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new CustomerInfoOut
                {
                    
                    
                    cus_ID = dr["cus_ID"].ToString(),
                    cus_Name = dr["cus_Name"].ToString(),
                    cus_Code = dr["cus_Code"].ToString(),
                    cus_Currency = dr["cus_Currency"].ToString(),
                    cus_CountryCode = dr["cus_CountryCode"].ToString(),
                    cus_VAT = dr["cus_VAT"].ToString(),
                    cus_AvailableCredit = dr["cus_AvailableCredit"].ToString(),
                    cus_TotalCredit = dr["cus_TotalCredit"].ToString(),
                    


                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }
        //public string SelLogininvalidJson()
        //{

        //    List<invalidUserLogin> listItems = new List<invalidUserLogin>();

        //    listItems.Add(new UserLogin
        //        {
        //            Title = "",
        //            Descr = dr["Descr"].ToString(),
        public string Loginfail()
        {
            List<UserLogin> listItems = new List<UserLogin>();


            listItems.Add(new UserLogin
            {

                Title = "Failure",
                Descr = "Invalid Username/Password..!",
                 userID = "",
                firstName = "",
                lastName ="",
                FirstnameArbic ="",
                LastnameArabic = "",
                cus_ID = "",
                MobileNumber = "",
                cus_Name = "",
                cus_Code = "",
                cus_Currency = "",
                cus_CountryCode ="",
                cus_VAT = "",
                cus_MobileNumber = "",
                email ="",
                cus_AvailableCredit = "",
                cus_TotalCredit = "",
                Roles = "",
                csh_Name ="",
                isverified = "",
                Newuser = ""


            });


            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelPromoJson(DataTable dt)
        {
            List<PromoParams> listItems = new List<PromoParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new PromoParams
                {
                    prm_ID = dr["prm_ID"].ToString(),
                    prm_Image = dr["prm_Image"].ToString(),
                    prm_Name = dr["prm_Name"].ToString(),
                    rcp_EndDate = dr["rcp_EndDate"].ToString(),
                    rcp_cus_ID = dr["rcp_cus_ID"].ToString(),
                    itmCount = dr["itmCount"].ToString(),
                    prt_Name = dr["prt_Name"].ToString(),
                    rcp_FromDate = dr["rcp_FromDate"].ToString(),
                    prt_Value = dr["prt_Value"].ToString(),
                    prm_ArabicName = dr["prm_ArabicName"].ToString(),
                    prt_ArabicName = dr["prt_ArabicName"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelActFGPromosJson(DataTable dt)
        {
            List<ActFGParams> listItems = new List<ActFGParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new ActFGParams
                {
                    prm_ID = dr["prm_ID"].ToString(),
                    prm_Name = dr["prm_Name"].ToString(),
                    rcp_cus_ID = dr["rcp_cus_ID"].ToString(),
                    prt_Name = dr["prt_Name"].ToString(),
                    
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelActPromosJson(DataTable dt)
        {
            List<ActParams> listItems = new List<ActParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new ActParams
                {
                    prm_ID = dr["prm_ID"].ToString(),
                    prm_Name = dr["prm_Name"].ToString(),
                    rcp_cus_ID = dr["rcp_cus_ID"].ToString(),
                    prt_Name = dr["prt_Name"].ToString(),
                    prt_Value = dr["prt_Value"].ToString(),
                    prm_ArabicName = dr["prm_ArabicName"].ToString(),
                    prt_ArabicName = dr["prt_ArabicName"].ToString(),

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelRangeByQtyJson(DataTable dt)
        {
            List<SelRangeQty> listItems = new List<SelRangeQty>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new SelRangeQty
                {
                    
                    prr_ID = dr["prr_ID"].ToString(),
                    prr_Value = dr["prr_Value"].ToString(),
                    prr_MinValue = dr["prr_MinValue"].ToString(),
                    prr_MaxValue = dr["prr_MaxValue"].ToString(),

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelfeaturedsubcatJson(DataTable dt)
        {
            List<Featuredsubcat> listItems = new List<Featuredsubcat>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new Featuredsubcat
                {
                    sct_Image = dr["sct_Image"].ToString(),
                    sct_ID = dr["sct_ID"].ToString(),
                    sct_Name = dr["sct_Name"].ToString(),
                    itmCount = dr["itmCount"].ToString(),
                    sct_cat_ID = dr["sct_cat_ID"].ToString(),
                    sct_ArabicName = dr["sct_ArabicName"].ToString(),



                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelfeaturedCategoryJson(DataTable dt)
        {
            List<Featuredcat> listItems = new List<Featuredcat>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new Featuredcat
                {
                    fct_Name = dr["fct_Name"].ToString(),
                    fct_Image = dr["fct_Image"].ToString(),             
                    tct_cat_ID = dr["tct_cat_ID"].ToString(),
                    cat_Name = dr["cat_Name"].ToString(),
                    fct_ArabicName = dr["fct_ArabicName"].ToString(),
                    cat_ArabicName = dr["cat_ArabicName"].ToString()
                   

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelInvJson(DataTable dt)
        {
            List<InvParams> listItems = new List<InvParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new InvParams
                {
                    inv_ID = dr["inv_ID"].ToString(),
                    inv_Number = dr["inv_Number"].ToString(),
                    inv_PaymentMode = dr["inv_PaymentMode"].ToString(),
                    InvoicedOn = dr["InvoicedOn"].ToString(),
                    inv_SubTotal = dr["inv_SubTotal"].ToString(),
                    inv_Vat = dr["inv_Vat"].ToString(),
                    inv_GrandTotal = dr["inv_GrandTotal"].ToString(),
                    Status = dr["Status"].ToString(),
                    inv_PaymentMode_Arabic = dr["inv_PaymentMode_Arabic"].ToString(),
                    ArabicStatus = dr["inv_PaymentMode_Arabic"].ToString(),
                    Balance = dr["inv_Balance"].ToString()


                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelInvDetJson(DataTable dt)
        {
            List<InvDetoutParams> listItems = new List<InvDetoutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new InvDetoutParams
                {
                    ind_ID = dr["ind_ID"].ToString(),
                    itm_ID = dr["itm_ID"].ToString(),
                    itm_Name = dr["itm_Name"].ToString(),
                    itm_Image =  dr["itm_Image"].ToString().Replace("../",""),
                    itm_Code = dr["itm_Code"].ToString(),
                    HUOM = dr["HUOM"].ToString(),
                    LUOM = dr["LUOM"].ToString(),
                    ind_HPrice = dr["ind_HPrice"].ToString(),
                    ind_LPrice = dr["ind_LPrice"].ToString(),
                    ind_SubTotal = dr["ind_SubTotal"].ToString(),
                    ind_VAT = dr["ind_VAT"].ToString(),
                    ind_GrandTotal = dr["ind_HPrice"].ToString(),
                    ind_HQty = dr["ind_HQty"].ToString(),
                    ind_LQty = dr["ind_LQty"].ToString(),
                    uom_ArabicName = dr["uom_ArabicName"].ToString(),
                    itm_ArabicName = dr["itm_ArabicName"].ToString(),
                    HUOMID = dr["HUOMID"].ToString(),
                    LUOMID = dr["LUOMID"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelPrmTotalCount(DataTable dt)
        {
            List<PrmTotCountOut> listItems = new List<PrmTotCountOut>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new PrmTotCountOut
                {
                    TotalPcs = dr["TotalPcs"].ToString(),
                    ElgQty = dr["ElgQty"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelPrmAssgnQty(DataTable dt)
        {
            List<PrmTotCountOut> listItems = new List<PrmTotCountOut>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new PrmTotCountOut
                {
                    TotalPcs = dr["TotalPcs"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelVerStatus(string res)
        {
            List<otpgenerateout> listItems = new List<otpgenerateout>();
          
            listItems.Add(new otpgenerateout
            {
                VerStatus = res
            });
            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelProceed(DataTable dt)
        {
            List<otpproceedout> listItems = new List<otpproceedout>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new otpproceedout
                {
                    VerProceed = dr["res"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
           

        }
        public string SelVerify(string res,string msg)
        {
            List<verifyout> listItems = new List<verifyout>();
            listItems.Add(new verifyout
            {
                isVerified = res,
                res = msg


            }); 
          

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;


        }
       
        public string SelchangePassStatus()
        {
            List<changePassout> listItems = new List<changePassout>();

            listItems.Add(new changePassout
            {
                Mode = "1",
                Status = "Password updated successfully"
               
            });
            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelchangePassfail()
        {
            List<changePassout> listItems = new List<changePassout>();

            listItems.Add(new changePassout
            {
                Mode = "0",
                Status = "Your old password is wrong, please try again"
            });
            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelBrochure(DataTable dt)
        {
            List<brochureout> listItems = new List<brochureout>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new brochureout
                {

                    Url = dr["com_Brochure"].ToString()
                   
                });
            }
            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelOutStandingInv(DataTable dt)
        {
            List<OutStandingInvout> listItems = new List<OutStandingInvout>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new OutStandingInvout
                {
                    Name = dr["Name"].ToString(),

                    InvoiceID = dr["InvoiceID"].ToString(),

                    InvoicedOn = dr["InvoicedOn"].ToString(),

                    InvoiceAmount = dr["InvoiceAmount"].ToString(),

                    PendingAmount = dr["PendingAmount"].ToString()
                  

                });
            }
            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelExpDelDateJson(DataTable dt)
        {
            List<ExpDelDateout> listItems = new List<ExpDelDateout>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new ExpDelDateout
                {
                    DelDate = dr["NextDay"].ToString()

                   
                });
            }
            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelcutoffJson(DataTable dt)
        {
            List<Cutoffout> listItems = new List<Cutoffout>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new Cutoffout
                {
                    cus_ID = dr["cus_ID"].ToString(),
                    cus_Name = dr["cus_Name"].ToString(),
                    ID = dr["ID"].ToString(),
                    UserName = dr["UserName"].ToString(),
                    name = dr["name"].ToString(),
                    NewUser = dr["NewUser"].ToString(),
                    Roles = dr["Roles"].ToString(),
                    RoleName = dr["RoleName"].ToString(),
                    Timings = dr["Timings"].ToString(),
                    Message = "Orders placed after " + dr["Timings"].ToString() +" will be delivered within two business days,excluding holidays.",
                    ArabicMessage= "مساءً خلال يومي عمل، باستثناء أيام العطلات" + dr["Timings"].ToString() + "سيتم تسليم الطلبات المقدمة بعد الساعة"

                });
            }
            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelReasonJson(DataTable dt)
        {
            List<ReasonParams> listItems = new List<ReasonParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new ReasonParams
                {
                    rsn_ID = dr["rsn_ID"].ToString(),
                    rsn_Name = dr["rsn_Name"].ToString(),
                    rsn_Type = dr["rsn_Type"].ToString(),
                    Status = dr["Status"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelSearchItemsJson(DataTable dt)
        {
            List<SearchParams> listItems = new List<SearchParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new SearchParams
                {
                    itm_Name = dr["itm_Name"].ToString(),
                  

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelOrderPDF(string res)
        {
            List<orderpdfout> listItems = new List<orderpdfout>();
            
                listItems.Add(new orderpdfout
                {

                    Url = res

                });
          
            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }

        public string SelForgotpass(string res, string messg)
        {
            List<forgotpassout> listItems = new List<forgotpassout>();

            listItems.Add(new forgotpassout
            {
                res = res,
                msg = messg
              

            });

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;

        }
        public string SelCompInfo(DataTable dt)
        {
            List<CompanyInfoOut> listItems = new List<CompanyInfoOut>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new CompanyInfoOut
                {
                    Tollfree = dr["com_HelpLineNumber"].ToString(),
                    Time = dr["com_WorkingHours"].ToString(),
                    Address = dr["com_Address"].ToString(),
                    Support= dr["com_SupportCenter"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }

        public string SelDisputeNoteRequestHeader(DataTable dt)
        {
            List<DesputeOut> listItems = new List<DesputeOut>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new DesputeOut
                {
                    drhID= dr["drh_ID"].ToString(),
                    Type = dr["DisputeType"].ToString(),
                    Date = dr["CreatedDate"].ToString(),
                    ReqID = dr["drh_TransID"].ToString(),
                    Amount = dr["drh_Amount"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }

        public string SelCreditNoteRequestHeader(DataTable dt)
        {
            List<CreditOut> listItems = new List<CreditOut>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new CreditOut
                {
                    crhID = dr["cnh_ID"].ToString(),
                    Date = dr["CreatedDate"].ToString(),
                    ReqID = dr["cnh_Number"].ToString(),
                    Amount = dr["cnh_Amount"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }

        public string SelCustomerRequestHeader(DataTable dt)
        {
            List<NewReqOut> listItems = new List<NewReqOut>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new NewReqOut
                {
                    CusReqID = dr["req_ID"].ToString(),
                    Type = dr["rqt_Name"].ToString(),
                    Date = dr["CreatedDate"].ToString(),
                    ReqID = dr["req_TransID"].ToString(),
                    Status = dr["Status"].ToString(),
                    ReqCode = dr["req_Code"].ToString()
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }

        public string SelRetReqHeader(DataTable dt)
        {
            List<RetReqOut> listItems = new List<RetReqOut>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new RetReqOut
                {
                    rrhID = dr["rrh_ID"].ToString(),
                    Date = dr["CreatedDate"].ToString(),
                    ReqID = dr["rrh_RequestNumber"].ToString(),
                    InvoiceNo = dr["inv_Number"].ToString(),
                    InvDate= dr["InvTime"].ToString(),
                    VAT= dr["rrh_VatPerc"].ToString(),
                    SubTotal= dr["rrh_SubTotal"].ToString(),
                    Total= dr["rrh_Total"].ToString(),

                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }


        public string InsertCNReq(DataTable dt)
        {
            List<OutParams> listItems = new List<OutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new OutParams
                {
                    Res = dr["Res"].ToString(),
                    Title = dr["Title"].ToString(),
                    Descr = dr["Descr"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }
        public string InsertCustRequest(DataTable dt)
        {
            List<OutParams> listItems = new List<OutParams>();
            foreach (DataRow dr in dt.Rows)
            {

                listItems.Add(new OutParams
                {
                    Res = dr["Res"].ToString(),
                    Title = dr["Title"].ToString(),
                    Descr = dr["Descr"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }

        public string InsReturnRequest(DataTable dt)
        {
            List<ScheduledReturnout> listDn = new List<ScheduledReturnout>();
            foreach (DataRow dr in dt.Rows)
            {
                listDn.Add(new ScheduledReturnout
                {
                    Res = dr["Res"].ToString(),
                    Message = dr["Message"].ToString(),
                    TransID = dr["TransID"].ToString()



                });
            }
            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listDn
            });

            return JSONString;
        }

        public string SelCreditNoteRequestHeaderByID(DataTable dt)
        {
            List<CNRHeaderByIDOut> listItems = new List<CNRHeaderByIDOut>();
            foreach (DataRow dr in dt.Rows)
            {
                listItems.Add(new CNRHeaderByIDOut
                {
                    crhID = dr["cnh_ID"].ToString(),
                    Date = dr["CreatedDate"].ToString(),
                    ReqID = dr["cnh_Number"].ToString(),
                    Vat = dr["ind_Vat"].ToString(),
                    SubTotal = dr["ind_SubTotal"].ToString(),
                    GrandTotal = dr["ind_GrandTotal"].ToString(),
                });
            }

            string JSONString = JsonConvert.SerializeObject(new
            {
                result = listItems
            });

            return JSONString;
        }
    }

}

