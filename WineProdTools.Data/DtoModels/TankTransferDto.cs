using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WineProdTools.Data.Validation;

namespace WineProdTools.Data.DtoModels
{
    public class TankTransferDto
    {
        public Int64 FromId { get; set; }
        public Int64 ToId { get; set; }
        [Required]
        public string Name { get; set; }
        [PositiveNumber]
        [MustNotOverflowTank]
        public decimal? Gallons { get; set; }
        [PositiveNumber]
        [Range(0, 14)]
        public double? Ph { get; set; }
        [PositiveNumber]
        public double? So2 { get; set; }
    }
}
