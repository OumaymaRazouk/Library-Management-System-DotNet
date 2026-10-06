using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestionLivres.Pages.Categorie
{
    public class EditCModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public EditCModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CategorieInfo CategorieInfo = new CategorieInfo();
        public void OnGet()
        {
            try
            {
                if (int.TryParse(Request.Query["IDCat"], out int IDCat))
                {
                    string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string sql = "SELECT * FROM Categorie WHERE IDCat=@IDCat";
                        using SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@IDCat", IDCat);

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                CategorieInfo.IDCat = rd.GetInt32(0);
                                CategorieInfo.NomCat = rd.GetString(1);
                                CategorieInfo.DescriptionCat = rd.GetString(2);
                            }
                        }
                    }
                }
                else
                {
                    // Handle the case where IDCat is not a valid integer
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
            CategorieInfo.IDCat = Convert.ToInt32(Request.Form["IDCat"]);
            CategorieInfo.NomCat = Request.Form["NomCat"];
            CategorieInfo.DescriptionCat = Request.Form["DescriptionCat"];

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string sql = "UPDATE Categorie SET NomCat=@NomCat, DescriptionCat=@DescriptionCat WHERE IDCat=@IDCat";
                    using SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@NomCat", CategorieInfo.NomCat);
                    cmd.Parameters.AddWithValue("@DescriptionCat", CategorieInfo.DescriptionCat);
                    cmd.Parameters.AddWithValue("@IDCat", CategorieInfo.IDCat);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
                // Log or handle the exception appropriately
            }

            Response.Redirect("/Categorie/Categorie");
        }
    }
    }

