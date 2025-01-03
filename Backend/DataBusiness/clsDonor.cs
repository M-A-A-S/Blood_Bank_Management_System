using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBusinessLayer
{

    public class clsDonor
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
        public DonorDTO DonorDTO
        {
            get
            {
                return new DonorDTO(this.Id, this.Name, this.DateOfBirth, this.IsMale, this.Phone, this.BloodGroupId, this.Address);
            }
        }

        public clsDonor(DonorDTO DonorDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = DonorDTO.Id;
            this.Name = DonorDTO.Name;
            this.DateOfBirth = DonorDTO.DateOfBirth;
            this.IsMale = DonorDTO.IsMale;
            this.Phone = DonorDTO.Phone;
            this.BloodGroupId = DonorDTO.BloodGroupId;
            this.Address = DonorDTO.Address;
            this.Mode = Mode;
        }

        public bool _AddNewDonor()
        {
            this.Id = clsDonorData.AddDonor(DonorDTO);
            return (this.Id != -1);
        }
        public bool _UpdateDonor()
        {
            return clsDonorData.UpdateDonor(DonorDTO);
        }

        public static List<DonorDTO> GetAllDonors()
        {
            return clsDonorData.GetAllDonors();
        }
        public static clsDonor Find(int Id)
        {
            DonorDTO DonorDTO = clsDonorData.GetDonorById(Id);
            if (DonorDTO != null)
            {
                return new clsDonor(DonorDTO, enMode.Update);
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
                    if (_AddNewDonor())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateDonor();
            }

            return false;
        }
        public static bool DeleteDonor(int Id)
        {
            return clsDonorData.DeleteDonor(Id);
        }

        public static int GetNumberOfDonors()
        {
            return clsDonorData.GetNumberOfDonors();
        }

    }
}
