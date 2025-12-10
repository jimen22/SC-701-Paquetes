using Auto.Abstracc.DA;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.DA.Repositorios
{
    public class RepositorioDapper : IRepositorioDapper
    {
        private readonly IConfiguration _configuration;
        private SqlConnection _conection; 
        public RepositorioDapper(IConfiguration configuracion, SqlConnection conection)
        {
            _configuration = configuracion;
            _conection = new SqlConnection(_configuration.GetConnectionString("BDSeguridad"));
        }

        public SqlConnection ObtenerRepositorioDapper()
        {
            return _conection;
        }
    }
}
