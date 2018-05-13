namespace PokeMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Voting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        VoteId = c.Int(nullable: false, identity: true),
                        AspNetUserId = c.String(),
                        SightingId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VoteId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Votes");
        }
    }
}
