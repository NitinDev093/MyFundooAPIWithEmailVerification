using CommonLayer.RequestModel;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.IRepositoryLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.RepositoryLayer
{
    public class NotesRepositoryLayer : INotesRepositoryLayer
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public NotesRepositoryLayer(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public DataTable CreateNote(NotesRequestModel item)
        {
            using (SqlConnection sqlcon = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("InsertNoteWithDetails", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Title",item.Title);
                cmd.Parameters.AddWithValue("@Description",item.Description);

                //string.Join ek C# method hai jo list ke elements ko ek string me
                //jod deta hai, aur har element ke beech me comma (,) daal deta hai.
                var collaboratorString = string.Join(",", item.Collaborators);
                cmd.Parameters.AddWithValue("@Collaborators", collaboratorString);
                var labelString = string.Join(",", item.Labels);
                cmd.Parameters.AddWithValue("@Labels", labelString);
                //end
                cmd.Parameters.AddWithValue("@Reminder", item.Reminder);
                cmd.Parameters.AddWithValue("@Color", item.Color);
                cmd.Parameters.AddWithValue("@Image", item.Image);
                cmd.Parameters.AddWithValue("@IsPinned", item.IsPinned);
                cmd.Parameters.AddWithValue("@IsArchived", item.IsArchived);
                cmd.Parameters.AddWithValue("@IsDeleted", item.IsDeleted);
                sqlcon.Open();
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                return dt;
            }
        }

        public DataTable getNotes()
        {
            using (SqlConnection sqlcon=new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_getNotes",sqlcon);
                DataTable dt = new DataTable();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                return dt;
            }
        }

        public bool HandleAllNotesAction(int NoteId, string Action)
        {
            using (SqlConnection sqlcon=new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_HandleNoteAction", sqlcon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NoteId", NoteId);
                cmd.Parameters.AddWithValue("@Action", Action);
                sqlcon.Open();
                int status = cmd.ExecuteNonQuery();
                return Convert.ToBoolean(status);
            }
        }
    }
}
