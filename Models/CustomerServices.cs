using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace b2b_solution.Models
{
    public class CustomerServices
    {
    }

    public class ViewCRNote
    {
        public string id { get; set; }
        public string ReqID { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Amount { get; set; }
        public string GradnTotal { get; set; }

    }

    public class ViewCRNoteByID
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

    public class CRNList
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class ViewDISNote
    {
        public string id { get; set; }
        public string ReqID { get; set; }
        public string Status { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string Amount { get; set; }
        public string VAT { get; set; }
        public string GradnTotal { get; set; }

    }

    public class ViewDISNoteByID
    {
        public string id { get; set; }
        public string InvoiceID { get; set; }
        public string Amount { get; set; }
        public string InvAmount { get; set; }
        
    }

    public class DSNList
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

}