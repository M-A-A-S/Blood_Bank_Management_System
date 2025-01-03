using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class BloodDTO
    {
        public int Id { get; set; }
        public string BloodGroupName { get; set; }
        public int QuantityInStock { get; set; }

        public BloodDTO(int id, string bloodGroupName, int quantityInStock)
        {
            this.Id = id;
            this.BloodGroupName = bloodGroupName;
            this.QuantityInStock = quantityInStock;
        }
    }
    public class clsBloodData
    {
        public static List<BloodDTO> GetAllBlood()
        {
            var BloodList = new List<BloodDTO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetAllBlood", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BloodList.Add(new BloodDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("BloodGroupName")),
                                    reader.GetInt32(reader.GetOrdinal("QuantityInStock"))
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

            return BloodList;
        }

        public static BloodDTO GetBloodById(int id)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetBloodById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new BloodDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("Id")),
                                    reader.GetString(reader.GetOrdinal("BloodGroupName")),
                                    reader.GetInt32(reader.GetOrdinal("QuantityInStock"))
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

        public static int AddBlood(BloodDTO bloodDTO)
        {
            int bloodId = -1;
            //try
            {
                using (var connection = new SqlConnection (clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewBlood", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BloodGroupName", bloodDTO.BloodGroupName);
                        //command.Parameters.AddWithValue("@QuantityInStock", bloodDTO.QuantityInStock);
                        command.Parameters.AddWithValue("@QuantityInStock", 0);
                        var outputIdParam = new SqlParameter("@NewBloodId", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output,
                        };
                        command.Parameters.Add(outputIdParam);
                        connection.Open();
                        command.ExecuteNonQuery();
                        bloodId = (int)outputIdParam.Value;
                        return bloodId;
                    }
                }
            }
            //catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return bloodId;
        }

        public static bool UpdateBlood(BloodDTO bloodDTO)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdateBlood", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Id", bloodDTO.Id);
                        command.Parameters.AddWithValue("@BloodGroupName", bloodDTO.BloodGroupName);
                        command.Parameters.AddWithValue("@QuantityInStock", bloodDTO.QuantityInStock);
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

        public static bool DeleteBlood(int id)
        {
            int rowsAffected = 0;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_DeleteBlood", connection))
                    {
                        command.CommandType= CommandType.StoredProcedure;
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

        public static int GetNumberOfBlood(string bloodGroupName)
        {
            int numberOfBlood = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("SP_GetNumberOfBlood", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@BloodGroupName", bloodGroupName);
                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            int.TryParse(result.ToString(), out numberOfBlood);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return numberOfBlood;
        }

    }
}
