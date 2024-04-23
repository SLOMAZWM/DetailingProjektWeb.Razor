using BCrypt.Net;
using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebProjektRazor.Models;
using WebProjektRazor.Models.User;

namespace WebProjektRazor.Database
{
    public static class UserDatabase
    {
        public static string? ConnectionString { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("connectionString")
                ?? throw new InvalidOperationException("Nie znaleziono connection string w konfiguracji.");
        }

        public static async Task<Client?> AddUserToDatabase(RegisterUser user)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                await conn.OpenAsync();
                await using (SqlCommand cmd = conn.CreateCommand())
                {
                    SqlTransaction transaction = conn.BeginTransaction();
                    cmd.Transaction = transaction;

                    try
                    {
                        cmd.CommandText = @"
                            INSERT INTO [dbo].[User] (FirstName, LastName, Email, Password, PhoneNumber, Discriminator) 
                            VALUES (@FirstName, @LastName, @Email, @Password, @PhoneNumber, 'Client');
                            SELECT CAST(scope_identity() AS int);";

                        cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", user.LastName);
                        cmd.Parameters.AddWithValue("@Email", user.Email);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);

                        int userId = (int)await cmd.ExecuteScalarAsync();

                        cmd.Parameters.Clear();

                        cmd.CommandText = "INSERT INTO [dbo].[Client] (UserId) VALUES (@UserId);";
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        await cmd.ExecuteNonQueryAsync();

                        transaction.Commit();

                        return new Client
                        {
                            UserId = userId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            ClientId = userId,
                            HistoryProductsOrders = new ObservableCollection<OrderProducts>(),
                            HistoryServiceOrders = new ObservableCollection<OrderService>()
                        };
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return null;
                    }
                }
            }
        }

        public static async Task<Client> TryLoginUser(string email, string password)
        {
            try
            {
                await using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    await using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT u.UserId, u.FirstName, u.LastName, u.Email, u.Password, u.PhoneNumber, 
                                    c.ClientId FROM [dbo].[User] u 
                                    INNER JOIN [dbo].[Client] c ON u.UserId = c.UserId
                                    WHERE u.Email = @Email;";  

                        cmd.Parameters.AddWithValue("@Email", email);

                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if (reader.Read())
                        {
                            var hashedPassword = reader.GetString(reader.GetOrdinal("Password"));
                            if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                            {
                                var client = new Client
                                {
                                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                    ClientId = reader.GetInt32(reader.GetOrdinal("ClientId")),
                                    HistoryProductsOrders = new ObservableCollection<OrderProducts>(),
                                    HistoryServiceOrders = new ObservableCollection<OrderService>()
                                };
                                return client;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas próby logowania: " + ex.Message);
            }

            return null;
        }


    }
}
