namespace PokeMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SightingDateTimeNull : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sightings", "TimeAdded", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sightings", "TimeAdded");
        }
    }
}
