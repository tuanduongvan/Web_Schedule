using Data_Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PBL_WEB.Models;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace PBL_WEB.Controllers
{
    public class HomeController : Controller
    {
       
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly AppDbContext db;
        public HomeController(AppDbContext _db, UserManager<AppUser> _user, SignInManager<AppUser> _signInManager)
        {
            db = _db;
            userManager = _user;
            signInManager = _signInManager;
        }

        //HTTP GET Home/Dangky
        public IActionResult Dangky()
        {
            return View();
        }
        //HTTP POST
        [HttpPost]
        public async Task<IActionResult> Dangky(RegisterModel user)
        {
            if(ModelState.IsValid)
            {
                AppUser newUser = new AppUser()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                };
                var result = await userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)
                {
                    result = await userManager.AddToRoleAsync(newUser, "Customer");
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(newUser, isPersistent: false);
                        Customers customers = new Customers()
                        {
                            Email = user.Email,
                            Phonenumber = user.PhoneNumber,
                            Fullname = user.FullName,
                            Address = user.Address,
                            DayofBirth = user.DayOfBirth,
                            AcountId = newUser.Id,
                        };
                        await db.customers.AddAsync(customers);
                        await db.SaveChangesAsync();
                        return RedirectToAction("DangNhap");
                    }
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            return View(user);
        }
        

        //httpget        
        public IActionResult DangNhap()
        {
            return View();
        }
        //http post
        [HttpPost]
        public async Task<IActionResult> DangNhap(SignInModel model)
        {
            if(ModelState.IsValid)
            {
                var result =  await signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent : true, lockoutOnFailure: true);
                if(result.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(model.UserName);
                    //var roles = await userManager.GetRolesAsync(user);

                    //// Thêm vai trò vào Claims của người dùng
                    //var claims = new List<Claim>
                    //{
                    //    new Claim(ClaimTypes.Name, user.UserName),
                    //    // Thêm các claim về vai trò của người dùng vào đây
                    //};

                    //foreach (var role in roles)
                    //{
                    //    claims.Add(new Claim(ClaimTypes.Role, role));
                    //}

                    //var userIdentity = new ClaimsIdentity(claims, "login");
                    //var principal = new ClaimsPrincipal(userIdentity);

                    //// Thay đổi đối tượng ClaimsPrincipal của HttpContext để thêm vai trò của người dùng
                    //HttpContext.User = principal;

                    //// Redirect đến trang sau khi đăng nhập thành công
                    //if (roles.Contains("Admin"))
                    //{
                    //    return RedirectToAction("Admin", "Admin");
                    //}
                    //else if(roles.Contains("Customer"))
                    //{
                    //    return RedirectToAction("CustomerVIew");
                    //}

                    //hoặc có thể dùng cahcs ni:
                    if(await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Admin", "Admin");
                    }
                    else if( await userManager.IsInRoleAsync(user, "Customer"))
                    {
                        return RedirectToAction("CustomerView");
                    }
                    else if(await userManager.IsInRoleAsync(user, "Doctor"))
                    {
                        return RedirectToAction("DoctorPage", "Doctor");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "Your username or password not right");
                }
            }
            //ViewBag.message = "not valid" con cai valid ni rang he
            return View(model);
        }
        public IActionResult CustomerVIew()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangXuat()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index");
        }
        public IActionResult Information(string id)
        {
            var customer = db.customers.Where(ct => ct.AcountId == id).FirstOrDefault();
            var ctmedit = new CustomerEdit()
            {
                Id = customer.Id,
                Fullname = customer.Fullname,
                Email = customer.Email,
                Phonenumber = customer.Phonenumber,
                Address = customer.Address,
                DayofBirth = customer.DayofBirth,
                AcountId = customer.AcountId,
            };

            return View(ctmedit);
        }

        [HttpPost]
        public async Task<IActionResult> Information(CustomerEdit ctm)
        {

            if (ModelState.IsValid)
            {
                var customer = await db.customers.FindAsync(ctm.Id);
                customer.Fullname = ctm.Fullname;
                customer.Address = ctm.Address;
                customer.Phonenumber = ctm.Phonenumber;
                customer.Email = ctm.Email;
                customer.DayofBirth = ctm.DayofBirth;
                customer.AcountId = ctm.AcountId;
                var user = await db.appUsers.FindAsync(ctm.AcountId);

                user.PhoneNumber = ctm.Phonenumber;
                user.Email = ctm.Email;
                await db.SaveChangesAsync();
                return View(ctm);
            }
            return View(ctm);
        }
        public IActionResult ViewDoctor(string search = "")
        {
            var doctor = db.doctors.Where(d => d.FullName.Contains(search) || d.Specialize.Contains(search)).ToList();
            ViewBag.Search = search;
            return View(doctor);
        }

        public IActionResult DoctorDetail(string id)
        {
            var doctor = db.doctors.Where(dt => dt.id == id).FirstOrDefault();
            return View(doctor);
        }

        public IActionResult SetAppointment(string id)
        {
            int countMonday = 0, countTuesday = 0, countWednesday = 0, countThursday = 0, countFriday = 0, countSaturday = 0, countSunday = 0;
            var doctoravailable = db.doctorSchedules.Where(dt => dt.DoctorId == id).Select(dt => dt.ScheduleId).ToList();
            Dictionary<string, List<string>> Shiftavailable = new Dictionary<string, List<string>>();
            //lay ngay lam viec va so ca tuong ung
            Dictionary<string, List<string>> dayavailable = new Dictionary<string, List<string>>();
            foreach(var item in doctoravailable)
            {
                var temp = db.schedules.Where(sche => sche.ID == item).FirstOrDefault();
                if (temp.DayofWeek == "Monday")
                {
                    var start = temp.Starrttime.ToString();
                    var end = temp.Endtime.ToString();
                    if (dayavailable.ContainsKey("Monday"))
                    {
                        dayavailable["Monday"].Add(start);
                        dayavailable["Monday"].Add(end);
                    }
                    else
                    {
                        var time = new List<string>();
                        time.Add(start);
                        time.Add(end);
                        dayavailable.Add("Monday", time);
                    }
                    countMonday += 1;
                }
                else if (temp.DayofWeek == "Tuesday")
                {
                    var start = temp.Starrttime.ToString();
                    var end = temp.Endtime.ToString();
                    if (dayavailable.ContainsKey("Tuesday"))
                    {
                        dayavailable["Tuesday"].Add(start);
                        dayavailable["Tuesday"].Add(end);
                    }
                    else
                    {
                        var time = new List<string>();
                        time.Add(start);
                        time.Add(end);
                        dayavailable.Add("Tuesday", time);
                    }
                    countTuesday += 1;
                }
                else if (temp.DayofWeek == "Wednesday")
                {
                    var start = temp.Starrttime.ToString();
                    var end = temp.Endtime.ToString();
                    if (dayavailable.ContainsKey("Wednesday"))
                    {
                        dayavailable["Wednesday"].Add(start);
                        dayavailable["Wednesday"].Add(end);
                    }
                    else
                    {
                        var time = new List<string>();
                        time.Add(start);
                        time.Add(end);
                        dayavailable.Add("Wednesday", time);
                    }
                    countWednesday += 1;
                }
                else if (temp.DayofWeek == "Thursday")
                {
                    var start = temp.Starrttime.ToString();
                    var end = temp.Endtime.ToString();
                    if (dayavailable.ContainsKey("Thursday"))
                    {
                        dayavailable["Thursday"].Add(start);
                        dayavailable["Thursday"].Add(end);
                    }
                    else
                    {
                        var time = new List<string>();
                        time.Add(start);
                        time.Add(end);
                        dayavailable.Add("Thursday", time);
                    }
                    countThursday += 1;
                }
                else if (temp.DayofWeek == "Friday")
                {
                    var start = temp.Starrttime.ToString();
                    var end = temp.Endtime.ToString();
                    if (dayavailable.ContainsKey("Friday"))
                    {
                        dayavailable["Friday"].Add(start);
                        dayavailable["Friday"].Add(end);
                    }
                    else
                    {
                        var time = new List<string>();
                        time.Add(start);
                        time.Add(end);
                        dayavailable.Add("Friday", time);
                    }
                    countFriday += 1;
                }
                else if(temp.DayofWeek == "Saturday")
                {
                    var start = temp.Starrttime.ToString();
                    var end = temp.Endtime.ToString();
                    if (dayavailable.ContainsKey("Saturday"))
                    {
                        dayavailable["Saturday"].Add(start);
                        dayavailable["Saturday"].Add(end);
                    }
                    else
                    {
                        var time = new List<string>();
                        time.Add(start);
                        time.Add(end);
                        dayavailable.Add("Saturday", time);
                    }
                    countSaturday += 1;
                }
            }
            //so ca duoc dat cua bac si voi ngay tuong ung
            var doctorappointment = db.appointmentSchedules.Where(app => app.DoctorId == id).ToList();
            foreach(var item in doctorappointment)
            {
                //tim gia tri de them vao dictionary
                var datekey = item.Date.ToString("MM-dd-yyyy");
                var timeline = item.StartTime.ToString();
                if(Shiftavailable.ContainsKey(datekey))
                {
                    Shiftavailable[datekey].Add(timeline);
                }
                else
                {
                    List<string> listtime = new List<string>();
                    listtime.Add(timeline);
                    Shiftavailable.Add(datekey, listtime);
                }
            }

            List<string> Dayunvailable = new List<string>();

            //tim ngay ban cua bac si
            foreach(var item in Shiftavailable)
            {
                DateTime date = DateTime.ParseExact(item.Key, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                string dateofweek = date.DayOfWeek.ToString();
                if(dateofweek == "Monday")
                {
                    //neu so ca duoc dat == so sa bsi da dang ki thi xem do la ngay ban
                    if(item.Value.Count == countMonday)
                    {
                        Dayunvailable.Add(item.Key);
                    }
                }
                else if (dateofweek == "Tuesday")
                {
                    if (item.Value.Count == countTuesday)
                    {
                        Dayunvailable.Add(item.Key);
                    }
                }
                else if (dateofweek == "Wednesday")
                {
                    if (item.Value.Count == countWednesday)
                    {
                        Dayunvailable.Add(item.Key);
                    }
                }
                else if (dateofweek == "Thursday")
                {
                    if (item.Value.Count == countThursday)
                    {
                        Dayunvailable.Add(item.Key);
                    }
                }
                else if (dateofweek == "Friday")
                {
                    if (item.Value.Count == countFriday)
                    {
                        Dayunvailable.Add(item.Key);
                    }
                }
                else if (dateofweek == "Saturday")
                {
                    if (item.Value.Count == countSaturday)
                    {
                        Dayunvailable.Add(item.Key);
                    }
                }


            }

            DateTime currentday = DateTime.Now.Date;
            DateTime endday = currentday.AddDays(365);

            while (currentday <= endday) 
            {
                var dateofweek = currentday.DayOfWeek.ToString();
                if (dateofweek == "Monday")
                {
                    if(countMonday == 0)
                        Dayunvailable.Add(currentday.ToString("MM-dd-yyyy"));
                }
                else if (dateofweek == "Tuesday")
                {
                    if (countTuesday == 0)
                        Dayunvailable.Add(currentday.ToString("MM-dd-yyyy"));
                }
                else if (dateofweek == "Wednesday")
                {
                    if (countWednesday == 0)
                        Dayunvailable.Add(currentday.ToString("MM-dd-yyyy"));
                }
                else if (dateofweek == "Thursday")
                {
                    if (countThursday == 0)
                        Dayunvailable.Add(currentday.ToString("MM-dd-yyyy"));
                }
                else if (dateofweek == "Friday")
                {
                    if (countFriday == 0)
                        Dayunvailable.Add(currentday.ToString("MM-dd-yyyy"));
                }
                else if (dateofweek == "Saturday")
                {
                    if (countSaturday == 0)
                        Dayunvailable.Add(currentday.ToString("MM-dd-yyyy"));
                }
                else if (dateofweek == "Sunday")
                {
                    if (countSunday == 0)
                        Dayunvailable.Add(currentday.ToString("MM-dd-yyyy"));
                }

                currentday = currentday.AddDays(1);
            }
            //chuyen doi chuoi ngay ban thanh chuoi Json
            if(Dayunvailable != null)
            {
                ViewBag.Dayunavailablejson = JsonConvert.SerializeObject(Dayunvailable);
            }
            ViewBag.Doctorid = id;

            ViewBag.doctoravailability = dayavailable;
            return View();
        }
        [HttpPost]
        public IActionResult SetAppointment(string doctorid, string customerid, SetAppointment appoint)
        {
            if(ModelState.IsValid)
            {
                var customer = db.customers.Where(ctm => ctm.AcountId == customerid).FirstOrDefault();
                var newappointment = new AppointmentSchedules()
                {
                    Date = appoint.date.Date,
                    StartTime = appoint.starttime,
                    DoctorId = doctorid,
                    CustomerId = customer.Id,
                };
                db.appointmentSchedules.Add(newappointment);
                db.SaveChanges();
                return RedirectToAction("CustomerView");
            }

            return View(appoint);
        }
        public IActionResult MyAppointment(string id)
        {
            var customer = db.customers.Where(ctm => ctm.AcountId == id).FirstOrDefault();
            var appointment = db.appointmentSchedules.Where(ap => ap.CustomerId == customer.Id).Include(dt => dt.doctor).ToList();
            return View(appointment);
        }
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult GetOptionTime(string date)
        {
            List<string> unvailableAppointment = new List<string>();
            var newdate = date.Split('-');
            var newdatetime = new DateTime(int.Parse(newdate[2]), int.Parse(newdate[0]), int.Parse(newdate[1]));
            var appointment = db.appointmentSchedules.Where(ap => ap.Date == newdatetime).ToList();
            foreach(var item in appointment)
            {
                unvailableAppointment.Add(item.StartTime.ToString());
            }

            return Ok(unvailableAppointment);
        }

    }
}
