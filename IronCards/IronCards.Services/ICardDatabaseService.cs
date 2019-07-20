using System.Collections.Generic;
using IronCards.Objects;

namespace IronCards.Services
{
    public interface ICardDatabaseService
    {
        List<CardDocument> Get(int laneId);
        bool Update(CardDocument cardDocument);
        bool Delete(int cardId);
        int Insert(int parentLaneId, string cardName, string cardDescription, int cardPoints, CardTypes parsedCardType, int featureId, string featureName);
    }
}