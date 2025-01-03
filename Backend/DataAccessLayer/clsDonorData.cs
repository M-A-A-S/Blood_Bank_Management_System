using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DonorDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }     
        public DateTime DateOfBirth { get; set; }     
        public bool IsMale { get; set; }     
        public string Phone { get; set; }     
        public int BloodGroupId { get; set; }
        public string Address { get; set; }     

        public DonorDTO(int id, string Name, DateTime DateOfBirth, bool IsMale, string Phone, int BloodGroupId, string Address)
        {
            this.Id = id;
            this.Name = Name;
            this.DateOfBirth = DateOfBirth;
            this.IsMale = IsMale;
            this.Phone = Phone;
            this.BloodGroupId = BloodGroupId;
            this.Address = Address;
        }
    }
    public class clsDonorData
    {
        public static List<DonorDTO> GetAllDonors()
        {
            var DonorsList = new List<DonorDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllDonors", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DonorsList.Add(new DonorDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    reader.GetBoolean(reader.GetOrdinal("IsMale")),
                                    reader.GetString(reader.GetOrdinal("Phone")),
                                    reader.GetInt32(reader.GetOrdinal("BloodGroupId")),
                                    reader.GetString(reader.GetOrdinal("Address"))
                                ));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return DonorsList;
        }

        public static DonorDTO GetDonorById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetDonorById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new DonorDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("Name")),
                                    reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                                    reader.GetBoolean(reader.GetOrdinal("IsMale")),
                                    reader.GetString(reader.GetOrdinal("Phone")),
                                    reader.GetInt32(reader.GetOrdinal("BloodGroupId")),
                                    reader.GetString(reader.GetOrdinal("Address"))
                                );
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return null;
        }

        public static int AddDonor(DonorDTO donorDTO)
        {
            int donorId = -1;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewDonor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Name", donorDTO.Name);
                        command.Parameters.AddWithValue("@DateOfBirth", donorDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@IsMale", donorDTO.IsMale);
                        command.Parameters.AddWithValue("@Phone", donorDTO.Phone);
                        command.Parameters.AddWithValue("@BloodGroupId", donorDTO.BloodGroupId);
                        command.Parameters.AddWithValue("@Address", donorDTO.Address);
                        var outputIdParam = new SqlParameter("@NewDonorId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output,
                        };
                        command.Parameters.Add(outputIdParam);
                        connection.Open();
                        command.ExecuteNonQuery();
                        donorId = (int)outputIdParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return donorId;
        }

        public static bool UpdateDonor(DonorDTO donorDTO)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdateDonor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", donorDTO.Id);
                        command.Parameters.AddWithValue("@Name", donorDTO.Name);
                        command.Parameters.AddWithValue("@DateOfBirth", donorDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@IsMale", donorDTO.IsMale);
                        command.Parameters.AddWithValue("@Phone", donorDTO.Phone);
                        command.Parameters.AddWithValue("@BloodGroupId", donorDTO.BloodGroupId);
                        command.Parameters.AddWithValue("@Address", donorDTO.Address);
                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return (rowsAffected > 0);
        }

        public static bool DeleteDonor(int id)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_DeleteDonor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        rowsAffected = (int)command.ExecuteNonQuery();
                        return (rowsAffected == 1);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return (rowsAffected > 0);
        }

        public static int GetNumberOfDonors()
        {
            int numberOfDonors = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetNumberOfDonors", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            int.TryParse(result.ToString(), out numberOfDonors);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return numberOfDonors;
        }

    }
}
