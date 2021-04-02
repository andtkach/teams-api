namespace Teams.Services.Core.Services.Interfaces
{
    using Core.Configuration;

    public interface IConfigurationService
    {
        DatastoreConfiguration DatastoreSettings();
    }
}
