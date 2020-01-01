using System;
using System.Collections.Generic;
using System.Linq;
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
        //public async Task<List<ICard>> DisplayCards()
        //{

        //    var board = GetBoard();

        //    //Authenticate();

        //    //var board = new Board("5e0b6ef2a603db30daa1fa4c", auth);
        //    //var card = new Card("5e0bad3a22934f0c715b3fd4", auth);

        //    //await board.Cards.Refresh();
        //    //var cards = board.Cards.ToList();

        //    return cards;
        //}
    }
}
