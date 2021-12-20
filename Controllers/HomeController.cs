using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Power_Billing_Portal.Models;
namespace Power_Billing_Portal.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Info()
        {


            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ContactButton(Contact model)
        {
            try
            {
                IKUPAYEntities db = new IKUPAYEntities();
                Contact ticket = new Contact();
                ticket.Name = model.Name;
                ticket.Email = model.Email;
                ticket.Gsm = model.Gsm;
                ticket.Subject = model.Subject;
                ticket.Message = model.Message;
                ticket.TicketDate = DateTime.Now;
                ticket.Status = "Active";
                db.Contacts.Add(ticket);
                db.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Success", "Home");
        }

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult LoginButton(User model)
        {
            string query = "SELECT * FROM Users where Email=@user AND Password=@pass AND sysRole = 'customer'";
            SqlConnection con = new SqlConnection(" data source=.;initial catalog=IKUPAY;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework");
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user", model.Email);
            cmd.Parameters.AddWithValue("@pass", model.Password);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                model.Active = 1;
                model.UserID = dr.GetInt32(0);
                con.Close();
                return RedirectToAction("CustomerPanel", "Home", model);
            }
            else
            {
                con.Close();
                return RedirectToAction("Register", "Home");
            }

        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterButton(User model)
        {
            try
            {
                IKUPAYEntities db = new IKUPAYEntities();
                User user = new User();
                user.FirstName = model.FirstName;
                user.MiddleName = model.MiddleName;
                user.LastName = model.LastName;
                user.Gsm = model.Gsm;
                user.Email = model.Email;
                user.Password = model.Password;
                user.RegisterDate = DateTime.Now;
                user.sysRole = "customer";
                db.Users.Add(user);
                db.SaveChanges();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Login");
        }

        public ActionResult CustomerPanel(User model)
        {
            if (model.Active == 1)
            {
                IKUPAYEntities db = new IKUPAYEntities();

                List<Bill> BillList = db.Bills.ToList();
                List<Bill> UsersBills = new List<Bill>();

                foreach (var item in BillList.ToList())
                {
                    if (item.UserID == model.UserID)
                    {
                        UsersBills.Add(item);
                    }
                }
                return View(UsersBills);
            }

            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Success2()
        {
            return View();
        }


        public ActionResult Pay(int? deletedbill)
        {
            IKUPAYEntities db = new IKUPAYEntities();
            List<Bill> BillList = db.Bills.ToList();
            var st = BillList.Find(c => c.BillID == deletedbill); //finding the bill from locally created BillList.
            db.Bills.Remove(st);//removing found bill from database.
            db.SaveChanges();//saving the changes on database.
            return RedirectToAction("Success2", "Home");
        }
    }
}