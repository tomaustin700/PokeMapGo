namespace PokeMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VoteAction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "Action", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Votes", "Action");
        }
    }
}
