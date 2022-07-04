using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using appProyectoDSWI_Tienda.Models;

namespace appProyectoDSWI_Tienda.Controllers
{
    public class CategoriaController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        // GET: Categoria
        public ActionResult Index()
        {
            return View();
        }

        List<Categoria> listarCategoria()
        {
            List<Categoria> aCategorias = new List<Categoria>();
            SqlCommand cmd = new SqlCommand("SP_CATEGORIA", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Categoria objC = new Categoria()
                {
                    idCateg = int.Parse(dr[0].ToString()),
                    nomCateg = dr[1].ToString(),
                    desCateg = dr[2].ToString()
                };
                aCategorias.Add(objC);
            }
            cn.Close();
            return aCategorias;
        }

        public ActionResult listadoCategorias()
        {
            return View(listarCategoria());
        }

        public ActionResult nuevoCategoria()
        {
            return View(new Categoria());
        }

        [HttpPost]
        public ActionResult nuevoCategoria(Categoria objC)
        {
            if (!ModelState.IsValid)
            {
                return View(objC);
            }
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_REGISTROCATEGORIA", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOM", objC.nomCateg);
                cmd.Parameters.AddWithValue("@DES", objC.desCateg);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            cn.Close();
            return RedirectToAction("listadoCategorias");
        }
    }
}