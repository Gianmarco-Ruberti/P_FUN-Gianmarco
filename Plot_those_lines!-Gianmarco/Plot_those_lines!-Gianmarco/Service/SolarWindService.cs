using Plot_those_lines__Gianmarco;
using System.Globalization;
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
            if (root.ValueKind != JsonValueKind.Array) return dataPoints;

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