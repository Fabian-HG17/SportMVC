using SportMVC.Models;
using SportMVC.Models.ViewModels;
using SportMVC.Models.ViewModels.Maquinas;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SportMVC.Models.Utilidades;

namespace SportMVC.Controllers.Maquinas
{
    public class MaquinasController : Controller
    {
       public ActionResult Maquinas()
        {
           // Crear una lista de Maquinas en función de mi VM
            List<MaquinasLista> list = new List<MaquinasLista>();
            //Llenar mi lista de Maquinas desde el contexto que traje de mi BD
            //Utilizando EF y LinQ
            using (sportBDEntities db = new sportBDEntities())
            {
                list = (from m in db.Maquinas
                        join cat in db.Categorias on m.Categoria_Id equals cat.id_Categoria
                        select new MaquinasLista
                        {
                            id_Maquina = m.id_Maquina,
                            modelo = m.modelo,
                            precio = m.precio,
                            url_foto = m.url_foto,
                            Categoria_Id = m.Categoria_Id,
                            categoria = cat.nombre_Categoria
                        }).ToList();

            }
            ViewData["Title"] = "Lista de máquinas";
            return View(list);
        }

        public ActionResult AgregarMaquina()
        {
            CargarDDL();
            ViewData["Title"] = "Agregar maquinaria";
            return View();
        }
        [HttpPost]
        public ActionResult AgregarMaquina(MaquinasDTO model, HttpPostedFileBase imagen)
        {
            CargarDDL();
            try
            {
                if (ModelState.IsValid)
                {
                    using (sportBDEntities context = new sportBDEntities())
                    {
                        var maquina = new Models.Maquinas();
                        maquina.id_Maquina = model.id_Maquina;
                        maquina.modelo = model.modelo;
                        maquina.marca = model.marca;
                        maquina.precio = model.precio;
                        maquina.modelo = model.modelo;
                        maquina.Categoria_Id = model.Categoria_Id;
                      
                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Multimedia/Maquinas/");
                            if (!Directory.Exists(pathdir))
                            {
                                Directory.CreateDirectory(pathdir);
                            }
                            imagen.SaveAs(pathdir + filename);
                            maquina.url_foto = "/Multimedia/Maquinas/" + filename;
                        }
                        //Agregamos nuestro camión al contexto
                        context.Maquinas.Add(maquina);
                        //Guardamos camnios en la BD
                        context.SaveChanges();
                        Alert("Todo OK", NotificationType.success);
                        return Redirect("~/Maquinas/Maquinas");
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
        public ActionResult EditarMaquina(int id)
        {
            CargarDDL();
            
            Models.Maquinas maquina = new Models.Maquinas();
            if (id > 0)
            {
                using (sportBDEntities context = new sportBDEntities())
                {
                    maquina = context.Maquinas.Where(x => x.id_Maquina == id).FirstOrDefault();
                }
                ViewBag.Title = "Editar Maquina no: " + maquina.id_Maquina;
                if (maquina != null)
                {
                    return View(maquina);
                }
                else
                {
                    Alert("No existe la máquina", NotificationType.error);
                    return Redirect("~/Maquinas/Maquinas");
                }

            }
            else
            {
                Alert("No existe el Id", NotificationType.error);
                return Redirect("~/Maquinas/Maquinas");
            }

        }
        [HttpPost]
        public ActionResult EditarMaquina(Models.Maquinas model, HttpPostedFileBase imagen)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (sportBDEntities context = new sportBDEntities())
                    {
                        var maquina = new Models.Maquinas();
                        maquina.id_Maquina = model.id_Maquina;
                        maquina.modelo = model.modelo;
                        maquina.marca = model.marca;
                        maquina.precio = model.precio;
                        maquina.Categoria_Id = model.Categoria_Id;

                        if (imagen != null && imagen.ContentLength > 0)
                        {
                            string filename = Path.GetFileName(imagen.FileName);
                            string pathdir = Server.MapPath("~/Multimedia/Maquinas/");

                            if (model.url_foto.Contains(filename))
                            {
                                //Es el mismo, no actualizamos
                                maquina.url_foto = model.url_foto;
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
                                    string pathdir_old = Server.MapPath("~" + model.url_foto);
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
                                maquina.url_foto = "/Multimedia/Maquinas/" + filename;

                                if (!Directory.Exists(pathdir))
                                {
                                    Directory.CreateDirectory(pathdir);
                                }
                                imagen.SaveAs(pathdir + filename);
                                maquina.url_foto = "/Multimedia/Maquinas/" + filename;
                            }

                        }
                        else
                        {
                            maquina.url_foto = model.url_foto;
                        }
                        //Agregamos nuestro camión al contexto
                        context.Entry(maquina).State = System.Data.Entity.EntityState.Modified;
                        //Guardamos camnios en la BD
                        context.SaveChanges();
                        Alert("Todo OK", NotificationType.success);
                        return Redirect("~/Maquinas/Maquinas");
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
        public ActionResult EliminarMaquina(int id)
        {
            try
            {
                using (sportBDEntities context = new sportBDEntities())
                {
                    Models.Maquinas _maquina = context.Maquinas.Where(
                        f => f.id_Maquina == id).FirstOrDefault();
                    if (_maquina != null)
                    {
                        context.Maquinas.Remove(_maquina);
                        context.SaveChanges();
                        Alert("Registro Eliminado con éxito", NotificationType.success);
                        return Redirect("~/Maquinas/Maquinas");
                    }
                    else
                    {
                        Alert("No existe el recurso solicitado", NotificationType.info);
                        return Redirect("~/Maquinas/Maquinas");
                    }
                }
            }
            catch (Exception e)
            {
                Alert("Error: " + e.Message, NotificationType.error);
                return Redirect("~/Maquinas/Maquinas");
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