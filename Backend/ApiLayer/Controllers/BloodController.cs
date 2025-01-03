using DataAccessLayer;
using DataBusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/blood")]
    [ApiController]
    public class BloodController : ControllerBase
    {
        [HttpGet("", Name = "GetAllBlood")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BloodDTO>> GetAllBlood()
        {
            List<BloodDTO> BloodList = clsBlood.GetAllBlood();
            if (BloodList.Count == 0)
            {
                return NotFound("No Blood Found!");
            }
            return Ok(BloodList);
        }

        [HttpGet("{Id}", Name = "GetBloodById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BloodDTO> GetBloodById(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }
            clsBlood Blood = clsBlood.Find(Id);
            if (Blood == null)
            {
                return NotFound($"Blood with ID {Id} not found");
            }
            BloodDTO BloodDTO = Blood.BloodDTO;
            return Ok(BloodDTO);
        }

        [HttpPost(Name = "AddBlood")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<BloodDTO> AddBlood(BloodDTO newBloodDTO)
        {
            if (newBloodDTO == null || string.IsNullOrEmpty(newBloodDTO.BloodGroupName))
            {
                return BadRequest("Invalid blood data.");
            }
            //clsBlood Blood = new clsBlood(new BloodDTO(newBloodDTO.Id, newBloodDTO.BloodGroupName, newBloodDTO.QuantityInStock));
            //clsBlood Blood = new clsBlood(new BloodDTO(newBloodDTO.Id, newBloodDTO.BloodGroupName, 0));
            clsBlood Blood = new clsBlood(new BloodDTO(0, newBloodDTO.BloodGroupName, 0));
            Blood.Save();
            newBloodDTO.Id = Blood.Id;
            //we don't return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetBloodById", new { id = newBloodDTO.Id }, newBloodDTO);
        }

        [HttpPut("{Id}", Name = "UpdateBlood")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BloodDTO> UpdateBlood(int Id, BloodDTO updatedBloodDTO)
        {
            if (Id < 1 || updatedBloodDTO == null || string.IsNullOrEmpty(updatedBloodDTO.BloodGroupName))
            {
                return BadRequest("Invalid blood data.");
            }
            clsBlood Blood = clsBlood.Find(Id);
            if (Blood == null)
            {
                return NotFound($"Blood with ID {Id} not found.");
            }
            Blood.BloodGroupName = updatedBloodDTO.BloodGroupName;
            Blood.QuantityInStock = updatedBloodDTO.QuantityInStock;
            Blood.Save();
            return Ok(Blood.BloodDTO);
        }

        [HttpDelete("{Id}", Name = "DeleteBlood")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteBlood(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }
            if (clsBlood.DeleteBlood(Id))
            {
                return Ok($"Blood with ID {Id} has been deleted.");
            }
            else
            {
                return NotFound($"Blood with ID {Id} not found. no rows deleted!");
            }
        }

    }
}
