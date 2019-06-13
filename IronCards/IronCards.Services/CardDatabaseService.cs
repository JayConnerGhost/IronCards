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
    }
}