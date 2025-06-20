using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos
{
    public class Pelicula
    {
        [Key]
        public int Id { get; set; }
        public String Nombre { get; set; }
        public string Descripcion { get;  set; }  
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public enum TipoClasificacion { Siete, Trece,Dieciseis, Diechiocho }
        public TipoClasificacion Clasificacion { set; get; }
        public DateTime FechaCreacion { get; set; }


        // Relacion con Categoria

        public int categoriaId { get; set; }
        [ForeignKey("categoriaId")]
        public  Categoria Categoria { get; set; }
    }
}
