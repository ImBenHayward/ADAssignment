using System;
using System.Collections.Generic;
using System.Xml.Schema;
using ADAssignment.Managers;
using ADAssignment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADAssignment.Controllers
{
    [Authorize]
    public class NotesController : Controller
    {
        readonly NoteManager _noteManager = new NoteManager();

        public IActionResult Index()
        {
            var notes = GetNotes();

            return View(notes);
        }

        public IActionResult Category(string category)
        {
            var notes = _noteManager.GetNotesForCategory(category);

            return View(notes);
        }

        public List<Note> GetNotes()
        {
            var notes = _noteManager.GetNotes();

            return notes;
        }

        public IActionResult ViewNote(long id)
        {
            var note = _noteManager.GetNote(id);
            return View(note);
        }

        public IActionResult Add(Category? category)
        {
            Note note = new Note();

            note.Category = category;

            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Note note)
        {
            if (!ModelState.IsValid) return View(note);

            _noteManager.AddNote(note);

            var noteCategory = note.Category;

            return RedirectToAction("Category", new { category = noteCategory });
        }

        public IActionResult Edit(long id)
        {
            var note = _noteManager.GetNote(id);
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Note note)
        {
            if (!ModelState.IsValid) return View(note);

            _noteManager.EditNote(note);

            return View("ViewNote", note);
        }

        public IActionResult Delete(long id)
        {
            var note = _noteManager.GetNote(id);

            return View(note);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Note note)
        {
            _noteManager.DeleteNote(note);

            var noteCategory = note.Category.ToString();

            return RedirectToAction("Category", new { category = noteCategory });
        }
    }
}