using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Power_Billing_Portal.Models;
namespace Power_Billing_Portal.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginButton(User model)
        {
            string query = "SELECT * FROM Users where Email=@user AND Password=@pass AND sysRole='admin'";
            SqlConnection con = new SqlConnection("data source=.;initial catalog=IKUPAY;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework");
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user", model.Email);
            cmd.Parameters.AddWithValue("@pass", model.Password);
            // cmd.Parameters.AddWithValue("@role", "admin");
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                model.Active = 1;
                con.Close();
                TempData["tempmodel"] = model;
                //var model = TempData["model"] as User;
                return RedirectToAction("CheckAdminLogin", "Admin", model);
            }
            else
            {
                con.Close();
                return RedirectToAction("Register", "Home");
            }
        }

        public ActionResult CheckAdminLogin(User model)
        {
            if (model.Active == 1)
            {
                return RedirectToAction("AdminPanel", "Admin", model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        IKUPAYEntities db = new IKUPAYEntities();
        public ActionResult AdminPanel(User model)
        {
            if (model.Active == 1)
            {
                List<Contact> ContactList = db.Contacts.ToList();
               // db.Contacts.ToList().Where(c => c.FormID == 3);
                
                return View(ContactList);
            }

            else
            {
                return RedirectToAction("Login", "Admin");
            }

        }

        public ActionResult Delete(int? deleted)
        {
            IKUPAYEntities db = new IKUPAYEntities();
            List<Contact> FormList = db.Contacts.ToList();
            var st = FormList.Find(c => c.FormID == deleted); //finding the bill from locally created BillList.
            db.Contacts.Remove(st);//removing found bill from database.
            db.SaveChanges();//saving the changes on database.

            return RedirectToAction("Success", "Admin");
        }

        public ActionResult Success()
        {
            return View();
        }

    }
}