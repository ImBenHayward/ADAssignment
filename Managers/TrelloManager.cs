using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Manatee.Trello;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ADAssignment.Helpers
{
    public class TrelloManager
    {
        private readonly TrelloFactory trelloFactory;

        public TrelloManager()
        {
            trelloFactory = new TrelloFactory();
        }

        private readonly TrelloAuthorization _auth = new TrelloAuthorization()
        {
            AppKey = "5b31d6c9fdcc035f3f482f8000e4d6ee",
            UserToken = "3ead3f559527964cc9a7bfcccf25d7a4804a5064fd81b13d46bfd95612a9407e"
        };

        public async Task<IBoard> GetBoard()
        {
            var board = trelloFactory.Board("5e0b6ef2a603db30daa1fa4c", _auth);

            await board.Refresh();

            return board;
        }

        public async Task<List<ICard>> GetCards()
        {
            var board = GetBoard();

            await board.Result.Cards.Refresh();

            var cards = board.Result.Cards.ToList();

            return cards;
        }

        public async Task<ICard> GetCard(string id)
        {
            var board = GetBoard();

            await board.Result.Cards.Refresh();

            var trelloCard = board.Result.Cards.FirstOrDefault(x => x.Id == id);

            return trelloCard;
        }

        public async Task AddCard(string name, string description, DateTime dueDate)
        {
            var board = GetBoard();
            var trelloList = board.Result.Lists.FirstOrDefault(x => x.Name == "To Do");

            if (trelloList != null)
            {
                await trelloList.Cards.Add(name, description, null, dueDate);
            }
        }

        public async Task EditCard(string id, string name, string description, DateTime dueDate)
        {
            //var board = GetBoard();
            var trelloCard = GetCard(id);

            trelloCard.Result.Name = name;
            trelloCard.Result.Description = description;
            trelloCard.Result.DueDate = dueDate;

            await TrelloProcessor.Flush();
        }

        public async Task DeleteCard(string id)
        {
            var trelloCard = GetCard(id);

            await trelloCard.Result.Delete();
        }
    }
}