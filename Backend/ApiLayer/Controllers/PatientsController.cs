using DataAccessLayer;
using DataBusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiLayer.Controllers
{
    [Authorize]
    //[Route("api/[controller]")]
    [Route("api/patients")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        [HttpGet("", Name = "GetAllPatients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<PatientDTO>> GetAllPatients()
        {
            List<PatientDTO> PatientsList = clsPatient.GetAllPatients();
            if (PatientsList.Count == 0)
            {
                return NotFound("No Patient Found!");
            }
            return Ok(PatientsList);
        }

        [HttpGet("{Id}", Name = "GetPatientById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PatientDTO> GetPatientById(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }
            clsPatient Patient = clsPatient.Find(Id);
            if (Patient == null)
            {
                return NotFound($"Patient with ID {Id} not found");
            }
            PatientDTO PatientDTO = Patient.PatientDTO;
            return Ok(PatientDTO);
        }

        [HttpGet("NumberOfPatients", Name = "GetNumberOfPatients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<int> GetNumberOfPatients()
        {
            return Ok(clsPatient.GetNumberOfPatients());
        }

        [HttpPost(Name = "AddPatient")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<PatientDTO> AddPatient(PatientDTO newPatientDTO)
        {
            if (newPatientDTO == null || string.IsNullOrEmpty(newPatientDTO.Name) || string.IsNullOrEmpty(newPatientDTO.Phone) || string.IsNullOrEmpty(newPatientDTO.Address))
            {
                return BadRequest("Invalid Patient data.");
            }
            clsPatient Patient = new clsPatient(new PatientDTO(newPatientDTO.Id, newPatientDTO.Name, newPatientDTO.DateOfBirth, newPatientDTO.IsMale, newPatientDTO.Phone, newPatientDTO.BloodGroupId, newPatientDTO.Address));
            Patient.Save();
            newPatientDTO.Id = Patient.Id;
            //we don't return Ok here,we return createdAtRoute: this will be status code 201 created.
            return CreatedAtRoute("GetPatientById", new { id = newPatientDTO.Id }, newPatientDTO);
        }

        [HttpPut("{Id}", Name = "UpdatePatient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<PatientDTO> UpdatePatient(int Id, PatientDTO updatedPatientDTO)
        {
            if (Id < 1 || updatedPatientDTO == null || string.IsNullOrEmpty(updatedPatientDTO.Name) || string.IsNullOrEmpty(updatedPatientDTO.Phone) || string.IsNullOrEmpty(updatedPatientDTO.Address))
            {
                return BadRequest("Invalid Patient data.");
            }
            clsPatient Patient = clsPatient.Find(Id);
            if (Patient == null)
            {
                return NotFound($"Patient with ID {Id} not found.");
            }
            Patient.Name = updatedPatientDTO.Name;
            Patient.DateOfBirth = updatedPatientDTO.DateOfBirth;
            Patient.IsMale = updatedPatientDTO.IsMale;
            Patient.Phone = updatedPatientDTO.Phone;
            Patient.BloodGroupId = updatedPatientDTO.BloodGroupId;
            Patient.Address = updatedPatientDTO.Address;
            Patient.Save();
            return Ok(Patient.PatientDTO);
        }

        [HttpDelete("{Id}", Name = "DeletePatient")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeletePatient(int Id)
        {
            if (Id < 1)
            {
                return BadRequest($"Not accepted Id {Id}");
            }
            if (clsPatient.DeletePatient(Id))
            {
                return Ok($"Patient with ID {Id} has been deleted.");
            }
            else
            {
                return NotFound($"Patient with ID {Id} not found. no rows deleted!");
            }
        }

    }
}
