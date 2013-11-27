using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WineProdTools.Data.EntityModels;
using WineProdTools.Data.Managers;

namespace WineProdTools.Data.DtoModels
{
    public class NoteDto
    {
        public Int64 Id { get; set; }
        [Required]
        public string Comment { get; set; }
        public string TimeSinceCreated { get; set; }

        public NoteDto() { }

        public NoteDto(Note note)
        {
            this.Id = note.Id;
            this.Comment = note.Comment;
            this.TimeSinceCreated = new NoteManager().DateTimeToTimeSinceNow(note.DateCreated);
        }
    }
}
