using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.DtoModels;
using WineProdTools.Data.EntityModels;

namespace WineProdTools.Data.Managers
{
    public class TankManager
    {
        public IEnumerable<TankDto> GetTanksForAccount(Int64 accountId)
        {
            using (var db = new WineProdToolsContext())
            {
                return db.Tanks
                    .Where(t => t.AccountId == accountId && t.DateDeleted == null)
                    .AsEnumerable()
                    .Select(t => new TankDto(t))
                    .ToList();
            }
        }

        public Int64 AddTankForAccount(TankDto tankDto, Int64 accountId)
        {
            var tank = new Tank 
            { 
                AccountId = accountId,
                Name = tankDto.Name,
                Gallons = (int)tankDto.Gallons,
                DateDeleted = null,
                XPosition = 250,
                YPosition = 250
            };

            using (var db = new WineProdToolsContext())
            {
                db.Tanks.Add(tank);
                db.SaveChanges();
            }

            return tank.Id;
        }

        public void UpdateTankForAccount(TankDto tankDto, Int64 accountId)
        {     
            using (var db = new WineProdToolsContext())
            {
                var tank = db.Tanks.Single(t => t.Id == tankDto.Id);
                if (tank.AccountId != accountId)
                {
                    throw new System.Security.Authentication.AuthenticationException();
                }
                tank.Gallons = (int)tankDto.Gallons;
                tank.Name = tankDto.Name;
                tank.XPosition = tankDto.XPosition;
                tank.YPosition = tankDto.YPosition;
                db.SaveChanges();
            }
        }
    }
}
