namespace MyTinyCollege.Migrations.CollegeMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "FirstName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Person", "FirstMidName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "FirstMidName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Person", "FirstName");
        }
    }
}
