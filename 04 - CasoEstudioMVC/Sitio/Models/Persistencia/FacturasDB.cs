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
  
        public class FacturasDB
        {
            public void AgregarFactura(Factura F)
            {
                SqlConnection oConexion = new SqlConnection(Conexion.Cnn);
                SqlCommand oComando = new SqlCommand("AltaFactura", oConexion);
                oComando.CommandType = CommandType.StoredProcedure;

                SqlParameter _numF = new SqlParameter("@numFact", F.Nro);
                SqlParameter _usu = new SqlParameter("@NomUsu ", F.Usu.UsuLog);

                SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                _Retorno.Direction = ParameterDirection.ReturnValue;

                oComando.Parameters.Add(_numF);
                oComando.Parameters.Add(_usu);
                oComando.Parameters.Add(_Retorno);

                int oAfectados = -1;
                SqlTransaction _transaccion = null;

                try
                {
                    oConexion.Open();
                    _transaccion = oConexion.BeginTransaction();
                    oComando.Transaction = _transaccion;

                    oComando.ExecuteNonQuery();
                    oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                    if (oAfectados == -1)
                        throw new Exception("EL Usuario no Existe");
                    if (oAfectados == -2)
                        throw new Exception("EL Numero de Factura ya existe");

                    foreach (LineasFacturas L in F.ListaL)
                        new LineasFacturasDB().AgregarLinea(F.Nro, L, _transaccion);

                    _transaccion.Commit();
                }
                catch (Exception ex)
                {
                    _transaccion.Rollback();
                    throw ex;
                }
                finally
                {
                    oConexion.Close();
                }
            }

            public List<Factura> ListarFacturas()
            {
                List<Factura> _Lista = new List<Factura>();

                SqlConnection _Conexion = new SqlConnection(Conexion.Cnn);
                SqlCommand _Comando = new SqlCommand("ListoFacturas", _Conexion);
                _Comando.CommandType = CommandType.StoredProcedure;

                SqlDataReader _Reader;
                try
                {
                    _Conexion.Open();
                    _Reader = _Comando.ExecuteReader();

                    while (_Reader.Read())
                    {
                        int _nro = (int)_Reader["NumFact"];
                        DateTime _fecha = Convert.ToDateTime(_Reader["FechaFact"]);
                        string _usu = (string)_Reader["UsuLog"];
                        Usuario u = new UsuariosDB().BuscarUsuario(_usu);
                        Factura f = new Factura(_nro, _fecha, u, new LineasFacturasDB().ListarLineas(_nro));
                        _Lista.Add(f);
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