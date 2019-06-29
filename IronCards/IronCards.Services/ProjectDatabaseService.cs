using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using IronCards.Objects;
using LiteDB;

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

        public List<ProjectDocument> GetAll()
        {
            List<ProjectDocument> results;
           using (var database = new LiteDB.LiteDatabase(ConnectionString))
           {
               var projects = database.GetCollection<ProjectDocument>();
               results = projects.FindAll().ToList();
           }

           return results;
        }

        public ProjectDocument Get(int projectId)
        {
            ProjectDocument result;
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var projects = database.GetCollection<ProjectDocument>();
                projects.EnsureIndex("Id");
                result = projects.Find(x=>x.Id==projectId).FirstOrDefault();
            }

            return result;
        }
    }
}