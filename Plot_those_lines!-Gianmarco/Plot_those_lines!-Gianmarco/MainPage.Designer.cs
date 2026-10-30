namespace Plot_those_lines__Gianmarco
{
    partial class MainPage
    {
        private System.ComponentModel.IContainer components = null;

        // Composants WinForms
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Button btnImportJson;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox grpSeries;
        private System.Windows.Forms.GroupBox grpAddData;
        private System.Windows.Forms.ComboBox cmbSeriesSelect;
        private System.Windows.Forms.DateTimePicker dtpTimestamp;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Button btnAddValue;

        // Composant ScottPlot
        private ScottPlot.WinForms.FormsPlot formsPlot1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            panelControl = new Panel();
            btnImportJson = new Button();
            lblStatus = new Label();
            grpSeries = new GroupBox();
            grpAddData = new GroupBox();
            cmbSeriesSelect = new ComboBox();
            dtpTimestamp = new DateTimePicker();
            txtValue = new TextBox();
            btnAddValue = new Button();
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            panelCheckboxes = new FlowLayoutPanel();
            panelControl.SuspendLayout();
            grpSeries.SuspendLayout();
            SuspendLayout();
            // 
            // panelControl
            // 
            panelControl.Controls.Add(btnImportJson);
            panelControl.Controls.Add(lblStatus);
            panelControl.Controls.Add(grpSeries);
            panelControl.Controls.Add(grpAddData);
            panelControl.Dock = DockStyle.Left;
            panelControl.Location = new Point(0, 0);
            panelControl.Name = "panelControl";
            panelControl.Padding = new Padding(10);
            panelControl.Size = new Size(260, 600);
            panelControl.TabIndex = 1;
            // 
            // btnImportJson
            // 
            btnImportJson.Dock = DockStyle.Top;
            btnImportJson.Location = new Point(10, 200);
            btnImportJson.Name = "btnImportJson";
            btnImportJson.Size = new Size(240, 38);
            btnImportJson.TabIndex = 0;
            btnImportJson.Text = "📂 Importer JSON";
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Top;
            lblStatus.Location = new Point(10, 170);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(240, 30);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Statut : Aucune donnée";
            // 
            // grpSeries
            // 
            grpSeries.Controls.Add(panelCheckboxes);
            grpSeries.Dock = DockStyle.Top;
            grpSeries.Location = new Point(10, 10);
            grpSeries.Name = "grpSeries";
            grpSeries.Size = new Size(240, 160);
            grpSeries.TabIndex = 2;
            grpSeries.TabStop = false;
            grpSeries.Text = "Affichage des Séries";
            // 
            // grpAddData
            // 
            grpAddData.Location = new Point(0, 0);
            grpAddData.Name = "grpAddData";
            grpAddData.Size = new Size(200, 100);
            grpAddData.TabIndex = 3;
            grpAddData.TabStop = false;
            // 
            // cmbSeriesSelect
            // 
            cmbSeriesSelect.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSeriesSelect.Items.AddRange(new object[] { "bt", "by_gse", "bz_gse", "by_gsm", "bz_gsm" });
            cmbSeriesSelect.Location = new Point(15, 25);
            cmbSeriesSelect.Name = "cmbSeriesSelect";
            cmbSeriesSelect.Size = new Size(200, 23);
            cmbSeriesSelect.TabIndex = 0;
            // 
            // dtpTimestamp
            // 
            dtpTimestamp.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpTimestamp.Format = DateTimePickerFormat.Custom;
            dtpTimestamp.Location = new Point(15, 55);
            dtpTimestamp.Name = "dtpTimestamp";
            dtpTimestamp.Size = new Size(200, 23);
            dtpTimestamp.TabIndex = 1;
            // 
            // txtValue
            // 
            txtValue.Location = new Point(15, 85);
            txtValue.Name = "txtValue";
            txtValue.PlaceholderText = "Valeur en nT (ex: -12.5)";
            txtValue.Size = new Size(200, 23);
            txtValue.TabIndex = 2;
            // 
            // btnAddValue
            // 
            btnAddValue.Location = new Point(15, 115);
            btnAddValue.Name = "btnAddValue";
            btnAddValue.Size = new Size(200, 23);
            btnAddValue.TabIndex = 3;
            btnAddValue.Text = "Ajouter la mesure";
            // 
            // formsPlot1
            // 
            formsPlot1.Dock = DockStyle.Fill;
            formsPlot1.Location = new Point(260, 0);
            formsPlot1.Name = "formsPlot1";
            formsPlot1.Size = new Size(840, 600);
            formsPlot1.TabIndex = 0;
            // 
            // panelCheckboxes
            // 
            panelCheckboxes.Location = new Point(0, 12);
            panelCheckboxes.Name = "panelCheckboxes";
            panelCheckboxes.Size = new Size(240, 148);
            panelCheckboxes.TabIndex = 0;
            // 
            // MainPage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 600);
            Controls.Add(formsPlot1);
            Controls.Add(panelControl);
            Name = "MainPage";
            Text = "Plot Those Lines (PTL) - Météo Spatiale";
            panelControl.ResumeLayout(false);
            grpSeries.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel panelCheckboxes;
    }
}