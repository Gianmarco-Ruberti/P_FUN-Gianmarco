using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot;
using ScottPlot.WinForms;
using Plot_those_lines__Gianmarco;

namespace Plot_those_lines__Gianmarco.Service
{
    public class GraphicService
    {
        public void PlotSolarWindData(FormsPlot formsPlot, List<SolarWindPoint> points, bool showBt = false, bool showByGse = true, bool showBzGse = true, bool showByGsm = true, bool showBzGsm = true)
        {
            // Réinitialise le graphique
            formsPlot.Plot.Clear();

            if (points == null || points.Count == 0) return;

            // Définition des 5 séries avec leurs couleurs
            var seriesConfig = new (string Name, System.Drawing.Color Color, Func<SolarWindPoint, double?> Selector, bool IsVisible)[]
            {
                ("Bt", System.Drawing.Color.Black, p => p.Bt, showBt),
                ("By GSE", System.Drawing.Color.Blue, p => p.ByGse, showByGse),
                ("Bz GSE", System.Drawing.Color.Red, p => p.BzGse, showBzGse),
                ("By GSM", System.Drawing.Color.Green, p => p.ByGsm, showByGsm),
                ("Bz GSM", System.Drawing.Color.Orange, p => p.BzGsm, showBzGsm)
            };

            // Filtrer les configurations pour ne garder que les séries marquées comme visibles
            seriesConfig
                .Where(config => config.IsVisible)
                .Select(config => new
                {
                    Config = config,
                    // Pour chaque configuration visible, filtrer et transformer la liste des points bruts
                    ValidPoints = points
                        .Where(p => config.Selector(p).HasValue) // Ne garde que les points où la valeur sélectionnée existe
                        .Select(p => new {
                            Time = p.TimeTag.ToOADate(),         // Convertit la date/heure en format numérique (OADate) compatible avec le graphique
                            Value = config.Selector(p)!.Value    // Extrait la valeur numérique de la donnée
                        })
                       .ToList()
                })
                // Ne conserver que les configurations qui possèdent au moins un point valide à tracer
                .Where(x => x.ValidPoints.Count > 0)
                .ToList()
                // Parcourir chaque série de données prête pour l'ajouter visuellement au graphique
                .ForEach(x =>
                {
                    // Extraction des coordonnées X (temps) et Y (valeur) sous forme de tableaux de doubles
                    double[] xs = x.ValidPoints.Select(p => p.Time).ToArray();
                    double[] ys = x.ValidPoints.Select(p => p.Value).ToArray();

                    // Création et ajout de la courbe de dispersion (Scatter plot) dans le composant ScottPlot
                    ScottPlot.Plottables.Scatter scatter = formsPlot.Plot.Add.Scatter(xs, ys);

                    scatter.Label = x.Config.Name; // Assigne le nom de la série

                    // Applique la couleur de la série en convertissant les composantes RVB
                    scatter.Color = new ScottPlot.Color(x.Config.Color.R, x.Config.Color.G, x.Config.Color.B);

                    scatter.LineWidth = 2;   // Définit l'épaisseur de la ligne à 2 pixels
                    scatter.MarkerSize = 0;  // Masque les points de marquage individuels
                });

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