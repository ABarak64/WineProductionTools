using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WineProdTools.Data.EntityModels
{
    public class Note
    {
        [Key]
        public Int64 Id { get; set; }
        public Int64 AccountId { get; set; }
        public virtual Account Account { get; set; }
        public DateTime DateCreated { get; set; }
        public string Comment { get; set; }
    }
}
