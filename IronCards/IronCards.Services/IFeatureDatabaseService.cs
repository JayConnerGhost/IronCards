using System.Collections.Generic;

namespace IronCards.Services
{
    public interface IFeatureDatabaseService
    {
        List<FeatureDocument> GetAllByProjectId(int projectId);
    }
}