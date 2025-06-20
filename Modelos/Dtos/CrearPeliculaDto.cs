namespace ApiPeliculas.Modelos.Dtos
{
    public class CrearPeliculaDto
    {

        public String Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public enum CrearTipoClasificacion { Siete, Trece, Dieciseis, Diechiocho }
        public CrearTipoClasificacion Clasificacion { set; get; }
        
        public int categoriaId { get; set; }

    }
}
