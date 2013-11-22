﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.EntityModels;

namespace WineProdTools.Data.DtoModels
{
    public class TankDto
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public int Gallons { get; set; }
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
