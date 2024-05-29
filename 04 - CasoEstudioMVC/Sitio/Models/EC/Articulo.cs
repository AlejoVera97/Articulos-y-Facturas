using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace Sitio.Models
{
    public class Articulo
    {
        //atributos
        private int _codigo;
        private string _nombre;
        private int _precio;

        //propiedades 
        public int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }
        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; }
        }
        public int Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }


        //Constructor completo 
        public Articulo(int pCodigo, string pNombre, int pPrecio)
        {
            Codigo = pCodigo;
            Nombre = pNombre;
            Precio = pPrecio;
        }

        //Constructor por defecto
        public Articulo() { }

        //validar Contenido de Objeto
        public void Validar()
        {
            if (this.Codigo <= 0)
                throw new Exception("El codigo debe ser positivo");
            if (this.Nombre.Trim().Length <= 2)
                throw new Exception("El Nombre debe Tener al menos dos letras");
            if (this.Precio <= 0)
                throw new Exception("El precio debe ser positivo");
          }

        //mostrar contenido del objeto
        public override string ToString()
        {
            return (this.Codigo + " -" + this.Nombre.Trim() + " - " + this.Precio.ToString());
        }

    }

}