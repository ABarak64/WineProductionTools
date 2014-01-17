using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.DtoModels;

namespace WineProdTools.Data.Managers
{
    public interface INoteManager
    {
        IEnumerable<NoteDto> GetSomeNotesAfterThisManyForAccount(int count, Int64 accountId);
        void AddNoteForAccount(NoteDto noteDto, Int64 accountId);
        string TimeSince(DateTime time, DateTime endTime);
    }
}
