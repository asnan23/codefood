using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFood_API.Asnan.Models.Response
{
    public class SuccessDTO
    {
        public bool success { get; set; } = true;
        public string message { get; set; } = "Success";
        public object data { get; set; }
    }

    public class ErrorDTO
    {
        public bool success { get; set; } = false;
        public string message { get; set; }
    }
}
