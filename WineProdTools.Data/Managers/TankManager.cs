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
        private readonly Dictionary<TankContentState, string> _tankStateToStateNameMap = new Dictionary<TankContentState, string>()
        {
            { TankContentState.None, "None" },
            { TankContentState.PrimaryFermentation, "Primary Fermentation" },
            { TankContentState.MalolacticFermentation, "Malolactic Fermentation" },
            { TankContentState.CompleteSulfured, "Complete and Sulfured" },
            { TankContentState.Finished, "Finished" }
        };

        public string GetContentStateName(TankContentState state)
        {
            return _tankStateToStateNameMap[state];
        }

        public IEnumerable<TankAndContentsDto> GetTanksForAccount(Int64 accountId)
        {
            using (var db = new WineProdToolsContext())
            {
                return db.Tanks
                    .Include(t => t.Contents)
                    .Where(t => t.AccountId == accountId && t.DateDeleted == null)
                    .AsEnumerable()
                    .Select(t => new TankAndContentsDto(t))
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

        public bool TankContainsAtLeastThisManyGallons(decimal gallonsToTransfer, Int64 tankId)
        {
            if (gallonsToTransfer == 0)
            {
                return true;
            }
            using (var db = new WineProdToolsContext())
            {
                var tank = db.Tanks.Include(t => t.Contents)
                    .Single(t => t.Id == tankId);
                if (tank.Contents == null)
                {
                    return false;
                }
                return (tank.Contents.Gallons - gallonsToTransfer >= 0);
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

        public void UpdateTankContentsForAccount(TankContentsDto contentsDto, Int64 accountId)
        {
            using (var db = new WineProdToolsContext())
            {
                var tank = db.Tanks.Include(t => t.Contents)
                    .Single(t => t.Id == contentsDto.TankId);
                if (tank.AccountId != accountId)
                {
                    throw new AuthenticationException();
                }
                if (tank.Contents == null)
                {
                    throw new InvalidOperationException();
                }
                tank.Contents.Name = contentsDto.Name;
                tank.Contents.Ph = contentsDto.Ph;
                tank.Contents.So2 = contentsDto.So2;
                tank.Contents.Alcohol = contentsDto.Alcohol;
                tank.Contents.TA = contentsDto.TA;
                tank.Contents.VA = contentsDto.VA;
                tank.Contents.MA = (int?)contentsDto.MA;
                tank.Contents.RS = (int?)contentsDto.RS;
                tank.Contents.State = contentsDto.State;
                db.SaveChanges();
            }
        }

        public void TankContentsTransferForAccount(TankTransferDto transferDto, Int64 accountId)
        {
            // Can't transfer from non-tank to non-tank.
            if (transferDto.FromId == 0 && transferDto.ToId == 0)
            {
                return;
            }
            var tankIds = new List<Int64>();
                if (transferDto.FromId != 0)
                    tankIds.Add(transferDto.FromId);
                if (transferDto.ToId != 0)
                    tankIds.Add(transferDto.ToId);

            using (var db = new WineProdToolsContext())
            {
                 var relevantTanks = db.Tanks.Include(t => t.Contents)
                    .Where(t => tankIds.Contains(t.Id)).ToList();
                foreach (var tank in relevantTanks)
                {
                    if (tank.AccountId != accountId)
                    {
                        throw new AuthenticationException();
                    }
                }
                if (transferDto.FromId != 0)
                {
                    EmptyTank(relevantTanks.Single(t => t.Id == transferDto.FromId), transferDto);
                }
                if (transferDto.ToId != 0)
                {
                    FillTank(relevantTanks.Single(t => t.Id == transferDto.ToId), transferDto);
                }
                db.SaveChanges();
            }
        }

        private void FillTank(Tank tankToFill, TankTransferDto transferDto)
        {
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
            tankToFill.Contents.Alcohol = transferDto.Alcohol;
            tankToFill.Contents.TA = transferDto.TA;
            tankToFill.Contents.VA = transferDto.VA;
            tankToFill.Contents.MA = (int?)transferDto.MA;
            tankToFill.Contents.RS = (int?)transferDto.RS;
            tankToFill.Contents.State = transferDto.State;
        }

        private void EmptyTank(Tank tankToEmpty, TankTransferDto transferDto)
        {
            if (tankToEmpty.Contents == null)
            {
                return;
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
        }

        private void TransferBetweenTanksForAccount(TankTransferDto transferDto, Int64 accountId)
        {
            using (var db = new WineProdToolsContext())
            {
                var relevantTanks = db.Tanks
                    .Where(t => new List<Int64> { transferDto.ToId, transferDto.FromId }.Contains(t.Id));

            }
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
