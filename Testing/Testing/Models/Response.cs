using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Testing.Model
{ 
    public class ResponseMessage
    {
        public string message { set; get; }
        public bool isSuccessful { get; set; }
    }
}