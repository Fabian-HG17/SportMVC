//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SportMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stock
    {
        public int id_stock { get; set; }
        public int cantidad { get; set; }
        public int Producto_Id { get; set; }
        public int Almacen_Id { get; set; }
    
        public virtual Almacen Almacen { get; set; }
        public virtual Productos Productos { get; set; }
    }
}
