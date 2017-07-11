namespace VA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upDateAppointmentDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointment", "startTime", c => c.DateTime());
            AlterColumn("dbo.Appointment", "endTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointment", "endTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Appointment", "startTime", c => c.DateTime(nullable: false));
        }
    }
}
