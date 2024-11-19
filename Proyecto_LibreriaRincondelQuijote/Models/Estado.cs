using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Proyecto_LibreriaRincondelQuijote.Models
{
    public class Estado
    {
        [Key] // Definir la clave primaria
        public int CodigoEstado { get; set; }

        public string Estado_actual { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}
