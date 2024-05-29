using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


//agregar----------------------------
using Sitio.Models;
//------------------------------


namespace Sitio.Controllers
{
    public class ArticulosController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        //******************************** Listar articulos 
        public ActionResult FormArticulosListar(string DatoFiltro) //recibo el contenido de la cajita
          {
              try
              {
                  //obtengo lista de articulos
                  List<Articulo> _lista = new ArticulosBD().ListarArticulo();

                  //si hay datos... defino despliegue
                  if (_lista.Count >= 1)
                  {
                      //primero reviso si hay que filtrar...
                      if (String.IsNullOrEmpty(DatoFiltro))
                          return View(_lista); //no hay filtro - muestro compelto
                      else
                      {
                          //hay dato para filtro
                          _lista = (from unA in _lista
                                    where unA.Nombre.ToUpper().StartsWith(DatoFiltro.ToUpper())
                                    select unA).ToList();
                          return View(_lista);
                      }
                  }
                  else //no hay datos - no hago nada
                      throw new Exception("No hay Articulos para mostar");
              }
              catch (Exception ex)
              {
                  ViewBag.Mensaje = ex.Message;
                  return View(new List<Articulo>());
              }
          }
         


        //******************************* Modificar Articulo
        [HttpGet] 
        public ActionResult FormArticuloModificar(int Codigo)
        {
            try
            {
                //obtengo el articulos
                Articulo _A = new ArticulosBD().BuscarArticulo(Codigo);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el Articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());
            }
        }

        [HttpPost]
        public ActionResult FormArticuloModificar(Articulo A)
        {
            try
            {
                //valido objeto correcto
                A.Validar();

                //intento modificar
                new ArticulosBD().ModificarArticulo(A);
                ViewBag.Mensaje = "Modificacion Exitosa";
                return View(new Articulo(0,"",0));
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());
            }
        }



        //********************************** Eliminar Articulos
        [HttpGet]
        public ActionResult FormArticuloEliminar(int Codigo)
        {
            try
            {
                //obtengo el articulos
                Articulo _A = new ArticulosBD().BuscarArticulo(Codigo);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el Articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());
            }
        }

        [HttpPost]
        public ActionResult FormArticuloEliminar(Articulo A)
        {
            try
            {
                //intento eliminar
                new ArticulosBD().EliminarArticulo(A);
                return RedirectToAction("FormArticulosListar", "Articulos");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());
            }
        }




        //*********************************** Nuevo Articulo 
        [HttpGet]
        public ActionResult FormArticuloNuevo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FormArticuloNuevo(Articulo A)
        {
            try
            {
                //valido objeto correcto
                A.Validar();

                //intento agregar articulo en la bd
                new ArticulosBD().AgregarArticulo(A);
                // no hubo error, alta correcto
               return RedirectToAction("FormArticulosListar", "Articulos");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        //************************************ consulta individual Articulo 
        [HttpGet] //no es obligatorio - se infiere por ser la unica op asoc a la vista
                  //no da problemas si igual lo pongo por prolijidad
        public ActionResult FormArticuloConsultar(int Codigo)
        {
            try
            {
                //obtengo el articulos
                Articulo _A = new ArticulosBD().BuscarArticulo(Codigo);
                if (_A != null)
                    return View(_A);
                else
                    throw new Exception("No existe el Articulo");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new Articulo());
            }
        }

    }
}