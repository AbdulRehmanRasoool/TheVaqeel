namespace TheVaqeel.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatusColumnInAppointmentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "Status", c => c.Int(nullable: false,defaultValue:0));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "Status");
        }
    }
}
