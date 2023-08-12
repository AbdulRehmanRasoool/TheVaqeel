namespace TheVaqeel.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppointmentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Date = c.DateTime(nullable: false),
                        TimeSlotId = c.Int(nullable: false),
                        LawyerDetailId = c.String(),
                        LawyerDetail_LawyerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LawyerDetails", t => t.LawyerDetail_LawyerId)
                .ForeignKey("dbo.TimeSlots", t => t.TimeSlotId, cascadeDelete: true)
                .Index(t => t.TimeSlotId)
                .Index(t => t.LawyerDetail_LawyerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "TimeSlotId", "dbo.TimeSlots");
            DropForeignKey("dbo.Appointments", "LawyerDetail_LawyerId", "dbo.LawyerDetails");
            DropIndex("dbo.Appointments", new[] { "LawyerDetail_LawyerId" });
            DropIndex("dbo.Appointments", new[] { "TimeSlotId" });
            DropTable("dbo.Appointments");
        }
    }
}
