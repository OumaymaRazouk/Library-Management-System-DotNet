# GestionLivres — configuration SQL Server

Application ASP.NET Core Razor Pages ciblant .NET 6, avec les CRUD Auteur, Categorie, Editeur et Livre.

## Connexion centralisée

Tous les PageModel reçoivent IConfiguration par leur constructeur et utilisent GetConnectionString("DefaultConnection"). ASP.NET Core fournit IConfiguration automatiquement : aucun service supplémentaire n'est nécessaire. Les vues Razor ne contiennent plus d'accès SQL.

La clé standard est **ConnectionStrings:DefaultConnection**. Sa valeur dans appsettings.json est vide volontairement : aucun serveur personnel ni identifiant réel n'est publié. L'application signale une configuration manquante au démarrage, sans afficher sa valeur.

En développement, Program.cs charge le fichier facultatif appsettings.Local.json. Les variables d'environnement et les arguments de ligne de commande restent prioritaires. En production, le fichier local n'est pas chargé.

## Configuration locale

1. Disposer d'un SDK .NET capable de compiler net6.0 et du runtime ASP.NET Core 6 pour exécuter cette cible, ou planifier séparément une mise à niveau du framework.
2. Disposer de SQL Server et de la base GestionLivres avec ses tables existantes. Aucun script de création de base ou de migration n'est exécuté par ce refactoring.
3. Dans le dossier GestionLivres du projet, copier appsettings.Local.example.json vers appsettings.Local.json si ce dernier n'existe pas déjà.
4. Renseigner l'instance SQL Server locale uniquement dans ce fichier privé. L'exemple utilise localhost et l'authentification Windows : aucun mot de passe SQL n'est nécessaire pour Integrated Security=True, mais le compte Windows exécutant l'application doit disposer des droits sur la base.
5. Lancer depuis la racine de la solution :

```powershell
dotnet build .\GestionLivres.sln
dotnet run --project .\GestionLivres\GestionLivres.csproj --launch-profile GestionLivres
```

Le profil fourni utilise l'environnement Development et l'adresse http://localhost:5085.

Une autre possibilité est de définir la variable d'environnement standard :

```powershell
$env:ConnectionStrings__DefaultConnection = 'Server=localhost;Database=GestionLivres;Integrated Security=True;Encrypt=True;TrustServerCertificate=True'
dotnet run --project .\GestionLivres\GestionLivres.csproj
```

Cette chaîne est un exemple générique. Adapter l'instance localement. TrustServerCertificate=True convient à un certificat local autosigné ; configurer un certificat valide pour un serveur exposé.

## Fichiers privés

Le .gitignore existant exclut appsettings.Local.json, les fichiers locaux par environnement, .env, secrets.json, .vs, bin et obj. Le projet exclut aussi les configurations locales de la copie des résultats de compilation et de publication. Ne jamais ajouter ces fichiers de force ni placer une vraie connexion dans appsettings.json, appsettings.Development.json ou launchSettings.json.

Lors de ce refactoring, la connexion précédemment présente dans le code est transférée vers appsettings.Local.json seulement si ce fichier n'existe pas déjà. Un fichier local existant est conservé. Ce fichier privé doit rester sur le poste du développeur.

## Périmètre fonctionnel

Les requêtes SQL, paramètres, noms des formulaires et redirections des CRUD sont conservés. Les quatre suppressions passent désormais par OnGet dans les PageModel, avec les mêmes paramètres et routes. Leur déclenchement par GET est un comportement préexistant conservé pour ce refactoring ; un passage à POST avec protection antifalsification relève d'un changement distinct.

La compilation ne valide pas la connectivité ni les CRUD sur SQL Server. Une validation manuelle avec la base de développement reste nécessaire. Aucun test ne doit supprimer ou modifier des données réelles sans préparation.

Aucune commande Git ni modification d'historique n'est nécessaire à cette configuration. Les exclusions Git ne retirent pas des informations déjà présentes dans un ancien historique : celui-ci n'a pas été modifié.
