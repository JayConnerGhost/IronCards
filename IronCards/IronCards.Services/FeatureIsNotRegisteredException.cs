using System;

namespace IronCards.Services
{
    public class FeatureIsNotRegisteredException : Exception
    {
        public FeatureIsNotRegisteredException(string featureName):base($"This feature is not known to the system {featureName}")
        {
        }
    }
}