using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VillaCenter_Api.Data;
using VillaCenter_Api.Models;
using VillaCenter_Api.Models.DTO;

namespace VillaCenter_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;

        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db) {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult <IEnumerable<VillaDto>> GetVillas() {

            _logger.LogInformation("Obtener las Villas");
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("id:int",Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int _id)
        {

            if (_id == 0) {
                _logger.LogError("Error al traer la villa con id >"+_id);
                return BadRequest();
            }

            //var villa = VillaStore.villaList.FirstOrDefault(i => i.Id == _id);
            var villa = _db.Villas.FirstOrDefault(x => x.Id == _id);

            if (villa != null) {
                return Ok(villa);
            }
            else {
                return NotFound();
            }

            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> NewVilla([FromBody] VillaDto villaDto) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (_db.Villas.FirstOrDefault(item => item.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("Nombre", "La Villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }

            if (villaDto != null)
            {
                if (villaDto.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                else {

                    Villa newVilla = new()
                    {
                        Nombre = villaDto.Nombre,
                        Detalle = villaDto.Detalle,
                        ImagenUrl = villaDto.ImagenUrl,
                        Ocupantes = villaDto.Ocupantes,
                        Tarifa = villaDto.Tarifa,
                        MetrosCuadrados  = villaDto.MetrosCuadrados,
                        Amenidad = villaDto.Amenidad,
                    };

                    _db.Villas.Add(newVilla);
                    _db.SaveChanges();

                    return CreatedAtRoute("GetVilla", new { id = villaDto.Id,villaDto});
                
                }
            }else {
                return BadRequest(villaDto);
            }
        }

        [HttpDelete("_id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteVilla(int _id) {

            if (_id > 0)
            {
                var DeleteVilla = _db.Villas.FirstOrDefault(i => i.Id == _id);
                if (DeleteVilla != null)
                {
                    _db.Villas.Remove(DeleteVilla);
                    _db.SaveChanges();
                    return NoContent();
                }
                else {
                    return NotFound();
                }
            }
            else {
                return BadRequest();
            }
            
            
        }

        [HttpPut("_id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateVilla(int _id, [FromBody] VillaDto villaDto) {

            if (villaDto != null || _id == villaDto.Id)
            {
                Villa updateVilla = new()
                {
                    Id = villaDto.Id,
                    Nombre = villaDto.Nombre,
                    Detalle = villaDto.Detalle,
                    ImagenUrl = villaDto.ImagenUrl,
                    Ocupantes = villaDto.Ocupantes,
                    Tarifa = villaDto.Tarifa,
                    MetrosCuadrados = villaDto.MetrosCuadrados,
                    Amenidad = villaDto.Amenidad,
                };

                _db.Villas.Update(updateVilla);
                _db.SaveChanges();

                return NoContent();

            }
            else {
                return BadRequest();
            }
        }


        [HttpPatch("_id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateParcialVilla(int _id, JsonPatchDocument<VillaDto> patchDto)
        {

            if (patchDto != null || _id != 0)
            {
                var updateVilla = _db.Villas.FirstOrDefault(i => i.Id == _id);

                VillaDto villaDto = new() {
                    Id = updateVilla.Id,
                    Nombre = updateVilla.Nombre,
                    Detalle = updateVilla.Detalle,
                    ImagenUrl = updateVilla.ImagenUrl,
                    Ocupantes = updateVilla.Ocupantes,
                    Tarifa = updateVilla.Tarifa,
                    MetrosCuadrados = updateVilla.MetrosCuadrados,
                    Amenidad = updateVilla.Amenidad,
                };

                if (updateVilla != null) return BadRequest();   

                patchDto.ApplyTo(villaDto, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);

                }

                return NoContent();

            }
            else
            {
                return BadRequest();
            }
        }

    }
}
