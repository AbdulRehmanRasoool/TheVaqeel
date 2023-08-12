namespace TheVaqeel.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveExperienceInYearsColumnInLawyerDetailTable : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.LawyerDetails", "ExperienceInYearss");
        }
        
        public override void Down()
        {
            AddColumn("dbo.LawyerDetails", "ExperienceInYearss", c => c.Int(nullable: false));
        }
    }
}
