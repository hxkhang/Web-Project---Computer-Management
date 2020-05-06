using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Models
{
    public class UserDetail
    {
        public Account Accounts { get; set; }
        public List<Equip> Equips { get; set; }
        public List<List> Lists { get; set; }
        public Models.Type Types  { get; set; }

        //tao list chua userIndex => Index chi lay 1 du lieu
        public List<UserIndex> UserIndex { get; set; }
        //tao list chua EquipInfo
        public List<EquipInfo> EquipInfos { get; set; }
    }
    public class UserPass
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 20")]
        public string Password { get; set; }
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 20")]
        public string Newpass { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the new pass again")]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessage = "Length must be between 5 to 20")]
        public string Newpass2 { get; set; }
    }
}
