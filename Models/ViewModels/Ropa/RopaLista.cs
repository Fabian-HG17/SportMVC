using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportMVC.Models.ViewModels.Ropa
{
    public class RopaLista
    {
        public int id_Ropa { get; set; }
        public string nombre { get; set; }
        public string marca { get; set; }
        public string talla { get; set; }
        public double precio { get; set; }
        public string url_Foto { get; set; }
        public Nullable<int> Categoria_Id { get; set; }
        public string categoria { get; set; }

    }
}