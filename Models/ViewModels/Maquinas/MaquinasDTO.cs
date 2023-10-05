using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SportMVC.Models.ViewModels.Maquinas
{
    public class MaquinasDTO
    {
        public int id_Maquina { get; set; }
        [Required]
        public string modelo { get; set; }
        public string marca { get; set; }
        [Required]
        public double precio { get; set; }
        public string url_foto { get; set; }
        [Required]
        public int Categoria_Id { get; set; }
    }
}