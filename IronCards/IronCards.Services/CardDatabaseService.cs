using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using LiteDB;

namespace IronCards.Services
{
    public class CardDatabaseService : ICardDatabaseService
    {
        public int Insert(int parentLaneId, string cardName, string cardDescription, int cardPoints)
        {
            int id;
            using (var database = new LiteDB.LiteDatabase("Lanes.db"))
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
            using (var database = new LiteDB.LiteDatabase("Lanes.db"))
            {
                var cards = database.GetCollection<CardDocument>();
                cards.EnsureIndex("ParentLaneId");
                cardDocuments = cards.Find(x=>x.ParentLaneId == laneId).ToList();
            }

            return cardDocuments;
        }
     }
}