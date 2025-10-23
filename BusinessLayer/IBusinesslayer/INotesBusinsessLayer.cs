using CommonLayer.RequestModel;
using CommonLayer.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.IBusinesslayer
{
    public interface INotesBusinsessLayer
    {
        ApiResponseModel<List<NotesResponseModel>> CreateNote(NotesRequestModel item);
        ApiResponseModel<List<NotesResponseModel>> getNotes();
        ApiResponseModel<string> HandleAllNotesAction(int NoteId, string Action);
    }
}
