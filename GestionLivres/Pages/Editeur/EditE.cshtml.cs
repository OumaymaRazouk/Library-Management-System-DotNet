using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace GestionLivres.Pages.Editeur
{
    public class EditEModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public EditEModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public EditeurInfo EditeurInfo = new EditeurInfo();

        public void OnGet()
        {
            try
            {
                if (int.TryParse(Request.Query["IDEditeur"], out int IDEditeur))
                {
                    string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string sql = "SELECT * FROM Editeur WHERE IDEditeur=@IDEditeur";
                        using SqlCommand cmd = new SqlCommand(sql, con);
                        cmd.Parameters.AddWithValue("@IDEditeur", IDEditeur);

                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                EditeurInfo.IDEditeur = rd.GetInt32(0);
                                EditeurInfo.NomEditeur = rd.GetString(1);
                                EditeurInfo.DescripEditeur = rd.GetString(2);
                                EditeurInfo.EmailEdit = rd.GetString(3);
                                EditeurInfo.telephonrEdi = rd.GetString(4);
                                EditeurInfo.AddressEdit = rd.GetString(5);

                            }
                        }
                    }
                }
                else
                {
                    // Handle the case where IDEditeur is not a valid integer
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
            EditeurInfo.IDEditeur = Convert.ToInt32(Request.Form["IDEditeur"]);
            EditeurInfo.NomEditeur = Request.Form["NomEditeur"];
            EditeurInfo.DescripEditeur = Request.Form["DescripEditeur"];
            EditeurInfo.EmailEdit = Request.Form["EmailEdit"];
            EditeurInfo.telephonrEdi = Request.Form["telephonrEdi"];
            EditeurInfo.AddressEdit = Request.Form["AddressEdit"];

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string sql = "UPDATE Editeur SET NomEditeur=@NomEditeur, DescripEditeur=@DescripEditeur ,EmailEdit=@EmailEdit ,telephonrEdi=@telephonrEdi ,AddressEdit=@AddressEdit WHERE IDEditeur=@IDEditeur";
                    using SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@NomEditeur", EditeurInfo.NomEditeur);
                    cmd.Parameters.AddWithValue("@DescripEditeur", EditeurInfo.DescripEditeur);
                    cmd.Parameters.AddWithValue("@EmailEdit", EditeurInfo.EmailEdit);
                    cmd.Parameters.AddWithValue("@telephonrEdi", EditeurInfo.telephonrEdi);
                    cmd.Parameters.AddWithValue("@AddressEdit", EditeurInfo.AddressEdit);
                    cmd.Parameters.AddWithValue("@IDEditeur", EditeurInfo.IDEditeur);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex.ToString());
                // Log or handle the exception appropriately
            }

            Response.Redirect("/Editeur/Editeur");
        }
    }
    }

