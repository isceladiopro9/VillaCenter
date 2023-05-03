using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;


        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db, IMapper mapper) {
            _logger = logger;
            _db = db;
            _mapper = mapper;       
        }

        // Metodo Asyncrono 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult <IEnumerable<VillaDto>>> GetVillas() {

            _logger.LogInformation("Obtener las Villas");
            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
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
                return Ok(_mapper.Map<VillaDto>(villa));
            }
            else {
                return NotFound();
            }

            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> NewVilla([FromBody] VillaCreateDto villaCreateDto) {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if (_db.Villas.FirstOrDefault(item => item.Nombre.ToLower() == villaCreateDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("Nombre", "La Villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }

            if (villaCreateDto != null)
            {
                Villa newVilla = _mapper.Map<Villa>(villaCreateDto); 

                await _db.Villas.AddAsync(newVilla);
                await _db.SaveChangesAsync();

                return CreatedAtRoute("GetVilla", new { id = newVilla.Id, newVilla });

            }
            else {
                return BadRequest(villaCreateDto);
            }
        }

        [HttpDelete("_id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task <IActionResult> DeleteVilla(int _id) {

            if (_id > 0)
            {
                var DeleteVilla = _db.Villas.FirstOrDefault(i => i.Id == _id);
                if (DeleteVilla != null)
                {
                    _db.Villas.Remove(DeleteVilla);
                    await _db.SaveChangesAsync();
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
        public async Task<IActionResult> UpdateVilla(int _id, [FromBody] VillaUpdateDto villaUpdateDto) {

            if (villaUpdateDto != null || _id == villaUpdateDto.Id)
            {
                Villa updateVilla = _mapper.Map<Villa>(villaUpdateDto);
                
                _db.Villas.Update(updateVilla);
                await _db.SaveChangesAsync();

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
        public async Task <IActionResult> UpdateParcialVilla(int _id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {

            if (patchDto != null || _id != 0)
            {
                var updateVilla = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(i => i.Id == _id);

                VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(updateVilla);

                if (updateVilla != null) return BadRequest();

                patchDto.ApplyTo(villaDto, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);

                }

                Villa villa = _mapper.Map<Villa>(villaDto);
                _db.Villas.Update(villa);
                await _db.SaveChangesAsync();

                return NoContent();

            }
            else
            {
                return BadRequest();
            }
        }

    }
}
