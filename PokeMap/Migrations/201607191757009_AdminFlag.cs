namespace PokeMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdminFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "IsAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "IsAdmin");
        }
    }
}
