using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportMVC.Models.ViewModels.Maquinas
{
    public class MaquinasLista
    {
        public int id_Maquina { get; set; }
        public string modelo { get; set; }
        public double precio { get; set; }
        public string url_foto { get; set; }
        public Nullable<int> Categoria_Id { get; set; }
        public string categoria { get; set; }
    }
}