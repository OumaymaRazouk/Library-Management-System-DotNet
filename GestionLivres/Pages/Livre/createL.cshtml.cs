using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Serilog;
using System.Data;
using System.Runtime;
using System.Dynamic;


namespace GestionLivres.Pages.Livre
{
    public class createLModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public createLModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LivreInfo LivreInfo = new LivreInfo();
        public List<EditeurInfo> Editeur = new List<EditeurInfo>();
        public List<AuteurInfo> Auteur = new List<AuteurInfo>();
        public List<CategorieInfo> Categorie = new List<CategorieInfo>();
        public string ErrorMessage = "";
        public string SuccessMessage = "";

        public void OnGet()
        {
            PopulateDropdowns();
        }

        public void OnPost()
        {
            PopulateDropdowns(); // Repopulate dropdowns to ensure the lists have the latest data

            LivreInfo.Titre = Request.Form["titre"]!;
            LivreInfo.SPM = Request.Form["isbn"]!;

            // Validate and set ID values
            if (int.TryParse(Request.Form["idediteur"], out int idediteur))
            {
                LivreInfo.IDEditeur = idediteur;
            }
            else
            {
                HandleInvalidValue("ID Editeur");
                return;
            }

            if (int.TryParse(Request.Form["idauteur"], out int idauteur))
            {
                LivreInfo.IDAuteur = idauteur;
            }
            else
            {
                HandleInvalidValue("ID Auteur");
                return;
            }

            if (int.TryParse(Request.Form["idcategorie"], out int idcat))
            {
                LivreInfo.IDCat = idcat;
            }
            else
            {
                HandleInvalidValue("ID Categorie");
                return;
            }

            LivreInfo.DescripLivre = Request.Form["description"]!;

            // Validate and set AnneeEdit value
            if (int.TryParse(Request.Form["anneedit"], out int anneeEdit))
            {
                LivreInfo.AnneeEdition = anneeEdit;
            }
            else
            {
                HandleInvalidValue("AnneeEdit");
                return;
            }

            // Validate other required fields
            if (string.IsNullOrWhiteSpace(LivreInfo.Titre) || string.IsNullOrWhiteSpace(LivreInfo.SPM) || string.IsNullOrWhiteSpace(LivreInfo.DescripLivre))
            {
                ErrorMessage = "Tous les champs sont obligatoires";
                return;
            }

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "INSERT INTO Livre (Titre,SPM,IDEditeur,IDAuteur,IDCat,DescripLivre,AnneeEdition) " +
                        "VALUES (@title,@isbn,@idediteur,@idauteur,@idcategorie,@description,@anneedit)";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.Add("@title", SqlDbType.NVarChar).Value = LivreInfo.Titre;
                        cmd.Parameters.Add("@isbn", SqlDbType.NVarChar).Value = LivreInfo.SPM;
                        cmd.Parameters.Add("@idediteur", SqlDbType.NVarChar).Value = LivreInfo.IDEditeur;
                        cmd.Parameters.Add("@idauteur", SqlDbType.NVarChar).Value = LivreInfo.IDAuteur;
                        cmd.Parameters.Add("@idcategorie", SqlDbType.NVarChar).Value = LivreInfo.IDCat;
                        cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = LivreInfo.DescripLivre;
                        cmd.Parameters.Add("@anneedit", SqlDbType.NVarChar).Value = LivreInfo.AnneeEdition;
                        cmd.ExecuteNonQuery();
                    }
                    SuccessMessage = "livre ajout? avec succ?s";
                }
            }
            catch (SqlException ex)
            {
                Log.Error(ex, "SQL Exception occurred");
                ErrorMessage = "Une erreur s'est produite lors de l'ajout de livre.";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception occurred");
                ErrorMessage = "Une erreur inattendue s'est produite.";
            }

            RedirectToPage("/Livre");
        }

        private void HandleInvalidValue(string fieldName)
        {
            LivreInfo.AnneeEdition = 0;
            ErrorMessage = $"Invalid value for {fieldName}";
        }

        private void PopulateDropdowns()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Configurez ConnectionStrings:DefaultConnection dans la configuration locale ou les variables d’environnement.");

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Query to get all Editeurs
                string sqlEditeurs = "SELECT IDEditeur, NomEditeur FROM Editeur";
                using (SqlCommand cmd = new SqlCommand(sqlEditeurs, con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Editeur.Add(new EditeurInfo { IDEditeur = reader.GetInt32(0), NomEditeur = reader.GetString(1) });
                    }
                    reader.Close();
                }

                // Query to get all Auteurs
                string sqlAuteurs = "SELECT IDAuteur, NomAuteur FROM Auteur";
                using (SqlCommand cmd = new SqlCommand(sqlAuteurs, con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Auteur.Add(new AuteurInfo { IDAuteur = reader.GetInt32(0), NomAuteur = reader.GetString(1) });
                    }
                    reader.Close();
                }

                // Query to get all Categories
                string sqlCategories = "SELECT IDCat, NomCat FROM Categorie";
                using (SqlCommand cmd = new SqlCommand(sqlCategories, con))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Categorie.Add(new CategorieInfo { IDCat = reader.GetInt32(0), NomCat = reader.GetString(1) });
                    }
                    reader.Close();
                }
            }
        }
    }
}

