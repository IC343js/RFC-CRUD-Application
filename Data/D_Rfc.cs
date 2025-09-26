using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Entities;

namespace Data
{
    public class D_Rfc
    {
        private string CadenaConexion = ConfigurationManager.ConnectionStrings["sql"].ConnectionString;

        public List<Ent_Rfc> ObtenerTodos()
        {
            List<Ent_Rfc> lista = new List<Ent_Rfc>();
            SqlConnection conn = new SqlConnection(CadenaConexion);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("obtener_todos_rfc", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Ent_Rfc Rfc = new Ent_Rfc();
                    Rfc.idRfc = Convert.ToInt32(reader["id_RFC"]);
                    Rfc.Nombre = Convert.ToString(reader["nombre"]);
                    Rfc.ApellidoPaterno = Convert.ToString(reader["apellido_paterno"]);
                    Rfc.ApellidoMaterno = Convert.ToString(reader["apellido_materno"]);
                    Rfc.FechaNacimiento = Convert.ToDateTime(reader["fechaNacimiento"]);
                    Rfc.Rfc = Convert.ToString(reader["rfc"]);
                    lista.Add(Rfc);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return lista;
        }
        public void AgregarRFC(Ent_Rfc rfc)
        {
            SqlConnection conn = new SqlConnection(CadenaConexion);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("agregar_rfc", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", rfc.Nombre);
                cmd.Parameters.AddWithValue("@apellido_paterno", rfc.ApellidoPaterno);
                cmd.Parameters.AddWithValue("@apellido_materno", rfc.ApellidoMaterno);
                cmd.Parameters.AddWithValue("@fechaNacimiento", rfc.FechaNacimiento);
                cmd.Parameters.AddWithValue("@rfc", rfc.Rfc);
                //Ejecutamos Query
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public Ent_Rfc ObtenerRfcPorId(int idRfc)
        {
            Ent_Rfc rfc = new Ent_Rfc();
            SqlConnection conn = new SqlConnection(CadenaConexion);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("obtener_rfc", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idRfc", idRfc);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rfc.idRfc = Convert.ToInt32(reader["id_RFC"]);
                    rfc.Nombre = Convert.ToString(reader["nombre"]);
                    rfc.ApellidoPaterno = Convert.ToString(reader["apellido_paterno"]);
                    rfc.ApellidoMaterno = Convert.ToString(reader["apellido_materno"]);
                    rfc.FechaNacimiento = Convert.ToDateTime(reader["fechaNacimiento"]);
                    rfc.Rfc = Convert.ToString(reader["rfc"]);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rfc;
        }
        public void EditarRFC(Ent_Rfc rfc, int idRfc)
        {
            SqlConnection conn = new SqlConnection(CadenaConexion);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("editar_rfc", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idRfc", idRfc);
                cmd.Parameters.AddWithValue("@nombre", rfc.Nombre);
                cmd.Parameters.AddWithValue("@apellido_paterno", rfc.ApellidoPaterno);
                cmd.Parameters.AddWithValue("@apellido_materno", rfc.ApellidoMaterno);
                cmd.Parameters.AddWithValue("@fechaNacimiento", rfc.FechaNacimiento);
                cmd.Parameters.AddWithValue("@rfc", rfc.Rfc);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public void EliminarRFC(int idRfc)
        {
            SqlConnection conn = new SqlConnection(CadenaConexion);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("eliminar_rfc", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@idRfc", idRfc);
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<Ent_Rfc> Buscar(string busqueda)
        {
            List<Ent_Rfc> lista = new List<Ent_Rfc>();
            SqlConnection conn = new SqlConnection(CadenaConexion);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("buscar_rfc", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@texto", busqueda);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Ent_Rfc rfc = new Ent_Rfc();
                    rfc.idRfc = Convert.ToInt32(reader["id_RFC"]);
                    rfc.Nombre = Convert.ToString(reader["nombre"]);
                    rfc.ApellidoPaterno = Convert.ToString(reader["apellido_paterno"]);
                    rfc.ApellidoMaterno = Convert.ToString(reader["apellido_materno"]);
                    rfc.FechaNacimiento = Convert.ToDateTime(reader["fechaNacimiento"]);
                    rfc.Rfc = Convert.ToString(reader["rfc"]);
                    lista.Add(rfc);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return lista;
        }
        public Ent_Rfc ObtenerRfcPorNombre(string nombre)
        {
            Ent_Rfc rfc = null;
            SqlConnection conn = new SqlConnection(CadenaConexion);
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("buscar_rfc_por_nombre", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", nombre);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rfc = new Ent_Rfc();
                    rfc.idRfc = Convert.ToInt32(reader["id_RFC"]);
                    rfc.Nombre = Convert.ToString(reader["nombre"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return rfc;
        }
    }
}
