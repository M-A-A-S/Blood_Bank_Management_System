using DataAccessLayer;

namespace DataBusinessLayer
{
    public class clsBlood
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int Id { get; set; }
        public string BloodGroupName { get; set; }
        public int QuantityInStock { get; set; }
        public BloodDTO BloodDTO
        {
            get
            {
                return new BloodDTO(this.Id, this.BloodGroupName, this.QuantityInStock);
            }
        }

        public clsBlood(BloodDTO BloodDTO, enMode Mode = enMode.AddNew)
        {
            this.Id = BloodDTO.Id;
            this.BloodGroupName = BloodDTO.BloodGroupName;
            this.QuantityInStock = BloodDTO.QuantityInStock;
            this.Mode = Mode;
        }

        public bool _AddNewBlood()
        {
            this.Id = clsBloodData.AddBlood(BloodDTO);
            return (this.Id != -1);
        }
        public bool _UpdateBlood()
        {
            return clsBloodData.UpdateBlood(BloodDTO);
        }

        public static List<BloodDTO> GetAllBlood()
        {
            return clsBloodData.GetAllBlood();
        }
        public static clsBlood Find(int Id)
        {
            BloodDTO BloodDTO = clsBloodData.GetBloodById(Id);
            if (BloodDTO != null)
            {
                return new clsBlood(BloodDTO, enMode.Update);
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
                    if (_AddNewBlood())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateBlood();
            }

            return false;
        }
        public static bool DeleteBlood(int Id)
        {
            return clsBloodData.DeleteBlood(Id);
        }
        public static int GetNumberOfBlood(string bloodGroupName)
        {
            return clsBloodData.GetNumberOfBlood(bloodGroupName);
        }
    }
}
