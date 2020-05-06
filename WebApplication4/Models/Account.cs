using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [RegularExpression(@"^[A-Z]+[a-z0-9]*$", ErrorMessage = "Username is not available."), Required, StringLength(30)]
        public string UserName { get; set; }
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$", ErrorMessage = "Minimum 5 chars, 1 Caps and 1 Num."), Required, StringLength(30)]
        public string UserPass { get; set; }
        [Required]
        public string UserRole { get; set; }

    }
    public class Type
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeID { get; set; }
        public string TypeName { get; set; }
    }
    public class Equip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EquipID { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Vuot gioi han ki tu")]
        public string EquipName { get; set; }
        public string EquipStatus { get; set; }
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Vuot gioi han ki tu")]
        [Required]
        public string EquipDes { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Chua chon Type")]
        public int TypeID { get; set; }
    }
    public class List
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ListID { get; set; }
        [ForeignKey("Account")]
        public int UserID { get; set; }
        public Account Account { get; set; }
        [ForeignKey("Equip")]
        public int EquipID { get; set; }
        public Equip Equip { get; set; }
    }
    public class CompanyManageDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Type> Types { get; set; }
        public DbSet<Equip> Equips { get; set; }
        public DbSet<List> Lists { get; set; }
        public CompanyManageDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
