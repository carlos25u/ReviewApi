using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewApi.DAL;
using ReviewApi.Modelos;
using ReviewApi.Modelos.Customs;
using ReviewApi.Service;

namespace ReviewApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly Contexto _contexto;
        private readonly IAutorizacionService _autorizacionService;
        public UsuariosController(Contexto contexto, IAutorizacionService autorizacionService)
        {
            _contexto = contexto;
            _autorizacionService = autorizacionService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Aunteticar([FromBody] AutorizacionRequest autorizacion)
        {
            var resultados = await _autorizacionService.DevolverToker(autorizacion);

            if(resultados == null)
            {
                return Unauthorized();  
            }

            return Ok(resultados);  
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuarios>>> GetUsuarios()
        {
            return await _contexto.Usuarios.ToListAsync();
        }

        [HttpGet("/{id}")]
        public async Task<ActionResult<Usuarios>> Getusuario(int id)
        {
            var usuarios = await _contexto.Usuarios.FindAsync(id);

            if (usuarios == null)
                return NotFound("Usuario no encontrado...........");

            return usuarios;
        }

        [HttpPut]
        public async Task<IActionResult> PutUsuarios(int id, Usuarios usuarios)
        {
            if (id != usuarios.UsuariosId)
            {
                BadRequest("Id no coincide");
            }

            _contexto.Entry(usuarios).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuariosExiste(id))
                {
                    return BadRequest("El Usuarios no existe.........");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Usuarios>> PostUsuarios(Usuarios usuarios)
        {
            _contexto.Usuarios.Add(usuarios);
            await _contexto.SaveChangesAsync();
            return Ok("Se Agrego con exito!");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUsuarios(int id)
        {
            var usuarios = await _contexto.Usuarios.FindAsync(id);
            if (usuarios == null)
            {
                return NotFound("El usuario no fue encontrado.....");
            }

            _contexto.Usuarios.Remove(usuarios);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuariosExiste(int id)
        {
            return _contexto.Usuarios.Any(e => e.UsuariosId == id);
        }
    }
}
