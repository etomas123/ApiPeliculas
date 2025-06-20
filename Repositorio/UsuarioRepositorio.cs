using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ApiPeliculas.Repositorio
{
    public class UsuarioRepositorio:IUsuarioRepositorio
    {
        public readonly ApplicationDbContext _bd;
        public string claveSecreta;



        public UsuarioRepositorio(ApplicationDbContext bd, IConfiguration config)
        {
            _bd = bd;
            claveSecreta = config.GetValue<string>("ApiSettings:Secreta");
        }

        public Usuario GetUsuario(int usuarioId)
        {
            return _bd.Usuario.FirstOrDefault(C => C.Id == usuarioId);
        }

        public ICollection<Usuario> GetUsuarios()
        {
           return _bd.Usuario.OrderBy(C => C.NombreUsuario).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
            var usuarioBd = _bd.Usuario.FirstOrDefault(u => u.NombreUsuario == usuario);
            if (usuarioBd == null) { 
                return true;
            }
            return false;
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);
            var usuario = _bd.Usuario.FirstOrDefault( u=>u.NombreUsuario.ToLower() == usuarioLoginDto.NombreUsuario.ToLower()
            && u.Password == passwordEncriptado);
            if (usuario == null) {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null,
                };
            }
            // Procesamor el usuario 
            var manejandoToken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(claveSecreta);
            var tokenDescriptor = new SecurityTokenDescriptor { 
                
                Subject = new ClaimsIdentity(new Claim[] { 
                    new Claim(ClaimTypes.Name,usuario.NombreUsuario.ToString()),
                    new Claim(ClaimTypes.Role,usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
               // SigningCredentials = new
            };
            
        }

        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {

            
            var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);
            Usuario usuario  = new Usuario() { 
                 NombreUsuario = usuarioRegistroDto.NombreUsuario,
                 Password = passwordEncriptado,
                 Nombre = usuarioRegistroDto.Nombre,
                 Role = usuarioRegistroDto.Role
            };
            _bd.Usuario.Add(usuario);
            _bd.SaveChangesAsync();
            usuario.Password = passwordEncriptado;
            return usuario;
        }


        public string obtenermd5(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convertir el array de bytes a una cadena hexadecimal
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }



    }
}
