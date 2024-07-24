using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace b2b_solution.Models
{
    public class HandleError
    {
        public string message { get; set; }
        public int mode { get; set; }

        public string desc { get; set; }
        public string UserType { get; set; }
    }

    public class Creds
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool remembercheck { get; set; }
    }
}