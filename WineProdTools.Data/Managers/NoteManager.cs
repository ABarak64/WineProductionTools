using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineProdTools.Data.DtoModels;
using WineProdTools.Data.EntityModels;

namespace WineProdTools.Data.Managers
{
    public class NoteManager
    {
        private readonly Func<IWineProdToolsContext> _getNewContext;

        public NoteManager()
        {
            this._getNewContext = () => { return new WineProdToolsContext(); };
        }

        public NoteManager(Func<IWineProdToolsContext> contextFactory)
        {
            this._getNewContext = contextFactory;
        }

        private static readonly List<TimeType> _timeTypes = new List<TimeType>()
        {
            new TimeType 
            {
                Title = "second",
                divisorForSeconds = 1,
                Max = 60
            },
            new TimeType 
            {
                Title = "minute",
                divisorForSeconds = 60,
                Max = 60
            },
            new TimeType 
            {
                Title = "hour",
                divisorForSeconds = 60 * 60,
                Max = 24
            },
            new TimeType 
            {
                Title = "day",
                divisorForSeconds = 60 * 60 * 24,
                Max = 30
            },
            new TimeType 
            {
                Title = "month",
                divisorForSeconds = 60 * 60 * 24 * 30,
                Max = 12
            },
            new TimeType 
            {
                Title = "year",
                divisorForSeconds = 60 * 60 * 24 * 30 * 12,
                Max = int.MaxValue 
            },
        };

        public IEnumerable<NoteDto> GetSomeNotesAfterThisManyForAccount(int count, Int64 accountId)
        {
            using (var db = this._getNewContext())
            {
                return db.Notes
                    .Where(t => t.AccountId == accountId)
                    .OrderByDescending(t => t.Id)
                    .Skip(count)
                    .Take(10)
                    .AsEnumerable()
                    .Select(t => new NoteDto(t))
                    .ToList();
            }
        }

        public void AddNoteForAccount(NoteDto noteDto, Int64 accountId)
        {
            var note = new Note 
            {
                Comment = noteDto.Comment,
                DateCreated = DateTime.Now,
                AccountId = accountId 
            };

            using (var db = this._getNewContext())
            {
                db.Notes.Add(note);
                db.SaveChanges();
            }
        }

        public string TimeSince(DateTime time, DateTime endTime)
        {
            var diff = (endTime - time).TotalSeconds;
            var correctType = _timeTypes.Where(t => Math.Floor(diff / t.divisorForSeconds) < t.Max).First();
            var numOfThisType = Math.Floor(diff / correctType.divisorForSeconds);
            return ((int)numOfThisType).ToString() +  ' ' + correctType.Title + (numOfThisType == 1 ? string.Empty : "s");
        }

        private class TimeType
        {
            public string Title { get; set; }
            public double divisorForSeconds { get; set; }
            public int Max { get; set; }
        }
    }
}
