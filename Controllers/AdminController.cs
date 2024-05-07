using Data_Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL_WEB.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace PBL_WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _environment;

        public AdminController(AppDbContext db, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment environment )
        {
            this._db = db;
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._environment = environment;
        }

        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult CustomerManager(string search = "")
        {
            var customer = _db.customers.Where(x => x.Fullname.Contains(search)).ToList();
            ViewBag.Search = search;
            return View(customer);
        }

        public IActionResult DoctorManager(string search = "")
        {
            var doctor = _db.doctors.Where(d => d.FullName.Contains(search) || d.Specialize.Contains(search)).ToList();
            ViewBag.Search = search;
            return View(doctor);
        }

        public IActionResult CreateDoctor()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateDoctor(CreateDoctor newdt)
        {
            //if(newdt.ImageFile == null)
            //{
            //    ModelState.AddModelError("ImageFile", "imagefile is require");
            //}
            if (ModelState.IsValid)
            {
                //tao ten file anh
                string filename = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                filename += Path.GetExtension(newdt.ImageFile!.FileName);
                string fullpath = _environment.WebRootPath + "/Doctor/" + filename;
                using (var stream = System.IO.File.Create(fullpath))
                {
                    newdt.ImageFile.CopyTo(stream);
                }
                //////////////

                AppUser newuser = new AppUser()
                {
                    UserName = newdt.UserName,
                    PhoneNumber = newdt.PhoneNumber,
                    Email = newdt.Email,
                };
                var result = await _userManager.CreateAsync(newuser, newdt.Password);
                if (result.Succeeded)
                {
                    result = await _userManager.AddToRoleAsync(newuser, "Doctor");
                    if (result.Succeeded)
                    {
                        //await _signInManager.SignInAsync(newuser, isPersistent: false);
                        Doctor newdoctor = new Doctor()
                        {
                            FullName = newdt.FullName,
                            PhoneNumber = newdt.PhoneNumber,
                            Email = newdt.Email,
                            Specialize = newdt.specialize,
                            AcountId = newuser.Id,
                            Discripsion = newdt.Discripsion,
                            ImageFileName = filename,
                        };
                        await _db.doctors.AddAsync(newdoctor);
                        await _db.SaveChangesAsync();
                        return RedirectToAction("DoctorManager");
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }
                }
            }
            return View(newdt);
        }
        public IActionResult CreateSchedule()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateSchedule(ScheduleModel schedule)
        {
            if(ModelState.IsValid)
            {
                Schedule newschedule = new Schedule()
                {
                    DayofWeek = schedule.DayofWeek,
                    Starrttime = schedule.Starrttime,
                    Endtime = schedule.Endtime,
                };
                _db.schedules.Add(newschedule);
                _db.SaveChanges();
                return RedirectToAction("CreateSchedule");
            }
            return View(schedule);
        }


        public IActionResult ImportWarehouse()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ImportWarehouse(ImportMedicine medicine)
        {
            if (ModelState.IsValid)
            {
               var med = _db.medicines.Where(md => md.Name == medicine.Name).FirstOrDefault();
                if(med == null)
                {
                    var newmedicine = new Medicine()
                    {
                        Name = medicine.Name,
                    };
                    _db.medicines.Add(newmedicine);
                }
                else
                {
                    var inven = _db.medicineinventories.Where(inven => inven.MedicineId == med.ID).FirstOrDefault();
                    if(inven == null)
                    {
                        var newinventory = new MedicineInventories()
                        {
                            MedicineId = med.ID,
                            ExpiryDate = medicine.ExpiryDate,
                            InventoryQuantity = medicine.Quantity,
                        };
                        _db.medicineinventories.Add(newinventory);
                    }
                    else
                    {
                        if(inven.ExpiryDate == medicine.ExpiryDate)
                        {
                            inven.InventoryQuantity += medicine.Quantity;
                        }
                        else
                        {
                            var newinventory = new MedicineInventories()
                            {
                                MedicineId = med.ID,
                                ExpiryDate = medicine.ExpiryDate,
                                InventoryQuantity = medicine.Quantity,
                            };
                            _db.medicineinventories.Add(newinventory);
                        }
                    }
                }
                _db.SaveChanges();
            }
            return View();
        }


    }
}
