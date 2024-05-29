using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Sitio.Models;

namespace Sitio.Controllers
{
    public class Facturas2Controller : Controller
    {
        //no tiene vista asociada, es para inicializar la factura
        public ActionResult ComienzoFactura()
        {
            //armo las session necesarias
            Session["ListaLineas2"] = new List<LineasFacturas>();

            //redirecciono a la primera vista: seleccionar articulos
            return RedirectToAction("ListaArticulosElegir", "Facturas2");
        }


        //listar + articulos
        public ActionResult ListaArticulosElegir()
        {
            try
            {
                //manejo errores que me puede dar el agregar una linea a la factura
                if (Session["ErrorF2"] != null)
                {
                    ViewBag.Mensaje = Session["ErrorF2"].ToString();
                    Session["ErrorF2"] = ""; //borramos para que no siga apareciendo el error
                }
                //obtengo lista de articulos
                List<Articulo> _lista = new ArticulosBD().ListarArticulo();
                if (_lista.Count >= 1)
                    return View(_lista);
                else
                    throw new Exception("No hay Articulos para mostar");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Articulo>());
            }

        }


        //no tiene vista asociada, es para agregar el articulo a una linea y guardarlo en memoria
        public ActionResult AgregarLinea(int Codigo)
        {
            try
            {
                List<LineasFacturas> listaL = (List<LineasFacturas>)Session["ListaLineas2"];

                //obtengo el articulo
                Articulo _A = new ArticulosBD().BuscarArticulo(Codigo);

                //en contre articulo - armo linea y agrego
                if (_A != null)
                {
                    //valido si ya no se agrego el articulo
                    LineasFacturas unaL = listaL.Where(L => L.Art.Codigo == _A.Codigo).FirstOrDefault();

                    if (unaL != null) //ya estaba - acumulo un elemento)
                        unaL.Cant += 1;
                    else //no estaba, creo linea
                    {
                        LineasFacturas nueva = new LineasFacturas(1, _A);
                        nueva.Validar();
                        listaL.Add(nueva);
                    }
                }
                else
                    Session["ErrorF2"] = "Articulo No existe - No se Agrego";

                //permito vovler a seleccionar otro articulo
                return RedirectToAction("ListaArticulosElegir", "Facturas2");

            }
            catch (Exception ex)
            {
                Session["ErrorF2"] = ex.Message;
                return RedirectToAction("ListaArticulosElegir", "Facturas2");
            }

        }


        //me pide los datos de la factura (crear + factura)
        [HttpGet]
        public ActionResult DatosFacturaIngresar()
        {
            return View();
        }


        //proceso final de la factura
        [HttpPost]
        public ActionResult DatosFacturaIngresar(Factura F)
        {
            try
            {
                //determino usuario que genera la factura
                F.Usu = (Usuario)Session["Logueo"];

                //asigno la lista de articulos que se fue armando
                F.ListaL = (List<LineasFacturas>)Session["ListaLineas2"];

                //Valido el modelo
                F.Validar();

                //mando a dar de alta la factura a la bd
                new FacturasDB().AgregarFactura(F);

                return RedirectToAction("FormAltaExito", "Facturas");
            }
            catch (Exception ex)
            {
                Session["ErrorFactura"] = ex.Message;
                return RedirectToAction("FormAltaError", "Facturas");
            }
        }
    }
}