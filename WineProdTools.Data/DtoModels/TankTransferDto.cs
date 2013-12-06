using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WineProdTools.Data.Validation;
using WineProdTools.Data.EntityModels;

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
        [FromTankMustContainAtLeastThisMuch]
        public decimal? Gallons { get; set; }
        [PositiveNumber]
        [Range(0, 14)]
        public double? Ph { get; set; }
        [PositiveNumber]
        public double? So2 { get; set; }
        [PositiveNumber]
        [Range(0, 100)]
        public double? Alcohol { get; set; }
        [PositiveNumber]
        public double? TA { get; set; }
        [PositiveNumber]
        public double? VA { get; set; }
        [PositiveInteger]
        public double? MA { get; set; }
        [PositiveInteger]
        public double? RS { get; set; }
        public TankContentsState State { get; set; }
    }
}
