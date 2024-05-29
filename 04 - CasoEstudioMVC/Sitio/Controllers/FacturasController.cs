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
    public class FacturasController : Controller
    {
        // GET: Facturas
        public ActionResult Index()
        {
            return View();
        }

     
        //***************************************** ALta Factura 
        [HttpGet]
        public ActionResult FormFacturaAlta()
        {
            try
            {
                //muestro la vista
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult FormFacturaAlta(Factura F)
        {
            try
            {
                //determino usuario que genera la factura
                F.Usu = (Usuario)Session["Logueo"];

                //creo la lista de lineas - para ir agregando las lineas 
                //(el obejto se crea vacio en la vista, no hay lista
                F.ListaL = new List<LineasFacturas>();

                //guardo en seesion para agregar lineas
                Session["Factura"] = F;

                return RedirectToAction("FormLineaFacturaAlta", "Facturas");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpGet]
        public ActionResult FormLineaFacturaAlta()
        {
            try
            {
                //carga lista de articulos para mostrar en una lista desplegable en la vista
                List<Articulo> _ListaA = new ArticulosBD().ListarArticulo();
                ViewBag.ListaArticulos = new SelectList(_ListaA, "Codigo", "Nombre");

                //muestro la vista
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ListaArticulos = new SelectList(null);
                ViewBag.Mensaje = ex.Message;
                return View();
            }
        }

        [HttpPost]
        public ActionResult FormLineaFacturaAlta(LineasFacturas LF)
        {
            try
            {
                //si se selecciono un articulo, lo busco
                if (LF.CodigoArticulo.Trim().Length > 0)
                {
                    int _codigo = Convert.ToInt32(LF.CodigoArticulo);
                    LF.Art = new ArticulosBD().BuscarArticulo(_codigo);
                }

                //valido Linea
                LF.Validar();

                //Agrego Linea a la Factura en la session
                ((Factura)Session["Factura"]).ListaL.Add(LF);

                //muestro la vista de nuevo, peor que la muestre vacia, provoco el get 
                return RedirectToAction("FormLineaFacturaAlta", "Facturas");

            }
            catch (Exception ex)
            {
                List<Articulo> _ListaA = new ArticulosBD().ListarArticulo();
                ViewBag.ListaArticulos = new SelectList(_ListaA, "codigo", "Nombre");
                ViewBag.Mensaje = ex.Message;
                return View(); 
            }
        }


        //************* Operacion sin vista asoc
        [HttpGet]
        public ActionResult GuardarFactura()
        {
            try
            {
                //obtengo la factura armada 
                Factura F = ((Factura)Session["Factura"]);

                //Valido el modelo
                F.Validar();

                //mando a dar de alta la factura a la bd
                new FacturasDB().AgregarFactura(F);

                //si todo ok:
                return RedirectToAction("FormAltaExito", "Facturas");

            }
            catch (Exception ex)
            {
                Session["ErrorFactura"] = ex.Message;
                return RedirectToAction("FormAltaError", "Facturas");
            }

        }


        //********************* Vistas solo mensaje sin proceso
        public ActionResult FormAltaExito()
        {
            return View();
        }

        public ActionResult FormAltaError()
        {
            ViewBag.Mensaje = Session["ErrorFactura"].ToString();
            return View();
        }


        //****************** Listado Parametrizado de Facturas 
        public ActionResult FormFacturasListar(string FechaFiltro, string UsuarioFiltro, string ArticuloFiltro)
        {
            try
            {
                //obtengo lista para la vista (facturas)
                List<Factura> _lista = null;
                //obtengo lista de facturas
                if (Session["Lista"] == null)
                {
                    _lista = new FacturasDB().ListarFacturas();
                    Session["Lista"] = _lista;
                }
                else
                    _lista = (List<Factura>)Session["Lista"];

                //no hay facturas
                if (_lista.Count == 0)
                    throw new Exception("No hay Facturas para mostar");
                //obtengo lista para el drop de articulos filtro 
                //*************
                List<Articulo> _ListaA = new ArticulosBD().ListarArticulo();
                _ListaA.Insert(0, new Articulo(0, "Seleccione", 0)); //parche para que no quede articulo seleccionado
                ViewBag.ListaA = new SelectList(_ListaA, "Codigo", "Nombre");


                //filtros o no
                if (!String.IsNullOrEmpty(FechaFiltro))
                {
                    _lista = (from unaF in _lista
                             where unaF.Fecha.Date == Convert.ToDateTime(FechaFiltro).Date
                             select unaF).ToList();
                }

                if (!String.IsNullOrEmpty(UsuarioFiltro))
                {
                    _lista = (from unaF in _lista
                              where unaF.Usu.UsuLog.Trim() == UsuarioFiltro.Trim()
                              select unaF).ToList();
                }

                if (!String.IsNullOrEmpty(ArticuloFiltro) && ArticuloFiltro.Trim() != "0")
                {
                    _lista = (from unaF in _lista
                              from unaL in unaF.ListaL
                              where unaL.Art.Codigo == Convert.ToInt32(ArticuloFiltro)
                              select unaF).ToList();
                }
                //retorno resultado
                return View(_lista);
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<Factura>());
            }
        }

        public ActionResult FormFacturaConsultar(int Nro)
        {
            try
            {
                //obtengo el articulos
                List<LineasFacturas> _ListaLineas = new LineasFacturasDB().ListarLineas(Nro);
                if (_ListaLineas != null)
                    return View(_ListaLineas);
                else
                    throw new Exception("Error - No se encontraron las lineas de la Factura");
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = ex.Message;
                return View(new List<LineasFacturas>());
            }
        }
    }
}