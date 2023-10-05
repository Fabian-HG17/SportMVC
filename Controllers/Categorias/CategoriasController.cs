using SportMVC.Models;
using SportMVC.Models.ViewModels;
using SportMVC.Models.ViewModels.Categorias;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SportMVC.Models.Utilidades;

namespace SportMVC.Controllers.Categorias
{
    public class CategoriasController : Controller
    {
        // GET: Categorias
        public ActionResult Categorias()
        {
            List<CategoriasLista> list = new List<CategoriasLista>();
            using (sportBDEntities db = new sportBDEntities())
            {
                list = (from c in db.Categorias
                        select new CategoriasLista
                        {
                            id_Categoria = c.id_Categoria,
                            nombre_Categoria = c.nombre_Categoria,
                            subcategoria = c.subcategoria
                        }).ToList();
            }
            ViewData["Title"] = "Lista de categorías";
            return View(list);
        }

        public ActionResult AgregarCategoria()
        {
            CargarDDL();
            ViewData["Title"] = "Agregar Categoría";
            return View();
        }
        [HttpPost]
        public ActionResult AgregarCategoria(CategoriasDTO model)
        {
            CargarDDL();
            try
            {
                if (ModelState.IsValid)
                {
                    using (sportBDEntities context = new sportBDEntities())
                    {
                        var categoria = new Models.Categorias();
                        categoria.id_Categoria = model.id_Categoria;
                        categoria.nombre_Categoria = model.nombre_Categoria;
                        if(model.subcategoria == "Seleccione una categoría")
                        {
                            Alert("Seleccione una subcategoría", NotificationType.warning);
                            return Redirect("~/Categorias/AgregarCategoria");
                        }
                        else
                        {
                        categoria.subcategoria = model.subcategoria;
                        }                      
                        
                        //Agregamos nuestro camión al contexto
                        context.Categorias.Add(categoria);
                        //Guardamos camnios en la BD
                        context.SaveChanges();
                        Alert("Todo OK", NotificationType.success);
                        return Redirect("~/Categorias/Categorias");
                    }
                }
                else
                {
                    Alert("Datos no válidos", NotificationType.error);
                    return View(model);
                }


            }
            catch (DbEntityValidationException ex)
            {
                //varaiable para respuesta
                string resp = "";
                //Recorro todos los posibles errores de validacion de la entidad referencial de lña BD
                foreach (var error in ex.EntityValidationErrors)
                {
                    //Recorro los detalles de cada uno de esos posibles errores
                    foreach (var validationerror in error.ValidationErrors)
                    {
                        resp = "Error en la entidad: " + error.Entry.Entity.GetType().Name;
                        resp += "  Propiedad: " + validationerror.PropertyName;
                        resp += " Error: " + validationerror.ErrorMessage;
                    }
                }
                return View(model);
            }
        }

        public ActionResult EditarCategoria(int id)
        {
            CargarDDL();
            Models.Categorias categoria = new Models.Categorias();
            if (id > 0)
            {
                using (sportBDEntities context = new sportBDEntities())
                {
                    categoria = context.Categorias.Where(x => x.id_Categoria == id).FirstOrDefault();
                }
                ViewBag.Title = "Editar categoría no: " + categoria.id_Categoria;
                if (categoria != null)
                {
                    return View(categoria);
                }
                else
                {
                    return Redirect("~/Index");
                }

            }
            else
            {
                return Redirect("~/Index");
            }

        }

        [HttpPost]
        public ActionResult EditarCategoria(Models.Categorias model)
        {
            CargarDDL();
            try
            {
                if (ModelState.IsValid)
                {
                    using (sportBDEntities context = new sportBDEntities())
                    {
                        var categoria = new Models.Categorias();
                        categoria.id_Categoria = model.id_Categoria;
                        categoria.nombre_Categoria = model.nombre_Categoria;
                        if (model.subcategoria == "Seleccione una categoría")
                        {
                            Alert("Seleccione una subcategoría", NotificationType.warning);
                            return Redirect("~/Categorias/EditarCategoria/"+model.id_Categoria);
                        }
                        else
                        {
                            categoria.subcategoria = model.subcategoria;
                        }

                        //Agregamos nuestra categoría al contexto
                        context.Entry(categoria).State = System.Data.Entity.EntityState.Modified;
                        //Guardamos cambios en la BD
                        context.SaveChanges();
                        Alert("Todo OK", NotificationType.success);
                        return Redirect("~/Categorias/Categorias");
                    }
                }
                else
                {
                    Alert("Datos no válidos", NotificationType.error);
                    return View(model);
                }

            }
            catch (DbEntityValidationException ex)
            {
                //varaiable para respuesta
                string resp = "";
                //Recorro todos los posibles errores de validacion de la entidad referencial de lña BD
                foreach (var error in ex.EntityValidationErrors)
                {
                    //Recorro los detalles de cada uno de esos posibles errores
                    foreach (var validationerror in error.ValidationErrors)
                    {
                        resp = "Error en la entidad: " + error.Entry.Entity.GetType().Name;
                        resp += "  Propiedad: " + validationerror.PropertyName;
                        resp += " Error: " + validationerror.ErrorMessage;
                    }
                }
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult EliminarCategoria(int id)
        {
            try
            {
                using (sportBDEntities context = new sportBDEntities())
                {
                    Models.Categorias _categoria = context.Categorias.Where(
                        f => f.id_Categoria == id).FirstOrDefault();
                    if (_categoria != null)
                    {
                        context.Categorias.Remove(_categoria);
                        context.SaveChanges();
                        Alert("Registro Eliminado con éxito", NotificationType.success);
                        return Redirect("~/Categorias/Categorias");
                    }
                    else
                    {
                        Alert("No existe el recurso solicitado", NotificationType.info);
                        return Redirect("~/Categorias/Categorias");
                    }
                }
            }
            catch (Exception e)
            {
                Alert("Error: " + e.Message, NotificationType.error);
                return Redirect("~/Categorias/Categorias");
            }
        }

        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "<script language='javascript'>Swal.fire('" +
                notificationType.ToString().ToUpper() + "', '" + message + "','" +
                notificationType + "')" + "</script>";
            TempData["notification"] = msg;
        }
        public void CargarDDL()
        {
            //ViewBag
            List<SubategoriasDDL> lisCategoria = new List<SubategoriasDDL>();
            lisCategoria.Add(new SubategoriasDDL() { subcategoria = "Seleccione una categoría" });
            lisCategoria.Add(new SubategoriasDDL() { subcategoria = "Ropa" });
            lisCategoria.Add(new SubategoriasDDL() { subcategoria = "Maquinas" });
            lisCategoria.Add(new SubategoriasDDL() { subcategoria = "Accesorios" });

            ViewBag.lisCategoria = lisCategoria;
        }
    }
}