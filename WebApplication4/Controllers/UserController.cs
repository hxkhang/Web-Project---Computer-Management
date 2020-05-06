using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;

namespace WebApplication3.Controllers
{
    [Authorize(Policy = "Must Be User")]
    public class UserController : Controller
    {
        CompanyManageDbContext db;
        public UserController(CompanyManageDbContext DB)
        {
            db = DB;
        }
        public IActionResult Index()
        {
            var username = User.Identity.Name;
            var userDetail = db.Accounts.Where(x => x.UserName == username).FirstOrDefault();
            var userList = db.Lists.Where(x => x.UserID == userDetail.UserID).ToList();
            var equipDetail = new List<Equip>();
            foreach (List d in userList)
            {
                equipDetail.Add(db.Equips.Where(x => x.EquipID == d.EquipID).FirstOrDefault());
            }

            //var result = db.Lists.Join(db.Accounts, p => p.UserID, (p) => new { UserId = p.UserID })
            //    .Join(db.Equips, p => p.EquipID, (p) => new { EquipID = p.EquipID }).where
            var UserDetail = new UserDetail();
            var result = from x in db.Lists
                         join y in db.Accounts on x.UserID equals y.UserID
                         join z in db.Equips on x.EquipID equals z.EquipID
                         join t in db.Types on z.TypeID equals t.TypeID
                         where y.UserName == username
                         select new UserIndex { UserName = y.UserName, EquipName = z.EquipName, EquipDes = z.EquipDes, TypeName = t.TypeName, EquipID = z.EquipID };
            UserDetail.UserIndex = result.ToList();

            //ViewData["Count"] = equipDetail.Count();
            //int index = 0;
            //var UserDetail = new UserDetail { Accounts = userDetail, Equips = equipDetail };
            //{
            //    ViewData["UserName"+index.ToString()] = userDetail.UserName;
            //    ViewData["EquipName"] = i.EquipName;
            //    ViewData["EquipDes"] + index.ToString()] = i.EquipDes;
            //    var typeDetail = db.Types.Where(x => x.TypeID == i.TypeID).FirstOrDefault();
            //    ViewData["TypeName"] + index.ToString()] = typeDetail.TypeName;
            //}
            //foreach(Equip el in userList)
            //{

            //}

            return View(UserDetail);
        }
        public IActionResult Add()
        {
            //var username = HttpContext.Session.GetString("username");
            //var userDetail = db.Accounts.Where(x => x.UserName == username).FirstOrDefault();
            //var equipList = db.Equips.Where(x => x.EquipStatus == "available").ToList();
            var username = User.Identity.Name;
            var userDetail = db.Accounts.Where(x => x.UserName == username).FirstOrDefault();
            var userList = db.Lists.Where(x => x.UserID == userDetail.UserID).ToList();
            var equipDetail = new List<Equip>();
            var UserDetail = new UserDetail();
            foreach (List d in userList)
            {
                equipDetail.Add(db.Equips.Where(x => x.EquipID == d.EquipID).FirstOrDefault());
            }
            var testlist = db.Equips.Where(x => !equipDetail.Any(p => p.EquipID == x.EquipID)).Where(x => x.EquipStatus == "available").Select(x => x.EquipID).ToList();
            //var testlist1 = new List<Equip>();
            //foreach(var equip in testlist)
            //{
            //    if (equip.EquipStatus == "available")
            //        testlist1.Add(equip);

            //}
            //testlist1.Select(x => x.EquipID);
            var result = from x in db.Equips
                         join y in db.Types on x.TypeID equals y.TypeID
                         where testlist.Contains(x.EquipID)
                         select new UserIndex
                         {
                             EquipName = x.EquipName,
                             EquipDes = x.EquipDes,
                             TypeName = y.TypeName,
                             EquipID = x.EquipID
                         };

            UserDetail.UserIndex = result.ToList();
            return View(UserDetail);
        }
        public IActionResult Edit()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Addbtn(int Add)
        {
            int addEquip = Add;
            var username = User.Identity.Name;
            var userDetail = db.Accounts.Where(x => x.UserName == username).FirstOrDefault();
            var equipDetail = db.Equips.Where(x => x.EquipID == Add).FirstOrDefault();
            equipDetail.EquipStatus = "unavailable";
            var newList = db.Lists.Add(new List() { UserID = userDetail.UserID, EquipID = addEquip });
            db.SaveChanges();
            return RedirectToAction("Index", "User");
        }
        public IActionResult Del(int Delete)
        {
            var username = User.Identity.Name;
            var userDetail = db.Accounts.FirstOrDefault(x => x.UserName == username);
            var equip = db.Lists.FirstOrDefault(x => x.EquipID == Delete);
            var changeStatus = db.Equips.Where(x => x.EquipID == Delete).FirstOrDefault();
            changeStatus.EquipStatus = "available";
            var equipremove = db.Lists.FirstOrDefault(x => x.EquipID == equip.EquipID);
            db.Lists.Remove(equip);
            db.SaveChanges();
            return RedirectToAction("Index", "User");

        }
        [HttpGet]
        public IActionResult Editinfo(UserPass pass)
        {
            var username = User.Identity.Name;
            var userpass = db.Accounts.Where(x => x.UserName == username).FirstOrDefault();
            if (ModelState.IsValid)
            {
                bool validPassword = BCrypt.Net.BCrypt.Verify(pass.Password, userpass.UserPass);
                if (validPassword)
                {
                    if (pass.Password == userpass.UserPass)
                    {
                        if (pass.Newpass == pass.Newpass2)
                        {
                            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(pass.Newpass, SaltRevision.Revision2A);
                            userpass.UserPass = hashedPassword;
                            db.SaveChanges();
                            return View("Edit");
                        }
                        else
                        {
                            ViewData["error"] = "Mat khau nhap khong khop";
                            return View("Edit");
                        }

                    }
                    else
                    {
                        ViewData["error"] = "Mat khau bi sai";
                        return View("Edit");
                    }
                }
                ViewData["error"] = "Mat khau bi sai";
                return View("Edit");

            }
            else
                return View("Edit");
        }
    }
}