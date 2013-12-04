using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using WineProdTools.Data.DtoModels;
using WineProdTools.Data.EntityModels;
using System.Security.Authentication;

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

        public TankAndContentsDto GetTankForAccount(Int64 tankId, Int64 accountId)
        {
            using (var db = new WineProdToolsContext())
            {
                return db.Tanks
                    .Include(t => t.Contents)
                    .Where(t => t.AccountId == accountId && t.DateDeleted == null && t.Id == tankId)
                    .AsEnumerable()
                    .Select(t => new TankAndContentsDto(t))
                    .SingleOrDefault();
            }
        }

        public bool TankWillOverflow(decimal gallonsToAdd, Int64 tankId)
        {
            using (var db = new WineProdToolsContext())
            {
                var tank = db.Tanks.Include(t => t.Contents)
                    .Single(t => t.Id == tankId);
                var emptyGallons = tank.Contents != null ? tank.Gallons - tank.Contents.Gallons : tank.Gallons;
                return (emptyGallons - gallonsToAdd < 0);
            }
        }

        public Int64 AddTankForAccount(TankDto tankDto, Int64 accountId)
        {
            var tank = new Tank 
            { 
                AccountId = accountId,
                Name = tankDto.Name,
                Gallons = (decimal)tankDto.Gallons,
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
                    throw new AuthenticationException();
                }
                tank.Gallons = (decimal)tankDto.Gallons;
                tank.Name = tankDto.Name;
                tank.XPosition = tankDto.XPosition;
                tank.YPosition = tankDto.YPosition;
                db.SaveChanges();
            }
        }

        public void TankContentsTransferForAccount(TankTransferDto transferDto, Int64 accountId)
        {
            if (transferDto.FromId == 0 && transferDto.ToId == 0)
            {
                throw new InvalidOperationException();
            }
            else if (transferDto.FromId == 0)
            {
                FillTankForAccount(transferDto, accountId);
            }
            else if (transferDto.ToId == 0)
            {
                EmptyTankForAccount(transferDto, accountId);
            }
            else
            {
                TransferBetweenTanksForAccount(transferDto, accountId);
            }
        }

        private void FillTankForAccount(TankTransferDto transferDto, Int64 accountId)
        {
            using (var db = new WineProdToolsContext())
            {
                var tankToFill = db.Tanks.Include(t => t.Contents)
                    .Single(t => t.Id == transferDto.ToId);
                    
                if (tankToFill.AccountId != accountId)
                {
                    throw new AuthenticationException();
                }
                if (tankToFill.Contents == null)
                {
                    tankToFill.Contents = new TankContents 
                    {
                        LastTankId = tankToFill.Id,
                        Gallons = 0,
                        DateDeleted = null
                    };
                }             
                tankToFill.Contents.Name = transferDto.Name;
                tankToFill.Contents.Gallons += (decimal)transferDto.Gallons;
                tankToFill.Contents.Ph = transferDto.Ph;
                tankToFill.Contents.So2 = transferDto.So2;
                db.SaveChanges();
            }
        }

        private void EmptyTankForAccount(TankTransferDto transferDto, Int64 accountId)
        {
            using (var db = new WineProdToolsContext())
            {
                var tankToEmpty = db.Tanks.Include(t => t.Contents)
                    .Single(t => t.Id == transferDto.FromId);

                if (tankToEmpty.AccountId != accountId)
                {
                    throw new AuthenticationException();
                }
                if (tankToEmpty.Contents.Gallons - transferDto.Gallons <= 0)
                {
                    tankToEmpty.TankContentsId = null;
                    tankToEmpty.Contents.DateDeleted = DateTime.Now;
                }
                else
                {
                    tankToEmpty.Contents.Gallons -= (decimal)transferDto.Gallons;
                }
                db.SaveChanges();
            }
        }

        private void TransferBetweenTanksForAccount(TankTransferDto tankDto, Int64 accountId)
        {
        }

        public void DeleteTankForAccount(Int64 tankId, Int64 accountId)
        {
            using (var db = new WineProdToolsContext())
            {
                var tank = db.Tanks.Single(t => t.Id == tankId);
                if (tank.AccountId != accountId)
                {
                    throw new AuthenticationException();
                }
                tank.DateDeleted = DateTime.Now;
                db.SaveChanges();
            }
        }
    }
}
