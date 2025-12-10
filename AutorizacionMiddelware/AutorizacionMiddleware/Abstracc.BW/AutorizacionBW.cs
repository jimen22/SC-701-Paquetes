using Auto.Abstracc.BW;
using Auto.Abstracc.DA;
using Auto.Abstracc.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auto.BW
{
    public class AutorizacionBW : IAutorizaciónBW
    {
        private readonly ISeguridadDA _seguridadDA;

        public AutorizacionBW(ISeguridadDA seguridadDA)
        {
            _seguridadDA = seguridadDA;
        }

        public async Task<IEnumerable<Perfil>> ObtenerPerfilesxUsuario(Usuario usuario)
        {
           return await _seguridadDA.ObtenerPerfilesxUsuario(usuario);
        }

        public async Task<Usuario> ObtenerUsuario(Usuario usuario)
        {
            return await _seguridadDA.ObtenerUsuario(usuario);
        }
    }
}
