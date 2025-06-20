using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/peliculas")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio _pelRepo;
        private readonly IMapper _mapper;
        public PeliculasController(IPeliculaRepositorio pelRepo, IMapper mapper)
        {
            _pelRepo = pelRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetPelicula()
        {
            var listaPeliculas = _pelRepo.GetPeliculas();
            var listaPeliculasDto = new List<PeliculaDto>();
            System.Diagnostics.Debug.WriteLine("Salida");

            if ( !listaPeliculas.Any())
            {
                System.Diagnostics.Debug.WriteLine("No encuentra nada", listaPeliculas);
                return NotFound(new
                {
                    StatusCode = 404,
                    message = $"No existe ninguna pelicula registrada",
                    data = listaPeliculas,
                });
            }

            foreach (var lista in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(lista));
            }

          


            return Ok(new
            {
                StatusCode = 200,
                message = "Consulta realizada con éxito",
                data = listaPeliculasDto,
            }
            );
        }



        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult GetPelicula(int peliculaId)
        {

            var itemPelicula = _pelRepo.GetPelicula(peliculaId);
            if (itemPelicula == null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    message = $"La Pelicula con id {peliculaId} no existe",
                    data = itemPelicula,
                });
            }
            var itemPeliculaDto = _mapper.Map<PeliculaDto>(itemPelicula);

            return Ok(new
            {
                StatusCode = 200,
                message = "Pelicula obtenida con éxito",
                data = itemPeliculaDto,
            }
            );
        }


        [HttpPost]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]


        public IActionResult CrearPelicula([FromForm] CrearPeliculaDto crearPeliculaDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    message = "El modelo no es valido",
                    // data = ModelState,
                });

                // return BadRequest(ModelState);
            }

            if (crearPeliculaDto == null)
            {

                return BadRequest(new
                {
                    StatusCode = 400,
                    message = "El modelo no es valido",
                    // data = ModelState,
                });
                //  return BadRequest(ModelState);
            }

            if (_pelRepo.ExistePelicula(crearPeliculaDto.Nombre))
            {

                //ModelState.AddModelError("", "La categoría ya existe");
                //return StatusCode(404,ModelState);

                return Conflict(new
                {
                    StatusCode = 409,
                    message = $"La categoría ya existe",
                });
            }


            var pelicula = _mapper.Map<Pelicula>(crearPeliculaDto);
            if (!_pelRepo.CrearPelicula(pelicula))
            {


                // ModelState.AddModelError("", $"Algo salio mal guardando el registro{categoria.Nombre}");
                // return StatusCode(404, ModelState);

                ModelState.AddModelError("", $"Algo salio mal guardando el registro{pelicula.Nombre}");
                return Conflict(new
                {
                    StatusCode = 409,
                    ModelState
                });
            }

            return Ok(new
            {
                StatusCode = 200,
                message = "la categoria ha sido creado con éxito",
                data = pelicula,
            });

            //return CreatedAtRoute("GetCategoria", new { categoriaId =categoria.Id },categoria);
        }




        [HttpPut("{peliculaId:int}", Name = "ActualizarPatchPelicula")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]


        public IActionResult ActualizarPatchPelicula(int peliculaId, [FromForm] PeliculaDto peliculaDto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    message = "El modelo no es valido",

                });
            }

            if (peliculaDto == null || peliculaId != peliculaDto.Id)
            {

                return BadRequest(new
                {
                    StatusCode = 400,
                    message = "El modelo no es valido",

                });

            }




            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            if (!_pelRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{pelicula.Nombre}");
                return Conflict(new
                {
                    StatusCode = 409,
                    ModelState
                });
            }

            return Ok(new
            {
                StatusCode = 200,
                message = "la categoria ha sido creado con éxito",
                data = pelicula,
            });

        }

        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]


        public IActionResult BorrarPelicula(int peliculaId)
        {


            if (!_pelRepo.ExistePelicula(peliculaId))
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    message = $"La pelicula con id {peliculaId} no existe",
                });
            }
            var pelicula = _pelRepo.GetPelicula(peliculaId);

            if (!_pelRepo.BorrarPelicula(pelicula))
            {
                //return NotFound();
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = "Error el server ",
                });
            }
            return Ok(new
            {
                StatusCode = 200,
                message = $"la pelicula : {pelicula.Nombre} ha sido eliminada con éxito "
            });


        }
        [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            var listaPeliculas = _pelRepo.GetPeliculasEnCategoria(categoriaId);
            if (!listaPeliculas.Any())
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    message = $"No existen películas en la categoría con id {categoriaId}",
                });
            }

            var listaPeliculasDto = new List<PeliculaDto>();
            foreach (var pelicula in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(pelicula));
            }

            return Ok(new
            {
                StatusCode = 200,
                message = "Películas obtenidas con éxito",
                data = listaPeliculasDto,
            });
        }

        [HttpGet("Buscar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Buscar(string nombre)
        {
            try {
                var resultado = _pelRepo.BuscarPelicula(nombre);
                if (resultado.Any()) {
                    return Ok(new
                    {
                        StatusCode = 200,
                        message = "Películas obtenidas con éxito",
                        data = resultado,
                    });
                }
                return NotFound(new
                {
                    StatusCode = 404,
                    message = $"No se encontro ninguna pelicula",
                });

            } catch (Exception e) {
                
                
                return StatusCode(500, new
                {
                    StatusCode = 500,
                    message = "Error el server ",
                });

            }
        }





    }
}
