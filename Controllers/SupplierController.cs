using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Power_Billing_Portal.Models;
namespace Power_Billing_Portal.Controllers
{
    public class SupplierController : Controller
    {

        public ActionResult SupplierLogin()
        {

            return View();
        }

        [HttpPost]
        public ActionResult LoginButton(User model)
        {
            string query = "SELECT * FROM Users where Email=@user AND Password=@pass AND sysRole='supplier'";
            SqlConnection con = new SqlConnection("data source=.;initial catalog=IKUPAY;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework");
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@user", model.Email);
            cmd.Parameters.AddWithValue("@pass", model.Password);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                model.Active = 1;
                con.Close();
                //TempData["tempmodel"] = model;
                //var model = TempData["model"] as User;
                return RedirectToAction("CheckSupplierLogin", "Supplier", model);
            }
            else
            {
                con.Close();
                return RedirectToAction("Register", "Home");
            }
        }

        public ActionResult CheckSupplierLogin(User model)
        {
            if (model.Active == 1)
            {
                return RedirectToAction("SupplierPanel", "Supplier", model);
            }
            else
            {
                return RedirectToAction("SupplierLogin", "Supplier");
            }
        }

        public ActionResult SupplierPanel(User model)
        {

            if (model.Active == 1)       //double-checking if the Supplier user's account is Active or not.
            {
                return View();
            }
            else
            {
                return RedirectToAction("SupplierLogin", "Supplier");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Success(Bill model)
        {
            try
            {
                IKUPAYEntities db = new IKUPAYEntities();
                Bill bill = new Bill();
                bill.UserID = model.UserID;
                bill.CompanyName = model.CompanyName;
                bill.Date = DateTime.Now;
                bill.Price = model.Price;
                bill.LastDate = model.LastDate;
                db.Bills.Add(bill);
                db.SaveChanges();
            }

            catch (Exception ex)
            {
                return RedirectToAction("Fail", "Supplier", model);
            }

            return View();
        }

        public ActionResult Fail(Bill model)
        {
            return View(model);
        }
    }
}