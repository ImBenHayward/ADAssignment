using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ADAssignment.Data;
using ADAssignment.Helpers;
using ADAssignment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ADAssignment.Controllers
{
    [Authorize]
    public class ToDoListController : Controller
    {
        private readonly TrelloManager _trelloManager;
        private readonly List<ToDoList> _cardList;

        public ToDoListController(ApplicationDbContext context)
        {
            _cardList = new List<ToDoList>();
            _trelloManager = new TrelloManager();
        }

        public async Task<IActionResult> Index()
        {
            var cards = await _trelloManager.GetCards();

            foreach (var card in cards)
            {
                var model = new Models.ToDoList();

                model.Id = card.Id;
                model.Name = card.Name;
                model.Description = card.Description;
                if (card.DueDate != null) model.DueDate = (DateTime) card.DueDate;
                model.Url = card.Url;

                _cardList.Add(model);
            }

            return View(_cardList);
        }

        /*
         * Add Methods
         */

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ToDoList toDoList)
        {
            if (!ModelState.IsValid) return View(toDoList);
            await _trelloManager.AddCard(toDoList.Name, toDoList.Description, toDoList.DueDate);
            return RedirectToAction(nameof(Index));
        }

        /*
         * Edit Methods
         */

        public IActionResult Edit(string id)
        {
            var model = new Models.ToDoList();
            var trelloCard = _trelloManager.GetCard(id);

            model.Name = trelloCard.Result.Name;
            model.Description = trelloCard.Result.Description;
            if (trelloCard.Result.DueDate != null) trelloCard.Result.DueDate = (DateTime) trelloCard.Result.DueDate;
            model.Url = trelloCard.Result.Url;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                await _trelloManager.EditCard(toDoList.Id, toDoList.Name, toDoList.Description, toDoList.DueDate);
                return RedirectToAction(nameof(Index));
            }

            return View(toDoList);
        }

        /*
         * Delete Methods
         */

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trelloCard = await _trelloManager.GetCard(id);

            var model = new Models.ToDoList();

            model.Id = trelloCard.Id;
            model.Name = trelloCard.Name;
            model.Description = trelloCard.Description;
            if (trelloCard.DueDate != null) model.DueDate = (DateTime) trelloCard.DueDate;

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(ToDoList toDoList)
        {
            await _trelloManager.DeleteCard(toDoList.Id);

            return RedirectToAction(nameof(Index));
        }
    }
}