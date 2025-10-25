using CommonLayer.RequestModel;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.IRepositoryLayer;
using System.Data;
using System.Data.SqlClient;


namespace RepositoryLayer.RepositoryLayer
{
    public class UserRepositoryLayer : IUserRepositoryLayer
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;
        public UserRepositoryLayer(IConfiguration _config)
        {
            _configuration = _config;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        
        public int InsertuserData(UserRequestModel User,string token)
        {
            DateTime expiry = DateTime.UtcNow.AddHours(24);
            using (SqlConnection con=new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_InsertUserData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FirstName", User.firstName);
                cmd.Parameters.AddWithValue("@SecondName",User.secondName);
                cmd.Parameters.AddWithValue("@Email",User.emailAddress);
                cmd.Parameters.AddWithValue("@Password",User.password);
                cmd.Parameters.AddWithValue("@VerificationToken", token);
                cmd.Parameters.AddWithValue("@TokenExpiry", expiry);
                con.Open();
                object result = cmd.ExecuteScalar();
                int userId = Convert.ToInt32(result);
                return  userId;
            }
        }
        public DataTable EmailVerificationr(string token)
        {
            using (SqlConnection sqlcon=new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_EmailVerifcation", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VerificationToken",token);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                return dt;
            }
        }

        public DataTable UserLogin(string email, string password)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_loginUser", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                return dt;
            }
        }

        public DataTable CheckEmailExistance(string email)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_checkemailExistance", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                return dt;
            }
        }

        public DataTable GetUserByEmailAddress(string email)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetUserByEmailAddress", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                return dt;
            }
        }

        public bool ResetPassword(int userId,string newPassword)
        {
            using (SqlConnection sqlcon = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_ResetPassword", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NewPassword", newPassword);
                cmd.Parameters.AddWithValue("@UserId", userId);
                sqlcon.Open();
                int result = cmd.ExecuteNonQuery();
                return Convert.ToBoolean(result);
            }
        }
    }
}
