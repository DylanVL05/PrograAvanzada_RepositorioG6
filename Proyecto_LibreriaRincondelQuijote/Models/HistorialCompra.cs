using System;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_LibreriaRincondelQuijote.Models
{
    public class HistorialCompra
    {
        [Key] // Definir la clave primaria
        public int CodigoHistorial { get; set; }

        public string UserId { get; set; }
        public ApplicationUser Usuario { get; set; }

        public int CodigoProducto { get; set; }
        public Producto Producto { get; set; }

        public decimal Precio { get; set; }
        public DateTime FechaCompra { get; set; }


        public int Cantidad { get; set; }


    }
}
