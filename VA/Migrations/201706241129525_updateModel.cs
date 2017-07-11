namespace VA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Appointment", "memberId", "dbo.Member");
            CreateTable(
                "dbo.VAService",
                c => new
                    {
                        id = c.Int(nullable: false),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            AddColumn("dbo.Appointment", "serviceId", c => c.Int(nullable: false));
            AddColumn("dbo.Appointment", "startTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Appointment", "endTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Member", "email", c => c.String());
            CreateIndex("dbo.Appointment", "serviceId");
            AddForeignKey("dbo.Appointment", "serviceId", "dbo.VAService", "id");
            AddForeignKey("dbo.Appointment", "memberId", "dbo.Member", "id");
            DropColumn("dbo.Member", "codeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Member", "codeId", c => c.String());
            DropForeignKey("dbo.Appointment", "memberId", "dbo.Member");
            DropForeignKey("dbo.Appointment", "serviceId", "dbo.VAService");
            DropIndex("dbo.Appointment", new[] { "serviceId" });
            DropColumn("dbo.Member", "email");
            DropColumn("dbo.Appointment", "endTime");
            DropColumn("dbo.Appointment", "startTime");
            DropColumn("dbo.Appointment", "serviceId");
            DropTable("dbo.VAService");
            AddForeignKey("dbo.Appointment", "memberId", "dbo.Member", "id", cascadeDelete: true);
        }
    }
}
