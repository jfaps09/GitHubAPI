namespace Application
{
    public interface IConfigurationService
    {
        void GetConfiguration();
    }

    public class ConfigurationService : IConfigurationService
    {
        public ConfigurationService()
        {
            // Read config file
        }

        public void GetConfiguration()
        {
        }
    }
}