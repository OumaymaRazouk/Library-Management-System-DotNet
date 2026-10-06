using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestionLivres.Pages
{
    public class EditeurModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public EditeurModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<EditeurInfo> listEditeur = new List<EditeurInfo>();
        public void OnGet()
        {


            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");
                using SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                string sql = "select * from Editeur";
                using SqlCommand cmd = new SqlCommand(sql, con);
                using SqlDataReader rd = cmd.ExecuteReader();

                while (rd.Read())
                {
                    EditeurInfo Editeurinf = new EditeurInfo();
                    Editeurinf.IDEditeur = rd.GetInt32(0);
                    Editeurinf.NomEditeur = rd.GetString(1);
                    Editeurinf.DescripEditeur = rd.GetString(2);
                    Editeurinf.EmailEdit = rd.GetString(3);
                    Editeurinf.telephonrEdi = rd.GetString(4);
                    Editeurinf.AddressEdit = rd.GetString(5);
                    listEditeur.Add(Editeurinf);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
            }
        }
    }
    public class EditeurInfo
    {
        public int IDEditeur;
        public string? NomEditeur;
        public string? DescripEditeur;
        public string? EmailEdit;
        public string? telephonrEdi;
        public string? AddressEdit;

    }

}
