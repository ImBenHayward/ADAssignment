using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ADAssignment.Models;
using Google.Cloud.Datastore.V1;

namespace ADAssignment.Managers
{
    public class NoteManager
    {
        private readonly DatastoreDb _db;

        public ObservableCollection<Note> Notes { get; set; }

        public NoteManager()
        {
            var projectId = "wessex-apps-ltd";
            _db = DatastoreDb.Create(projectId);
        }

        public List<Note> GetNotes()
        {
            List<Note> notes = new List<Note>();
            Query query = new Query("Note");

            DatastoreQueryResults results = _db.RunQuery(query);

            foreach (var entity in results.Entities)
            {
                Enum.TryParse($"{entity["Category"].StringValue}", out Category categoryName);

                Note note = new Note();
                note.Id = entity.Key.Path[0].Id;
                note.Title = (string) entity["Title"];
                note.Category = categoryName;
                note.NoteBody = (string) entity["NoteBody"];
                notes.Add(note);
            }

            return notes;
        }

        public List<Note> GetNotesForCategory(string category)
        {
            List<Note> notes = new List<Note>();

            Query query = new Query("Note")
            {
                Filter = Filter.Equal("Category", category)
            };

            DatastoreQueryResults results = _db.RunQuery(query);

            foreach (var entity in results.Entities)
            {
                Enum.TryParse($"{entity["Category"].StringValue}", out Category categoryName);

                Note note = new Note();
                note.Id = entity.Key.Path[0].Id;
                note.Title = (string) entity["Title"];
                note.Category = categoryName;
                note.NoteBody = (string) entity["NoteBody"];
                notes.Add(note);
            }

            return notes;
        }

        public Note GetNote(long id)
        {
            KeyFactory keyFactory = _db.CreateKeyFactory("Note");
            var key = keyFactory.CreateKey(id);
            var entity = _db.Lookup(key);

            Enum.TryParse($"{entity["Category"].StringValue}", out Category category);

            var note = new Note()
            {
                Title = (string) entity["Title"],
                Category = category,
                NoteBody = (string) entity["NoteBody"],
                Id = id
            };

            return note;
        }

        public void AddNote(Note note)
        {
            KeyFactory keyFactory = _db.CreateKeyFactory("Note");

            Entity noteEntity = new Entity()
            {
                Key = keyFactory.CreateIncompleteKey(),
                ["Title"] = note.Title,
                ["Category"] = note.Category.ToString(),
                ["NoteBody"] = note.NoteBody
            };

            _db.Insert(noteEntity);
        }

        public void EditNote(Note note)
        {
            KeyFactory keyFactory = _db.CreateKeyFactory("Note");
            var key = keyFactory.CreateKey(note.Id);
            var entity = _db.Lookup(key);

            entity["Title"] = note.Title;
            entity["Category"] = note.Category.ToString();
            entity["NoteBody"] = note.NoteBody;

            _db.Update(entity);
        }

        public void DeleteNote(Note note)
        {
            KeyFactory keyFactory = _db.CreateKeyFactory("Note");
            var key = keyFactory.CreateKey(note.Id);
            var entity = _db.Lookup(key);

            _db.Delete(entity);
        }
    }
}