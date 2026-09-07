using System.Globalization;
using System.Text.Json;

namespace Plot_those_lines__Gianmarco
{
    public partial class Form1 : Form
    {
        private readonly SolarWindService _solarWindService = new SolarWindService();
        private List<SolarWindPoint> _dataPoints = new List<SolarWindPoint>();

        public Form1()
        {
            InitializeComponent();
            btnImportJson.Click += BtnImportJson_Click;
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