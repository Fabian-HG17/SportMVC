﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class sportBDEntities : DbContext
    {
        public sportBDEntities()
            : base("name=sportBDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Accesorios> Accesorios { get; set; }
        public virtual DbSet<Almacen> Almacen { get; set; }
        public virtual DbSet<Categorias> Categorias { get; set; }
        public virtual DbSet<Maquinas> Maquinas { get; set; }
        public virtual DbSet<Producto_Venta> Producto_Venta { get; set; }
        public virtual DbSet<Productos> Productos { get; set; }
        public virtual DbSet<Ropa> Ropa { get; set; }
        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<Trabajadores> Trabajadores { get; set; }
        public virtual DbSet<Ventas> Ventas { get; set; }
    }
}