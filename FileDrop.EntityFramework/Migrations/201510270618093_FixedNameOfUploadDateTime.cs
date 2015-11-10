namespace FileDrop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedNameOfUploadDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "UploadDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Files", "UploaDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Files", "UploaDateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.Files", "UploadDateTime");
        }
    }
}
