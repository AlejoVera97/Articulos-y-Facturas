using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;








namespace Sitio.Controllers
{
    public class HomeController : Controller
    {




        //no tiene el HTTPGET explicitamente - pero al ser la unica operacion
        //asociada a la vista con nombre index del controlador
        //se asume es la operacion que se ejecuta al SOLICITAR la vista == HttpGet
        public ActionResult Index()
        {
            Session["Logueo"] = null;
            return View();
        }






        public ActionResult Principal()
        {
            //limpio variables de listados
            Session["Lista"] = null; 
            return View();
        }

    }
}