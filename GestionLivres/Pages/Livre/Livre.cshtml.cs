using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace GestionLivres.Pages
{
    public class LivreModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public LivreModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<LivreInfo> listLivre = new List<LivreInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string sql = "SELECT * FROM Livre";
                using SqlCommand cmd = new SqlCommand(sql, con);
                using SqlDataReader rd = cmd.ExecuteReader();
            
            while (rd.Read())
            {
                LivreInfo Livreinf = new LivreInfo();
                Livreinf.IDLivre = rd.GetInt32(0);
                Livreinf.Titre = rd.GetString(1);
                Livreinf.SPM = rd.GetString(2);
                Livreinf.IDEditeur = rd.GetInt32(3);
                Livreinf.IDAuteur = rd.GetInt32(4);
                Livreinf.IDCat = rd.GetInt32(5);
                Livreinf.DescripLivre = rd.GetString(6);
                Livreinf.AnneeEdition = rd.GetInt32(7);
                listLivre.Add(Livreinf);
            }
        }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
                }
                }
                }
                public class LivreInfo
    {
                        public int? IDLivre { get; set; }
                        public string? Titre { get; set; }
                       public string? SPM { get; set; }
                       public int? IDEditeur { get; set; }
                       public int? IDAuteur { get; set; }
                       public int? IDCat { get; set; }
                       public string? DescripLivre { get; set; }
                       public int? AnneeEdition { get; set; }

    }

    }

