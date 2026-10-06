using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace GestionLivres.Pages.Categorie
{
    public class DeleteCModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public DeleteCModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            try
            {
                int IDCat = Convert.ToInt32(Request.Query["IDCat"]);
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string sql = "delete from Categorie where IDCat=@IDCat";
                using SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@IDCat", IDCat);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
            }

            return RedirectToPage("/Categorie/Categorie");
        }
    }
}
