using Auto.Abstracc.DA;
using Auto.Abstracc.Modelos;
using Dapper;
using Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Auto.DA
{
    public class SeguridadDA : ISeguridadDA
    {
        IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public SeguridadDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = repositorioDapper.ObtenerRepositorioDapper();
        }

        public async Task<IEnumerable<Perfil>> ObtenerPerfilesxUsuario(Usuario usuario)
        {
            string sql = @"[ObtenerPerfilesxUsuario]";
            var consulta = await _sqlConnection.QueryAsync<Abstracc.Entidades.Perfil>(sql,
                new { CorreoElectronico = usuario.CorreoElectronico, NombreUsuario = usuario.NombreUsuario });

            return Convertidor.ConvertirLista<Abstracc.Entidades.Perfil, Abstracc.Modelos.Perfil>(consulta);
        }

        public async Task<Usuario> ObtenerUsuario(Usuario usuario)
        {
            string sql = @"[ObtenerUsuario]";
            var consulta = await _sqlConnection.QueryAsync<Abstracc.Entidades.Usuario>(sql,
                new { CorreoElectronico = usuario.CorreoElectronico, NombreUsuario = usuario.NombreUsuario });

            return Convertidor.Convertir<Abstracc.Entidades.Usuario, Abstracc.Modelos.Usuario>(consulta.FirstOrDefault());
        }
    }
}
