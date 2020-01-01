using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADAssignment.Helpers;
using Manatee.Trello;
using Microsoft.AspNetCore.Mvc;
using ADAssignment.Models;
using Microsoft.EntityFrameworkCore;

//namespace ADAssignment.Controllers
//{
//    public class TrelloController : Controller
//    {

//        TrelloManager trelloManager = new TrelloManager();

//        public async Task<IActionResult> GetCards()
//        {
//            List<ToDoList> trelloList = new List<ToDoList>();

//            var model = new Models.ToDoList();
//            var cards = await trelloManager.GetCards();

//            cards.ForEach(x => x.Name = model.Name);
//            cards.ForEach(x => x.Description = model.Description);
//            cards.ForEach(x => x.DueDate = model.DueDate);
//            cards.ForEach(x => );

//            trelloList.Add(model);

//            return View("~/Views/ToDoList/Index.cshtml", trelloList);
//        }

//    }
//}