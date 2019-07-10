namespace IronCards.Services
{
    public class NoteDatabaseService:BaseDatabaseService,INotesDatabaseService
    {
        public int Insert(string title, string description, int projectId)
        {
            int id;
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var notes = database.GetCollection<NoteDocument>();
                id = notes.Insert(new NoteDocument()
                {
                    Title=title,
                    Text=description,
                    ProjectId=projectId
                });
            }
            return id;
        }
    }
}