using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Proyecto_LibreriaRincondelQuijote.Models
{
    public class Producto
    {
        [Key] // Definir la clave primaria
        public int CodigoProducto { get; set; }

        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Disponibilidad { get; set; }
        public string Imagen1 { get; set; }
        public string Imagen2 { get; set; }
        public string Imagen3 { get; set; }

        public int CodigoEstado { get; set; }
        public Estado Estado { get; set; }

        public int CodigoCategoria { get; set; }
        public Categoria Categoria { get; set; }

        public ICollection<CarritoProducto> CarritosProductos { get; set; }
        public ICollection<HistorialCompra> HistorialCompras { get; set; }
        public ICollection<Resena> Resenas { get; set; }
    }
}
