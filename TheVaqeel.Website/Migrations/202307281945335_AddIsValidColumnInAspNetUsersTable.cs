namespace TheVaqeel.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsValidColumnInAspNetUsersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsValid", c => c.Byte(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsValid");
        }
    }
}
