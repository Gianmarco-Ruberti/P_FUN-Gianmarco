using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot;
using ScottPlot.WinForms;
using Plot_those_lines__Gianmarco;

namespace Plot_those_lines__Gianmarco.Service
{
    public class GraphiqueService
    {
        public void PlotSolarWindData(FormsPlot formsPlot, List<SolarWindPoint> points)
        {
            // Réinitialise le graphique
            formsPlot.Plot.Clear();

            if (points == null || points.Count == 0) return;

            // Définition des 5 séries avec leurs couleurs
            var seriesConfig = new (string Name, System.Drawing.Color Color, Func<SolarWindPoint, double?> Selector)[]
            {
                ("Bt", System.Drawing.Color.Black, p => p.Bt),
                ("By GSE", System.Drawing.Color.Blue, p => p.ByGse),
                ("Bz GSE", System.Drawing.Color.Red, p => p.BzGse),
                ("By GSM", System.Drawing.Color.Green, p => p.ByGsm),
                ("Bz GSM", System.Drawing.Color.Orange, p => p.BzGsm)
            };

            foreach (var config in seriesConfig)
            {
                // On extrait les points où le temps et la valeur sont valides
                var validPoints = points
                    .Where(p => config.Selector(p).HasValue)
                    .Select(p => new { Time = p.TimeTag.ToOADate(), Value = config.Selector(p)!.Value })
                    .ToList();

                if (validPoints.Count == 0) continue;

                double[] xs = validPoints.Select(p => p.Time).ToArray();
                double[] ys = validPoints.Select(p => p.Value).ToArray();

                // Ajout de la courbe
                var scatter = formsPlot.Plot.Add.Scatter(xs, ys);
                scatter.Label = config.Name;
                scatter.Color = new ScottPlot.Color(config.Color.R, config.Color.G, config.Color.B);
                scatter.LineWidth = 2;
                scatter.MarkerSize = 0; // Pas de points/marqueurs, uniquement la ligne
            }

            // Configuration de l'axe X pour le temps
            formsPlot.Plot.Axes.DateTimeTicksBottom();

            // Titres et légende
            formsPlot.Plot.Axes.Bottom.Label.Text = "Temps";
            formsPlot.Plot.Axes.Left.Label.Text = "Champ magnétique (nT)";
            formsPlot.Plot.ShowLegend();

            // Ajustement automatique des axes aux données
            formsPlot.Plot.Axes.SetLimits();

            // Rafraîchissement de l'affichage
            formsPlot.Refresh();
        }
    }
}