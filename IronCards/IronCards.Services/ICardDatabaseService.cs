namespace IronCards.Services
{
    public interface ICardDatabaseService
    {
        int Insert(int parentLaneId, string cardName, string cardDescription, int cardPoints);
    }
}