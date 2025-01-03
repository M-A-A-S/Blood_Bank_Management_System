using DataAccessLayer;
using DataBusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/bloodTransfers")]
    [ApiController]
    public class BloodTransfersController : ControllerBase
    {
        [HttpGet("", Name = "GetAllBloodTransfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BloodTransferDTO>> GetAllBloodTransfer()
        {
            List<BloodTransferDTO> BloodTransferList = clsBloodTransfer.GetAllBloodTransfer();
            if (BloodTransferList.Count == 0)
            {
                return NotFound("No Blood Transfer Found!");
            }
            return Ok(BloodTransferList);
        }

        [HttpGet("{Id}", Name = "GetBloodTransferById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BloodTransferDTO> GetBloodTransferById(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }
            clsBloodTransfer BloodTransfer = clsBloodTransfer.Find(Id);
            if (BloodTransfer == null)
            {
                return NotFound($"Blood Transfer with ID {Id} not found");
            }
            BloodTransferDTO BloodTransferDTO = BloodTransfer.BloodTransferDTO;
            return Ok(BloodTransferDTO);
        }

        [HttpGet("NumberOfBloodTransfers", Name = "GetNumberOfBloodTransfers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> GetNumberOfPatients()
        {
            return Ok(clsBloodTransfer.GetNumberOfBloodTransfers());
        }

        [HttpPost(Name = "AddBloodTransfer")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<BloodTransferDTO> AddBloodTransfer(BloodTransferDTO newBloodTransferDTO)
        {
            if (newBloodTransferDTO == null || newBloodTransferDTO.PatientId < 1 || newBloodTransferDTO.BloodGroupId < 1)
            {
                return BadRequest("Invalid Blood Transfer data.");
            }
            clsBloodTransfer BloodTransfer = new clsBloodTransfer(new BloodTransferDTO(newBloodTransferDTO.Id, newBloodTransferDTO.PatientId, newBloodTransferDTO.BloodGroupId, newBloodTransferDTO.TransferDate));
            //BloodTransfer.Save();
            if (!BloodTransfer.Save())
            {
                return StatusCode(500, "Internal Server Error");
            }
            newBloodTransferDTO.Id = BloodTransfer.Id;
            //we don't return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetBloodTransferById", new { id = newBloodTransferDTO.Id }, newBloodTransferDTO);
        }

        [HttpPut("{Id}", Name = "UpdateBloodTransfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<BloodTransferDTO> UpdateBloodTransfer(int Id, BloodTransferDTO updatedBloodTransferDTO)
        {
            if (Id < 1 || updatedBloodTransferDTO == null || updatedBloodTransferDTO.PatientId < 1 || updatedBloodTransferDTO.BloodGroupId < 1)
            {
                return BadRequest("Invalid Blood Transfer data.");
            }
            clsBloodTransfer BloodTransfer = clsBloodTransfer.Find(Id);
            if (BloodTransfer == null)
            {
                return NotFound($"Blood Transfer with ID {Id} not found.");
            }
            BloodTransfer.PatientId = updatedBloodTransferDTO.PatientId;
            BloodTransfer.BloodGroupId = updatedBloodTransferDTO.BloodGroupId;
            BloodTransfer.TransferDate = updatedBloodTransferDTO.TransferDate;
            BloodTransfer.Save();
            return Ok(BloodTransfer.BloodTransferDTO);
        }

        [HttpDelete("{Id}", Name = "DeleteBloodTransfer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteBloodTransfer(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }
            if (clsBloodTransfer.DeleteBloodTransfer(Id))
            {
                return Ok($"Blood Transfer with ID {Id} has been deleted.");
            }
            else
            {
                return NotFound($"Blood Transfer with ID {Id} not found. no rows deleted!");
            }
        }

    }
}
