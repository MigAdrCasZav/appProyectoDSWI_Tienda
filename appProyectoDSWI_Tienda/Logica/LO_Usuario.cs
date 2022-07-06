using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using appProyectoDSWI_Tienda.Models;

namespace appProyectoDSWI_Tienda.Logica
{
    public class LO_Usuario
    {
        SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["cn"].ConnectionString);

        public Usuario EncontrarUsuario(string email, string clave)
        {
            Usuario objU = new Usuario();
            string query = "SELECT CODIGO_USUAR,NOMBRE_USUAR,APELLI_USUAR,EMAIL_USUAR,CLAVE_USUAR,CODIGO_ROL " +
                           "FROM USUARIO WHERE EMAIL_USUAR = @EMAIL AND CLAVE_USUAR = @CLAVE";
            SqlCommand cmd = new SqlCommand(query, cn);
            cmd.Parameters.AddWithValue("@EMAIL", email);
            cmd.Parameters.AddWithValue("@CLAVE", clave);
            cmd.CommandType = CommandType.Text;
            cn.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    objU = new Usuario()
                    {
                        codUsuario = int.Parse(dr["CODIGO_USUAR"].ToString()),
                        nomUsuario = dr["NOMBRE_USUAR"].ToString(),
                        apeUsuario = dr["APELLI_USUAR"].ToString(),
                        emailUsuario = dr["EMAIL_USUAR"].ToString(),
                        claveUsuario = dr["CLAVE_USUAR"].ToString(),
                        idRol = (Rol)dr["CODIGO_ROL"],
                    };
                }
            }
            return objU;
        }
    }
}