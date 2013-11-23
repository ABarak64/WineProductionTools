using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.EntityModels;
using System.ComponentModel.DataAnnotations;
using WineProdTools.Data.Validation;

namespace WineProdTools.Data.DtoModels
{
    public class TankDto
    {
        public Int64 Id { get; set; }
        [Required]
        public string Name { get; set; }
        [PositiveInteger]
        public int? Gallons { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }

        public TankDto() { }

        public TankDto(Tank tank)
        {
            this.Id = tank.Id;
            this.Name = tank.Name;
            this.Gallons = tank.Gallons;
            this.XPosition = tank.XPosition;
            this.YPosition = tank.YPosition;
        }
    }
}
