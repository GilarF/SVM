using System.Collections.Generic;

namespace ModSettingsTab.Framework.Integration
{
    public class ModIntegrationSettings
    {
        public I18n Description { get; set; } = new I18n();
        public List<Param> Config { get; set; } = new List<Param>();
    }
}