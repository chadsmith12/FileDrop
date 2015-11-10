using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Abp.Configuration;

namespace FileDrop.Web.App_Start
{
    public class FileDropSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition("key", "zxcvbgfdsaqwert54321") 
            };
        }
    }
}