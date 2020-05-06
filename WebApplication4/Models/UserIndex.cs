using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace WebApplication4.Models
{
    public class UserIndex
    {
        public string UserName { get; set; }
        public string EquipName { get; set; }
        public string EquipDes { get; set; }
        public string TypeName { get; set; }
        public int EquipID { get; set; }
    }
    public class EquipInfo
    {
        [Required]
        [StringLength(maximumLength: 40, MinimumLength = 0, ErrorMessage = "Vuot qua gioi han ki tu")]
        public string EquipName { get; set; }
        [Required]
        [StringLength(maximumLength: 40, MinimumLength = 0, ErrorMessage = "Vuot qua gioi han ki tu")]
        public string EquipDes { get; set; }
        public string EquipStatus { get; set; }
        public string TypeName { get; set; }
        public int EquipID { get; set; }
    }
    public class AddEmploy
    {
        [Required, StringLength(30)]
        public string UserName { get; set; }
        [Required, StringLength(30)]
        public string UserPass { get; set; }
        [Required]
        public string UserRole { get; set; }
    }

}
