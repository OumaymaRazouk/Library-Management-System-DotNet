using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestionLivres.Pages.Shared
{
    public class createModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public createModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuteurInfo AuteurInfo = new AuteurInfo();
        public string errorMessage = "";
        public string SuccessMessage = "";

        public void OnGet()
        {
            
            }
        public void OnPost()
        {
            AuteurInfo.NomAuteur = Request.Form["NomAuteur"];
            AuteurInfo.EmailAuteur = Request.Form["EmailAuteur"];
            AuteurInfo.telephoneAut = Request.Form["telephoneAut"];
            AuteurInfo.AdressAut = Request.Form["AdressAut"];
            if (AuteurInfo.NomAuteur.Length == 0 || AuteurInfo.EmailAuteur.Length == 0 || AuteurInfo.telephoneAut.Length == 0 || AuteurInfo.AdressAut.Length == 0)
            {
                errorMessage = "tous les champs sont obligatoires";
                return;
            }
           
                try
                {
                    string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");
                    using SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    string sql = "insert into Auteur(NomAuteur,EmailAuteur,telephoneAut,AdressAut)values(@NomAuteur, @EmailAuteur, @telephoneAut, @AdressAut)";
                    using SqlCommand cmd = new SqlCommand(sql, con);
                    // cmd.Parameters.AddWithValue("@id",AuteurInfo.id);
                    cmd.Parameters.AddWithValue("@NomAuteur", AuteurInfo.NomAuteur);
                    cmd.Parameters.AddWithValue("@EmailAuteur", AuteurInfo.EmailAuteur);
                    cmd.Parameters.AddWithValue("@telephoneAut", AuteurInfo.telephoneAut);
                    cmd.Parameters.AddWithValue("@AdressAut", AuteurInfo.AdressAut);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception" + ex.ToString());
                }
                Response.Redirect("/Auteur/Auteur");


        }

    }
}
