using Plot_those_lines__Gianmarco.Service;
using System.Globalization;
using System.Text.Json;

namespace Plot_those_lines__Gianmarco
{
    public partial class Form1 : Form
    {
        private readonly SolarWindService _solarWindService = new SolarWindService();
        private List<SolarWindPoint> _dataPoints = new List<SolarWindPoint>();

        private readonly GraphiqueService _chartService = new GraphiqueService();

        public Form1()
        {
            InitializeComponent();
            btnImportJson.Click += BtnImportJson_Click;
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

                        _chartService.PlotSolarWindData(formsPlot1, _dataPoints);
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