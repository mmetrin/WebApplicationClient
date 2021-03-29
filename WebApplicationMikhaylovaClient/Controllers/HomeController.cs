using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebApplicationMikhaylovaClient.Models;

namespace WebApplicationMikhaylovaClient.Controllers
{
    public class HomeController : Controller
    {
        private PateticoRPMEntities db = new PateticoRPMEntities();

        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Parameters);
            return View(products.ToList());
        }


        //[Authorize]
        //public ActionResult Index()
        //{
        //    //var x = Request.IsAuthenticated;
        //    //var y = User.Identity.Name;
        //    //var z = User.IsInRole("Admin");
        //    return View();
        //}


        //НАДО
        [Authorize]
        public ActionResult Logout()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult AccessError()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("AccessError");
            else
                return View();
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(string UserLog, string UserPas, string returnUrl)
        {
            var user = db.Users.FirstOrDefault(u => u.email == UserLog && u.password == UserPas);
            if (user == null)
            {
                return Json(new { ms = true });
            }
            else
            {
                FormsAuthentication.SetAuthCookie(UserLog, false);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                                    && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    return Redirect(returnUrl);
                else
                    return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult CheckRepeatLogin(string UserEmail)
        {
            var user = db.Users.FirstOrDefault(p => p.email == UserEmail);
            if (user != null)
            {
                return Json(new { msg = true });
            }
            else
            {
                return Json(new { msg = false });
            }
        }

        [HttpPost]

        public ActionResult Create(string UserEmail, string UserPassword, string UserName, string UserSurname, string UserPatronymic, string UserDateBirth, string UserPhone)
        {

            Users users = new Users
            {
                name = UserName,
                surname = UserSurname,
                patronymic = UserPatronymic,
                date_of_birth = Convert.ToDateTime(UserDateBirth),
                email = UserEmail,
                password = UserPassword,
                phone = UserPhone,
                rolead = "U"
            };
            db.Users.Add(users);
            db.SaveChanges();


            //var products = db.Products.Include(p => p.Parameters);
            //return View(products.ToList());
            return RedirectToAction("Index");

        }




        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}