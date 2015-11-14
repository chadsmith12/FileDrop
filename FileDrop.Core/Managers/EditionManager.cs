using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Editions;
using Abp.Application.Features;
using FileDrop.Domains;

namespace FileDrop.Managers
{
    public class FeatureValueStore : AbpFeatureValueStore<Tenant, Role, User>
    {
        public FeatureValueStore(TenantManager tenantManager)
            : base(tenantManager)
        {

        }
    }

    public class EditionManager : AbpEditionManager
    {
    }
}
