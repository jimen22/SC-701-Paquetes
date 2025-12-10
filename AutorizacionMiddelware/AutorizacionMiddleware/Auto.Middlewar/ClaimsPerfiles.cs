using Auto.Abstracc.BW;
using Auto.Abstracc.Modelos;
using Auto.BW;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Security.Claims;


namespace Auto.Middlewar
{
    public class ClaimsPerfiles
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private IAutorizaciónBW _autorizacionBW;

        public ClaimsPerfiles(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext httpContext, IAutorizaciónBW autorizaciónBW, HttpContext context)
        {
            _autorizacionBW = autorizaciónBW;
            ClaimsIdentity appIdentity = await verificarAutorizacion(httpContext);
            httpContext.User.AddIdentity(appIdentity);
            await _next(httpContext);

        }

        private async Task<ClaimsIdentity> verificarAutorizacion(HttpContext httpContext)
        {
           var claims=new List<Claim>();
            if(httpContext.User != null && httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
            {
                await ObtenerUsuario(httpContext, claims);
                await ObtenerPerfiles(httpContext, claims);
            }
            var appIdentity = new ClaimsIdentity(claims);
            return appIdentity;
        }

        private async Task ObtenerPerfiles(HttpContext httpContext, List<Claim> claims)
        {
            var perfiles = await obtenerInformacionPerfiles(httpContext);
            if (perfiles != null && perfiles.Any()) {
                foreach (var perfil in perfiles) {
                claims.Add(new Claim(ClaimTypes.Role, perfil.Id.ToString()));
                }
            }
        }

        private async Task<IEnumerable<Perfil>> obtenerInformacionPerfiles(HttpContext httpContext)
        {
            return await _autorizacionBW.ObtenerPerfilesxUsuario(new Abstracc.Modelos.Usuario
            {
                NombreUsuario = httpContext.User.Claims.Where(c => c.Type == 
                "usuario").FirstOrDefault().Value });
        }

        private async Task ObtenerUsuario(HttpContext httpContext, List<Claim> claims)
        {
            var usuario = await ObtenerInformacionUsuario(httpContext);
            if (usuario is not null && !string.IsNullOrEmpty(usuario.Id.ToString()) && !string.IsNullOrEmpty(usuario.NombreUsuario.ToString()) &&
                !string.IsNullOrEmpty(usuario.CorreoElectronico.ToString()))
            {
                claims.Add(new Claim(ClaimTypes.Email, usuario.CorreoElectronico));
                claims.Add(new Claim(ClaimTypes.Name, usuario.NombreUsuario));
                claims.Add(new Claim("IdUsuario", usuario.Id.ToString()));
            }
        }

        private async Task<Usuario> ObtenerInformacionUsuario(HttpContext httpContext)
        {
            return await _autorizacionBW.ObtenerUsuario(new Abstracc.Modelos.Usuario { NombreUsuario=httpContext.User.Claims.Where(c=>c.Type == "usuario").
            FirstOrDefault().Value });
        }
    }
}
