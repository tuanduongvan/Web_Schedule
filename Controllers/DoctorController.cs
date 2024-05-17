using Data_Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PBL_WEB.Models;

namespace PBL_WEB.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly AppDbContext db;
        //tạo mấy thuộc tính lưu lại mấy cái dữ liệu ở hàm get 
        //rồi truyền qua view bag của hàm post view mới nhận được data
        //cách hai thì mình truyền dữ liệu và ViewData hay TemData thì mọi controller trong App đều sử dụng được
        //nhớ mang máng thế thôi
        //mà dùng cách 1 là ok nhất
        public static List<SelectListItem> listMonday { get; set; }
        public static List<SelectListItem> listTuesday { get; set; }
        public static List<SelectListItem> listWednesday { get; set; }
        public static List<SelectListItem> listThursday { get; set; }
        public static List<SelectListItem> listFriday { get; set; }
        public static List<SelectListItem> listSaturday { get; set; }
        public static List<string> listschedule { get; set; }
        public DoctorController(AppDbContext db)
        {
            this.db = db;
        }
        public IActionResult RegisterSchedule(string id)
        {
            var doctor = db.doctors.Where(dt => dt.AcountId == id).FirstOrDefault();
            //tim ra lịch hiện tại của bác sĩ
            var listscheduleid = db.doctorSchedules.Where(sh => sh.DoctorId == doctor.id).Select(item => item.ScheduleId).ToList();
            listschedule = new List<string>();
            foreach(var item in listscheduleid)
            {
                listschedule.Add(item);
            }
            ViewBag.listscheduleid = listschedule;
            var monday = db.schedules.Where(md => md.DayofWeek == "Monday").ToList();
            listMonday = new List<SelectListItem>();
            foreach(var item in monday)
            {
                listMonday.Add(new SelectListItem()
                { 
                    Value = item.ID,
                    Text = "(" + item.Starrttime + "->" + item.Endtime + ")",
                });

            }
            ViewBag.listmonday = listMonday;
            

            var tuesday = db.schedules.Where(md => md.DayofWeek == "Tuesday").ToList();
            listTuesday = new List<SelectListItem>();
            foreach (var item in tuesday)
            {
                listTuesday.Add(new SelectListItem()
                {
                    Value = item.ID,
                    Text = "(" + item.Starrttime + "->" + item.Endtime + ")",
                });

            }
            ViewBag.listtuesday = listTuesday;

            var wednesday = db.schedules.Where(md => md.DayofWeek == "Wednesday").ToList();
            listWednesday = new List<SelectListItem>();
            foreach (var item in wednesday)
            {
                listWednesday.Add(new SelectListItem()
                {
                    Value = item.ID,
                    Text = "(" + item.Starrttime + "->" + item.Endtime + ")",
                });

            }
            ViewBag.listwednesday = listWednesday;

            var Thursday = db.schedules.Where(md => md.DayofWeek == "Thursday").ToList();
            listThursday = new List<SelectListItem>();
            foreach (var item in Thursday)
            {
                listThursday.Add(new SelectListItem()
                {
                    Value = item.ID,
                    Text = "(" + item.Starrttime + "->" + item.Endtime + ")",
                });

            }
            //listThursday = listThursday;
            ViewBag.listThursday = listThursday;

            var friday = db.schedules.Where(md => md.DayofWeek == "Friday").ToList();
            listFriday = new List<SelectListItem>();
            foreach (var item in friday)
            {
                listFriday.Add(new SelectListItem()
                {
                    Value = item.ID,
                    Text = "(" + item.Starrttime + "->" + item.Endtime + ")",
                });

            }
            ViewBag.listfriday = listFriday;
            //listFriday = listfriday;

            var saturday = db.schedules.Where(md => md.DayofWeek == "Saturday").ToList();
            listSaturday = new List<SelectListItem>();
            foreach (var item in saturday)
            {
                listSaturday.Add(new SelectListItem()
                {
                    Value = item.ID,
                    Text = "(" + item.Starrttime + "->" + item.Endtime + ")",
                });

            }
            ViewBag.listsaturday = listSaturday;
            //listSaturday = listsaturday;

            return View();
        }

        [HttpPost]
        public IActionResult RegisterSchedule(ListSchedule list)
        {
         
            var doctor = db.doctors.Where(dt => dt.AcountId == list.IdDoctor).FirstOrDefault();
            var dt = new List<DoctorSchedule>();
            if (list.Monday != null)
            {
                foreach (var item in list.Monday)
                {
                    var tem = db.doctorSchedules.Where(itm => itm.ScheduleId == item && itm.DoctorId == list.IdDoctor).FirstOrDefault();
                    if (tem == null)
                    {
                        dt.Add(new DoctorSchedule()
                        {
                            DoctorId = doctor.id,
                            ScheduleId = item
                        });
                    }
                }
            }
            if (list.Tuesday != null)
            {
                foreach (var item in list.Tuesday)
                {
                    var tem = db.doctorSchedules.Where(itm => itm.ScheduleId == item && itm.DoctorId == list.IdDoctor).FirstOrDefault();
                    if (tem == null)
                    {
                        dt.Add(new DoctorSchedule()
                        {
                            DoctorId = doctor.id,
                            ScheduleId = item
                        });
                    }
                }
            }
            if (list.Wednesday != null)
            {
                foreach (var item in list.Wednesday)
                {
                    var tem = db.doctorSchedules.Where(itm => itm.ScheduleId == item && itm.DoctorId == list.IdDoctor).FirstOrDefault();
                    if (tem == null)
                    {
                        dt.Add(new DoctorSchedule()
                        {
                            DoctorId = doctor.id,
                            ScheduleId = item
                        });
                    }
                }
            }

            if (list.Thursday != null)
            {
                foreach (var item in list.Thursday)
                {
                    var tem = db.doctorSchedules.Where(itm => itm.ScheduleId == item && itm.DoctorId == list.IdDoctor).FirstOrDefault();
                    if (tem == null)
                    {
                        dt.Add(new DoctorSchedule()
                        {
                            DoctorId = doctor.id,
                            ScheduleId = item
                        });
                    }
                }
            }

            if (list.Friday != null)
            {
                foreach (var item in list.Friday)
                {
                    var tem = db.doctorSchedules.Where(itm => itm.ScheduleId == item && itm.DoctorId == list.IdDoctor).FirstOrDefault();
                    if (tem == null)
                    {
                        dt.Add(new DoctorSchedule()
                        {
                            DoctorId = doctor.id,
                            ScheduleId = item
                        });
                    }
                }
            }

            if (list.Saturday != null)
            {
                foreach (var item in list.Saturday)
                {
                    var tem = db.doctorSchedules.Where(itm => itm.ScheduleId == item && itm.DoctorId == list.IdDoctor).FirstOrDefault();
                    if (tem == null)
                    {
                        dt.Add(new DoctorSchedule()
                        {
                            DoctorId = doctor.id,
                            ScheduleId = item
                        });
                    }
                }
            }
            db.doctorSchedules.AddRange(dt);
            db.SaveChanges();
            ViewBag.listmonday = listMonday;
            ViewBag.listtuesday = listTuesday;
            ViewBag.listwednesday = listWednesday;
            ViewBag.listThursday = listThursday;
            ViewBag.listfriday = listFriday;
            ViewBag.listsaturday = listSaturday;
            ViewBag.listscheduleid = listschedule;
            return Redirect($"/Doctor/RegisterSchedule/{list.IdDoctor}");
        }

        public IActionResult MyAppointment(string Id)
        {
            var Doctor = db.doctors.Where(dt => dt.AcountId == Id).FirstOrDefault();
            if (Doctor != null)
            {
                var appointment = db.appointmentSchedules.Where(ap => ap.DoctorId == Doctor.id).Include(ctm => ctm.customer).ToList();
                return View(appointment);
            }
            else return RedirectToAction("DoctorPage");
        }
        public IActionResult DoctorPage()
        {
            return View();
        }
    }
}
