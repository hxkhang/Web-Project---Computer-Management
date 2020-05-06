using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Html;
using WebApplication4.Models;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;

namespace WebApplication4.Controllers
{
    [Authorize(Policy = "Must Be Admin")]
    public class AdminController : Controller
    {
        CompanyManageDbContext db;
        public AdminController(CompanyManageDbContext DB)
        {
            db = DB;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {
            return View();
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult ShowEquip()
        {
            var UserDetail = new UserDetail();
            var equipDetail = from a in db.Equips
                              join b in db.Types on a.TypeID equals b.TypeID
                              select new EquipInfo
                              {
                                  EquipName = a.EquipName,
                                  EquipDes = a.EquipDes,
                                  EquipStatus = a.EquipStatus,
                                  TypeName = b.TypeName,
                                  EquipID = a.EquipID
                              };
            UserDetail.EquipInfos = equipDetail.ToList();
            return View(UserDetail);
        }
        public IActionResult EditEquip(int Edit)
        {
            var result = from a in db.Equips
                         join b in db.Types on a.TypeID equals b.TypeID
                         where a.EquipID == Edit
                         select new EquipInfo { EquipID = a.EquipID, EquipName = a.EquipName, TypeName = b.TypeName, EquipDes = a.EquipDes };
            //var result = db.Equips.Where(x => x.EquipID == Edit).Select(x => new EquipInfo() { EquipName = x.EquipName }).FirstOrDefault();

            return View(result.FirstOrDefault());
        }

        [HttpPost]
        public IActionResult ConfirmEdit(EquipInfo eqinfo,int Edit)
        {
            if (ModelState.IsValid)
            {
                var result = db.Equips.Where(x => x.EquipID == Edit).FirstOrDefault();
                result.EquipName = eqinfo.EquipName;
                result.EquipDes = eqinfo.EquipDes;
                db.SaveChanges();
                ViewData["Info"] = "Edit Thanh Cong";
                return RedirectToAction("ShowEquip", "Admin");
            }
            else
            {
                var result = from a in db.Equips
                             join b in db.Types on a.TypeID equals b.TypeID
                             where a.EquipID == eqinfo.EquipID
                             select new EquipInfo { EquipID = a.EquipID, EquipName = a.EquipName, TypeName = b.TypeName, EquipDes = a.EquipDes };
                return View("EditEquip", result.FirstOrDefault());
            }
        }

        public IActionResult DelEquip(int EquipID)
        {
            var result = db.Equips.Where(x => x.EquipID == EquipID).FirstOrDefault();
            db.Remove(result);
            db.SaveChanges();
            var UserDetail = new UserDetail();
            var equipDetail = from a in db.Equips
                              join b in db.Types on a.TypeID equals b.TypeID
                              select new EquipInfo
                              {
                                  EquipName = a.EquipName,
                                  EquipDes = a.EquipDes,
                                  EquipStatus = a.EquipStatus,
                                  TypeName = b.TypeName,
                                  EquipID = a.EquipID
                              };
            UserDetail.EquipInfos = equipDetail.ToList();
            return View("ShowEquip",UserDetail);
        }

        public IActionResult AddEquip()
        {
            return View();
        }

        public IActionResult CreateEquip(Equip equip)
        {
            if (ModelState.IsValid)
            {
                var newequip = new Equip();
                newequip.EquipName = equip.EquipName;
                newequip.EquipDes = equip.EquipDes;
                newequip.TypeID = equip.TypeID;
                newequip.EquipStatus = "available";
                db.Add(newequip);
                db.SaveChanges();
                return View("AddEquip");
            }
            else
                return View("AddEquip");
        }

        public IActionResult AddAcc()
        {
            return View();
        }

        public IActionResult CreateAcc(Account acc)
        {
            if (ModelState.IsValid)
            {
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(acc.UserPass, SaltRevision.Revision2A);
                var newacc = new Account();
                newacc.UserName = acc.UserName;
                newacc.UserPass = hashedPassword;
                newacc.UserRole = acc.UserRole;
                db.Add(newacc);
                db.SaveChanges();
                return View("AddAcc");
            }
            else
                return View("AddAcc");
        }
    }
}