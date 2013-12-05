using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WineProdTools.Data.EntityModels
{
    public class TankContents
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }
        public Int64 LastTankId { get; set; }
        public string Name { get; set; }
        public decimal Gallons { get; set; }
        public double? Ph { get; set; }
        public double? So2 { get; set; }
        public double? Alcohol { get; set; }
        public double? TA { get; set; }
        public double? VA { get; set; }
        public int? MA { get; set; }
        public int? RS { get; set; }
        public TankContentState? State { get; set; }
        public DateTime? DateDeleted { get; set; }
    }

    public enum TankContentState
    {
        None = 0,
        PrimaryFermentation,
        MalolacticFermentation,
        CompleteSulfured,
        Finished
    }
}
