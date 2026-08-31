namespace Plot_those_lines__Gianmarco
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        // Composants WinForms
        private System.Windows.Forms.Panel panelControl;
        private System.Windows.Forms.Button btnImportJson;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.GroupBox grpSeries;
        private System.Windows.Forms.CheckBox chkBx;
        private System.Windows.Forms.CheckBox chkBy;
        private System.Windows.Forms.CheckBox chkBz;
        private System.Windows.Forms.CheckBox chkBt;
        private System.Windows.Forms.CheckBox chkLat;
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
            chkBx = new CheckBox();
            chkBy = new CheckBox();
            chkBz = new CheckBox();
            chkBt = new CheckBox();
            chkLat = new CheckBox();
            grpAddData = new GroupBox();
            cmbSeriesSelect = new ComboBox();
            dtpTimestamp = new DateTimePicker();
            txtValue = new TextBox();
            btnAddValue = new Button();
            formsPlot1 = new ScottPlot.WinForms.FormsPlot();
            panelControl.SuspendLayout();
            grpSeries.SuspendLayout();
            grpAddData.SuspendLayout();
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
            btnImportJson.Location = new Point(10, 360);
            btnImportJson.Name = "btnImportJson";
            btnImportJson.Size = new Size(240, 38);
            btnImportJson.TabIndex = 0;
            btnImportJson.Text = "📂 Importer JSON";
            // 
            // lblStatus
            // 
            lblStatus.Dock = DockStyle.Top;
            lblStatus.Location = new Point(10, 330);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(240, 30);
            lblStatus.TabIndex = 1;
            lblStatus.Text = "Statut : Aucune donnée";
            // 
            // grpSeries
            // 
            grpSeries.Controls.Add(chkBx);
            grpSeries.Controls.Add(chkBy);
            grpSeries.Controls.Add(chkBz);
            grpSeries.Controls.Add(chkBt);
            grpSeries.Controls.Add(chkLat);
            grpSeries.Dock = DockStyle.Top;
            grpSeries.Location = new Point(10, 170);
            grpSeries.Name = "grpSeries";
            grpSeries.Size = new Size(240, 160);
            grpSeries.TabIndex = 2;
            grpSeries.TabStop = false;
            grpSeries.Text = "Affichage des Séries";
            // 
            // chkBx
            // 
            chkBx.Checked = true;
            chkBx.CheckState = CheckState.Checked;
            chkBx.Location = new Point(15, 25);
            chkBx.Name = "chkBx";
            chkBx.Size = new Size(104, 24);
            chkBx.TabIndex = 0;
            chkBx.Text = "Bx (Nord-Sud)";
            // 
            // chkBy
            // 
            chkBy.Checked = true;
            chkBy.CheckState = CheckState.Checked;
            chkBy.Location = new Point(15, 50);
            chkBy.Name = "chkBy";
            chkBy.Size = new Size(104, 24);
            chkBy.TabIndex = 1;
            chkBy.Text = "By (Est-Ouest)";
            // 
            // chkBz
            // 
            chkBz.Checked = true;
            chkBz.CheckState = CheckState.Checked;
            chkBz.Location = new Point(15, 75);
            chkBz.Name = "chkBz";
            chkBz.Size = new Size(104, 24);
            chkBz.TabIndex = 2;
            chkBz.Text = "Bz (Vertical)";
            // 
            // chkBt
            // 
            chkBt.Checked = true;
            chkBt.CheckState = CheckState.Checked;
            chkBt.Location = new Point(15, 100);
            chkBt.Name = "chkBt";
            chkBt.Size = new Size(104, 24);
            chkBt.TabIndex = 3;
            chkBt.Text = "Bt (Total)";
            // 
            // chkLat
            // 
            chkLat.Checked = true;
            chkLat.CheckState = CheckState.Checked;
            chkLat.Location = new Point(15, 125);
            chkLat.Name = "chkLat";
            chkLat.Size = new Size(104, 24);
            chkLat.TabIndex = 4;
            chkLat.Text = "Lat (Angle)";
            // 
            // grpAddData
            // 
            grpAddData.Controls.Add(cmbSeriesSelect);
            grpAddData.Controls.Add(dtpTimestamp);
            grpAddData.Controls.Add(txtValue);
            grpAddData.Controls.Add(btnAddValue);
            grpAddData.Dock = DockStyle.Top;
            grpAddData.Location = new Point(10, 10);
            grpAddData.Name = "grpAddData";
            grpAddData.Size = new Size(240, 160);
            grpAddData.TabIndex = 3;
            grpAddData.TabStop = false;
            grpAddData.Text = "➕ Saisie Manuelle";
            // 
            // cmbSeriesSelect
            // 
            cmbSeriesSelect.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSeriesSelect.Items.AddRange(new object[] { "Bx", "By", "Bz", "Bt", "Lat" });
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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 600);
            Controls.Add(formsPlot1);
            Controls.Add(panelControl);
            Name = "Form1";
            Text = "Plot Those Lines (PTL) - Météo Spatiale";
            panelControl.ResumeLayout(false);
            grpSeries.ResumeLayout(false);
            grpAddData.ResumeLayout(false);
            grpAddData.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
    }
}