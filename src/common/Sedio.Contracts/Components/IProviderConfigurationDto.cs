using Newtonsoft.Json.Linq;

namespace Sedio.Contracts.Components
{
    public interface IProviderConfigurationDto
    {
        string     ProviderId { get; set; }

        JObject    ProviderParameters { get; set; }
    }
}