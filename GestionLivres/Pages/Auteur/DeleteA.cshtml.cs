using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace GestionLivres.Pages.Auteur
{
    public class DeleteAModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public DeleteAModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            try
            {
                int IDAuteur = Convert.ToInt32(Request.Query["IDAuteur"]);
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string sql = "delete from Auteur where IDAuteur=@IDAuteur";
                using SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@IDAuteur", IDAuteur);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
            }

            return RedirectToPage("/Auteur/Auteur");
        }
    }
}
