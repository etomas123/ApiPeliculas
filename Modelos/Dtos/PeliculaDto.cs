﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos.Dtos
{
    public class PeliculaDto
    {

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Diechiocho }
        public TipoClasificacion Clasificacion { set; get; }
        public DateTime FechaCreacion { get; set; }

        public int categoriaId { get; set; }
     
    }
}
