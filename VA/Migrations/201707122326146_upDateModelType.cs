namespace VA.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upDateModelType : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pet", "typeId", "dbo.PetType");
            DropPrimaryKey("dbo.PetType");
            AlterColumn("dbo.PetType", "id", c => c.Int(nullable: false, identity: false));
            AddPrimaryKey("dbo.PetType", "id");
            AddForeignKey("dbo.Pet", "typeId", "dbo.PetType", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pet", "typeId", "dbo.PetType");
            DropPrimaryKey("dbo.PetType");
            AlterColumn("dbo.PetType", "id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.PetType", "id");
            AddForeignKey("dbo.Pet", "typeId", "dbo.PetType", "id", cascadeDelete: true);
        }
    }
}
