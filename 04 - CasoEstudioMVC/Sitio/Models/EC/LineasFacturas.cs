using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


//-------------------------------------------
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
//--------------------------------------------


namespace Sitio.Models
{
    public class LineasFacturas
    {
        private string _CodigoArticulo;
        private int _Cant;

        [DisplayName("Articulo")]
        public string CodigoArticulo
        {
            get
            {
                return _CodigoArticulo;
            }

            set
            {
                _CodigoArticulo = value;
            }
        }

        [DisplayName("Cantidad")]
        public int Cant
        {
            get
            {
                return _Cant;
            }

            set
            {
                _Cant = value;
            }
        }


        //------------- Objeto Real para factura-------
        private Articulo _Art;

        public Articulo Art
        {
            get
            {
                return _Art;
            }

            set
            {
                _Art = value;
            }
        }
        //-----------Fin Objeto Real para Factura


        // contructor por defecto
        public LineasFacturas()
        { }


        //constructor completo
        public LineasFacturas(int cant, Articulo art)
        {
            Cant = cant;
            Art = art;
        }

        public void Validar()
        {
            if (this.Art == null)
                throw new Exception("Debe seleccionar una articulo obligatoriamente");
            if (this.Cant <= 0)
                throw new Exception("La cantidad a vender debe ser positiva");
        }

    }

    
}