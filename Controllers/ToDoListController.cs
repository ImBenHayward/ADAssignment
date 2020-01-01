using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ADAssignment.Data;
using ADAssignment.Helpers;
using ADAssignment.Migrations;
using ADAssignment.Models;
using Manatee.Trello;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ADAssignment.Controllers
{
    [Authorize]
    public class ToDoListController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly TrelloManager trelloManager;
        private readonly List<ToDoList> cardList;
        

        public ToDoListController(ApplicationDbContext context)
        {
            //_context = context;
            cardList = new List<ToDoList>();
            trelloManager = new TrelloManager();
        }

        public async Task<IActionResult> Index()
        {
            var cards = await trelloManager.GetCards();

            foreach (var card in cards)
            {
                var model = new Models.ToDoList();

                model.Name = card.Name;
                model.Description = card.Description;
                model.DueDate = card.DueDate;
                model.Url = card.Url;

                cardList.Add(model);
            }

            return View(cardList);
        }

        //public IActionResult Add()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Add(ToDoList toDoList)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //    //    _context.Add(toDoList);
        //    //    _context.SaveChanges();
        //    //    return RedirectToAction(nameof(Index));
        //    //}

        //    return View(toDoList);
        //}

        //public IActionResult Edit(int? id)
        //{
        //    //if (id == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //var toDoTask = _context.ToDoList.Find(id);

        //    //if (toDoTask == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    return View(toDoTask);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(int id, ToDoList toDoList)
        //{
        //    if (id != toDoList.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(toDoList);
        //            _context.SaveChanges();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ToDoTaskExists(toDoList.Id))
        //            {
        //                return NotFound();
        //            }

        //            throw;
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(toDoList);
        //}

        //public IActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var toDoList = _context.ToDoList
        //        .FirstOrDefault(x => x.Id == id);
        //    if (toDoList == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(toDoList);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var toDoListItem = _context.ToDoList.Find(id);
        //    _context.ToDoList.Remove(toDoListItem);
        //    _context.SaveChanges();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool ToDoTaskExists(int id)
        //{
        //    return _context.ToDoList.Any(e => e.Id == id);
        //}
    }
}