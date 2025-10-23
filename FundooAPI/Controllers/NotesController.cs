using BusinessLayer.IBusinesslayer;
using CommonLayer.RequestModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FundooAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesBusinsessLayer _notesBusinessLayer;
        public NotesController(INotesBusinsessLayer _notes)
        {
            _notesBusinessLayer = _notes;
        }

        [HttpPost]
        [Route("CreateNote")]
        public IActionResult CreateNote([FromBody] NotesRequestModel item)
        {
            var response=_notesBusinessLayer.CreateNote(item);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetNotes")]
        public IActionResult getNotes()
        {
            var response = _notesBusinessLayer.getNotes();
            return Ok(response);
        }

        [HttpPost]
        [Route("HandleAllNotesActions")]
        public IActionResult HandleAllNotesAction(int NoteId,string Action)
        {
            var response = _notesBusinessLayer.HandleAllNotesAction(NoteId,Action);
            return Ok(response);
        }

    }
}
