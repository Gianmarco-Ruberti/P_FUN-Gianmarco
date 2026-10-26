using Plot_those_lines__Gianmarco;
using ScottPlot;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

public class SolarWindService
{
    // Chemin local de stockage du cache (dans le dossier ApplicationData de l'utilisateur)
    private readonly string _localCachePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "PlotThoseLines",
        "cached_data.json"
    );

    /// <summary>
    /// Sérialise et sauvegarde une liste de points de vent solaire dans le fichier de cache local.
    /// </summary>
    public void SaveToLocalCache(List<SolarWindPoint> dataPoint)
    {
        if (dataPoint == null) return;

        // Récupère le chemin du répertoire du cache
        string directory = Path.GetDirectoryName(_localCachePath)!;

        // Crée le dossier s'il n'existe pas encore sur le disque
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Convertit la liste en JSON formaté (indenté) et l'écrit dans le fichier
        string jsonContent = JsonSerializer.Serialize(dataPoint, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_localCachePath, jsonContent);
    }

    /// <summary>
    /// Charge les données depuis le fichier de cache local, s'il existe.
    /// </summary>
    public List<SolarWindPoint> LoadFromLocalCache()
    {
        // Si le fichier de cache n'existe pas, retourne une liste vide
        if (!File.Exists(_localCachePath)) return new List<SolarWindPoint>();

        try
        {
            // Lit le contenu du cache et le désérialise en liste de points
            string jsonContent = File.ReadAllText(_localCachePath);
            var points = JsonSerializer.Deserialize<List<SolarWindPoint>>(jsonContent);
            return points ?? new List<SolarWindPoint>();
        }
        catch
        {
            // En cas d'erreur de lecture ou de format, retourne une liste vide par sécurité
            return new List<SolarWindPoint>();
        }
    }

    /// <summary>
    /// Charge, filtre et convertit des données du vent solaire depuis un fichier JSON externe.
    /// </summary>
    public List<SolarWindPoint> LoadFromFile(string filePath)
    {
        string jsonContent = File.ReadAllText(filePath);
        List<SolarWindPoint> dataPoints;

        // Analyse le document JSON de manière sécurisée avec JsonDocument (dispose des ressources à la fin)
        using (JsonDocument doc = JsonDocument.Parse(jsonContent))
        {
            JsonElement root = doc.RootElement;

            // 1. Validation de la structure : la racine du JSON doit obligatoirement être un tableau
            if (root.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidDataException("Le fichier JSON doit contenir un tableau d'objets.");
            }

            // Convertit la racine JSON en tableau et applique une série de filtres et de transformations LINQ
            dataPoints = root.EnumerateArray()
                .ToArray()
                .Where(elem =>
                {
                    // Vérifie que l'élément est bien un objet JSON
                    if (elem.ValueKind != JsonValueKind.Object)
                        return false;

                    // Vérifie la présence et le type de la propriété "source", puis filtre sur "SOLAR1" (insensible à la casse)
                    if (elem.TryGetProperty("source", out JsonElement sourceElem) &&
                        sourceElem.ValueKind == JsonValueKind.String)
                    {
                        return string.Equals(sourceElem.GetString(), "SOLAR1", StringComparison.OrdinalIgnoreCase);
                    }

                    return false;
                })
                // Transforme chaque objet JSON filtré en un objet C# de type SolarWindPoint
                .Select(elem =>
                {
                    // Instanciation de l'objet de données
                    SolarWindPoint point = new SolarWindPoint();

                    // Tente de récupérer et de convertir la propriété "time_tag" en objet DateTime UTC de manière sécurisée
                    if (elem.TryGetProperty("time_tag", out JsonElement timeElem) &&
                       timeElem.ValueKind == JsonValueKind.String &&
                       // Analyse suivant un standard international (ignore la langue/paramètre machine) et convertit en UTC
                       DateTime.TryParse(timeElem.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime dt))
                    {
                        point.TimeTag = dt; // Affecte la date convertie si le parsing réussit
                    }

                    // Extrait les différentes mesures du vent solaire à l'aide de la méthode utilitaire sécurisée GetDoubleProperty
                    point.ByGse = GetDoubleProperty(elem, "by_gse");
                    point.BzGse = GetDoubleProperty(elem, "bz_gse");
                    point.ByGsm = GetDoubleProperty(elem, "by_gsm");
                    point.BzGsm = GetDoubleProperty(elem, "bz_gsm");
                    point.Bt = GetDoubleProperty(elem, "bt");

                    return point; // Renvoie l'objet SolarWindPoint complété
                })
                .ToList(); // Convertit le résultat final de la requête LINQ en une liste C# standard
        }

        // 2. Validation du contenu : s'assure qu'au moins un point contient une donnée physique valide
        bool hasValidData = dataPoints.Exists(p => p.Bt.HasValue || p.ByGse.HasValue ||
                                                   p.BzGse.HasValue || p.ByGsm.HasValue ||
                                                   p.BzGsm.HasValue);

        // Si la liste est vide ou ne contient aucune valeur numérique exploitable, on lève une exception
        if (dataPoints.Count == 0 || !hasValidData)
        {
            throw new InvalidDataException("Le fichier ne contient aucune donnée valide du vent solaire pour SOLAR1.");
        }

        return dataPoints;
    }

    /// <summary>
    /// Méthode utilitaire sécurisée pour extraire une valeur numérique décimale (double?) d'un élément JSON.
    /// Gère les types numériques et textuels, ainsi que les valeurs nulles.
    /// </summary>
    private double? GetDoubleProperty(JsonElement element, string propertyName)
    {
        // Si la propriété est absente ou de valeur nulle, retourne null
        if (!element.TryGetProperty(propertyName, out JsonElement propElem) || propElem.ValueKind == JsonValueKind.Null)
            return null;

        // Si c'est directement un nombre JSON, extrait sa valeur en double
        if (propElem.ValueKind == JsonValueKind.Number && propElem.TryGetDouble(out double valNum))
            return valNum;

        // Si c' اله stocké sous forme de chaîne de caractères, tente de la parser en double de manière neutre (InvariantCulture)
        if (propElem.ValueKind == JsonValueKind.String &&
            double.TryParse(propElem.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double valStr))
            return valStr;

        return null;
    }
}