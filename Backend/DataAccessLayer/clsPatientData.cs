using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{

    public class PatientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsMale { get; set; }
        public string Phone { get; set; }
        public int BloodGroupId { get; set; }
        public string Address { get; set; }

        public PatientDTO(int id, string Name, DateTime DateOfBirth, bool IsMale, string Phone, int BloodGroupId, string Address)
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
    public class clsPatientData
    {
        public static List<PatientDTO> GetAllPatients()
        {
            var PatientsList = new List<PatientDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllPatients", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PatientsList.Add(new PatientDTO
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

            return PatientsList;
        }

        public static PatientDTO GetPatientById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetPatientById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new PatientDTO
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

        public static int AddPatient(PatientDTO patientDTO)
        {
            int patientId = -1;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewPatient", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Name", patientDTO.Name);
                        command.Parameters.AddWithValue("@DateOfBirth", patientDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@IsMale", patientDTO.IsMale);
                        command.Parameters.AddWithValue("@Phone", patientDTO.Phone);
                        command.Parameters.AddWithValue("@BloodGroupId", patientDTO.BloodGroupId);
                        command.Parameters.AddWithValue("@Address", patientDTO.Address);
                        var outputIdParam = new SqlParameter("@NewPatientId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output,
                        };
                        command.Parameters.Add(outputIdParam);
                        connection.Open();
                        command.ExecuteNonQuery();
                        patientId = (int)outputIdParam.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return patientId;
        }

        public static bool UpdatePatient(PatientDTO patientDTO)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdatePatient", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", patientDTO.Id);
                        command.Parameters.AddWithValue("@Name", patientDTO.Name);
                        command.Parameters.AddWithValue("@DateOfBirth", patientDTO.DateOfBirth);
                        command.Parameters.AddWithValue("@IsMale", patientDTO.IsMale);
                        command.Parameters.AddWithValue("@Phone", patientDTO.Phone);
                        command.Parameters.AddWithValue("@BloodGroupId", patientDTO.BloodGroupId);
                        command.Parameters.AddWithValue("@Address", patientDTO.Address);
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

        public static bool DeletePatient(int id)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_DeletePatient", connection))
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

        public static int GetNumberOfPatients()
        {
            int numberOfPatients = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetNumberOfPatients", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            int.TryParse(result.ToString(), out numberOfPatients);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return numberOfPatients;
        }

    }
}
