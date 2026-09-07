using Plot_those_lines__Gianmarco;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;

public class SolarWindService
{
    public List<SolarWindPoint> LoadFromFile(string filePath)
    {
        string jsonContent = File.ReadAllText(filePath);
        var dataPoints = new List<SolarWindPoint>();

        using (JsonDocument doc = JsonDocument.Parse(jsonContent))
        {
            JsonElement root = doc.RootElement;

            // 1. Validation du type racine (doit être un tableau JSON)
            if (root.ValueKind != JsonValueKind.Array)
            {
                throw new InvalidDataException("Le fichier JSON doit contenir un tableau d'objets.");
            }

            foreach (JsonElement element in root.EnumerateArray())
            {
                if (element.ValueKind != JsonValueKind.Object) continue;

                var point = new SolarWindPoint();

                if (element.TryGetProperty("time_tag", out JsonElement timeElem) &&
                    timeElem.ValueKind == JsonValueKind.String &&
                    DateTime.TryParse(timeElem.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime dt))
                {
                    point.TimeTag = dt;
                }

                point.ByGse = GetDoubleProperty(element, "by_gse");
                point.BzGse = GetDoubleProperty(element, "bz_gse");
                point.ByGsm = GetDoubleProperty(element, "by_gsm");
                point.BzGsm = GetDoubleProperty(element, "bz_gsm");
                point.Bt = GetDoubleProperty(element, "bt");

                dataPoints.Add(point);
            }
        }

        // 2. Validation du contenu : au moins une donnée ou propriété reconnue
        bool hasValidData = dataPoints.Exists(p => p.Bt.HasValue || p.ByGse.HasValue ||
                                                  p.BzGse.HasValue || p.ByGsm.HasValue ||
                                                  p.BzGsm.HasValue);

        if (dataPoints.Count == 0 || !hasValidData)
        {
            throw new InvalidDataException("Le fichier ne contient aucune donnée valide du vent solaire.");
        }

        return dataPoints;
    }

    private double? GetDoubleProperty(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out JsonElement propElem) || propElem.ValueKind == JsonValueKind.Null)
            return null;

        if (propElem.ValueKind == JsonValueKind.Number && propElem.TryGetDouble(out double valNum))
            return valNum;

        if (propElem.ValueKind == JsonValueKind.String &&
            double.TryParse(propElem.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out double valStr))
            return valStr;

        return null;
    }
}