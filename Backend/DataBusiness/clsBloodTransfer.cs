using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBusinessLayer
{

    public class clsBloodTransfer
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int BloodGroupId { get; set; }
        public DateTime TransferDate { get; set; }
        public BloodTransferDTO BloodTransferDTO
        {
            get
            {
                return new BloodTransferDTO(this.Id, this.PatientId, this.BloodGroupId, this.TransferDate);
            }
        }

        public clsBloodTransfer(BloodTransferDTO BloodTransferDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = BloodTransferDTO.Id;
            this.PatientId = BloodTransferDTO.PatientId;
            this.BloodGroupId = BloodTransferDTO.BloodGroupId;
            this.TransferDate = BloodTransferDTO.TransferDate;
            this.Mode = Mode;
        }

        public bool _AddNewBloodTransfer()
        {
            this.Id = clsBloodTransferData.AddBloodTransfer(BloodTransferDTO);
            return (this.Id != -1);
        }
        public bool _UpdateBloodTransfer()
        {
            return clsBloodTransferData.UpdateBloodTransfer(BloodTransferDTO);
        }

        public static List<BloodTransferDTO> GetAllBloodTransfer()
        {
            return clsBloodTransferData.GetAllBloodTransfers();
        }
        public static clsBloodTransfer Find(int Id)
        {
            BloodTransferDTO BloodTransferDTO = clsBloodTransferData.GetBloodTransferById(Id);
            if (BloodTransferDTO != null)
            {
                return new clsBloodTransfer(BloodTransferDTO, enMode.Update);
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
                    if (_AddNewBloodTransfer())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateBloodTransfer();
            }

            return false;
        }
        public static bool DeleteBloodTransfer(int Id)
        {
            return clsBloodTransferData.DeleteBloodTransfer(Id);
        }
        public static int GetNumberOfBloodTransfers()
        {
            return clsBloodTransferData.GetNumberOfBloodTransfers();
        }

    }
}
