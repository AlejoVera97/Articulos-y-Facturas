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
         public class ArticulosBD
        {
            public void AgregarArticulo(Articulo A)
            {
                SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
                SqlCommand oComando = new SqlCommand("AltaArticulo", oConexion);
                oComando.CommandType = CommandType.StoredProcedure;

                SqlParameter _codigo = new SqlParameter("@cod", A.Codigo);
                SqlParameter _nombre = new SqlParameter("@nom", A.Nombre);
                SqlParameter _precio = new SqlParameter("@pre", A.Precio);

                SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                _Retorno.Direction = ParameterDirection.ReturnValue;

                oComando.Parameters.Add(_codigo);
                oComando.Parameters.Add(_nombre);
                oComando.Parameters.Add(_precio);
                oComando.Parameters.Add(_Retorno);

                int oAfectados = -1;

                try
                {
                    oConexion.Open();
                    oComando.ExecuteNonQuery();
                    oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                    if (oAfectados == -1)
                        throw new Exception("EL Codigo de Articulo ya Existe");
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

            public void EliminarArticulo(Articulo A)
            {
                SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
                SqlCommand oComando = new SqlCommand("BajaArticulo", oConexion);
                oComando.CommandType = CommandType.StoredProcedure;

                SqlParameter _codigo = new SqlParameter("@cod", A.Codigo);

                SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                _Retorno.Direction = ParameterDirection.ReturnValue;

                oComando.Parameters.Add(_codigo);
                oComando.Parameters.Add(_Retorno);

                int oAfectados = -1;

                try
                {
                    oConexion.Open();
                    oComando.ExecuteNonQuery();
                    oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                    if (oAfectados == -1)
                        throw new Exception("El Articulo no existe - No se elimina");
                    if (oAfectados == -2)
                        throw new Exception("El Articulo tiene facturas asignadas - No se elimina");
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

            public void ModificarArticulo(Articulo A)
            {
                SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
                SqlCommand oComando = new SqlCommand("ModArticulo", oConexion);
                oComando.CommandType = CommandType.StoredProcedure;

                SqlParameter _codigo = new SqlParameter("@cod", A.Codigo);
                SqlParameter _nombre = new SqlParameter("@nom", A.Nombre);
                SqlParameter _precio = new SqlParameter("@pre", A.Precio);

                SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                _Retorno.Direction = ParameterDirection.ReturnValue;

                oComando.Parameters.Add(_codigo);
                oComando.Parameters.Add(_nombre);
                oComando.Parameters.Add(_precio);
                oComando.Parameters.Add(_Retorno);

                int oAfectados = -1;

                try
                {
                    oConexion.Open();
                    oComando.ExecuteNonQuery();
                    oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                    if (oAfectados == -1)
                        throw new Exception("No existe el Articulo - No se modifico");
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

            public Articulo BuscarArticulo(int pCodigo)
            {
                int _codigo;
                string _nombre;
                int _precio;
                Articulo a = null;

                SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
                SqlCommand oComando = new SqlCommand("Exec BuscoArticulo " + pCodigo, oConexion);

                SqlDataReader oReader;

                try
                {
                    oConexion.Open();
                    oReader = oComando.ExecuteReader();

                    if (oReader.Read())
                    {
                        _codigo = (int)oReader["CodArt"];
                        _nombre = (string)oReader["NomArt"];
                        _precio = (int)oReader["PreArt"];
                        a = new Articulo(_codigo, _nombre, _precio);
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
                return a;
            }

            public List<Articulo> ListarArticulo()
            {
                int _codigo;
                string _nombre;
                int _precio;
                List<Articulo> _Lista = new List<Articulo>();

                SqlConnection _Conexion = new SqlConnection(Conexion.Cnn);
                SqlCommand _Comando = new SqlCommand("Exec ListoArticulo", _Conexion);

                SqlDataReader _Reader;
                try
                {
                    _Conexion.Open();
                    _Reader = _Comando.ExecuteReader();

                    while (_Reader.Read())
                    {
                        _codigo = (int)_Reader["CodArt"];
                        _nombre = (string)_Reader["NomArt"];
                        _precio = (int)_Reader["PreArt"];
                        Articulo a = new Articulo(_codigo, _nombre, _precio);
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
        }
   
}