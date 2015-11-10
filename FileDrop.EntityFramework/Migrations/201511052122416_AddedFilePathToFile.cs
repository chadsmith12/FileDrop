namespace FileDrop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFilePathToFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "FilePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "FilePath");
        }
    }
}
