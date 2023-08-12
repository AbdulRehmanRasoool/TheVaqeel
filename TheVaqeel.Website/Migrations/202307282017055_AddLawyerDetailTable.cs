namespace TheVaqeel.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLawyerDetailTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LawyerDetails",
                c => new
                    {
                        LawyerId = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        MobileNumber = c.String(),
                        OfficeAddress = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ExperienceInYearss = c.Byte(nullable: false),
                        LawyerPracticeAreasId = c.Int(nullable: false),
                        LawyerLocationId = c.Int(nullable: false),
                        Description = c.String(),
                        Image = c.String(),
                        IsValid = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.LawyerId)
                .ForeignKey("dbo.LawyerLocations", t => t.LawyerLocationId, cascadeDelete: true)
                .ForeignKey("dbo.LawyerPracticeAreas", t => t.LawyerPracticeAreasId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.LawyerId)
                .Index(t => t.LawyerId)
                .Index(t => t.LawyerPracticeAreasId)
                .Index(t => t.LawyerLocationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LawyerDetails", "LawyerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.LawyerDetails", "LawyerPracticeAreasId", "dbo.LawyerPracticeAreas");
            DropForeignKey("dbo.LawyerDetails", "LawyerLocationId", "dbo.LawyerLocations");
            DropIndex("dbo.LawyerDetails", new[] { "LawyerLocationId" });
            DropIndex("dbo.LawyerDetails", new[] { "LawyerPracticeAreasId" });
            DropIndex("dbo.LawyerDetails", new[] { "LawyerId" });
            DropTable("dbo.LawyerDetails");
        }
    }
}
