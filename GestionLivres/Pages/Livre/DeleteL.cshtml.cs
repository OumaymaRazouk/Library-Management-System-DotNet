using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace GestionLivres.Pages.Livre
{
    public class DeleteLModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public DeleteLModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            try
            {
                int IDLivre = Convert.ToInt32(Request.Query["IDLivre"]);
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string sql = "delete from Livre where IDLivre=@IDLivre";
                using SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@IDLivre", IDLivre);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
            }

            return RedirectToPage("/Livre/Livre");
        }
    }
}
