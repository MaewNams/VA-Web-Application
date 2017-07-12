namespace VA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Appointment", "type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Appointment", "type", c => c.String());
        }
    }
}
