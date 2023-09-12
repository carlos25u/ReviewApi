using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using ReviewApi.DAL;
using ReviewApi.Modelos.Customs;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ReviewApi.Service
{
    public class AutorizacionService : IAutorizacionService
    {
        private readonly Contexto contexto;
        private readonly IConfiguration configuration;

        public AutorizacionService(Contexto contexto, IConfiguration configuration)
        {
            this.contexto = contexto;
            this.configuration = configuration;
        }

        private String GenerarToken(String idUsuario)
        {
            var Key = configuration.GetValue<String>("JetSetting:Key");
            var keyBytes = Encoding.ASCII.GetBytes(Key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUsuario));

            var credencialesToken = new SigningCredentials(
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
                );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = credencialesToken
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

            String tokenCreado = tokenHandler.WriteToken(tokenConfig);

            return tokenCreado;
        }

        public async Task<AutorizacionResponse> DevolverToker(AutorizacionRequest autorizacion)
        {
            var usuarioEncontrado = contexto.Usuarios.FirstOrDefault(x =>
                    x.Username == autorizacion.Username &&
                    x.Password == autorizacion.Password
                );

            if (usuarioEncontrado == null)
            {
                return await Task.FromResult<AutorizacionResponse>(null);
            }

            String tokenCreado = GenerarToken(usuarioEncontrado.UsuariosId.ToString());

            return new AutorizacionResponse() { Token =  tokenCreado, Resultado = true, MSG = "OK" };
        }
    }
}
