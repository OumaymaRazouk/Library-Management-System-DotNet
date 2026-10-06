using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestionLivres.Pages.Editeur
{
    public class createEModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public createEModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EditeurInfo EditeurInfo = new EditeurInfo();
        public string errorMessage = "";
        public string SuccessMessage = "";

        public void OnGet()
        {
        }
        public void OnPost()
        {
            EditeurInfo.NomEditeur = Request.Form["NomEditeur"];
            EditeurInfo.DescripEditeur = Request.Form["DescripEditeur"];
            EditeurInfo.EmailEdit = Request.Form["EmailEdit"];
            EditeurInfo.telephonrEdi = Request.Form["telephonrEdi"];
            EditeurInfo.AddressEdit = Request.Form["AddressEdit"];

            if (EditeurInfo.NomEditeur.Length == 0 || EditeurInfo.DescripEditeur.Length == 0 || EditeurInfo.telephonrEdi.Length == 0 || EditeurInfo.AddressEdit.Length == 0)
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
                string sql = "insert into Editeur(NomEditeur,DescripEditeur,EmailEdit,telephonrEdi,AddressEdit)values(@NomEditeur, @DescripEditeur,@EmailEdit,@telephonrEdi,@AddressEdit)";
                using SqlCommand cmd = new SqlCommand(sql, con);
                // cmd.Parameters.AddWithValue("@id",AuteurInfo.id);
                cmd.Parameters.AddWithValue("@NomEditeur", EditeurInfo.NomEditeur);
                cmd.Parameters.AddWithValue("@DescripEditeur", EditeurInfo.DescripEditeur);
                cmd.Parameters.AddWithValue("@EmailEdit", EditeurInfo.EmailEdit);
                cmd.Parameters.AddWithValue("@telephonrEdi", EditeurInfo.telephonrEdi);
                cmd.Parameters.AddWithValue("@AddressEdit", EditeurInfo.AddressEdit);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception" + ex.ToString());
            }
            Response.Redirect("/Editeur/Editeur");

        }

    }
}
