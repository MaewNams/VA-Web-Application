namespace VA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteDateonModelApp2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Appointment", "date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointment", "date", c => c.DateTime(nullable: false));
        }
    }
}
