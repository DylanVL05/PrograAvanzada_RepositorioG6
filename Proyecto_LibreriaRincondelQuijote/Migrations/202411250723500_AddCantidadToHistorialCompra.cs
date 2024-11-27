namespace Proyecto_LibreriaRincondelQuijote.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCantidadToHistorialCompra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HistorialCompras", "Cantidad", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HistorialCompras", "Cantidad");
        }
    }
}
