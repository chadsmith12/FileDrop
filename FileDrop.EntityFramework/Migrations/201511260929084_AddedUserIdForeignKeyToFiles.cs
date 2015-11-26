namespace FileDrop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserIdForeignKeyToFiles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "UserId", c => c.Long(nullable: false));
            CreateIndex("dbo.Files", "UserId");
            AddForeignKey("dbo.Files", "UserId", "dbo.AbpUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "UserId", "dbo.AbpUsers");
            DropIndex("dbo.Files", new[] { "UserId" });
            DropColumn("dbo.Files", "UserId");
        }
    }
}
