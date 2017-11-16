namespace VA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
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
                        id = c.Int(nullable: false, identity: true),
                        memberId = c.Int(nullable: false),
                        petId = c.Int(nullable: false),
                        serviceId = c.Int(nullable: false),
                        detail = c.String(),
                        suggestion = c.String(),
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
                "dbo.AppointmentTimeSlot",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        timeId = c.Int(nullable: false),
                        appointmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.TimeSlot", t => t.timeId, cascadeDelete: true)
                .ForeignKey("dbo.Appointment", t => t.appointmentID, cascadeDelete: true)
                .Index(t => t.timeId)
                .Index(t => t.appointmentID);
            
            CreateTable(
                "dbo.TimeSlot",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
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
                        id = c.Int(nullable: false, identity: true),
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
                        id = c.Int(nullable: false, identity: true),
                        memberId = c.Int(nullable: false),
                        name = c.String(),
                        specieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Specie", t => t.specieId, cascadeDelete: true)
                .ForeignKey("dbo.Member", t => t.memberId)
                .Index(t => t.memberId)
                .Index(t => t.specieId);
            
            CreateTable(
                "dbo.Specie",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.VAService",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
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
            DropForeignKey("dbo.Pet", "specieId", "dbo.Specie");
            DropForeignKey("dbo.Appointment", "petId", "dbo.Pet");
            DropForeignKey("dbo.Appointment", "memberId", "dbo.Member");
            DropForeignKey("dbo.AppointmentTimeSlot", "appointmentID", "dbo.Appointment");
            DropForeignKey("dbo.AppointmentTimeSlot", "timeId", "dbo.TimeSlot");
            DropIndex("dbo.Pet", new[] { "specieId" });
            DropIndex("dbo.Pet", new[] { "memberId" });
            DropIndex("dbo.AppointmentTimeSlot", new[] { "appointmentID" });
            DropIndex("dbo.AppointmentTimeSlot", new[] { "timeId" });
            DropIndex("dbo.Appointment", new[] { "serviceId" });
            DropIndex("dbo.Appointment", new[] { "petId" });
            DropIndex("dbo.Appointment", new[] { "memberId" });
            DropTable("dbo.Clinic");
            DropTable("dbo.VAService");
            DropTable("dbo.Specie");
            DropTable("dbo.Pet");
            DropTable("dbo.Member");
            DropTable("dbo.TimeSlot");
            DropTable("dbo.AppointmentTimeSlot");
            DropTable("dbo.Appointment");
            DropTable("dbo.Administrator");
        }
    }
}
