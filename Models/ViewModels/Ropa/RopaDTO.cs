using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportMVC.Models.ViewModels.Ropa
{
    public class RopaDTO
    {
        public int id_Ropa { get; set; }
        [Required]
        public string nombre { get; set; }
        public string marca { get; set; }
        public string talla { get; set; }
        [Required]
        public double precio { get; set; }
        public string url_Foto { get; set; }
        [Required]
        public int Categoria_Id { get; set; }
    }
}