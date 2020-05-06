using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Models
{
    public class AllInfo
    {
        public Account Accounts { get; set; }
        public List<Equip> Equips { get; set; }
        public List<List> Lists { get; set; }
        public Models.Type Types { get; set; }
    }
}
