using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Proyecto_LibreriaRincondelQuijote.Models
{
    public class Carrito
    {
        [Key] // Definir la clave primaria
        public int CodigoCarrito { get; set; }

        public string UserId { get; set; }
        public ApplicationUser Usuario { get; set; }

        public ICollection<CarritoProducto> CarritosProductos { get; set; }
    }
}
