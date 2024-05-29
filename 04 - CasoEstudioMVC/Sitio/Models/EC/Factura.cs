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
    public class Factura
    {
        private int _Nro;
        private DateTime _Fecha;
        private Usuario _UnUsu;
        private List<LineasFacturas> _listaL;

        [DisplayName("Numero")]
        public int Nro
        {
            get
            {
                return _Nro;
            }

            set
            {
                _Nro = value;
            }
        }
   
        [DataType(DataType.Date)]
        public DateTime Fecha
        {
            get
            {
                return _Fecha;
            }

            set
            {
                _Fecha = value;
            }
        }

        [DisplayName("Usuario")]
        public Usuario Usu
        {
            get
            {
                return _UnUsu;
            }

            set
            {
                _UnUsu = value;
            }

        }

        public List<LineasFacturas> ListaL
        {
            get { return _listaL; }
            set
            {
                _listaL = value;
            }
        }




        // contructor por defecto
        public Factura()
        { }


        //constructor completo
        public Factura(int pnro, DateTime pfecha, Usuario pUsu, List<LineasFacturas> lista)
        {
            Nro = pnro;
            Fecha = pfecha;
            ListaL = lista;
            Usu = pUsu;
        }


        //validar Contenido de Objeto
        public void Validar()
        {
            if (this.Nro <= 0)
                throw new Exception("El numero de factura debe ser positivo");
            if (this.Usu == null)
                throw new Exception("Debe saberse usuario que Genero Factura");
            if (this.ListaL == null)
                throw new Exception("Una Factura Sin Lineas no se Admite");
            if (this.ListaL.Count == 0)
                throw new Exception("Debe seleccionar al menos un articulo obligatoriamente");
        }


    }

   
}
