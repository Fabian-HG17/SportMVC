using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace SportMVC.Models.ViewModels
{
    public class CategoriasDTO
    {
        public int id_Categoria { get; set; }
        [Required]
        [Display(Name = "Categoria")]
        public string nombre_Categoria { get; set; }
        [Required]
        [Display(Name = "Subcategoria")]
        public string subcategoria { get; set; }
    }
}