using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace b2b_solution.Models
{
    public class AppParams
    {
    }

    public class GetItem
    {
        public string Category { get; set; }

        public string CatID { get; set; }
        public string itmID { get; set; }
        public string itmName { get; set; }
        public string itmImage { get; set; }
        public string sctName { get; set; }
        public string HUOM { get; set; }
        public string Hqty { get; set; }
        public string LUOM { get; set; }
        public string Lqty { get; set; }
        public List<ItemUOM> UOM { get; set; }
       
        public string cat_ArabicName { get; set; }
        public string itm_ArabicName { get; set; }
        public string sct_ArabicName { get; set; }

        public string itmCode { get; set; }
        public string FreqOrder { get; set; }
        public string itm_Type { get; set; }
        public string brd_Name { get; set; }
    }


    public class ItemUOM
    {
        public string uomID { get; set; }
        public string uomName { get; set; }
        public string UPC { get; set; }
        public string IsDefault { get; set; }
        public string Offerprice { get; set; }
        public string StandardPrice { get; set; }
        public string cusID { get; set; }

    }
    public class RecentItem
    {
        public string catName { get; set; }

        public string catID { get; set; }
        public string itmID { get; set; }
        public string itmName { get; set; }
        public string itmImage { get; set; }
        public string sctName { get; set; }
        public string itmCount { get; set; }
      
        public string CHUOM { get; set; }
        public string CHQty { get; set; }

        public string CLUOM { get; set; }
        public string CLQty { get; set; }

        public string itm_ArabicName { get; set; }
        public string sct_ArabicName { get; set; }
        public string cat_ArabicName { get; set; }
        public string itmCode { get; set; }
        public string FreqOrder { get; set; }
        public string itm_Type { get; set; }
        public List<ItemUOM> UOM { get; set; }
        public string brd_Name { get; set; }
    }

    public class InParams
    {
        public string prdName { get; set; }
        public string catID { get; set; }
        public string sctID { get; set; }
        public string brd_ID { get; set; }
        public bool specialPrice { get; set; }
        public string PageNum { get; set; } 
        public string userID { get; set; }
        public string cusID { get; set; }
    }
    public class OrdParams
    {
        public string OrdID { get; set; }
        public string OrdGrandTotal { get; set; }
        public string OrdDate { get; set; }
        public string Orditm_Count { get; set; }
    }

    public class cartParams
    {
        public string userID { get; set; }
        public string cusID { get; set; }
        public string itm_ID { get; set; }
        public string HigherQty { get; set; }
        public string HigherUOM { get; set; }
        public string LowerQty { get; set; }
        public string lowerUOM { get; set; }
    }

    public class PromocartParams
    {
        public string userID { get; set; }
        public string prdid { get; set; }
        public string ashID { get; set; }

        public string prmID { get; set; }
        public string HigherQty { get; set; }

        public string HigherUOM { get; set; }
        public string LowerQty { get; set; }
        public string LowerUOM { get; set; }
        public string prrVal { get; set; }

        public string cusID { get; set; }

    }


    public class CartTotalParams
    {
        public string userID { get; set; }
        public string cusID { get; set; }

    }

   

public class InsNewAddParams
    {
        public string userID { get; set; }
        public string BuldName { get; set; }
        public string RoomNo { get; set; }
        public string Street { get; set; }
        public string LandMark { get; set; }
        public string State { get; set; }
        public string CusID { get; set; }


    }

  
    public class CUpdMsgParams
    {
        public string userID { get; set; }
        public string CspID { get; set; }

    }
    public class InsSupMsgParams
    {
        public string userID { get; set; }
        public string CspID { get; set; }
        public string message { get; set; }
        public Bitmap img { get; set; }

    }
    public class InsCusSupParams
    {
        public string userID { get; set; }
        public string cus_ID { get; set; }
        public string message { get; set; }
        public string Title { get; set; }
        public string reason { get; set; }

        public  Bitmap img { get; set; }
}
    public class QualiGrpInParams
    {
        public string userID { get; set; }
        public string cusID { get; set; }
        public string prmID { get; set; }
    }
    public class PromorangeIn
    {
        public string prmID { get; set; }
        public string qty { get; set; }

    }
    public class ARInParams
    {
        
        public string cusID { get; set; }
        public string userID { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }

    }
    public class FGParams
    {

        public string CmpPrmID { get; set; }
        public string userID { get; set; }

        public string CusID { get; set; }

    }
    public class ActPromoParams
    {

       
        public string userID { get; set; }

        public string cusID { get; set; }

    }
    public class SelRangeParams
    {

        public string PrmID { get; set; }
        public string Range { get; set; }

       
    }
    public class DelAddParams
    {

        public string cuaID { get; set; }

    }
    public class DeleteAddJson
    {

        public string cuaID { get; set; }

    }
    public class SPMInParams
    {

        public string CspID { get; set; }

    }
    public class BrandInParams
    {
        public string cusID { get; set; }
        public string CatID { get; set; }

    }
    public class InvDetParams
    {

        public string InvID { get; set; }

    }

    public class ARDetailInParams
    {

        public string arc_ID { get; set; }

    }

    public class GetOrderInParams
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string cusID { get; set; }

    }
    public class StateParams
    {
        public string StateID { get; set; }
        public string State { get; set; }
        public string StateArabic { get; set; }

    }
    public class CSPOutParams
    {
        public string csp_ID { get; set; }
        public string csp_Title { get; set; }
        public string csp_Message { get; set; }
        public string csp_Image { get; set; }
        public string csp_Reason { get; set; }
        public string csp_Number { get; set; }
        public string CreatedDate { get; set; }
        public string csp_cus_ID { get; set; }
        public string stat_Name { get; set; }
        public string stat_ArabicName { get; set; }
        

    }
    public class CSPHOutParams
    {
        public string csp_ID { get; set; }
        public string csp_Title { get; set; }
        public string csp_Message { get; set; }
        public string csp_Image { get; set; }
        public string csp_Reason { get; set; }
        public string csp_Number { get; set; }
        public string CreatedDate { get; set; }
        public string csp_cus_ID { get; set; }
        public string stat_Name { get; set; }
        public string EnableEscalate { get; set; }
       
        public string csp_CurrentLevel { get; set; }
        public string stat { get; set; }

      
    }
    public class SPMOutParams
    {
        public string csd_Message { get; set; }
        public string csd_Image { get; set; }
        public string Mode { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string csp_ID { get; set; }
       
        public string EnableEscalate { get; set; }
        public string CreatedByArabic { get; set; }
        

    }
    public class SubCatParams
    {
        public string sct_ID { get; set; }
        public string sct_Name { get; set; }
        public string sct_Code { get; set; }
        public string sct_cat_ID { get; set; }
        public string sct_Image { get; set; }
        public string sct_ArabicName { get; set; }
        
    }
    public class ARDOutParams
    {
        public string inv_Number { get; set; }
        public string inv_GrandTotal { get; set; }
        public string ard_PaidAmount { get; set; }
        public string inv_Date { get; set; }
        public string inv_SubTotal { get; set; }



    }
    public class GetAllOrder
    {
        public string id { get; set; }
        public string OrderNo { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string SubTotal { get; set; }
        public string VAT { get; set; }
        public string GradndTotal { get; set; }
        public string LPO { get; set; }
        public string ExpectedDate { get; set; }
        public string Attachment { get; set; }

        public string Total { get; set; }
        public string Discount { get; set; }
        public string Counts { get; set; }
        public string ArabicStatus { get; set; }
        
    }

    public class GetorderByID
    {
        public string id { get; set; }
        public string itmName { get; set; }
        public string itmImage { get; set; }
        public string itmCode { get; set; }
        public string HUOM { get; set; }
        public string LUOM { get; set; }
        public string HQty { get; set; }
        public string LQty { get; set; }
        public string HPrice { get; set; }
        public string Lprice { get; set; }
        public string SubTotal { get; set; }
        public string VAT { get; set; }
        public string GrandTotal { get; set; }
        public string TransType { get; set; }
        public string Total { get; set; }
        public string Discount { get; set; }
        public string itm_ArabicName { get; set; }
    }

    public class DeleteCartParams
        {
            public string userID { get; set; }
            public string itmID { get; set; }
            public string cusID {  get; set; }

        }
        public class CartCountParams
        {
            public string userID { get; set; }
            public string cusID { get; set; }


        }
    public class GetOrdDetailOutParams
    {
        public string OrdID { get; set; }


    }
        public class CartCountoutParams
        {
             public string count { get; set; }
        }
    public class PromorangeoutParams
    {
       
        public string count { get; set; }
        public string prrID { get; set; }
        public string min { get; set; }
        public string max { get; set; }
        public string desc1 { get; set; }
        public string desc2 { get; set; }
        public string ArabicDesc1 { get; set; }
}
    public class ordtrackoutparams
    {

        public string CreatedDate { get; set; }
        public string olc_Remarks { get; set; }
        public string olc_Status { get; set; }
        public string olc_ArabicRemarks { get; set; }
        public string olc_ArabicStatus { get; set; }
        
                    

    }
    public class carttotParams
        {
            public string SubTotal { get; set; }
            public string VAT { get; set; }
            public string GrandTotal { get; set; }
            public string TotalCreditLimit { get; set; }
            public string TotalAvailableLimit { get; set; }
            public string MinOrderValue { get; set; }
            public string CreaditDays { get; set; }
            public string Discount { get; set; }

    }
        public class cartitemParams
        {
            public string itm_ID { get; set; }
            public string itm_Name { get; set; }
            public string itm_Image { get; set; }
            public string sct_Name { get; set; }
            public string cat_Name { get; set; }
            public string OfferPrice { get; set; }
            public string crd_HigherQty { get; set; }
            public string crd_HigherUOM { get; set; }
            public string crd_LowerQty { get; set; }
            public string crd_lowerUOM { get; set; }
            public string LowerPrice { get; set; }
            public string Total { get; set; }

            public string HUPC { get; set; }
            public string LUPC { get; set; }
            public string TotalDisc { get; set; }
            public string itm_ArabicName { get; set; }
            public string sct_ArabicName { get; set; }
            public string cat_ArabicName { get; set; }
            public string itmCode { get; set; }
        public List<ItemUOM> UOM { get; set; }
            
         

        }

    public class AssignitemParams
    {
        public string itm_ID { get; set; }
        public string itm_Name { get; set; }
        public string itm_Image { get; set; }
        public string sct_Name { get; set; }
        public string cat_Name { get; set; }
        public string OfferPrice { get; set; }
        public string crd_HigherQty { get; set; }
        public string crd_HigherUOM { get; set; }
        public string crd_LowerQty { get; set; }
        public string crd_lowerUOM { get; set; }
        public string LowerPrice { get; set; }
        public string Total { get; set; }

       
        public List<ItemUOM> UOM { get; set; }
        public string cat_ArabicName { get; set; }
        public string itm_ArabicName { get; set; }
        public string sct_ArabicName { get; set; }

    }

    public class OutParams
        {
            public string Res { get; set; }
            public string Title { get; set; }
            public string Descr { get; set; }
    }
    public class insAddressOutParams
    {
        public string Res { get; set; }
        public string Title { get; set; }
        
    }

    public class insPromocartOutParams
    {
        public string Res { get; set; }
       

    }

    public class selAshid
    {
        public string prmID { get; set; }


    }
    public class selAshidout
    {
        public string ashID { get; set; }


    }


    public class ShipAddOutParams
    {
        public string cua_ID { get; set; }
        public string cua_BuldgName { get; set; }

        public string cua_LandMark { get; set; }
        public string cua_RoomNo { get; set; }
        public string cua_Street { get; set; }

        public string sta_ID { get; set; }
        public string sta_Name { get; set; }
        public string cua_IsDefault { get; set; }

    }


    public class BillAddOutParams
    {
        public string cus_BuildingName { get; set; }
        public string cus_Street { get; set; }

        public string cus_LandMark { get; set; }
        public string cus_RoomNo { get; set; }
        public string sta_Name { get; set; }

       
    }
    public class updmsgrslParams
    {
        public string res { get; set; }
        public string status { get; set; }
        public string remarks { get; set; }
        public string seqNum { get; set; }

         
    }
    public class brandParams
    {
        public string brd_ID { get; set; }
        public string brd_Name { get; set; }
        public string brd_Image { get; set; }
        public string brd_Code { get; set; }
        public string brd_ArabicName {  get; set; }
        
    }

    public class categoryParams
        {
            public string id { get; set; }
            public string Name { get; set; }
            public string Img { get; set; }
            public string ArabicName { get; set; }
        
    }

        public class ReorderParams
        {

            public string odd_ord_ID { get; set; }
            public string cusID { get; set; }
            public string userID { get; set; }
        }
        public class BannerParams
        {

            public string Heading { get; set; }
            public string image { get; set; }
            public string ShortMessage { get; set; }
            public string ArabicHeading { get; set; }
            public string ArabicShortMessage { get; set; }
        
        } 
    

        public class PlaceOrderInputs
        {
            public string cusID { get; set; }
            public string userID { get; set; }
            public string ExpDate { get; set; }

            public string BuildingName { get; set; }
            public string RoomNo { get; set; }
            public string Street { get; set; }

            public string LandMark { get; set; }
            public string Emirate { get; set; }

            public string LPO { get; set; }
            public string SubTotal { get; set; }
            public string VAT { get; set; }

            public string GrandTotal { get; set; }
            public string Remarks { get; set; }

            public Bitmap attachment { get; set; }
        }

    public class ARParams
    {
        public string arc_ID { get; set; }
        public string arc_Code { get; set; }
        public string arc_PaymentMode { get; set; }

        public string ChequeDate { get; set; }
        public string arc_ChequeImg { get; set; }
        public string arc_ChequeNo { get; set; }

        public string arc_SubTotal { get; set; }
        public string arc_Vat { get; set; }

        public string arc_GrandTotal { get; set; }
        public string bank_Name { get; set; }
        public string CreatedDate { get; set; }
        public string arc_PaymentMode_Arabic { get; set; }
        public string bank_Name_Arabic { get; set; }
    }
    public class Credential
    {
        public string username { get; set; }
        public string password { get; set; }
       
    }

    public class UserLogin
    {
        public string Title { get; set; }
        public string Descr { get; set; }
        public string userID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string FirstnameArbic { get; set; }
        public string LastnameArabic { get; set; }
        public string cus_ID { get; set; }
        public string MobileNumber { get; set; }
        public string cus_Name { get; set; }
        public string cus_Code { get; set; }
        public string cus_Currency { get; set; }
        public string cus_CountryCode { get; set; }
        public string cus_VAT { get; set; }
        public string cus_MobileNumber { get; set; }
        public string email { get; set; }
        public string cus_AvailableCredit { get; set; }
        public string cus_TotalCredit { get; set; }      
        public string Roles { get; set; }
        public string csh_Name { get; set; }
        public string isverified { get; set; }
        public string Newuser { get; set; }
        public string Type { get; set; }
        public string csh_ID { get; set; }

    }
   
    public class invalidUserLogin
    {
        public string Title { get; set; }
        public string Descr { get; set; }
    }
    public class PromoParams
    {
        public string prm_ID { get; set; }
        public string prm_Image { get; set; }
        public string prm_Name { get; set; }
        public string rcp_EndDate { get; set; }
        public string rcp_cus_ID { get; set; }

        public string itmCount { get; set; }
        public string prt_Name { get; set; }
        public string rcp_FromDate { get; set; }
        public string prt_Value { get; set; }
        public string prm_ArabicName { get; set; }
        public string prt_ArabicName { get; set; }

                

    }

    public class ActFGParams
    {
        public string prm_ID { get; set; }
        public string prm_Name { get; set; }
        public string rcp_cus_ID { get; set; }
        public string prt_Name { get; set; }
       
    }
    public class ActParams
    {
        public string prm_ID { get; set; }
        public string prm_Name { get; set; }
        public string rcp_cus_ID { get; set; }
        public string prt_Name { get; set; }
        public string prt_Value { get; set; }
        public string prm_ArabicName { get; set; }
        public string prt_ArabicName { get; set; }
        


    }
    public class SelRangeQty
    {
        public string prr_ID { get; set; }
        public string prr_Value { get; set; }
        public string prr_MinValue { get; set; }
        public string prr_MaxValue { get; set; }

    }

    public class Featuredsubcat
    {
        public string sct_Image { get; set; }
        public string sct_ID { get; set; }
        public string sct_Name { get; set; }
        public string itmCount { get; set; }
        public string sct_cat_ID { get; set; }
        public string sct_ArabicName { get; set; }
        
    }
    public class Featuredcat
    {
        public string fct_Name { get; set; }
        public string fct_Image { get; set; }
        public string tct_cat_ID { get; set; }
        public string cat_Name { get; set; }
        public string cat_ArabicName { get; set; }
        public string fct_ArabicName { get; set; }

    }
    public class InvParams
    {
        public string inv_ID { get; set; }
        public string inv_Number { get; set; }
        public string inv_PaymentMode { get; set; }
        public string InvoicedOn { get; set; }
        public string inv_SubTotal { get; set; }
        public string inv_Vat { get; set; }
        public string inv_GrandTotal { get; set; }
        public string Status { get; set; }
        public string inv_PaymentMode_Arabic { get; set; }
        public string ArabicStatus { get; set; }
        public string Balance { get; set; }
        
    }
    public class InvDetoutParams
    {
        public string ind_ID { get; set; }
        public string itm_ID { get; set; }
        public string itm_Name { get; set; }

        public string itm_Image { get; set; }
        public string itm_Code { get; set; }
        public string HUOM { get; set; }

        public string LUOM { get; set; }
        public string ind_HPrice { get; set; }

        public string ind_LPrice { get; set; }
        public string ind_SubTotal { get; set; }
        public string ind_VAT { get; set; }
        public string ind_GrandTotal { get; set; }
        public string ind_HQty { get; set; }
        public string ind_LQty { get; set; }
        public string uom_ArabicName { get; set; }
        public string itm_ArabicName { get; set; }
        public string HUOMID { get; set; }

        public string LUOMID { get; set; }


    }

    public class PrmTotalInParams
    {
        public string cusID { get; set; }
        public string userID { get; set; }

        public string prmID { get; set; }
    }

    public class PrmTotCountOut
    {
        public string TotalPcs { get; set; }
        public string ElgQty { get; set; }
    }
    public class Otp
    {
        public string userID { get; set; }
       

    }
    public class OtpProceed
    {
        public string userID { get; set; }
        public string otp { get; set; }

        public string platform = "APP";


    }
    public class otpproceedout
    {
        public string VerProceed { get; set; }
      
    }
    public class otpgenerateout
    {
        public string VerStatus { get; set; }

    }
    public class verify
    {
        public string email { get; set; }

    }
    public class verifyout
    {
        public string isVerified { get; set; }
        public string res { get; set; }
     

    }
    public class changepass
    {
        public string cPass { get; set; }
        public string nPass { get; set; }
        public string uName { get; set; }

        public string userID { get; set; }
    }
    public class changePassout
    {
        public string Mode { get; set; }
        public string Status { get; set; }
      
    }
    public class brochureout
    {
        public string Url { get; set; }
     
    }
    public class OutStandingInvoices
    {
        public string Cus_ID { get; set; }
      
    }
    public class OutStandingInvout
    {
        public string Name { get; set; }
        public string InvoiceID { get; set; }
        public string InvoicedOn { get; set; }
        public string InvoiceAmount { get; set; }
        public string PendingAmount { get; set; }

    }
    public class ExpDelDate
    {
        public string CusID { get; set; }
      

    }
    public class ExpDelDateout
    {
        public string DelDate { get; set; }


    }
    public class cutoff
    {
        public string username { get; set; }


    }
    public class Cutoffout
    {
        public string cus_ID { get; set; }
        public string cus_Name { get; set; }
        public string ID { get; set; }
        public string UserName { get; set; }
        public string name { get; set; }
        public string NewUser { get; set; }
        public string Roles { get; set; }
        public string RoleName { get; set; }
        public string Timings { get; set; }
        public string Message { get; set; }
        public string ArabicMessage {  get; set; }
    }
    public class ReasonParams
    {
        public string rsn_ID { get; set; }
        public string rsn_Name { get; set; }
        public string rsn_Type { get; set; }
        public string Status { get; set; }
      

    }

    public class Search
    {
        public string CusID { get; set; }
        public string text { get; set; }
      
    }
    public class SearchParams
    {
        public string itm_Name { get; set; }
     
    }
    public class OrdPdf
    {
        public string OrderID { get; set; }
        public string OrderNum { get; set; }
        public string cusID { get; set; }
        
    }
    public class orderpdfout
    {
        public string Url { get; set; }
      

    }
    public class forgotpass
    {
        public string email { get; set; }

    }
    public class forgotpassout
    {
        public string res { get; set; }
        public string msg { get; set; }

    }
    public class CustomerProfileIn
    {
        public string CusID { get; set; }
        public string UserID { get; set; }

    }
    public class CustomerProfileOut
    {
        public string CusName { get; set; }
        public string CshName { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public  string Email {  get; set; }
        public string CusCode { get; set; }
        public string Roles { get; set; }
        public string Currency { get; set; }
        public string TotCreditLimit { get; set; }
        public string CreditDays { get; set; }
        public string UsedCreditLimits { get; set; }
        public string AvailableCreditLimit { get;set; }
        public string CusArabicName { get; set; }

    }
    public class SpecialPriceDetailOut
    {
        public string Code { get; set; }
        public string itm_Name { get; set; }
        public string cat_Code { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
        public string uom_Name { get; set; }
        public string OfferPrice { get; set; }
        public string standardPrice { get; set; }
        public string ReturnPrice { get; set; }
        public string VAT { get; set; }
    }
    public class SpecialPriceDetailIn
    {
        public string CusID { get; set; }
        public string UserID { get; set; }
    }
    public class UserListIn
    {
        public string CusID { get; set; }
        public string UserID { get; set; }
    }
    public class UserlistOut
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string ApprStatus { get; set; }
        public string Status { get; set; }
        public string type { get; set; }
        public string Roles { get; set; }
        public string CreatedOn { get; set; }
        public string OutletName { get; set;}
    }
    public class InsDoc
    {
        public string CusID { get; set; }
        public string DocNo { get; set; }
        public string TypeId { get; set; }
        public string StartDate { get; set; }
        public string ExpDate { get; set; }
        public string UploadedBy { get; set; }
        public Bitmap img { get; set; }

    }
    public class InsDocOut
    {
        public string Res { get; set; }
        public string Status {  set; get; }

    }
    public class UpdateDoc
    {
        public string Id { get; set; }
        public string UploadedBy { get; set; }
        public string StartDate { get; set; }
        public string ExpDate { get; set; }
        public string DOcNo { get; set; }
        public Bitmap img { get; set; }

    }
    
    public class UpdateDocOut
    {
        public string Res { get; set; }

    }
    public class DocumetsOut
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string DocNo { get; set; }
        public string StartDate { get; set; }
        public string ExpDate { get; set; }
        public string Document { get; set; }
        public string UploadDate { get; set; }
        public string UploadBy { get; set; }
        public string Status { get; set; }

    }
    public class DocIn
    {
        public string userID { get; set; }
        public string CusID { get; set; }

    }
    public class DocTypeOut
    {
        public string TypeID { get; set; }
        public string Type { get; set; }
        public string IsExpiry { get; set; }   

    }
    public class SelectCustomersIn
    {
        public string csh_ID { get; set; }

    }
    public class SelectCustomersOut
    {
        public string CusID { get; set; }
        public string CusName { get; set; }

    }
    public class CustomerInfoIn
    {
        public string CusID { get; set; }
        public string UserID { get; set; }

    }
    public class CustomerInfoOut
    {
       
        public string cus_ID { get; set; }
        public string cus_Name { get; set; }
        public string cus_Code { get; set; }
        public string cus_Currency { get; set; }
        public string cus_CountryCode { get; set; }
        public string cus_VAT { get; set; }
        public string cus_AvailableCredit { get; set; }
        public string cus_TotalCredit { get; set; }
        public string csh_Name { get; set; }

    }

    public class ReqUserIn
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string contactNum { get; set; } = "";
        public string roles { get; set; }
        public string type { get; set; }
        public string cusid { get; set; }
        public string cshid { get; set; }
        public string createdby { get; set; }

    }
    public class ReqUserOut
    {
        public string Res { get; set; }
        public string Status { get; set; }

    }
    public class CustRoles
    {
        public string rolID { get; set; }
        public string rolName { get; set; }
    }
    public class CompanyInfoIn
    {
        public string CusID { get; set; }
        public string UserID { get; set; }
    }
    public class CompanyInfoOut
    {
        public string Tollfree { get; set; }
        public string Time { get; set; }
        public string Address { get; set; }
        public string Support {  get; set; }

    }

    public class DesputeIN
    {
        public string CusID { get; set; }
        public string UserID { get; set; }
    }

    public class CreditIn
    {
        public string CusID { get; set; }
        public string UserID { get; set; }
    }

    public class NewReqIn
    {
        public string CusID { get; set; }
        public string UserID { get; set; }
    }
    public class RetReqIn
    {
        public string CusID { get; set; }
        public string UserID { get; set; }
    }
    public class DesputeOut
    {
        public string drhID {  get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string ReqID { get; set; }
        public string Amount { get; set; }
    }

    public class CreditOut
    {
        public string crhID { get; set; }
        public string Date { get; set; }
        public string ReqID { get; set; }
        public string Amount { get; set; }
    }

    public class NewReqOut
    {
        public string CusReqID { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string ReqID { get; set; }
        public string Status { get; set; }
        public string ReqCode { get; set; }
    }
    public class RetReqOut
    {
        public string rrhID { get; set; }
        public string ReqID { get; set; }
        public string InvoiceNo { get; set; }
        public string Date { get; set; }
        public string Total { get; set; }
        public string SubTotal { get; set; }
        public string VAT { get; set; }
        public string InvDate { get; set; }
    }
    public class InvoiceOutforCusTrans
    {
        public string InvID { get; set; }
        public string InvNumber { get; set; }
        public string Date { get; set; }
        public string fromDate {  get; set; }
        public string todate {  get; set; }
    }
    public class ScheduledReturnIn
    {
        public string CusID { get; set; }
        public string UserID { get; set; }
        public string type { get; set; }
        public string InvoiceID { get; set; }
        public string SubTotal { get; set; }
        public string Vat { get; set; }
        public string Total { get; set; }
        public string Signature { get; set; }
        public string JSONString { get; set; }



    }
    public class SRItemIDs
    {


        public string prdID { get; set; }
        public string HigherUOM { get; set; }

        public string HigherQty { get; set; }
        public string LowerUOM { get; set; }

        public string LowerQty { get; set; }
        public string HigherPrice { get; set; }
        public string LowerPrice { get; set; }
        public string LineTotal { get; set; }
        public string Vat { get; set; }
        public string GrandTotal { get; set; }


    }
    public class ScheduledReturnout
    {
        public string Res { get; set; }

        public string Message { get; set; }
        public string TransID { get; set; }

    }

    public class InsCRNHeader
    {
        public string cusid { get; set; }
        public string subtotal { get; set; }
        public string Amount { get; set; }
        public string usrid { get; set; }
        public string Detaildata { get; set; }
        public string invID { get; set; }
    }
    public class InsCRNDetail
    {
        public string invid { get; set; }
        public string itmid { get; set; }
        public string huom { get; set; }
        public string hqty { get; set; }
        public string luom { get; set; }
        public string lqty { get; set; }
        public string amount { get; set; }
        public string rsnid { get; set; }
    }
    public class InsCusReqParams
    {
        public string userID { get; set; }
        public string cus_ID { get; set; }
        public string type { get; set; }
        public string descr { get; set; }
        public Bitmap img { get; set; }
    }

    public class DisputeNoteReqIn
    {
        public string CusID { get; set; }
        public string UserID { get; set; }
        public string type { get; set; }

        public string date { get; set; }

        public string Remark { get; set; }

        public string JSONString { get; set; }
        public string OtherInfo { get; set; }
        public string Amount { get; set; }
        public Bitmap img { get; set; }


    }

    public class DisputeNoteReqOut
    {
        public string Res { get; set; }

        public string Message { get; set; }
        public string TransID { get; set; }

    }

    public class InvoiceIDs
    {


        public string oidID { get; set; }

        public string Balance { get; set; }

    }
    public class OutstandingINvIn
    {


        public string CusID { get; set; }
        public string UserID { get; set; }
        public string fromDate { get; set; }
        public string todate { get; set; }




    }

    public class OutstandingINvOut
    {


        public string oid_ID { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceBalance { get; set; }
        public string InvoicedOn { get; set; }
    }
    public class SRRequestDetailIn
    {


        public string RequestID { get; set; }


    }

    public class GetPendingRtnRequestDetail
    {

        public string prd_ID { get; set; }
        public string HUOM { get; set; }
        public string HQty { get; set; }
        public string LUOM { get; set; }
        public string LQty { get; set; }


        public string prd_Name { get; set; }

        public string prd_code { get; set; }
        public string prd_LongDesc { get; set; }
        public string prd_cat_id { get; set; }
        public string prd_sub_ID { get; set; }
        public string prd_brd_ID { get; set; }

        public string prd_NameArabic { get; set; }
        public string prd_LongDescArabic { get; set; }
        public string prd_Image { get; set; }

        public string InvoiceNumber { get; set; }
        public string inv_ID { get; set; }
        public string Image { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
        public string Vat { get; set; }
        public string GrandTotal { get; set; }
        public string HigherPrice { get; set; }
        public string LowerPrice { get; set; }

    }
    public class DisputeImageIn
    {



        public string TransID { get; set; }

    }
    public class DisputeImageOut
    {
        public string Res { get; set; }

        public string Message { get; set; }


    }

    public class CusReqTypeIn
    {
        public string CusID { get; set; }
        public string UserID { get; set; }

    }

    public class CusReqTypeOut
    {
        public string rqtID { get; set; }
        public string ReqName { get; set; }
    }
    public class ReasonOut
    {
        public string rsnID { get; set; }
        public string reason { get; set; }
    }
    public class CreditNoteImageIn
    {

        public string ReqID { get; set; }

        public string prdID { get; set; }

    }
    public class CreditNoteImageOut
    {
        public string Res { get; set; }

        public string Message { get; set; }


    }
    public class PendingDisputeReqDetailIn
    {


        public string drhID { get; set; }



    }

    public class PendingDisputeReqDetailOut
    {
        
        public string drhID { get; set; }
        public string Type { get; set; }
        public string OtherInfo { get; set; }
        public string Comment { get; set; }
        public string Amount { get; set; }
        public string InvCount { get; set; }
        public string Image { get; set; }


    }
    public class OutInvforDispIn
    {
        public string drhID { get; set; }

    }
    public class GetCNRDetailDataIn
    {


        public string cnhID { get; set; }



    }

    public class GetCNRDetailDataOut
    {

        public string cndID { get; set; }
        public string Date { get; set; }     
        public string itmCode { get; set; }
        public string itmName { get; set; }
        public string itmImg { get; set; }
        public string HUom { get; set; }
        public string HQty { get; set; }
        public string LUom { get; set; }
        public string LQty { get; set; }
        public string InvNum { get; set; }
        public string Reason { get; set; }
        public string Image { get; set; }
        public string amount { get; set; }


    }
    public class GetCusReqDetailIn
    {
        public string reqID { get; set; }
    }

    public class GetCusReqDetailOut
    {
        public string reqID { get; set; }
        public string description { get; set; }
        public string type { get; set; }
        public string reqImg { get; set; }

    }
    public class CNRHeaderByIDOut
    {
        public string crhID { get; set; }
        public string Date { get; set; }
        public string ReqID { get; set; }
        public string GrandTotal { get; set; }
        public string SubTotal { get; set; }
        public string Vat { get; set; }
    }

    public class GetProductsIn
    {
        public string CusID { get; set; }
        public string UserID { get; set; }

    }

    public class GetProductsOut
    {
        public string prdID { get; set; }
        public string prdName { get; set; }
    }

    public class GetInvProdIn
    {
        public string prdID { get; set; }

    }

    public class GetInvProdOut
    {
        public string invID { get; set; }
        public string invNum { get; set; }
    }

    public class GetInvProdDetIn
    {
        public string prdID { get; set; }
        public string invID { get; set; }

    }

    public class GetInvProdDetOut
    {
        public string invID { get; set; }
        public string itmCode { get; set; }
        public string itmName { get; set; }
        public string HQty { get; set; }
        public string HUom { get; set; }
        public string LQty { get; set; }
        public string LUom { get; set; }
        public string SubTotal { get; set; }
        public string VAT { get; set; }
        public string GrandTotal { get; set; }
        public string luomid {  get; set; }
        public string huomid { get; set;}
        
    }

}

