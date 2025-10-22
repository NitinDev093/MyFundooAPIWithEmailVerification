using CommonLayer.RequestModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.IRepositoryLayer
{
    public interface INotesRepositoryLayer
    {
        DataTable CreateNote(NotesRequestModel item);
        DataTable getNotes();
    }
}
