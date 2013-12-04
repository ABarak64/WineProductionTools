using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.EntityModels;

namespace WineProdTools.Data.DtoModels
{
    public class TankContentsDto
    {
        public Int64? Id { get; set; }
        public string Name { get; set; }
        public decimal? Gallons { get; set; }
        public double? Ph { get; set; }
        public double? So2 { get; set; }

        public TankContentsDto() { }
        public TankContentsDto(TankContents contents)
        {
            this.Id = contents.Id;
            this.Name = contents.Name;
            this.Gallons = contents.Gallons;
            this.Ph = contents.Ph;
            this.So2 = contents.So2;
        }
    }
}
