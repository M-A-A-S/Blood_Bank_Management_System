using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBusinessLayer
{

    public class clsPatient
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsMale { get; set; }
        public string Phone { get; set; }
        public int BloodGroupId { get; set; }
        public string Address { get; set; }
        public PatientDTO PatientDTO
        {
            get
            {
                return new PatientDTO(this.Id, this.Name, this.DateOfBirth, this.IsMale, this.Phone, this.BloodGroupId, this.Address);
            }
        }

        public clsPatient(PatientDTO PatientDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = PatientDTO.Id;
            this.Name = PatientDTO.Name;
            this.DateOfBirth = PatientDTO.DateOfBirth;
            this.IsMale = PatientDTO.IsMale;
            this.Phone = PatientDTO.Phone;
            this.BloodGroupId = PatientDTO.BloodGroupId;
            this.Address = PatientDTO.Address;
            this.Mode = Mode;
        }

        public bool _AddNewPatient()
        {
            this.Id = clsPatientData.AddPatient(PatientDTO);
            return (this.Id != -1);
        }
        public bool _UpdatePatient()
        {
            return clsPatientData.UpdatePatient(PatientDTO);
        }

        public static List<PatientDTO> GetAllPatients()
        {
            return clsPatientData.GetAllPatients();
        }
        public static clsPatient Find(int Id)
        {
            PatientDTO PatientDTO = clsPatientData.GetPatientById(Id);
            if (PatientDTO != null)
            {
                return new clsPatient(PatientDTO, enMode.Update);
            }
            else
            {
                return null;
            }
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPatient())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdatePatient();
            }

            return false;
        }
        public static bool DeletePatient(int Id)
        {
            return clsPatientData.DeletePatient(Id);
        }

        public static int GetNumberOfPatients()
        {
            return clsPatientData.GetNumberOfPatients();
        }

    }
}
