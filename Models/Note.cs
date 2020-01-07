using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Datastore.V1;

namespace ADAssignment.Models
{
    public class Note
    {
        public long id { get; set; }
        public string Title { get; set; }

        public string NoteBody { get; set; }

        public string Category { get; set; }
    }
}