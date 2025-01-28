namespace SistemaOro.Forms.Views.Reportes
{
	partial class Report1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Report1));
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.VerticalDetail = new DevExpress.XtraReports.UI.VerticalDetailBand();
            this.VerticalTotal = new DevExpress.XtraReports.UI.VerticalTotalBand();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.Name = "BottomMargin";
            // 
            // VerticalDetail
            // 
            this.VerticalDetail.Name = "VerticalDetail";
            this.VerticalDetail.WidthF = 637.4583F;
            // 
            // VerticalTotal
            // 
            this.VerticalTotal.Name = "VerticalTotal";
            this.VerticalTotal.WidthF = 371.875F;
            // 
            // Report1
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.VerticalDetail,
            this.VerticalTotal});
            this.Font = new DevExpress.Drawing.DXFont(resources.GetString("$this.Font"), 8.25F);
            this.Margins = new DevExpress.Drawing.DXMargins(49F, 40F, 100F, 100F);
            this.PageHeight = 1700;
            this.PageWidth = 1100;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Tabloid;
            this.Version = "24.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.VerticalDetailBand VerticalDetail;
        private DevExpress.XtraReports.UI.VerticalTotalBand VerticalTotal;
    }
}
