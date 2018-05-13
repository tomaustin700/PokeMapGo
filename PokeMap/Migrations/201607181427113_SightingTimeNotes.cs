namespace PokeMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SightingTimeNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sightings", "TimeOfDay", c => c.Int(nullable: false));
            AddColumn("dbo.Sightings", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sightings", "Notes");
            DropColumn("dbo.Sightings", "TimeOfDay");
        }
    }
}
