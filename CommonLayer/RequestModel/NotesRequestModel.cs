using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLayer.RequestModel
{
    public class NotesRequestModel
    {
            public string? Title { get; set; }
            public string? Description { get; set; }
            public List<string>? Collaborators { get; set; }
            public List<string>? Labels { get; set; }
            public string? Reminder { get; set; }
            public string? Color { get; set; }
            public string? Image { get; set; }
            public int IsPinned { get; set; }
            public int IsArchived { get; set; }
            public int IsDeleted { get; set; }
    }

}
