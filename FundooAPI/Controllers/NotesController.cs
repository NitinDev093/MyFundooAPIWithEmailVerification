using BusinessLayer.IBusinesslayer;
using CommonLayer.RequestModel;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FundooAPI.Controllers
{
    [Route("api/[controller]")]
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
    }
}
