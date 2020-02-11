using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewProjectAPI.Models
{
    public class Property
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string TypeOfProperty { get; set; }
        public string PropertyDescription { get; set; }
        public string ImageUrl{get; set;}
        public string ContacDetails { get; set; }

    }
}
