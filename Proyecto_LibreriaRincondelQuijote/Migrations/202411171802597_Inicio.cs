namespace Proyecto_LibreriaRincondelQuijote.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inicio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarritoProductoes",
                c => new
                    {
                        CodigoCarrito = c.Int(nullable: false),
                        CodigoProducto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CodigoCarrito, t.CodigoProducto })
                .ForeignKey("dbo.Carritoes", t => t.CodigoCarrito)
                .ForeignKey("dbo.Productoes", t => t.CodigoProducto)
                .Index(t => t.CodigoCarrito)
                .Index(t => t.CodigoProducto);
            
            CreateTable(
                "dbo.Carritoes",
                c => new
                    {
                        CodigoCarrito = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        Usuario_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CodigoCarrito)
                .ForeignKey("dbo.AspNetUsers", t => t.Usuario_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.HistorialCompras",
                c => new
                    {
                        CodigoHistorial = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        CodigoProducto = c.Int(nullable: false),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaCompra = c.DateTime(nullable: false),
                        Usuario_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CodigoHistorial)
                .ForeignKey("dbo.Productoes", t => t.CodigoProducto, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Usuario_Id)
                .Index(t => t.CodigoProducto)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        CodigoProducto = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Disponibilidad = c.Int(nullable: false),
                        Imagen1 = c.String(),
                        Imagen2 = c.String(),
                        Imagen3 = c.String(),
                        CodigoEstado = c.Int(nullable: false),
                        CodigoCategoria = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CodigoProducto)
                .ForeignKey("dbo.Categorias", t => t.CodigoCategoria, cascadeDelete: true)
                .ForeignKey("dbo.Estadoes", t => t.CodigoEstado, cascadeDelete: true)
                .Index(t => t.CodigoEstado)
                .Index(t => t.CodigoCategoria);
            
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        CodigoCategoria = c.Int(nullable: false, identity: true),
                        Categoria_ = c.String(),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.CodigoCategoria);
            
            CreateTable(
                "dbo.Estadoes",
                c => new
                    {
                        CodigoEstado = c.Int(nullable: false, identity: true),
                        Estado_actual = c.String(),
                    })
                .PrimaryKey(t => t.CodigoEstado);
            
            CreateTable(
                "dbo.Resenas",
                c => new
                    {
                        CodigoResena = c.Int(nullable: false, identity: true),
                        TextoResena = c.String(),
                        Calificacion = c.Int(nullable: false),
                        UserId = c.String(),
                        CodigoProducto = c.Int(nullable: false),
                        Usuario_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CodigoResena)
                .ForeignKey("dbo.Productoes", t => t.CodigoProducto, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Usuario_Id)
                .Index(t => t.CodigoProducto)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.CarritoProductoes", "CodigoProducto", "dbo.Productoes");
            DropForeignKey("dbo.CarritoProductoes", "CodigoCarrito", "dbo.Carritoes");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.HistorialCompras", "Usuario_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Resenas", "Usuario_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Resenas", "CodigoProducto", "dbo.Productoes");
            DropForeignKey("dbo.HistorialCompras", "CodigoProducto", "dbo.Productoes");
            DropForeignKey("dbo.Productoes", "CodigoEstado", "dbo.Estadoes");
            DropForeignKey("dbo.Productoes", "CodigoCategoria", "dbo.Categorias");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Carritoes", "Usuario_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.Resenas", new[] { "Usuario_Id" });
            DropIndex("dbo.Resenas", new[] { "CodigoProducto" });
            DropIndex("dbo.Productoes", new[] { "CodigoCategoria" });
            DropIndex("dbo.Productoes", new[] { "CodigoEstado" });
            DropIndex("dbo.HistorialCompras", new[] { "Usuario_Id" });
            DropIndex("dbo.HistorialCompras", new[] { "CodigoProducto" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Carritoes", new[] { "Usuario_Id" });
            DropIndex("dbo.CarritoProductoes", new[] { "CodigoProducto" });
            DropIndex("dbo.CarritoProductoes", new[] { "CodigoCarrito" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Resenas");
            DropTable("dbo.Estadoes");
            DropTable("dbo.Categorias");
            DropTable("dbo.Productoes");
            DropTable("dbo.HistorialCompras");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Carritoes");
            DropTable("dbo.CarritoProductoes");
        }
    }
}
