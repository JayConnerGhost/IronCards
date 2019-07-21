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
        private readonly IFeatureDatabaseService _featureDatabaseService;

        public CardDatabaseService(IFeatureDatabaseService featureDatabaseService ) : base()
        {
            _featureDatabaseService = featureDatabaseService;
        }
        public int Insert(int parentLaneId, string cardName, string cardDescription, int cardPoints,
            CardTypes parsedCardType,int featureId, string featureName)
        {

            if (!CheckFeatureIsInDatabase(featureId))
            {
                throw new FeatureIsNotRegisteredException(featureName);
            }
            //TODO: next refactoring spot for feature work 
            int id;
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var cards = database.GetCollection<CardDocument>();
                id = cards.Insert(new CardDocument()
                {
                    FeatureId=featureId,
                    ParentLaneId=parentLaneId,
                    CardName=cardName,
                    CardDescription=cardDescription,
                    CardPoints=cardPoints,
                    CardType=parsedCardType.ToString()
                });
            }
            return id;
        }

        private bool CheckFeatureIsInDatabase(int featureId)
        {
            return _featureDatabaseService.Find(featureId) != null;
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