namespace TheVaqeel.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocationColumnInHomeNavbarTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HomeNavbars", "Location", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HomeNavbars", "Location");
        }
    }
}
