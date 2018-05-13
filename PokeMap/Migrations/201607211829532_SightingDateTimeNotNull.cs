namespace PokeMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SightingDateTimeNotNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sightings", "TimeAdded", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sightings", "TimeAdded", c => c.DateTime());
        }
    }
}
