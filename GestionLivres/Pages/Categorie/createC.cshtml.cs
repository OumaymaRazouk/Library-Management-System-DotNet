using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestionLivres.Pages.Categorie
{
    public class createCModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public createCModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CategorieInfo CategorieInfo = new CategorieInfo();
        public string errorMessage = "";
        public string SuccessMessage = "";

        public void OnGet()
        {

        }
        public void OnPost()
        {
            CategorieInfo.NomCat = Request.Form["NomCat"];
            CategorieInfo.DescriptionCat = Request.Form["DescriptionCat"];
         
            if (CategorieInfo.NomCat.Length == 0 || CategorieInfo.DescriptionCat.Length == 0 )
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
                string sql = "insert into Categorie(NomCat,DescriptionCat)values(@NomCat, @DescriptionCat)";
                using SqlCommand cmd = new SqlCommand(sql, con);
                // cmd.Parameters.AddWithValue("@id",AuteurInfo.id);
                cmd.Parameters.AddWithValue("@NomCat", CategorieInfo.NomCat);
                cmd.Parameters.AddWithValue("@DescriptionCat", CategorieInfo.DescriptionCat);
               
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception" + ex.ToString());
            }
            Response.Redirect("/Categorie/Categorie");

        }
    }
}
