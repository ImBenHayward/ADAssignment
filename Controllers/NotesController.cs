using System.Collections.Generic;
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

        public List<Note> GetNotes()
        {
            var notes = _noteManager.GetNotes();

            return notes;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Note note)
        {
            if (!ModelState.IsValid) return View(note);

            _noteManager.AddNote(note);

            return RedirectToAction(nameof(Index));
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

            return RedirectToAction(nameof(Index));
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

            return RedirectToAction(nameof(Index));
        }
    }
}