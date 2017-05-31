namespace VA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                        detail = c.String(),
                        suggestion = c.String(),
                        date = c.DateTime(nullable: false),
                        status = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Member", t => t.memberId, cascadeDelete: true)
                .ForeignKey("dbo.Pet", t => t.petId, cascadeDelete: true)
                .Index(t => t.memberId)
                .Index(t => t.petId);
            
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        id = c.Int(nullable: false),
                        codeId = c.String(),
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
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Clinic",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        address = c.String(),
                        phonenumber = c.String(),
                        openingDetail = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pet", "memberId", "dbo.Member");
            DropForeignKey("dbo.Pet", "typeId", "dbo.PetType");
            DropForeignKey("dbo.Appointment", "petId", "dbo.Pet");
            DropForeignKey("dbo.Appointment", "memberId", "dbo.Member");
            DropIndex("dbo.Pet", new[] { "typeId" });
            DropIndex("dbo.Pet", new[] { "memberId" });
            DropIndex("dbo.Appointment", new[] { "petId" });
            DropIndex("dbo.Appointment", new[] { "memberId" });
            DropTable("dbo.Clinic");
            DropTable("dbo.PetType");
            DropTable("dbo.Pet");
            DropTable("dbo.Member");
            DropTable("dbo.Appointment");
            DropTable("dbo.Administrator");
        }
    }
}
