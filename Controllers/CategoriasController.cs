using ApiPeliculas.Modelos;
using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {

        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;
        public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper) {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public IActionResult GetCategorias() {
            var listaCategorias = _ctRepo.GetCategorias();
            var listaCategoriasDto = new List<CategoriaDto>();
            foreach (var lista in listaCategorias) {
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(lista));
            }
            return Ok(new
            {
                StatusCode = 200,
                message = "Consulta realizada con éxito",
                data = listaCategoriasDto,
            }
            );
        }


        
        [HttpGet("{CategoriaId:int}", Name ="GetCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public IActionResult GetCategorias(int CategoriaId)
        {

            var itemCategoria = _ctRepo.GetCategoria(CategoriaId);
            if (itemCategoria==null) {
                return NotFound(new
                {
                    StatusCode = 404,
                    message = $"La categoría con id {CategoriaId} no existe",
                    data = itemCategoria,   
                });
            }
            var itemCategoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);
 
            return Ok( new
                    {
                        StatusCode = 200,  
                        message = "Categoría obtenida con éxito",
                        data = itemCategoriaDto,
                    }
            );
        }



        


        [HttpPost]
   
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]


        public IActionResult GetCategorias([FromForm] CrearCategoriaDto crearCategoriaDto)
        {

            if (!ModelState.IsValid) {
                return BadRequest(new
                {
                    StatusCode = 400,
                    message = "El modelo no es valido",
                   // data = ModelState,
                });

               // return BadRequest(ModelState);
            }

            if (crearCategoriaDto==null)
            {
               
                 return BadRequest(new
                {
                    StatusCode = 400,
                    message = "El modelo no es valido",
                   // data = ModelState,
                });
              //  return BadRequest(ModelState);
            }

            if (_ctRepo.ExisteCategoria(crearCategoriaDto.Nombre)) {

                //ModelState.AddModelError("", "La categoría ya existe");
                //return StatusCode(404,ModelState);
               
                return Conflict(new
                {
                    StatusCode = 409,
                    message = $"La categoría ya existe",
                });
            }


            var categoria = _mapper.Map<Categoria>(crearCategoriaDto);
            if (!_ctRepo.CrearCategoria(categoria) ) {


               // ModelState.AddModelError("", $"Algo salio mal guardando el registro{categoria.Nombre}");
               // return StatusCode(404, ModelState);

                ModelState.AddModelError("", $"Algo salio mal guardando el registro{ categoria.Nombre}");
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
                data = categoria,
            });

            //return CreatedAtRoute("GetCategoria", new { categoriaId =categoria.Id },categoria);
        }




        [HttpPatch("{CategoriaId:int}", Name = "ActualizarPatchCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]


        public IActionResult ActualizarPatchCategoria(int CategoriaId, [FromForm] CategoriaDto categoriaDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    message = "El modelo no es valido",
                 
                });
            }

            if (categoriaDto == null || CategoriaId != categoriaDto.Id)
            {

                return BadRequest(new
                {
                    StatusCode = 400,
                    message = "El modelo no es valido",
                   
                });
             
            }

           


            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{categoria.Nombre}");
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
                data = categoria,
            });

            //return CreatedAtRoute("GetCategoria", new { categoriaId =categoria.Id },categoria);
        }





        [HttpPut("{CategoriaId:int}", Name = "ActualizarPutCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]


        public IActionResult ActualizarPutCategoria(int CategoriaId, [FromForm] CategoriaDto categoriaDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    StatusCode = 400,
                    message = "El modelo no es valido",

                });
            }

            if (categoriaDto == null || CategoriaId != categoriaDto.Id)
            {

                return BadRequest(new
                {
                    StatusCode = 400,
                    message = "El modelo no es valido",

                });

            }
            var categoriaExistente = _ctRepo.GetCategoria(CategoriaId);
            if (categoriaExistente==null)
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    message = $"La categoría con id {CategoriaId} no existe",
                });
            }



            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (!_ctRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro{categoria.Nombre}");
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
                data = categoria,
            });

         
        }



        [HttpDelete("{CategoriaId:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]

        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]


        public IActionResult BorrarCategoria(int CategoriaId)
        {

           
            if (!_ctRepo.ExisteCategoria(CategoriaId))
            {
                return NotFound(new
                {
                    StatusCode = 404,
                    message = $"La categoría con id {CategoriaId} no existe",
                });
            }
            var categoria = _ctRepo.GetCategoria(CategoriaId);

            if (!_ctRepo.BorrarCategoria(categoria)) {
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
                message = $"la categoria : {categoria.Nombre} ha sido eliminada con éxito "
            });


        }




    }
}
