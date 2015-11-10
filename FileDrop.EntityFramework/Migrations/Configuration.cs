using System.Data.Entity.Migrations;

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
            // This method will be called every time after migrating to the latest version.
            // You can add any seed data here...
        }
    }
}
