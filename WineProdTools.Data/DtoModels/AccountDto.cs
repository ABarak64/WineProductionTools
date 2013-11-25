using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WineProdTools.Data.EntityModels;

namespace WineProdTools.Data.DtoModels
{
    public class AccountDto
    {
        public Int64 Id { get; set; }
        [Required]
        public string Name { get; set; }

        public AccountDto() { }
        public AccountDto(Account account)
        {
            this.Id = account.Id;
            this.Name = account.Name;
        }
    }
}
