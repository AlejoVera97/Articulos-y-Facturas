using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


//-------agregar usuings-----//
using System.ComponentModel.DataAnnotations;
//---------------------------//


namespace Sitio.Models
{
    public class Usuario
    {

        //atributos
        string _UsuLog;
        string _PassLog;

        //propiedades

        [Required(ErrorMessage = "Ingrese el usuario")]
        public string UsuLog
        {
            get { return _UsuLog; }
            set { _UsuLog = value; }
        }

        [Required(ErrorMessage = "Ingrese la contraseña")]
        public string PassLog
        {
            get { return _PassLog; }
            set { _PassLog = value; }
        }

        //Contructor completo 
        public Usuario(string pNombre, string pPass)
        {
            UsuLog = pNombre;
            PassLog = pPass;
        }

        //Constructor por defecto
        public Usuario() { }


    }

   
}