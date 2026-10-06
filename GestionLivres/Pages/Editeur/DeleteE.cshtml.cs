using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace GestionLivres.Pages.Editeur
{
    public class DeleteEModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public DeleteEModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            try
            {
                int IDEditeur = Convert.ToInt32(Request.Query["IDEditeur"]);
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string sql = "delete from Editeur where IDEditeur=@IDEditeur";
                using SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@IDEditeur", IDEditeur);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
            }

            return RedirectToPage("/Editeur/Editeur");
        }
    }
}
