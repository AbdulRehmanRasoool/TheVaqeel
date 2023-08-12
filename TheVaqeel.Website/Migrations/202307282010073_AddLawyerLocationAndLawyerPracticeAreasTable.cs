namespace TheVaqeel.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLawyerLocationAndLawyerPracticeAreasTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LawyerLocations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LawyerPracticeAreas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.LawyerPracticeAreas");
            DropTable("dbo.LawyerLocations");
        }
    }
}
