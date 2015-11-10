namespace FileDrop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedIsImageToFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "IsImage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "IsImage");
        }
    }
}
