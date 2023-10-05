using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportMVC.Models.ViewModels.Categorias
{
    public class CategoriasLista
    {
        public int id_Categoria { get; set; }
        public string nombre_Categoria { get; set; }
        public string subcategoria { get; set; }
    }
}