using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestionLivres.Pages
{
    public class CategorieModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CategorieModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<CategorieInfo> listCategorie = new List<CategorieInfo>();

        public void OnGet()
        {

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string sql = "select * from Categorie";
                using SqlCommand cmd = new SqlCommand(sql, con);
                using SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    CategorieInfo Categorieinf = new CategorieInfo();
                    Categorieinf.IDCat = rd.GetInt32(0);
                    Categorieinf.NomCat = rd.GetString(1);
                    Categorieinf.DescriptionCat = rd.GetString(2);
                    
                    listCategorie.Add(Categorieinf);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
            }
        }
    }
    public class CategorieInfo
    {
        public int IDCat;
        public string? NomCat;
        public string? DescriptionCat;
        

    }
}

