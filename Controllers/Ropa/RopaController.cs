using SportMVC.Models.ViewModels;
using SportMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SportMVC.Models.Utilidades;
using SportMVC.Models.ViewModels.Ropa;

namespace SportMVC.Controllers.Ropa
{
    public class RopaController : Controller
    {
        public ActionResult Ropa()
        {
            
            List<RopaLista> list = new List<RopaLista>();
            
            using (sportBDEntities db = new sportBDEntities())
            {
                list = (from m in db.Ropa
                        join cat in db.Categorias on m.Categoria_Id equals cat.id_Categoria
                        select new RopaLista
                        {
                            id_Ropa = m.id_Ropa,
                            nombre = m.nombre,
                            marca = m.marca,
                            talla = m.talla,
                            precio = m.precio,
                            url_Foto = m.url_Foto,
                            Categoria_Id = m.Categoria_Id,
                            categoria = cat.nombre_Categoria
                        }).ToList();

            }
            ViewData["Title"] = "Lista de Ropa";
            return View(list);
        }

        public ActionResult AgregarRopa()
        {
            CargarDDL();
            ViewData["Title"] = "Agregar prenda";
            return View();
        }
        [HttpPost]
        public ActionResult AgregarRopa(RopaDTO model, HttpPostedFileBase imagen)
        {
            CargarDDL();
            try
            {
                if (ModelState.IsValid)
                {
                    using (sportBDEntities context = new sportBDEntities())
                    {
                        var ropa = new Models.Ropa();
                        ropa.id_Ropa = model.id_Ropa;
                        ropa.nombre = model.nombre;
                        ropa.marca = model.marca;
                        ropa.talla = model.talla;
                        ropa.precio = model.precio;
                        ropa.Categoria_Id = model.Categoria_Id;

                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Multimedia/Ropa/");
                            if (!Directory.Exists(pathdir))
                            {
                                Directory.CreateDirectory(pathdir);
                            }
                            imagen.SaveAs(pathdir + filename);
                            ropa.url_Foto = "/Multimedia/Ropa/" + filename;
                        }
                        //Agregamos nuestro camión al contexto
                        context.Ropa.Add(ropa);
                        //Guardamos camnios en la BD
                        context.SaveChanges();
                        Alert("Todo OK", NotificationType.success);
                        return Redirect("~/Ropa/Ropa");
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
        public ActionResult EditarRopa(int id)
        {
            CargarDDL();

            Models.Ropa ropa = new Models.Ropa();
            if (id > 0)
            {
                using (sportBDEntities context = new sportBDEntities())
                {
                    ropa = context.Ropa.Where(x => x.id_Ropa == id).FirstOrDefault();
                }
                ViewBag.Title = "Editar Prenda no: " + ropa.id_Ropa;
                if (ropa != null)
                {
                    return View(ropa);
                }
                else
                {
                    Alert("No existe la prenda", NotificationType.error);
                    return Redirect("~/Ropa/Ropa");
                }

            }
            else
            {
                Alert("No existe el Id", NotificationType.error);
                return Redirect("~/Ropa/Ropa");
            }

        }
        [HttpPost]
        public ActionResult EditarRopa(Models.Ropa model, HttpPostedFileBase imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (sportBDEntities context = new sportBDEntities())
                    {
                        var ropa = new Models.Ropa();
                        ropa.id_Ropa = model.id_Ropa;
                        ropa.nombre = model.nombre;
                        ropa.marca = model.marca;
                        ropa.talla = model.talla;
                        ropa.precio = model.precio;
                        ropa.Categoria_Id = model.Categoria_Id;

                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Multimedia/Ropa/");

                            if (model.url_Foto.Contains(filename))
                            {
                                //Es el mismo, no actualizamos
                                ropa.url_Foto = model.url_Foto;
                            }
                            else
                            {
                                //Es nuevo entonces actualizamos
                                if (!Directory.Exists(pathdir))
                                {
                                    Directory.CreateDirectory(pathdir);
                                }
                                //Borrar Imagen anterior 
                                //Validamos si existe
                                try
                                {
                                    string pathdir_old = Server.MapPath("~" + model.url_Foto);
                                    if (System.IO.File.Exists(pathdir_old))
                                    {
                                        //Si existe la borro
                                        System.IO.File.Delete(pathdir_old);
                                    }
                                }
                                catch (Exception e)
                                {

                                    Alert("Error: " + e.Message, NotificationType.error);
                                }
                                imagen.SaveAs(pathdir + filename);
                                ropa.url_Foto = "/Multimedia/Ropa/" + filename;

                                if (!Directory.Exists(pathdir))
                                {
                                    Directory.CreateDirectory(pathdir);
                                }
                                imagen.SaveAs(pathdir + filename);
                                ropa.url_Foto = "/Multimedia/Ropa/" + filename;
                            }

                        }
                        else
                        {
                            ropa.url_Foto = model.url_Foto;
                        }
                        //Agregamos nuestro camión al contexto
                        context.Entry(ropa).State = System.Data.Entity.EntityState.Modified;
                        //Guardamos camnios en la BD
                        context.SaveChanges();
                        Alert("Todo OK", NotificationType.success);
                        return Redirect("~/Ropa/Ropa");
                    }
                }
                else
                {
                    Alert("Datos no válidos", NotificationType.error);
                    CargarDDL();
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
                CargarDDL();
                return View(model);
            }
        }
        [HttpGet]
        public ActionResult EliminarRopa(int id)
        {
            try
            {
                using (sportBDEntities context = new sportBDEntities())
                {
                    Models.Ropa _ropa = context.Ropa.Where(
                        f => f.id_Ropa == id).FirstOrDefault();
                    if (_ropa != null)
                    {
                        context.Ropa.Remove(_ropa);
                        context.SaveChanges();
                        Alert("Registro Eliminado con éxito", NotificationType.success);
                        return Redirect("~/Ropa/Ropa");
                    }
                    else
                    {
                        Alert("No existe el recurso solicitado", NotificationType.info);
                        return Redirect("~/Ropa/Ropa");
                    }
                }
            }
            catch (Exception e)
            {
                Alert("Error: " + e.Message, NotificationType.error);
                return Redirect("~/Ropa/Ropa");
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
            List<CategoriasDDL> lisCategorias = new List<CategoriasDDL>();
            lisCategorias.Insert(0, new CategoriasDDL { id_Categoria = 0, nombre_Categoria = "Seleccione una categoría" });
            using (sportBDEntities db = new sportBDEntities())
            {
                foreach (var cat in db.Categorias)
                {
                    CategoriasDDL _aux = new CategoriasDDL();
                    _aux.id_Categoria = cat.id_Categoria;
                    _aux.nombre_Categoria = cat.nombre_Categoria;
                    lisCategorias.Add(_aux);
                }
            }
            ViewBag.ListaCategorias = lisCategorias;
        }
    }
}