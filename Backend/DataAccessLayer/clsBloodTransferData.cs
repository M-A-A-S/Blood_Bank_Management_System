using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{

    public class BloodTransferDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int BloodGroupId { get; set; }
        public DateTime TransferDate { get; set; }

        public BloodTransferDTO(int id, int patientId, int bloodGroupId, DateTime transferDate)
        {
            this.Id = id;
            this.PatientId = patientId;
            this.BloodGroupId = bloodGroupId;
            this.TransferDate = transferDate;
        }
    }
    public class clsBloodTransferData
    {
        public static List<BloodTransferDTO> GetAllBloodTransfers()
        {
            var BloodTransfersList = new List<BloodTransferDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllBloodTransfers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BloodTransfersList.Add(new BloodTransferDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("PatientId")),
                                    reader.GetInt32(reader.GetOrdinal("BloodGroupId")),
                                    reader.GetDateTime(reader.GetOrdinal("TransferDate"))
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

            return BloodTransfersList;
        }

        public static BloodTransferDTO GetBloodTransferById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetBloodTransferById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new BloodTransferDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetInt32(reader.GetOrdinal("PatientId")),
                                    reader.GetInt32(reader.GetOrdinal("BloodGroupId")),
                                    reader.GetDateTime(reader.GetOrdinal("TransferDate"))
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

        public static int AddBloodTransfer(BloodTransferDTO bloodTransferDTO)
        {
            int bloodTransferId = -1;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewBloodTransfer", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PatientId", bloodTransferDTO.PatientId);
                        command.Parameters.AddWithValue("@BloodGroupId", bloodTransferDTO.BloodGroupId);
                        command.Parameters.AddWithValue("@TransferDate", bloodTransferDTO.TransferDate);
                        var outputIdParam = new SqlParameter("@NewBloodTransferId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output,
                        };
                        command.Parameters.Add(outputIdParam);
                        connection.Open();
                        command.ExecuteNonQuery();
                        bloodTransferId = (int)outputIdParam.Value;
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return bloodTransferId;

        }

        public static bool UpdateBloodTransfer(BloodTransferDTO bloodTransferDTO)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdateBloodTransfer", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", bloodTransferDTO.Id);
                        command.Parameters.AddWithValue("@PatientId", bloodTransferDTO.PatientId);
                        command.Parameters.AddWithValue("@BloodGroupId", bloodTransferDTO.BloodGroupId);
                        command.Parameters.AddWithValue("@TransferDate", bloodTransferDTO.TransferDate);
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

        public static bool DeleteBloodTransfer(int id)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_DeleteBloodTransfer", connection))
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

        public static int GetNumberOfBloodTransfers()
        {
            int numberOfBloodTransfers = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetNumberOfBloodTransfers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            int.TryParse(result.ToString(), out numberOfBloodTransfers);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return numberOfBloodTransfers;
        }

    }
}
