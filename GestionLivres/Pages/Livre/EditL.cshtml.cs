using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestionLivres.Pages.Livre
{
    public class EditLModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public EditLModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       public LivreInfo LivreInfo = new LivreInfo () ;
        public void OnGet()
        {
            try
            {
                if (int.TryParse(Request.Query["IDLivre"], out int IDLivre))
                {
                    string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string sql = "SELECT * FROM Livre WHERE IDLivre=@IDLivre";
                        using SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@IDLivre", IDLivre);

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                LivreInfo.IDLivre = rd.GetInt32(0);
                                LivreInfo.Titre = rd.GetString(1);
                                LivreInfo.SPM = rd.GetString(2);
                                LivreInfo.DescripLivre = rd.GetString(3);
                                LivreInfo.AnneeEdition = rd.GetInt32(4);

                            }
                        }
                    }
                }
                else
                {
                    // Handle the case where IDLivre is not a valid integer
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
                // Log or handle the exception appropriately
            }
        }

        public void OnPost()
        {
            LivreInfo.IDLivre = Convert.ToInt32(Request.Form["IDLivre"]);
            LivreInfo.Titre = Request.Form["Titre"];
            LivreInfo.SPM = Request.Form["SPM"];
            LivreInfo.DescripLivre = Request.Form["DescripLivre"];
            LivreInfo.AnneeEdition = Convert.ToInt32(Request.Form["AnneeEdition"]);

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string sql = "UPDATE Livre SET Titre=@Titre, SPM=@SPM ,DescripLivre=@DescripLivre ,AnneeEdition=@AnneeEdition  WHERE IDLivre=@IDLivre";
                    using SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@Titre", LivreInfo.Titre);
                    cmd.Parameters.AddWithValue("@SPM", LivreInfo.SPM);
                    cmd.Parameters.AddWithValue("@DescripLivre", LivreInfo.DescripLivre);
                    cmd.Parameters.AddWithValue("@AnneeEdition", LivreInfo.AnneeEdition);
                    cmd.Parameters.AddWithValue("@IDLivre", LivreInfo.IDLivre);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
                // Log or handle the exception appropriately
            }

            Response.Redirect("/Livre/Livre");
        }
    }
}

