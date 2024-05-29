using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//-------agregar usuings-----//
using System.Data.SqlClient;
using System.Data;
//---------------------------//

namespace Sitio.Models
{
    public class UsuariosDB
    {
        public void Logueo(Usuario U)
        {
            SqlConnection _cnn = new SqlConnection(Conexion.Cnn);

            SqlCommand _comando = new SqlCommand("Logueo", _cnn);
            _comando.CommandType = System.Data.CommandType.StoredProcedure;
            _comando.Parameters.AddWithValue("@UsuL", U.UsuLog);
            _comando.Parameters.AddWithValue("@PassL", U.PassLog);

            try
            {
                _cnn.Open();
                SqlDataReader _lector = _comando.ExecuteReader();
                if (!_lector.HasRows)
                {
                    throw new Exception("Error - No es correcto el usuario y/o la contraseña");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _cnn.Close();
            }
        }

        public Usuario BuscarUsuario(string pNomUsu)
        {
            string _nombre;
            string _pass;
            Usuario u = null;

            SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
            SqlCommand oComando = new SqlCommand("Exec BuscoUsuario '" + pNomUsu + "'", oConexion);

            SqlDataReader oReader;

            try
            {
                oConexion.Open();
                oReader = oComando.ExecuteReader();

                if (oReader.Read())
                {
                    _nombre = (string)oReader["Usulog"];
                    _pass = (string)oReader["PassLog"];
                    u = new Usuario(_nombre, _pass);
                }

                oReader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                oConexion.Close();
            }
            return u;
        }

        public void ModificarPass(Usuario U)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
            SqlCommand oComando = new SqlCommand("CambioPass", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _usu = new SqlParameter("@usu", U.UsuLog);
            SqlParameter _pass = new SqlParameter("@pass", U.PassLog);

            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;

            oComando.Parameters.Add(_usu);
            oComando.Parameters.Add(_pass);
            oComando.Parameters.Add(_Retorno);

            int oAfectados = -1;

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();
                oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == -1)
                    throw new Exception("No existe el Usuario  - No se modifico");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConexion.Close();
            }
        }

        public List<Usuario> ListoUsuarios()
        {
            string _pass;
            string _usu;

            List<Usuario> _Lista = new List<Usuario>();

            SqlConnection _Conexion = new SqlConnection(Conexion.Cnn);
            SqlCommand _Comando = new SqlCommand("Exec ListoUsuarios", _Conexion);

            SqlDataReader _Reader;
            try
            {
                _Conexion.Open();
                _Reader = _Comando.ExecuteReader();

                while (_Reader.Read())
                {
                    _usu = (string)_Reader["UsuLog"];
                    _pass = (string)_Reader["PassLog"];
                    Usuario a = new Usuario(_usu, _pass);
                    _Lista.Add(a);
                }

                _Reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _Conexion.Close();
            }

            return _Lista;
        }

        public void AgregarUsuario(Usuario U)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
            SqlCommand oComando = new SqlCommand("AltaUsuario", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _nomu = new SqlParameter("@usu", U.UsuLog);
            SqlParameter _passu = new SqlParameter("@pass", U.PassLog);

            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;

            oComando.Parameters.Add(_nomu);
            oComando.Parameters.Add(_passu);
            oComando.Parameters.Add(_Retorno);

            int oAfectados = -1;

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();
                oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == -1)
                    throw new Exception("EL Usuario ya Existe");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConexion.Close();
            }
        }

        public void EliminarUsuario(Usuario U)
        {
            SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
            SqlCommand oComando = new SqlCommand("EliminarUsuario", oConexion);
            oComando.CommandType = CommandType.StoredProcedure;

            SqlParameter _nomu = new SqlParameter("@usu", U.UsuLog);

            SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
            _Retorno.Direction = ParameterDirection.ReturnValue;

            oComando.Parameters.Add(_nomu);
            oComando.Parameters.Add(_Retorno);

            int oAfectados = -1;

            try
            {
                oConexion.Open();
                oComando.ExecuteNonQuery();
                oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                if (oAfectados == -1)
                    throw new Exception("El Usuario no existe - No se elimina");
                if (oAfectados == -2)
                    throw new Exception("El Usuario tiene facturas asignadas - No se elimina");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                oConexion.Close();
            }
        }

    }
}