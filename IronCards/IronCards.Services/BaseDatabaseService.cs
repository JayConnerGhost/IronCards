namespace IronCards.Services
{
    public class BaseDatabaseService
    {
        public string ConnectionString;

        public BaseDatabaseService()
        {
            var appSettingsReader = new System.Configuration.AppSettingsReader();


            ConnectionString = (string)appSettingsReader.GetValue("ConnectionString", typeof(string));
        }
    }
}