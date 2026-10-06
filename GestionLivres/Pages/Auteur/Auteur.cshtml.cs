using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestionLivres.Pages
{
    public class AuteurModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public AuteurModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<AuteurInfo> listAuteur = new List<AuteurInfo>();

        public void OnGet()
        {

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string sql = "select * from Auteur";
                using SqlCommand cmd = new SqlCommand(sql, con);
                using SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    AuteurInfo Auteurinf = new AuteurInfo();
                    Auteurinf.IDAuteur = rd.GetInt32(0);
                    Auteurinf.NomAuteur = rd.GetString(1);
                    Auteurinf.EmailAuteur = rd.GetString(2);
                    Auteurinf.telephoneAut = rd.GetString(3);
                    Auteurinf.AdressAut = rd.GetString(4);
                    listAuteur.Add(Auteurinf);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
            }
        }
       
    }
    public class AuteurInfo
    {
        public int IDAuteur;
        public string? NomAuteur;
        public string? EmailAuteur;
        public string? telephoneAut;
        public string? AdressAut;

    }
}

