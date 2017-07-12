namespace VA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateClinicModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clinic", "maximumCase", c => c.Int(nullable: false));
            DropColumn("dbo.Clinic", "address");
            DropColumn("dbo.Clinic", "phonenumber");
            DropColumn("dbo.Clinic", "openingDetail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Clinic", "openingDetail", c => c.String());
            AddColumn("dbo.Clinic", "phonenumber", c => c.String());
            AddColumn("dbo.Clinic", "address", c => c.String());
            DropColumn("dbo.Clinic", "maximumCase");
        }
    }
}
