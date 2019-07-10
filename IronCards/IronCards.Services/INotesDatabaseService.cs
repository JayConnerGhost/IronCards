namespace IronCards.Services
{
    public interface INotesDatabaseService
    {
        int Insert(string title, string description, int projectId);
    }
}