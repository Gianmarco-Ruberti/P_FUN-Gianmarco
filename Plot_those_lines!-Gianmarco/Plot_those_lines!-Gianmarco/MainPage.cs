using Plot_those_lines__Gianmarco.Service;
using System.Globalization;
using System.Text.Json;

namespace Plot_those_lines__Gianmarco
{
    public partial class MainPage : Form
    {
        private readonly SolarWindService _solarWindService = new SolarWindService();
        private List<SolarWindPoint> _dataPoints = new List<SolarWindPoint>();

        private readonly GraphicService _chartService = new GraphicService();

        public MainPage()
        {
            InitializeComponent();
            btnImportJson.Click += BtnImportJson_Click;
            this.Load += MainPage_load;

            chkBt.CheckedChanged += OnCheckBoxChanged;
            chkByGse.CheckedChanged += OnCheckBoxChanged;
            chkBzGse.CheckedChanged += OnCheckBoxChanged;
            chkByGsm.CheckedChanged += OnCheckBoxChanged;
            chkBzGsm.CheckedChanged += OnCheckBoxChanged;
        }

        private void MainPage_load(object? sender, EventArgs e)
        {
            _dataPoints = _solarWindService.LoadFromLocalCache();

            if (_dataPoints.Count > 0)
            {
                lblStatus.Text = $"Statut : {_dataPoints.Count} points restaurés (hors-ligne)";
                UpdateSeriesLabels(_dataPoints);
                RefreshPlot();
            }
        }
        private void RefreshPlot()
        {
            _chartService.PlotSolarWindData(
                formsPlot1,
                _dataPoints,
                chkBt.Checked,
                chkByGse.Checked,
                chkBzGse.Checked,
                chkByGsm.Checked,
                chkBzGsm.Checked
            );
        }

        private void OnCheckBoxChanged(object? sender, EventArgs e)
        {
            RefreshPlot();
        }

        private void UpdateSeriesLabels(List<SolarWindPoint> points)
        {
            if (points == null) return;

            chkBt.Text = $"Bt (Total) ({points.Count(p => p.Bt.HasValue)})";
            chkByGse.Text = $"By (GSE) ({points.Count(p => p.ByGse.HasValue)})";
            chkBzGse.Text = $"Bz (GSE) ({points.Count(p => p.BzGse.HasValue)})";
            chkByGsm.Text = $"By (GSM) ({points.Count(p => p.ByGsm.HasValue)})";
            chkBzGsm.Text = $"Bz (GSM) ({points.Count(p => p.BzGsm.HasValue)})";
        }

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
                        // Chargement des nouvelles données
                        var newPoints = _solarWindService.LoadFromFile(ofd.FileName);

                        // Calcul des statistiques
                        var existingDates = new HashSet<DateTime>(_dataPoints.Select(p => p.TimeTag));

                        int totalImported = newPoints.Count;
                        int overwrittenCount = newPoints.Count(p => existingDates.Contains(p.TimeTag));
                        int newlyAddedCount = totalImported - overwrittenCount;

                        // Fusion : on combine l'ancienne liste et la nouvelle
                        // GroupBy sur TimeTag permet d'éliminer les doublons de date (on prend First() ou Last())
                        _dataPoints = _dataPoints
                            .Concat(newPoints)
                            .GroupBy(p => p.TimeTag)
                            .Select(g => g.Last()) // Garde la donnée du second JSON en cas de conflit
                            .OrderBy(p => p.TimeTag) // Conserve l'ordre chronologique
                            .ToList();

                        // Sauvegarde de la liste fusionnée dans le cache local
                        _solarWindService.SaveToLocalCache(_dataPoints);
                        lblStatus.Text = $"Statut : {_dataPoints.Count} points disponibless";
                        UpdateSeriesLabels(_dataPoints);

                        RefreshPlot();

                        // Pop-up de confirmation
                        string message = $"Importation réussie !\n\n" +
                                         $"• Données importées : {totalImported}\n" +
                                         $"• Nouvelles données : {newlyAddedCount}\n" +
                                         $"• Données écrasées : {overwrittenCount}";

                        MessageBox.Show(
                            message,
                            "Importation terminée",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                    }
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