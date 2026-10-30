using System.Globalization;
using System.Text.Json;

namespace Plot_those_lines__Gianmarco
{
    /// <summary>
    /// Fenêtre principale de l'application (Windows Forms) gérant l'interface graphique,
    /// l'affichage des graphiques (ScottPlot), le cache local et l'importation de fichiers JSON.
    /// </summary>
    public partial class MainPage : Form
    {
        // Service responsable du chargement, filtrage et mise en cache des données de vent solaire
        private readonly SolarWindService _solarWindService = new SolarWindService();

        // Liste en mémoire stockant l'ensemble des points de données du vent solaire actifs
        private List<SolarWindPoint> _dataPoints = new List<SolarWindPoint>();

        /// <summary>
        /// Classe interne représentant la configuration d'une série de données (Data-Driven).
        /// Permet de lier un nom, une couleur, un sélecteur de propriété et une case à cocher (CheckBox).
        /// </summary>
        private class SeriesInfo
        {
            public string Name { get; set; }                          // Nom de la série (ex: "Bt", "By GSE")
            public Color Color { get; set; }                          // Couleur de la courbe sur le graphique
            public Func<SolarWindPoint, double?> Selector { get; set; }// Expression lambda pour extraire la valeur spécifique du point
            public CheckBox AssociatedCheckBox { get; set; }          // Contrôle graphique associé pour activer/désactiver la série
            public bool IsVisible => AssociatedCheckBox != null && AssociatedCheckBox.Checked; // Indique si la série doit être affichée
        }

        // Liste des différentes séries de données gérées par l'application
        private List<SeriesInfo> _seriesList = new List<SeriesInfo>();

        /// <summary>
        /// Constructeur de la page principale. Initialise les composants graphiques et les abonnements aux événements.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            btnImportJson.Click += BtnImportJson_Click; // Événement du bouton d'importation JSON
            this.Load += MainPage_load;                // Événement déclenché au chargement de la fenêtre

            InitializeSeriesList();                    // Initialisation des séries de données et de leurs options
        }

        /// <summary>
        /// Méthode exécutée au chargement de la fenêtre. Tente de restaurer les données depuis le stockage local.
        /// </summary>
        private void MainPage_load(object? sender, EventArgs e)
        {
            // Récupère les données enregistrées dans le stockage local hors-ligne
            _dataPoints = _solarWindService.LoadFromLocalStorage();

            // S'il y a des données en stockage, met à jour l'interface et rafraîchit le graphique
            if (_dataPoints.Count > 0)
            {
                lblStatus.Text = $"Statut : {_dataPoints.Count} points restaurés (hors-ligne)";
                UpdateSeriesLabels(_dataPoints);
                RefreshPlot();
            }
        }

        /// <summary>
        /// Initialise dynamiquement le panneau de sélection des séries (cases à cocher) et la liste des séries configurées.
        /// </summary>
        private void InitializeSeriesList()
        {
            // Si le panneau des CheckBoxes n'existe pas encore, on le crée et on le positionne sur la forme
            if (panelCheckboxes == null)
            {
                panelCheckboxes = new FlowLayoutPanel();
                panelCheckboxes.Name = "panelCheckboxes";
                panelCheckboxes.Location = new Point(20, 20);
                panelCheckboxes.Size = new Size(220, 150);
                panelCheckboxes.FlowDirection = FlowDirection.TopDown;
                this.Controls.Add(panelCheckboxes);
            }

            // Définition de toutes les séries de données disponibles avec leurs couleurs et sélecteurs respectifs
            _seriesList = new List<SeriesInfo>
            {
                new SeriesInfo { Name = "Bt", Color = Color.Black, Selector = p => p.Bt },
                new SeriesInfo { Name = "By GSE", Color = Color.Blue, Selector = p => p.ByGse },
                new SeriesInfo { Name = "Bz GSE", Color = Color.Red, Selector = p => p.BzGse },
                new SeriesInfo { Name = "By GSM", Color = Color.Green, Selector = p => p.ByGsm },
                new SeriesInfo { Name = "Bz GSM", Color = Color.Orange, Selector = p => p.BzGsm }
            };

            panelCheckboxes.Controls.Clear();

            // Création dynamique d'une CheckBox pour chaque série de données
            _seriesList.Select(series =>
            {
                CheckBox chk = new CheckBox
                {
                    Text = series.Name,
                    ForeColor = series.Color,
                    AutoSize = true,
                    Checked = true // Activé par défaut
                };

                series.AssociatedCheckBox = chk;
                chk.CheckedChanged += Chk_CheckedChanged; // Événement lors du cochage/décochage
                return chk;
            })
            .ToList()
            .ForEach(chk => panelCheckboxes.Controls.Add(chk));
        }

        /// <summary>
        /// Gestionnaire d'événement déclenché lorsqu'une case à cocher d'une série change d'état.
        /// Actualise l'affichage du graphique en conséquence.
        /// </summary>
        private void Chk_CheckedChanged(object? sender, EventArgs e)
        {
            RefreshPlot();
        }

