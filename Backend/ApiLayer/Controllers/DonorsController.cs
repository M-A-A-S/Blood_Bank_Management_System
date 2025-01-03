using DataAccessLayer;
using DataBusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/donors")]
    [ApiController]
    public class DonorsController : ControllerBase
    {
        [HttpGet("", Name = "GetAllDonors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<DonorDTO>> GetAllDonors()
        {
            List<DonorDTO> DonorsList = clsDonor.GetAllDonors();
            if (DonorsList.Count == 0)
            {
                return NotFound("No Donor Found!");
            }
            return Ok(DonorsList);
        }

        [HttpGet("{Id}", Name = "GetDonorById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DonorDTO> GetDonorById(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }
            clsDonor Donor = clsDonor.Find(Id);
            if (Donor == null)
            {
                return NotFound($"Donor with ID {Id} not found");
            }
            DonorDTO DonorDTO = Donor.DonorDTO;
            return Ok(DonorDTO);
        }

        [HttpGet("NumberOfDonors", Name = "GetNumberOfDonors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> GetNumberOfDonors()
        {
            return Ok(clsDonor.GetNumberOfDonors());
        }

        [HttpPost(Name = "AddDonor")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<DonorDTO> AddDonor(DonorDTO newDonorDTO)
        {
            if (newDonorDTO == null || string.IsNullOrEmpty(newDonorDTO.Name) || string.IsNullOrEmpty(newDonorDTO.Phone) || string.IsNullOrEmpty(newDonorDTO.Address))
            {
                return BadRequest("Invalid Donor data.");
            }
            clsDonor Donor = new clsDonor(new DonorDTO(newDonorDTO.Id, newDonorDTO.Name, newDonorDTO.DateOfBirth, newDonorDTO.IsMale, newDonorDTO.Phone, newDonorDTO.BloodGroupId, newDonorDTO.Address));
            Donor.Save();
            newDonorDTO.Id = Donor.Id;
            //we don't return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetDonorById", new { id = newDonorDTO.Id }, newDonorDTO);
        }

        [HttpPut("{Id}", Name = "UpdateDonor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DonorDTO> UpdateDonor(int Id, DonorDTO updatedDonorDTO)
        {
            if (Id < 1 || updatedDonorDTO == null || string.IsNullOrEmpty(updatedDonorDTO.Name) || string.IsNullOrEmpty(updatedDonorDTO.Phone) || string.IsNullOrEmpty(updatedDonorDTO.Address))
            {
                return BadRequest("Invalid Donor data.");
            }
            clsDonor Donor = clsDonor.Find(Id);
            if (Donor == null)
            {
                return NotFound($"Donor with ID {Id} not found.");
            }
            Donor.Name = updatedDonorDTO.Name;
            Donor.DateOfBirth = updatedDonorDTO.DateOfBirth;
            Donor.IsMale = updatedDonorDTO.IsMale;
            Donor.Phone = updatedDonorDTO.Phone;
            Donor.BloodGroupId = updatedDonorDTO.BloodGroupId;
            Donor.Address = updatedDonorDTO.Address;
            Donor.Save();
            return Ok(Donor.DonorDTO);
        }

        [HttpDelete("{Id}", Name = "DeleteDonor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteDonor(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }
            if (clsDonor.DeleteDonor(Id))
            {
                return Ok($"Donor with ID {Id} has been deleted.");
            }
            else
            {
                return NotFound($"Donor with ID {Id} not found. no rows deleted!");
            }
        }

    }
}
