using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.MultiTenancy;

namespace FileDrop.Domains
{
    public class User : AbpUser<Tenant, User>
    {

    }

    public class Role : AbpRole<Tenant, User>
    {

    }

    public class Tenant : AbpTenant<Tenant, User>
    {

    }
}
