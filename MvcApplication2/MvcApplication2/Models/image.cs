using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication2.Models
{
    public class image
    {
        public int imageId { get; set; }
        public string imageName { get; set; }
        public DateTime createdDate{ get; set; }
        public string cover { get; set; }
        public int likes{ get; set; }
        public int dislikes{ get; set; }
    }
}