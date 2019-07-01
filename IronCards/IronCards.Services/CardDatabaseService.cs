using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using IronCards.Objects;
using LiteDB;

namespace IronCards.Services
{
    public class CardDatabaseService: BaseDatabaseService , ICardDatabaseService
    {
        public CardDatabaseService() : base()
        {

        }
        public int Insert(int parentLaneId, string cardName, string cardDescription, int cardPoints,
            CardTypes parsedCardType)
        {
            int id;
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var cards = database.GetCollection<CardDocument>();
                id = cards.Insert(new CardDocument()
                {
                    
                    ParentLaneId=parentLaneId,
                    CardName=cardName,
                    CardDescription=cardDescription,
                    CardPoints=cardPoints
                });
            }
            return id;
        }

        public List<CardDocument> Get(int laneId)
        {
            var cardDocuments = new List<CardDocument>();
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var cards = database.GetCollection<CardDocument>();
                cards.EnsureIndex("ParentLaneId");
                cardDocuments = cards.Find(x=>x.ParentLaneId == laneId).ToList();
            }

            return cardDocuments;
        }

        public bool Update(CardDocument cardDocument)
        {
            bool result;
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var cards = database.GetCollection<CardDocument>();
                cards.EnsureIndex("Id");
                result=cards.Update(cardDocument.Id, cardDocument);
            }

            return result;
        }

        public bool Delete(int cardId)
        {
            bool result;
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var cards = database.GetCollection<CardDocument>();
                cards.EnsureIndex("Id");
                result = cards.Delete(cardId);
            }

            return result;
        }
    }
}