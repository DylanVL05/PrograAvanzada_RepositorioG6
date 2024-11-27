namespace Proyecto_LibreriaRincondelQuijote.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cantidad_CarritoProducto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarritoProductoes", "Cantidad", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarritoProductoes", "Cantidad");
        }
    }
}
