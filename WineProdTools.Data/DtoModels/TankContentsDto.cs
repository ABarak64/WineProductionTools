using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.EntityModels;
using System.ComponentModel.DataAnnotations;
using WineProdTools.Data.Validation;
using WineProdTools.Data.Managers;

namespace WineProdTools.Data.DtoModels
{
    public class TankContentsDto
    {
        public Int64? Id { get; set; }
        public Int64? TankId { get; set; }
        [Required]
        public string Name { get; set; }
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
        public TankContentState? State { get; set; }
        public string StateName { get; set; }

        public TankContentsDto() { }
        public TankContentsDto(TankContents contents, Int64 tankId)
        {
            this.TankId = TankId;
            this.Id = contents.Id;
            this.Name = contents.Name;
            this.Gallons = contents.Gallons;
            this.Ph = contents.Ph;
            this.So2 = contents.So2;
            this.Alcohol = contents.Alcohol;
            this.TA = contents.TA;
            this.VA = contents.VA;
            this.MA = contents.MA;
            this.RS = contents.RS;
            this.State = contents.State;
            this.StateName = this.State == null ? null : new TankManager().GetContentStateName(this.State.Value);
        }
    }
}