        /// <summary>
        /// Met à jour les libellés (texte) des cases à cocher pour y intégrer le nombre de points valides disponibles par série.
        /// </summary>
        private void UpdateSeriesLabels(List<SolarWindPoint> points)
        {
            if (points == null || _seriesList == null) return;

            _seriesList.ForEach(series =>
            {
                if (series.AssociatedCheckBox != null)
                {
                    // Compte le nombre de points non nuls pour la propriété correspondante
                    int count = series.Name switch
                    {
                        "Bt" => points.Count(p => p.Bt.HasValue),
                        "By GSE" => points.Count(p => p.ByGse.HasValue),
                        "Bz GSE" => points.Count(p => p.BzGse.HasValue),
                        "By GSM" => points.Count(p => p.ByGsm.HasValue),
                        "Bz GSM" => points.Count(p => p.BzGsm.HasValue),
                        _ => 0
                    };

                    // Met à jour le texte de la CheckBox (ex: "Bt (1500)")
                    series.AssociatedCheckBox.Text = $"{series.Name} ({count})";
                }
            });
        }

        /// <summary>
        /// Nettoie et redessine entièrement le graphique (ScottPlot) en fonction des séries sélectionnées et visibles.
        /// </summary>
        private void RefreshPlot()
        {
            formsPlot1.Plot.Clear(); // Efface les tracés précédents
            if (_dataPoints == null || _dataPoints.Count == 0) return;

            // Filtrage et transformation des données pour chaque série visible
            _seriesList
                .Where(series => series.IsVisible)
                .Select(series => new
                {
                    Series = series,
                    ValidPoints = _dataPoints
                        .Where(p => series.Selector(p).HasValue) // Garde uniquement les points ayant une valeur
                        .Select(p => new { Time = p.TimeTag.ToOADate(), Value = series.Selector(p)!.Value })
                        .ToList()
                })
                .Where(x => x.ValidPoints.Count > 0)
                .ToList()
                .ForEach(x =>
                {
                    // Extraction des tableaux de coordonnées X (temps) et Y (valeurs)
                    double[] xs = x.ValidPoints.Select(p => p.Time).ToArray();
                    double[] ys = x.ValidPoints.Select(p => p.Value).ToArray();

                    // Ajout du graphique de type Scatter (courbe) sur le composant ScottPlot
                    var scatter = formsPlot1.Plot.Add.Scatter(xs, ys);
                    scatter.Label = x.Series.Name;
                    scatter.Color = new ScottPlot.Color(x.Series.Color.R, x.Series.Color.G, x.Series.Color.B);
                    scatter.LineWidth = 2;
                    scatter.MarkerSize = 0; // Pas de points marqués individuels, ligne continue pure
                });

            // Configuration finale des axes et mise à jour visuelle du composant graphique
            formsPlot1.Plot.Axes.DateTimeTicksBottom();
            formsPlot1.Plot.ShowLegend();
            formsPlot1.Plot.Axes.SetLimits();
            formsPlot1.Refresh();
            formsPlot1.Invalidate();
        }

        /// <summary>
        /// Gère le clic sur le bouton d'importation d'un fichier JSON externe.
        /// Fusionne les données importées avec les données existantes, met à jour le cache et actualise l'UI.
        /// </summary>
        private void BtnImportJson_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Fichiers JSON (*.json)|*.json|Tous les fichiers (*.*)|*.*";
                ofd.Title = "Sélectionner le fichier JSON du vent solaire";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Charge les nouveaux points depuis le fichier sélectionné
                        var newPoints = _solarWindService.LoadFromFile(ofd.FileName);

                        // Récupère l'ensemble des dates existantes pour détecter les doublons / écrasements
                        var existingDates = new HashSet<DateTime>(_dataPoints.Select(p => p.TimeTag));

                        int totalImported = newPoints.Count;
                        int overwrittenCount = newPoints.Count(p => existingDates.Contains(p.TimeTag));
                        int newlyAddedCount = totalImported - overwrittenCount;

                        // Fusionne les listes, supprime les doublons en gardant la dernière version par date, puis trie par ordre chronologique
                        _dataPoints = _dataPoints
                            .Concat(newPoints)
                            .GroupBy(p => p.TimeTag)
                            .Select(g => g.Last())
                            .OrderBy(p => p.TimeTag)
                            .ToList();

                        // Sauvegarde dans le cache local et met à jour l'interface
                        _solarWindService.SaveToLocalStorage(_dataPoints);
                        lblStatus.Text = $"Statut : {_dataPoints.Count} points disponibles";
                        UpdateSeriesLabels(_dataPoints);
                        RefreshPlot();

                        // Message de confirmation détaillé de l'importation
                        string message = $"Importation réussie !\n\n" +
                                         $"• Données importées de SOLAR1 : {totalImported}\n" +
                                         $"• Nouvelles données : {newlyAddedCount}\n" +
                                         $"• Données écrasées : {overwrittenCount}";

                        MessageBox.Show(
                            message,
                            "Importation terminée",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
                    // Gestion spécifique si le format du fichier JSON est incorrect
                    catch (InvalidDataException ex)
                    {
                        MessageBox.Show(
                            ex.Message,
                            "Format de fichier incorrect",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                        lblStatus.Text = "Statut : Fichier invalide";
                    }
                    // Gestion globale des autres erreurs
                    catch (Exception)
                    {
                        MessageBox.Show(
                            "Impossible de lire ce fichier. Assurez-vous qu'il s'agit d'un fichier JSON valide et qu'il n'est pas utilisé par un autre programme.",
                            "Erreur de lecture",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error
                        );
                        lblStatus.Text = "Statut : Erreur de lecture";
                    }
                }
            }
        }
    }
}