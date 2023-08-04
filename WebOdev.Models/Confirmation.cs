using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebOdev.Models
{
    public class Confirmation
    {
        public Product Product { get; set; }

        [Range(1, 4, ErrorMessage = "Pls 1-4")]
        public int Count { get; set; }
    }
}
