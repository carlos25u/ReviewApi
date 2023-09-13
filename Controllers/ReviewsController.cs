using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReviewApi.DAL;
using ReviewApi.Modelos;

namespace ReviewApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly Contexto _contexto;
        public ReviewsController(Contexto contexto)
        {
            _contexto = contexto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reviews>>> GetReviews()
        {
            var consultaFull = from r in _contexto.Reviews
                               join u in _contexto.Usuarios on r.UsuariosId equals u.UsuariosId into temp
                               from sub in temp.DefaultIfEmpty()
                               select new
                               {
                                   ReviewId = r.ReviewId,
                                   Restaurante = r.Restaurante,
                                   fecha = r.Fecha,
                                   comentario = r.Comentario,
                                   UsuarioId = sub != null ? sub.UsuariosId : (int?)null,
                                   username = sub != null ? sub.Username : null
                               };
            var result = await consultaFull.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reviews>> GetReview(int id)
        {
            var consultaFull = from r in _contexto.Reviews
                               join u in _contexto.Usuarios on r.UsuariosId equals u.UsuariosId into temp
                               from sub in temp.DefaultIfEmpty()
                               where r.ReviewId == id
                               select new
                               {
                                   ReviewId = r.ReviewId,
                                   Restaurante = r.Restaurante,
                                   fecha = r.Fecha,
                                   comentario = r.Comentario,
                                   UsuarioId = sub != null ? sub.UsuariosId : (int?)null,
                                   username = sub != null ? sub.Username : null
                               };
            var reviewEncontrada = await consultaFull.FirstOrDefaultAsync();

            if (reviewEncontrada == null)
            {
                return NotFound("Review no fue encontrada");
            }

            return Ok(reviewEncontrada);
        }

        [HttpPut]
        public async Task<IActionResult> PutReviews(int id, Reviews reviews)
        {
            if (id != reviews.ReviewId)
            {
                BadRequest("Id no coincide");
            }

            _contexto.Entry(reviews).State = EntityState.Modified;

            try
            {
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExiste(id))
                {
                    return BadRequest("La review no existe.........");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Reviews>> PostReview(Reviews review)
        {
            _contexto.Reviews.Add(review);
            await _contexto.SaveChangesAsync();
            return Ok("Se Agrego con exito!");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _contexto.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound("La review no fue encontrado.....");
            }

            _contexto.Reviews.Remove(review);
            await _contexto.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExiste(int id)
        {
            return _contexto.Reviews.Any(e => e.ReviewId == id);
        }
    }
}
