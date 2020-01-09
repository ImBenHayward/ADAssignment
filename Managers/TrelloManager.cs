using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;

namespace ADAssignment.Managers
{
    public class TrelloManager
    {
        readonly AzureKeyVaultManager _azureKeyVaultManager = new AzureKeyVaultManager();

        private readonly TrelloFactory _trelloFactory;
        public TrelloAuthorization Auth { get; }

        public TrelloManager()
        {
            Auth = new TrelloAuthorization()
            {
                AppKey = _azureKeyVaultManager.GetSecret(
                    "https://wessex-key-vault.vault.azure.net/secrets/trello-app-key/0e5210f09ee044589b1a34a771f1483d"),
                UserToken = _azureKeyVaultManager.GetSecret(
                    "https://wessex-key-vault.vault.azure.net/secrets/trello-user-token/f7ceb27be47e409bbe1171f6600fc1a9")
            };

            _trelloFactory = new TrelloFactory();
        }

        public async Task<IBoard> GetBoard()
        {
            var board = _trelloFactory.Board("5e0b6ef2a603db30daa1fa4c", Auth);

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