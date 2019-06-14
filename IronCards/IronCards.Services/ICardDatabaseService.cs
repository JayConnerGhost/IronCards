using System.Collections.Generic;

namespace IronCards.Services
{
    public interface ICardDatabaseService
    {
        int Insert(int parentLaneId, string cardName, string cardDescription, int cardPoints);
        List<CardDocument> Get(int laneId);
        bool Update(CardDocument cardDocument);
    }
}