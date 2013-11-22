using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WineProdTools.Data.EntityModels
{
    public class Tank
    {
        [Key]
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public int Gallons { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public Int64 AccountId { get; set; }
        public virtual Account Account { get; set; }
        public DateTime? DateDeleted { get; set; }
    }
}
