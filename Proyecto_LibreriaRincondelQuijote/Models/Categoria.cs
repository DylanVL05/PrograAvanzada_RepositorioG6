using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Proyecto_LibreriaRincondelQuijote.Models
{
    public class Categoria
    {
        [Key] // Definir la clave primaria
        public int CodigoCategoria { get; set; }

        public string Categoria_ { get; set; }
        public string Descripcion { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}
