using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestionLivres.Pages.Auteur
{
    public class EditAModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public EditAModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuteurInfo AuteurInfo  = new AuteurInfo();

        public void OnGet()
        {
            try
            {
                if (int.TryParse(Request.Query["IDAuteur"], out int IDAuteur))
                {
                    string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string sql = "SELECT * FROM Auteur WHERE IDAuteur=@IDAuteur";
                        using SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@IDAuteur", IDAuteur);

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                AuteurInfo.IDAuteur = rd.GetInt32(0);
                                AuteurInfo.NomAuteur = rd.GetString(1);
                                AuteurInfo.EmailAuteur = rd.GetString(2);
                                AuteurInfo.telephoneAut = rd.GetString(3);
                                AuteurInfo.AdressAut = rd.GetString(4);

                            }
                        }
                    }
                }
                else
                {
                    // Handle the case where IDAuteur is not a valid integer
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
                // Log or handle the exception appropriately
            }
        }

        public void OnPost()
        {
            AuteurInfo.IDAuteur = Convert.ToInt32(Request.Form["IDAuteur"]);
            AuteurInfo.NomAuteur = Request.Form["NomAuteur"];
            AuteurInfo.EmailAuteur = Request.Form["EmailAuteur"];
            AuteurInfo.telephoneAut = Request.Form["telephoneAut"];
            AuteurInfo.AdressAut = Request.Form["AdressAut"];

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string sql = "UPDATE Auteur SET NomAuteur=@NomAuteur, EmailAuteur=@EmailAuteur ,telephoneAut=@telephoneAut ,AdressAut=@AdressAut  WHERE IDAuteur=@IDAuteur";
                    using SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@NomAuteur", AuteurInfo.NomAuteur);
                    cmd.Parameters.AddWithValue("@EmailAuteur", AuteurInfo.EmailAuteur);
                    cmd.Parameters.AddWithValue("@telephoneAut", AuteurInfo.telephoneAut);
                    cmd.Parameters.AddWithValue("@AdressAut", AuteurInfo.AdressAut);
                    cmd.Parameters.AddWithValue("@IDAuteur", AuteurInfo.IDAuteur);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
                // Log or handle the exception appropriately
            }

            Response.Redirect("/Auteur/Auteur");
        }
    }
}

