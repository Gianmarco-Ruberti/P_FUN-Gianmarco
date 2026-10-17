# Questions de compréhension – Gianmarco Ruberti

> Pour l'entretien oral. Chaque question porte sur un passage technique du code commité. La section « Éléments attendus » est destinée à l'enseignant : la retirer avant de transmettre le fichier à l'élève.

---

## Question 1 – Configuration des séries avec des délégués

**Passage** : `Plot_those_lines!-Gianmarco/Plot_those_lines!-Gianmarco/Service/GraphicService.cs`, lignes 21-52

```csharp
var seriesConfig = new (string Name, System.Drawing.Color Color, Func<SolarWindPoint, double?> Selector, bool IsVisible)[]
{
    ("Bt", System.Drawing.Color.Black, p => p.Bt, showBt),
    ("By GSE", System.Drawing.Color.Blue, p => p.ByGse, showByGse),
    ...
};

foreach (var config in seriesConfig)
{
    if (!config.IsVisible) continue;
    var validPoints = points
        .Where(p => config.Selector(p).HasValue)
        .Select(p => new { Time = p.TimeTag.ToOADate(), Value = config.Selector(p)!.Value })
        .ToList();
```

- Quel est le type de seriesConfig ?
- Que représente le type `Func<SolarWindPoint, double?>`, et que contient concrètement le champ `Selector` pour la ligne « Bz GSE » ?
- Pourquoi `double?` et non `double` ?

---

## Question 2 – Fusion d'un nouvel import avec les données existantes

**Passage** : `Plot_those_lines!-Gianmarco/Plot_those_lines!-Gianmarco/MainPage.cs`, lignes 82-95

```csharp
var existingDates = new HashSet<DateTime>(_dataPoints.Select(p => p.TimeTag));
int overwrittenCount = newPoints.Count(p => existingDates.Contains(p.TimeTag));

_dataPoints = _dataPoints
    .Concat(newPoints)
    .GroupBy(p => p.TimeTag)
    .Select(g => g.Last())
    .OrderBy(p => p.TimeTag)
    .ToList();
```

- Expliquez pourquoi `g.Last()` garantit que c'est la valeur du **nouveau** fichier qui est conservée en cas de conflit.
