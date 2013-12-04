using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.EntityModels;

namespace WineProdTools.Data.DtoModels
{
    public class TankAndContentsDto
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public decimal? Gallons { get; set; }
        public int XPosition { get; set; }
        public int YPosition { get; set; }
        public TankContentsDto Contents { get; set; }

        public TankAndContentsDto() { }

        public TankAndContentsDto(Tank tank)
        {
            this.Id = tank.Id;
            this.Name = tank.Name;
            this.Gallons = tank.Gallons;
            this.XPosition = tank.XPosition;
            this.YPosition = tank.YPosition;
            if (tank.Contents != null)
            {
                this.Contents = new TankContentsDto(tank.Contents);
            }
            else
            {
                this.Contents = new TankContentsDto();
            }
        }
    } 
}
