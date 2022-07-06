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
using appProyectoDSWI_Tienda.Permisos;

namespace appProyectoDSWI_Tienda.Controllers
{
    [Authorize]
    [PermisosRol(Models.Rol.Administrador)]
    public class FabricanteController : Controller
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        // GET: Fabricante
        public ActionResult Index()
        {
            return View();
        }

        List<Fabricante> listarFabricantes()
        {
            List<Fabricante> aFabricantes = new List<Fabricante>();
            SqlCommand cmd = new SqlCommand("SP_FABRICANTE", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Fabricante objF = new Fabricante()
                {
                    idFabr = int.Parse(dr[0].ToString()),
                    nomFabr = dr[1].ToString(),
                    paisFabr = dr[2].ToString()
                };
                aFabricantes.Add(objF);
            }
            dr.Close();
            cn.Close();
            return aFabricantes;
        }

        List<Pais> listPais()
        {
            List<Pais> aPaises = new List<Pais>();
            SqlCommand cmd = new SqlCommand("SP_PAIS", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cn.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                Pais objPa = new Pais()
                {
                    idPais = int.Parse(dr[0].ToString()),
                    nomPais = dr[1].ToString()
                };
                aPaises.Add(objPa);
            }
            cn.Close();
            return aPaises;
        }

        public ActionResult ListadoPais()
        {
            return View(listPais());
        }

        public ActionResult listadoFabricantes()
        {
            return View(listarFabricantes());
        }

        public ActionResult nuevoFabricante()
        {
            ViewBag.pais = new SelectList(listPais(), "idPais", "nomPais");
            return View(new FabricanteA());
        }

        [HttpPost]
        public ActionResult nuevoFabricante(FabricanteA objF)
        {
            if (!ModelState.IsValid)
            {
                return View(objF);
            }
            cn.Open();
            try
            {
                SqlCommand cmd = new SqlCommand("SP_REGISTROFABRICANTE", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NOM", objF.nomFabr);
                cmd.Parameters.AddWithValue("@PAI", objF.pais);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ViewBag.mensaje = ex.Message;
            }
            cn.Close();
            return RedirectToAction("listadoFabricantes");
        }

    }
}