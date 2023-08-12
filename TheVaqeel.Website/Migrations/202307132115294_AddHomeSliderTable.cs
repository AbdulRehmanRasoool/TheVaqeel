namespace TheVaqeel.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddHomeSliderTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HomeSliders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HomeSliders");
        }
    }
}
