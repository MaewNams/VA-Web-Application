namespace VA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateModelAppTime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppointmentTimeBlock",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        timeId = c.Int(nullable: false),
                        appointmentID = c.Int(nullable: false),
                        amountOfCase = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.TimeBlock", t => t.timeId, cascadeDelete: true)
                .ForeignKey("dbo.Appointment", t => t.appointmentID, cascadeDelete: true)
                .Index(t => t.timeId)
                .Index(t => t.appointmentID);
            
            CreateTable(
                "dbo.TimeBlock",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        startTime = c.DateTime(nullable: false),
                        endTime = c.DateTime(nullable: false),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppointmentTimeBlock", "appointmentID", "dbo.Appointment");
            DropForeignKey("dbo.AppointmentTimeBlock", "timeId", "dbo.TimeBlock");
            DropIndex("dbo.AppointmentTimeBlock", new[] { "appointmentID" });
            DropIndex("dbo.AppointmentTimeBlock", new[] { "timeId" });
            DropTable("dbo.TimeBlock");
            DropTable("dbo.AppointmentTimeBlock");
        }
    }
}
