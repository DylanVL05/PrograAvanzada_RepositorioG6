using System.ComponentModel.DataAnnotations;

namespace Proyecto_LibreriaRincondelQuijote.Models
{
    public class Resena
    {
        [Key] // Definir la clave primaria
        public int CodigoResena { get; set; }

        public string TextoResena { get; set; }
        public int Calificacion { get; set; }

        public string UserId { get; set; }
        public ApplicationUser Usuario { get; set; }

        public int CodigoProducto { get; set; }
        public Producto Producto { get; set; }
    }
}
