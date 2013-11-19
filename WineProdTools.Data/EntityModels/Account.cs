using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WineProdTools.Data.EntityModels
{
    public class Account
    {
        [Key]
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
