using EserS6L.Models;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;

namespace EserS6L.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(Admin admin)
        {
            string connString = ConfigurationManager.ConnectionStrings["EserS6Conn"].ToString();
            SqlConnection conn = new SqlConnection(connString);

            try
            {
                conn.Open();
                string checkAdminQuery = "SELECT * FROM Admins WHERE Username = @username AND Password = @password";

                SqlCommand checkAdminCmd = new SqlCommand(checkAdminQuery, conn);
                checkAdminCmd.Parameters.AddWithValue("@username", admin.Username);
                checkAdminCmd.Parameters.AddWithValue("@password", admin.Password);

                SqlDataReader checkAdminReader = checkAdminCmd.ExecuteReader();

                if (checkAdminReader.HasRows)
                {
                    checkAdminReader.Read();
                    FormsAuthentication.SetAuthCookie(checkAdminReader["Id"].ToString(), true);

                    return RedirectToAction("Index", "Admin");
                }

            }
            catch (Exception ex) { }
            finally { conn.Close(); }

            return RedirectToAction("Index");
        }


        [Authorize, HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

    }
}