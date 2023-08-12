namespace TheVaqeel.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTypeOfExperienceInYears : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LawyerDetails", "ExperienceInYearss", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LawyerDetails", "ExperienceInYearss", c => c.Byte(nullable: false));
        }
    }
}
