using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Proyecto_LibreriaRincondelQuijote.Models


{
    public class CarritoProducto
    {
        
        public int CodigoCarrito { get; set; }
        public Carrito Carrito { get; set; }

        public int CodigoProducto { get; set; }
        public Producto Producto { get; set; }


    }
}
