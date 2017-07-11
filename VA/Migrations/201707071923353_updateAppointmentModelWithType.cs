namespace VA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateAppointmentModelWithType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointment", "type", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointment", "type");
        }
    }
}
