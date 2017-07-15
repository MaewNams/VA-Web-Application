namespace VA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrator",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        password = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Appointment",
                c => new
                    {
                        id = c.Int(nullable: false),
                        memberId = c.Int(nullable: false),
                        petId = c.Int(nullable: false),
                        serviceId = c.Int(nullable: false),
                        detail = c.String(),
                        suggestion = c.String(),
                        date = c.DateTime(nullable: false),
                        startTime = c.DateTime(nullable: false),
                        endTime = c.DateTime(nullable: false),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Member", t => t.memberId)
                .ForeignKey("dbo.Pet", t => t.petId, cascadeDelete: true)
                .ForeignKey("dbo.VAService", t => t.serviceId)
                .Index(t => t.memberId)
                .Index(t => t.petId)
                .Index(t => t.serviceId);
            
            CreateTable(
                "dbo.AppointmentTimeBlock",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        timeId = c.Int(nullable: false),
                        appointmentID = c.Int(nullable: false),
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
                        id = c.Int(nullable: false),
                        startTime = c.DateTime(nullable: false),
                        endTime = c.DateTime(nullable: false),
                        numberofCase = c.Int(nullable: false),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        id = c.Int(nullable: false),
                        email = c.String(),
                        password = c.String(),
                        name = c.String(),
                        surname = c.String(),
                        address = c.String(),
                        phonenumber = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Pet",
                c => new
                    {
                        id = c.Int(nullable: false),
                        memberId = c.Int(nullable: false),
                        name = c.String(),
                        typeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.PetType", t => t.typeId, cascadeDelete: true)
                .ForeignKey("dbo.Member", t => t.memberId)
                .Index(t => t.memberId)
                .Index(t => t.typeId);
            
            CreateTable(
                "dbo.PetType",
                c => new
                    {
                        id = c.Int(nullable: false),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.VAService",
                c => new
                    {
                        id = c.Int(nullable: false),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Clinic",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        maximumCase = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointment", "serviceId", "dbo.VAService");
            DropForeignKey("dbo.Pet", "memberId", "dbo.Member");
            DropForeignKey("dbo.Pet", "typeId", "dbo.PetType");
            DropForeignKey("dbo.Appointment", "petId", "dbo.Pet");
            DropForeignKey("dbo.Appointment", "memberId", "dbo.Member");
            DropForeignKey("dbo.AppointmentTimeBlock", "appointmentID", "dbo.Appointment");
            DropForeignKey("dbo.AppointmentTimeBlock", "timeId", "dbo.TimeBlock");
            DropIndex("dbo.Pet", new[] { "typeId" });
            DropIndex("dbo.Pet", new[] { "memberId" });
            DropIndex("dbo.AppointmentTimeBlock", new[] { "appointmentID" });
            DropIndex("dbo.AppointmentTimeBlock", new[] { "timeId" });
            DropIndex("dbo.Appointment", new[] { "serviceId" });
            DropIndex("dbo.Appointment", new[] { "petId" });
            DropIndex("dbo.Appointment", new[] { "memberId" });
            DropTable("dbo.Clinic");
            DropTable("dbo.VAService");
            DropTable("dbo.PetType");
            DropTable("dbo.Pet");
            DropTable("dbo.Member");
            DropTable("dbo.TimeBlock");
            DropTable("dbo.AppointmentTimeBlock");
            DropTable("dbo.Appointment");
            DropTable("dbo.Administrator");
        }
    }
}
