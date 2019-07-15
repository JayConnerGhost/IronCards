using System.Collections.Generic;
using System.Linq;

namespace IronCards.Services
{
    public class FeatureDatabaseService : BaseDatabaseService, IFeatureDatabaseService
    {
        public List<FeatureDocument> GetAllByProjectId(int projectId)
        {

            var featureDocuments = new List<FeatureDocument>();
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var features = database.GetCollection<FeatureDocument>();
                features.EnsureIndex("ProjectId");
                featureDocuments = features.Find(x => x.ProjectId == projectId).ToList();
            }

            return featureDocuments;
        }

        public int Insert(FeatureDocument featureDocument)
        {
            int featureId = 0;
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var features = database.GetCollection<FeatureDocument>();
                features.EnsureIndex("Id");
                featureId = features.Insert(featureDocument);
            }

            return featureId;
        }
    }
    
}