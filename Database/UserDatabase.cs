using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WebProjektRazor.Models;

namespace WebProjektRazor.Database
{
    public static class UserDatabase
    {
        public static string ? ConnectionString { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("connectionString")
                ?? throw new InvalidOperationException("Nie znaleziono connection string w konfiguracji.");
        }

        public static async Task AddUserToDatabase(User user)
        {
            await using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = conn.CreateCommand())
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
                        cmd.Parameters.AddWithValue("@Password", user.Password);
                        cmd.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);

                        int userId = (int)await cmd.ExecuteScalarAsync();

                        cmd.Parameters.Clear();

                        cmd.CommandText = "INSERT INTO [dbo].[Client] (UserId) VALUES (@UserId);";
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        await cmd.ExecuteNonQueryAsync();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception("Błąd podczas dodawania użytkownika: " + ex.Message, ex);
                    }
                }
            }
        }
    }
}
