namespace IronCards.Services
{
    public interface IDatabaseService
    {
        int Insert(string laneLabel);
        void Update(int targetId, string eNewTitle);
    }
}