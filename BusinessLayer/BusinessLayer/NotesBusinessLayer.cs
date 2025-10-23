using BusinessLayer.IBusinesslayer;
using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using Newtonsoft.Json;
using RepositoryLayer.IRepositoryLayer;
using System.Data;

namespace BusinessLayer.BusinessLayer
{
    public class NotesBusinessLayer : INotesBusinsessLayer
    {
        private readonly INotesRepositoryLayer _notesRepositoryLayer;
        public NotesBusinessLayer(INotesRepositoryLayer _notes)
        {
            _notesRepositoryLayer = _notes;
        }

        public ApiResponseModel<List<NotesResponseModel>> CreateNote(NotesRequestModel item)
        {
            ApiResponseModel<List<NotesResponseModel>> response=new ApiResponseModel<List<NotesResponseModel>>();
            DataTable noteData=_notesRepositoryLayer.CreateNote(item);
            List<NotesResponseModel> noteList = new List<NotesResponseModel>();
            if (noteData!=null && noteData.Rows.Count>0)
            {
               string  notesData=JsonConvert.SerializeObject(noteData);
               noteList = JsonConvert.DeserializeObject<List<NotesResponseModel>>(notesData);
               response.isSuccess= true;
               response.Data=noteList;
            }
            else
            {
                response.isSuccess = false;
                response.message = "Unable to create notes";
            }
            return response;
        }

        public ApiResponseModel<List<NotesResponseModel>> getNotes()
        {
            ApiResponseModel<List<NotesResponseModel>> response = new ApiResponseModel<List<NotesResponseModel>>();
            DataTable NotesData = _notesRepositoryLayer.getNotes();
            List<NotesResponseModel> noteList = new List<NotesResponseModel>();
            if (NotesData!=null && NotesData.Rows.Count>0)
            {
                string notesData = JsonConvert.SerializeObject(NotesData);
                noteList = JsonConvert.DeserializeObject<List<NotesResponseModel>>(notesData);
                response.isSuccess = true;
                response.Data = noteList;
            }
            else
            {
                response.isSuccess = false;
                response.message = "Unable to create notes";
            }
            return response;
        }

        public ApiResponseModel<string> HandleAllNotesAction(int NoteId, string Action)
        {
            ApiResponseModel<string> response = new ApiResponseModel<string>();
            bool result = _notesRepositoryLayer.HandleAllNotesAction(NoteId, Action);
            if (result)
            {
                response.isSuccess = true;
                response.message = "Action performed successfully";
            }
            else
            {
                response.isSuccess = false;
                response.message = "Unable to perform action";
            }
            return response;
        }
    }
}
