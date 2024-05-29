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
        public class LineasFacturasDB
        {
            public void AgregarLinea(int nroFact, LineasFacturas L, SqlTransaction trn)
            {
                SqlCommand oComando = new SqlCommand("AltaLinea ", trn.Connection);
                oComando.CommandType = CommandType.StoredProcedure;

                SqlParameter _numF = new SqlParameter("@numFact", nroFact);
                SqlParameter _codArt = new SqlParameter("@codArt", L.Art.Codigo);
                SqlParameter _cant = new SqlParameter("@cantidad", L.Cant);

                SqlParameter _Retorno = new SqlParameter("@Retorno", SqlDbType.Int);
                _Retorno.Direction = ParameterDirection.ReturnValue;

                oComando.Parameters.Add(_numF);
                oComando.Parameters.Add(_codArt);
                oComando.Parameters.Add(_cant);
                oComando.Parameters.Add(_Retorno);

                int oAfectados = -1;

                try
                {
                    oComando.Transaction = trn;
                    oComando.ExecuteNonQuery();
                    oAfectados = (int)oComando.Parameters["@Retorno"].Value;
                    if (oAfectados == -1)
                        throw new Exception("EL Codigo de Articulo no Existe");
                    if (oAfectados == -2)
                        throw new Exception("EL Numero de Factura no existe");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            public List<LineasFacturas> ListarLineas(int nroFact)
            {
                List<LineasFacturas> _Lista = new List<LineasFacturas>();

                SqlConnection _Conexion = new SqlConnection(Conexion.Cnn);
                SqlCommand _Comando = new SqlCommand("ListoLineas", _Conexion);
                _Comando.CommandType = CommandType.StoredProcedure;

                _Comando.Parameters.AddWithValue("@numFact", nroFact);

                SqlDataReader _Reader;
                try
                {
                    _Conexion.Open();
                    _Reader = _Comando.ExecuteReader();

                    while (_Reader.Read())
                    {
                        int _cant = (int)_Reader["Cant"];
                        int _codArt = (int)_Reader["CodArt"];
                        Articulo a = new ArticulosBD().BuscarArticulo(_codArt);
                        LineasFacturas f = new LineasFacturas(_cant, a);
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