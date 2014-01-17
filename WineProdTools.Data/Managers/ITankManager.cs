using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.DtoModels;
using WineProdTools.Data.EntityModels;

namespace WineProdTools.Data.Managers
{
    public interface ITankManager
    {
        IEnumerable<TankAndContentsDto> GetTanksForAccount(Int64 accountId);
        IEnumerable<TankContentsState> GetContentStates();
        TankAndContentsDto GetTankForAccount(Int64 tankId, Int64 accountId);
        bool TankWillOverflow(decimal gallonsToAdd, Int64 tankId);
        bool TankContainsAtLeastThisManyGallons(decimal gallonsToTransfer, Int64 tankId);
        Int64 AddTankForAccount(TankDto tankDto, Int64 accountId);
        void UpdateTankForAccount(TankDto tankDto, Int64 accountId);
        void UpdateTankContentsForAccount(TankContentsDto contentsDto, Int64 accountId);
        void TankContentsTransferForAccount(TankTransferDto transferDto, Int64 accountId);
        void DeleteTankForAccount(Int64 tankId, Int64 accountId);
    }
}
