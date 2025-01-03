using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{

    public class RefreshTokenDTO
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }

        public RefreshTokenDTO(int Id, string Token, int UserId)
        {
            this.Id = Id;
            this.Token = Token;
            this.UserId = UserId;
        }
    }

    public class clsRefreshTokenData
    {
        public static int AddNewRefreshToken(RefreshTokenDTO RefreshTokenDTO)
        {
            int refreshTokenId = -1;
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_AddNewRefreshToken", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Token", RefreshTokenDTO.Token);
                        command.Parameters.AddWithValue("@UserId", RefreshTokenDTO.UserId);
                        var outputIdParam = new SqlParameter("@NewRefreshTokenID", SqlDbType.Int)
                        {
                            Direction = ParameterDirection.Output,
                        };
                        command.Parameters.Add(outputIdParam);
                        connection.Open();
                        command.ExecuteNonQuery();
                        refreshTokenId = (int)outputIdParam.Value;
                        return refreshTokenId; 
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
            return refreshTokenId;
        }

        public static void UpdateRefreshToken(RefreshTokenDTO RefreshTokenDTO)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_UpdateRefreshToken", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Token", RefreshTokenDTO.Token);
                        command.Parameters.AddWithValue("@UserId", RefreshTokenDTO.UserId);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }
        }

        public static bool IsRefreshTokenForUser(int userId)
        {
            //SP_CheckRefreshTokenExistsForUser
            bool isExists = false;

            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_CheckRefreshTokenExistsForUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@UserId", userId);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            isExists = reader.HasRows;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error" + ex.Message);
            }

            return isExists;
        }

        public static RefreshTokenDTO GetRefreshToken(string token)
        {
            try
            {
                using (var connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    using (var command = new SqlCommand("SP_GetRefreshTokenByToken", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Token", token);
                        connection.Open();
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new RefreshTokenDTO
                                (
                                    reader.GetInt32(reader.GetOrdinal("@Id")),
                                    reader.GetString(reader.GetOrdinal("@Token")),
                                    reader.GetInt32(reader.GetOrdinal("@UserId"))
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
    }
}
