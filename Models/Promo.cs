using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace b2b_solution.Models
{
    public class Promo
    {
    }

    public class PromoCart
    {
        public string id { get; set; }

        public string prmID { get; set; }

        public string ashID { get; set; }
        public string HigherQty { get; set; }
        public string HigherUOM { get; set; }
        public string LowerQty { get; set; }
        public string LowerUOM { get; set; }
    }

    public class RangeRes
    {
        public string prrID { get; set; }

        public string Count { get; set; }
    }
}