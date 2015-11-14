using System.Data.Entity.Migrations;
using EntityFramework.DynamicFilters;

namespace FileDrop.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<FileDrop.EntityFramework.FileDropDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "FileDrop";
        }

        protected override void Seed(FileDrop.EntityFramework.FileDropDbContext context)
        {
            context.DisableAllFilters();
            new DefaultTenantRoleAndUserBuilder(context).Build();
        }
    }
}
