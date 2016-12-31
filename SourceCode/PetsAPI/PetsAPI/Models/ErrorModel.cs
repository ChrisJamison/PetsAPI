using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetsAPI.Models
{
    public class ErrorModel
    {
        public int errorCode { get; set; }
        public string errorDes { get; set; }
        public string errorMsg { get; set; }
        public string errorTrace { get; set; }
    }
}