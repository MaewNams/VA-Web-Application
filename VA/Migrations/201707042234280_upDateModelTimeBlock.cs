namespace VA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upDateModelTimeBlock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TimeBlock", "numberofCase", c => c.Int(nullable: false));
            DropColumn("dbo.AppointmentTimeBlock", "amountOfCase");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AppointmentTimeBlock", "amountOfCase", c => c.Int(nullable: false));
            DropColumn("dbo.TimeBlock", "numberofCase");
        }
    }
}
