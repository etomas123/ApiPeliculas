﻿using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        [Display(Name ="Fecha de Creación")]
        public DateTime FechaCreacion { get; set;}
    }
}
