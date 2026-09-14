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
                        _dataPoints = _solarWindService.LoadFromFile(ofd.FileName);
                        lblStatus.Text = $"Statut : {_dataPoints.Count} points chargés";
                        UpdateSeriesLabels(_dataPoints);

                        RefreshPlot();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Erreur lors de l'importation : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lblStatus.Text = "Statut : Erreur d'importation";
                    }
                }
            }
        }
    }
}