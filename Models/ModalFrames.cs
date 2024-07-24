using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace b2b_solution.Models
{
    public class ModalFrames
    {

    }


    public class Category
    {
        public int id { get; set; }
        public string name { get; set; }

        public string image { get; set; }
    }

    public class Products
    {
        public string id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string SubCat { get; set; }


    }

    public class UOMModel
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public int UPC { get; set; }
    }

    public class UomList
    {
        public List<UOMModel> ListUomModel { get; set; }

    }

    public class ItemRow
    {
        public string id { get; set; }
        public string LowerUOM { get; set; }

        public string LowerUOMOfferPrice { get; set; }

        public string LowerUOMStandardPrice { get; set; }

        public string lowerQty { get; set; }
    }

    public class ItemList
    {
        public List<ItemRow> Rowitem { get; set; }
    }

    public class ProductList
    {
        public UomList LowUOM { get; set; }
        public ItemList LowItem { get; set; }
    }
    public class ItemUomPrice
    {
        public string OfferPrice { get; set; }

        public string StandardPrice { get; set; }

    }

    public class ItemSubTotal
    {
        public string Total_WO_D { get; set; }
        public string TotalDisc { get; set; }

        public string Total_W_D { get; set; }

        public string VAT { get; set; }

        public string GrandTotal { get; set; }

        public string TotalCredit { get; set; }

        public string AvailableCredit { get; set; }
        public string MinOrderValue { get; set; }

        public string CreditDayStatus { get; set; }
    }

    public class CartProps
    {
        public string id { get; set; }
        public string HigherQty { get; set; }
        public string HigherUOM { get; set; }
        public string LowerQty { get; set; }
        public string LowerUOM { get; set; }
    }

    public class SubCats
    {
        public string id { get; set; }
        public string name { get; set; }

        public string image { get; set; }
    }

    public class SubCatList
    {
        public List<SubCats> SCLists { get; set; }
    }

    public class SearchSubCats
    {
        public SearchComplete SearchRes { get; set; }

        public SubCatList SubCategories { get; set; }
    }

    public class SearchComplete
    {
        public string prdName { get; set; } = "";
        public string catID { get; set; } = "0";
        public string sctID { get; set; }
        public string brdID { get; set; } = "0";
        public string catName { get; set; } = "";
        public string brdName { get; set; } = "";
        public SearchBrandsList brdIDs { get; set; }
        public string PageNum { get; set; } = "1";

        public bool specialPrice { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }

		public string SubDealer { get; set; }
	}


    public class Brands
    {
        public string brdid { get; set; }
        public string brdname { get; set; }
        public string brdimage { get; set; }
        public string brdcode { get; set; }
        public bool remembercheck { get; set; }
    }
    public class SearchBrandsList
    {
        public List<Brands> BrandsIDs { get; set; }
    }

    public class SpecialPrice
    {
        public bool remembercheck { get; set; }
        public string spName { get; set; }
    }

    public class ListCart
    {
        public string itemID { get; set; }
        public string itemName { get; set; }
        public string ItemImage { get; set; }

        public int HigherUOM { get; set; }

        public int HigherQty { get; set; }

        public int LowerUom { get; set; }

        public int LowerQty { get; set; }
    }


    public class OrderInputs
    {
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
        public string SubTotal_WO_D { get; set; }
        public string TotalDisc { get; set; }
    }

    public class ViewAllOrder
    {
        public string id { get; set; }
        public string OrderNo { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string SubTotal { get; set; }
        public string VAT { get; set; }
        public string GradnTotal { get; set; }
        public string LPO { get; set; }
        public string ExpectedDate { get; set; }
        public string Attachment { get; set; }
        public string Total { get; set; }
        public string Discount { get; set; }
		public string DealerCode { get; set; }
		
	}

    public class VieworderByID
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
    }

    public class OrderList
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }

		public string SubDealer { get; set; }
	}

    public class ViewAllInvoices
    {
        public string id { get; set; }
        public string Number { get; set; }
        public string PayMode { get; set; }
        public string InvoicedOn { get; set; }
        public string SubTotal { get; set; }
        public string VAT { get; set; }
        public string GradnTotal { get; set; }
        public string Status { get; set; }
    }

    public class ViewAllAR
    {
        public string id { get; set; }
        public string Number { get; set; }
        public string PayMode { get; set; }
        public string SubTotal { get; set; }
        public string VAT { get; set; }
        public string GradnTotal { get; set; }
        public string ChequeDate { get; set; }
        public string ChequeImg { get; set; }

        public string ChequeNo { get; set; }
        public string bank { get; set; }
        public string CreatedDate { get; set; }
    }

    public class ARDetail
    {
        public string Number { get; set; }
        public string InvAmount { get; set; }
        public string PaidAmount { get; set; }

    }

    public class ViewAllSupports
    {
        public string id { get; set; }
        public string Number { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string CreatedDate { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }
    }

    public class SupportInputs
    {

        public string id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string Reason { get; set; }
    }

    public class AutoSearch
    {
        public string itmName { get; set; }
    }

    public class CusRoles
    {
        public string rolID { get; set; }
        public string rolName { get; set; }
    }

    public class CusType
    {
        public string TypeName { get; set; }
        public string TypeCode { get; set; }
    }
    public class CreateUser
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string contactNum { get; set; } = "";
        public string roles { get; set; }
        public string type { get; set; }
        public string cusid { get; set; }
        public string cshid { get; set; }
        
    }

    public class Userlist
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Roles { get; set; }
        public string CreatedOn { get; set; }
        public string ApprStatus { get; set; }
        public string Status { get; set; }
        public string type { get; set; }
        public string Customer { get; set; }
        public string Id {  get; set; }
    }
    public class Outstanding
    {
        public string cshName { get; set; }
        public string InvoiceID { get; set; }
        public string InvoicedOn { get; set; }

        public string InvoiceAmount { get; set; }
        public string PendingAmount { get; set; }
    }

    public class NewUser
    {
        public string CusCode { get; set; } = "";
        public string Email { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string MobileNumber { get; set; } = "";
    }

    public class DelDates
    {
        public string Dates { get; set; }
    }

    public class Reason
    {
        public string ReasonCode { get; set; }
        public string ReasonName { get; set; }
    }
    public class OrderTemplate
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string CusID { get; set; }
        public string TempCode { get; set; }
        public string TempName { get; set; }
    }
    public class TemplateItems
    {
		public string ID { get; set; }
        public string tempcode { get; set; }
        public string itmid { get; set; }
        public string tempname { get; set; }
		public string itmcode { get; set; }
        public string itmname { get; set; }
        public string huomid { get; set; }
        public string itmhuom { get; set; }
        public string itmhqty { get; set; }
        public string luomid { get; set; }
        public string itmluom { get; set; }
        public string itmlqty { get; set; }
		public string itmuom { get; set; }
		public string itmqty { get; set; }
		
	}


    public class GridItems
    {
        public string itemid { get; set; }
        public string item { get; set; }
        public string huomid { get; set; }
        public string huom { get; set; }
        public string hqty { get; set; }
        public string luomid { get; set; }
        public string luom { get; set; }
        public string lqty { get; set; }


    }
        public class DocType
	{
		public string DocId { get; set; }
		public string DocumentType { get; set; }

		public string IsExpiryDate { get; set; }

	}
	public class DocUploadInputs
	{

		public string id { get; set; }
		public string DocType { get; set; }
		public string DocNo { get; set; }
		public string ExpDate { get; set; }
		public string StartDate { get; set; }
	}
	public class ViewAllCusDocuments
	{
		public string id { get; set; }
		public string Type { get; set; }
		public string DocNo { get; set; }
		public string DocPath { get; set; }
		public string CreatedDate { get; set; }
		public string Status { get; set; }
		public string Image { get; set; }
		public string CreatedBy { get; set; }
		public string ExpiryDate { get; set; }
		public string StartDate { get; set; }

	}
    public class Currency
    {
        public string CurrencyCode { get; set; }
    }
	public class OrderTemplateUOM
	{
		public string uomid { get; set; }
		public string uomname { get; set; }
	}
    public class DCustomer
    {
        public string cusID { get; set; }
        public string cusName { get; set; }
    }
    public class SpecialPriceGrid
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedOn { get; set; }
        public string Status { get; set; }
        
    }
    public class SpecialPriceDetailGrid
    {
        public string Code { get; set; }
        public string itm_Name { get; set; }
        public string cat_Code { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
        public string uom_Name { get; set; }
        public string OfferPrice { get; set; }
        public string standardPrice { get; set; }
        public string ReturnPrice { get; set;}
        public string VAT { get; set; }
    }

    
    public class PromotionGrid
    {
        public string id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Qualification { get; set; }
        public string Assignment { get; set; }
        public string Status { get; set; }
        public string rcpId { get; set; }

    }

    public class PromotionRange
    {
        public string MinVal { get; set; }
        public string MaxVal { get; set; }
        public string Val { get; set; }
    }
    public class Qitems
    {
        public string Code { get; set; }
        public string itm_Name { get; set; }
    }
    public class Aitems
    {
        public string Code { get; set; }
        public string itm_Name { get; set; }
    }

    public class ItemListGrid
    {
        public string Code { get; set; }
        public string itm_Name { get; set; }
        public string itm_desc { get; set; }
        public string category { get; set; }
        public string uom_Name { get; set; }
        public string OfferPrice { get; set; }
        public string standardPrice { get; set; }
    }

    public class ResetPass
    {
        public string res { get; set; }
        public string pass { get; set; }
    }

    public class DashboardItems
    {
        public string totalOrder { get; set; }
        public string CancelOrders { get; set; }
        public string PendingDel { get; set; }
    }
    public class StatusCount
    {
        public string count { get; set; }
        public string status { get; set; }
    }
	public class Dealers
	{
		public string cusID { get; set; }
		public string CusName { get; set; }

	}
}