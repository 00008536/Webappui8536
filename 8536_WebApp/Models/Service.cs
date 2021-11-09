using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _8536_WebApp.Models
{
    public class Service
    {
        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ServiceCategory { get; set; }
    }
}