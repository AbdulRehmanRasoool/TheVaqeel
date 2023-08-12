namespace TheVaqeel.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDateTimeInTimeSlotTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TimeSlots", "StartTime", c => c.String());
            AlterColumn("dbo.TimeSlots", "EndTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TimeSlots", "EndTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.TimeSlots", "StartTime", c => c.DateTime(nullable: false));
        }
    }
}
