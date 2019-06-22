namespace IronCards.Services
{
    public class ProjectDatabaseService : BaseDatabaseService, IProjectDatabaseService
    {
        public ProjectDatabaseService() : base()
        {

        }
        public int New(string projectName)
        {
            int result=0;
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var projects = database.GetCollection<ProjectDocument>();
                result = projects.Insert(new ProjectDocument()
                {
                    Name=projectName
                });
            }

            return result;
        }
    }
}