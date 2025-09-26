using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Data;

namespace Business
{
    public class N_Rfc
    {
        public List<Ent_Rfc> ObtenerTodos()
        {
            D_Rfc dao = new D_Rfc();
            List<Ent_Rfc> lista = dao.ObtenerTodos();
            return lista;
        }

        public void AgregarRFC(Ent_Rfc rfc)
        {
            D_Rfc dao = new D_Rfc();
            dao.AgregarRFC(rfc);
        }
        public void EditarRFC(Ent_Rfc rfc, int idRfc)
        {
            D_Rfc dao = new D_Rfc();
            dao.EditarRFC(rfc, idRfc);
        }
        public void EliminarRFC(int idRfc)
        {
            D_Rfc dao = new D_Rfc();
            dao.EliminarRFC(idRfc);
        }
        public Ent_Rfc ObtenerRfcPorId(int idRfc)
        {
            D_Rfc dao = new D_Rfc();
            return dao.ObtenerRfcPorId(idRfc);
        }
        public static List<Ent_Rfc> Buscar(string busqueda)
        {
            D_Rfc dao = new D_Rfc();
            return dao.Buscar(busqueda);
        }
    }
}
