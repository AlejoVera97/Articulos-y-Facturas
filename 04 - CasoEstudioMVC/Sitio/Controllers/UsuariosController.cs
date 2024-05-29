using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Sitio.Models;

namespace Sitio.Controllers
{
    public class UsuariosController : Controller
    {
        //****************************LOGUEO
        [HttpGet]
        public ActionResult FormLogueo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormLogueo(Usuario U)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    new UsuariosDB().Logueo(U);
                    // no hubo error, usuario correcto
                    Session["Logueo"] = U;
                    return RedirectToAction("Principal", "Home");
                }
                else
                    return View();

            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

 
        //****************************DESLOGUEO
        public ActionResult Deslogueo()
        {
            return RedirectToAction("Index", "Home");
        }
















        //****************************CAMBIOS PASS 
        [HttpGet]
        public ActionResult FormCambioPass()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormCambioPass(string PassActual, string PassNueva, string PassConfirmar)
        {
            try
            {
                //validamos si la pass actual es correcta
                if (PassActual.Trim() != ((Usuario)Session["Logueo"]).PassLog.Trim())
                    throw new Exception("La pass actual no es correcta - No se modifica");

                //validamos que nueva pass y confirm sean identicas 
                if (PassNueva.Trim() != PassConfirmar.Trim())
                    throw new Exception("La confirmacion de Pass nueva no es correcta - No se modifica");

                //mandar a modificar a la bd
                ((Usuario)Session["Logueo"]).PassLog = PassNueva.Trim();
                new UsuariosDB().ModificarPass((Usuario)Session["Logueo"]);

                //salio todo OK!
                return RedirectToAction("Principal", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }


        //***************************Listado Usuarios 
        //no necesito poner HTTPGet, pq hay una sola operacion asociada a la vista -es comportameinto por defecto
        public ActionResult FormListousuarios()
        {
            try
            {
                //traigo lista de usuarios de la bd
                List<Usuario> _lista = new UsuariosDB().ListoUsuarios();

                //mando mostrar la vista - la vista espera una coleccion de usuarios para despelgar
                return View(_lista);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
         return View(new List<Usuario>()); //la vista espera una coleccion - no tengo datos pero se parte sin coleccion
            }
        }


        //************************* Alta Usuario 
        [HttpGet]
        public ActionResult FormCrearUsuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormCrearUsuario(Usuario U)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    new UsuariosDB().AgregarUsuario(U);
                    return RedirectToAction("FormListousuarios", "Usuarios");
                }
                return View(); //sino entro al if-- no tengo objeto valido - vuelvo a la vista
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }


        //*************************** baja Usuario 
        [HttpGet]
        public ActionResult FormUsuarioBorrar(string usulog)
        {
            try
            {
                Usuario _unU = new UsuariosDB().BuscarUsuario(usulog);
                if (_unU == null)
                    throw new Exception("No existe el Usuario");

                return View(_unU);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Usuario());
            }
        }

        [HttpPost]
        public ActionResult FormUsuarioBorrar(Usuario U)
        {
            try
            {
                new UsuariosDB().EliminarUsuario(U);
                return RedirectToAction("FormListousuarios", "Usuarios");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Usuario());
            }

        }
    }
}