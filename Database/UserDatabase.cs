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
                        throw new Exception("Błąd podczas rejestrowania: " + ex.Message);
                    }
                }
            }
        }

        public static async Task<User> TryLoginUser(string email, string password)
        {
            try
            {
                await using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    await using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT u.UserId, u.FirstName, u.LastName, u.Email, u.Password, u.PhoneNumber, u.Discriminator,
                                    c.ClientId, e.EmployeeId, e.Position
                                    FROM [dbo].[User] u
                                    LEFT JOIN [dbo].[Client] c ON u.UserId = c.UserId
                                    LEFT JOIN [dbo].[Employee] e ON u.UserId = e.UserId
                                    WHERE u.Email = @Email;";

                        cmd.Parameters.AddWithValue("@Email", email);

                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if (reader.Read())
                        {
                            var hashedPassword = reader.GetString(reader.GetOrdinal("Password"));
                            if (BCrypt.Net.BCrypt.Verify(password, hashedPassword))
                            {
                                string discriminator = reader.GetString(reader.GetOrdinal("Discriminator"));
                                if (discriminator == "Client")
                                {
                                    return new Client
                                    {
                                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                        Email = reader.GetString(reader.GetOrdinal("Email")),
                                        PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                        ClientId = reader.GetInt32(reader.GetOrdinal("ClientId")),
                                        HistoryProductsOrders = new ObservableCollection<OrderProducts>(),
                                        HistoryServiceOrders = new ObservableCollection<OrderService>(),
                                        Role = UserRole.Client
                                    };
                                }
                                else if (discriminator == "Employee")
                                {
                                    return new Employee
                                    {
                                        UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                        Email = reader.GetString(reader.GetOrdinal("Email")),
                                        PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                        EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                                        Position = reader.IsDBNull(reader.GetOrdinal("Position")) ? null : reader.GetString(reader.GetOrdinal("Position")),
                                        AssignedOrders = new ObservableCollection<Order>(),
                                        Role = UserRole.Employee
                                    };
                                }
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

        public static async Task<bool> UpdateUserPassword(string userId, string currentPassword, string newPassword)
        {
            try
            {
                await using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    await using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "SELECT Password FROM [dbo].[User] WHERE UserId = @UserId;";
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        string hashedPassword = (string)await cmd.ExecuteScalarAsync();

                        Console.WriteLine($"Fetched hashed password for user {userId}: {hashedPassword}");

                        if (!BCrypt.Net.BCrypt.Verify(currentPassword, hashedPassword))
                        {
                            Console.WriteLine("Password verification failed.");
                            return false;
                        }

                        string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

                        Console.WriteLine($"New hashed password for user {userId}: {newHashedPassword}");

                        cmd.Parameters.Clear();
                        cmd.CommandText = "UPDATE [dbo].[User] SET Password = @NewPassword WHERE UserId = @UserId;";
                        cmd.Parameters.AddWithValue("@NewPassword", newHashedPassword);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        Console.WriteLine($"Rows affected: {rowsAffected}");
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during password update: {ex.Message}");
                throw new Exception("Błąd podczas aktualizacji hasła: " + ex.Message);
            }
        }



        public static async Task<User?> GetUserById(int userId)
        {
            try
            {
                await using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    await using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT UserId, FirstName, LastName, Email, PhoneNumber FROM [dbo].[User] WHERE UserId = @UserId;";
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        SqlDataReader reader = await cmd.ExecuteReaderAsync();
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Email = reader.GetString(reader.GetOrdinal("Email")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Błąd podczas pobierania użytkownika: " + ex.Message);
            }

            return null;
        }

        public static async Task<bool> UpdateUserEmail(string userId, string newEmail)
        {
            try
            {
                await using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    await using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "UPDATE [dbo].[User] SET Email = @NewEmail WHERE UserId = @UserId;";
                        cmd.Parameters.AddWithValue("@NewEmail", newEmail);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        Console.WriteLine($"Rows affected: {rowsAffected}");
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during email update: {ex.Message}");
                throw new Exception("Błąd podczas aktualizacji emaila: " + ex.Message);
            }
        }

        public static async Task<bool> UpdateUserPhoneNumber(string userId, string newPhoneNumber)
        {
            try
            {
                await using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();
                    await using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "UPDATE [dbo].[User] SET PhoneNumber = @NewPhoneNumber WHERE UserId = @UserId;";
                        cmd.Parameters.AddWithValue("@NewPhoneNumber", newPhoneNumber);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        Console.WriteLine($"Rows affected: {rowsAffected}");
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during phone number update: {ex.Message}");
                throw new Exception("Błąd podczas aktualizacji numeru telefonu: " + ex.Message);
            }
        }

    }
}
