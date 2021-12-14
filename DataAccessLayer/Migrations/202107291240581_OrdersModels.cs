namespace DataAccessLayer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrdersModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "oID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "oID");
        }
    }
}
