/*
 * Created by SharpDevelop.
 * User: Jose Luis
 * Date: 19/02/2013
 * Time: 04:29 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace IDE
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.Abrir_Archivo = new System.Windows.Forms.ToolStripMenuItem();
			this.Nuevo_Archivo = new System.Windows.Forms.ToolStripMenuItem();
			this.Guardar_Archivo = new System.Windows.Forms.ToolStripMenuItem();
			this.Guardar_Como_Archivo = new System.Windows.Forms.ToolStripMenuItem();
			this.Cerrar_Archivo = new System.Windows.Forms.ToolStripMenuItem();
			this.compilarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fuenteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.acercaDeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.menuStrip2 = new System.Windows.Forms.MenuStrip();
			this.léxicoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sintacticoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.Code_Area = new System.Windows.Forms.RichTextBox();
			this.menuStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.SuspendLayout();
			this.menuStrip2.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.archivoToolStripMenuItem,
									this.compilarToolStripMenuItem,
									this.fuenteToolStripMenuItem,
									this.salirToolStripMenuItem,
									this.acercaDeToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(883, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// archivoToolStripMenuItem
			// 
			this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.Abrir_Archivo,
									this.Nuevo_Archivo,
									this.Guardar_Archivo,
									this.Guardar_Como_Archivo,
									this.Cerrar_Archivo});
			this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
			this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
			this.archivoToolStripMenuItem.Text = "Archivo";
			// 
			// Abrir_Archivo
			// 
			this.Abrir_Archivo.Image = ((System.Drawing.Image)(resources.GetObject("Abrir_Archivo.Image")));
			this.Abrir_Archivo.Name = "Abrir_Archivo";
			this.Abrir_Archivo.Size = new System.Drawing.Size(152, 22);
			this.Abrir_Archivo.Text = "Abrir";
			this.Abrir_Archivo.ToolTipText = "Abrir un archivo";
			this.Abrir_Archivo.Click += new System.EventHandler(this.Abrir_ArchivoClick);
			// 
			// Nuevo_Archivo
			// 
			this.Nuevo_Archivo.Name = "Nuevo_Archivo";
			this.Nuevo_Archivo.Size = new System.Drawing.Size(152, 22);
			this.Nuevo_Archivo.Text = "Nuevo";
			this.Nuevo_Archivo.ToolTipText = "Crear nuevo archivo";
			this.Nuevo_Archivo.Click += new System.EventHandler(this.Nuevo_ArchivoClick);
			// 
			// Guardar_Archivo
			// 
			this.Guardar_Archivo.Enabled = false;
			this.Guardar_Archivo.Name = "Guardar_Archivo";
			this.Guardar_Archivo.Size = new System.Drawing.Size(152, 22);
			this.Guardar_Archivo.Text = "Guardar";
			this.Guardar_Archivo.ToolTipText = "Guardar archivo";
			this.Guardar_Archivo.Click += new System.EventHandler(this.Guardar_ArchivoClick);
			// 
			// Guardar_Como_Archivo
			// 
			this.Guardar_Como_Archivo.Enabled = false;
			this.Guardar_Como_Archivo.Name = "Guardar_Como_Archivo";
			this.Guardar_Como_Archivo.Size = new System.Drawing.Size(152, 22);
			this.Guardar_Como_Archivo.Text = "Guardar Como";
			this.Guardar_Como_Archivo.ToolTipText = "Guardar archivo en directorio específico";
			this.Guardar_Como_Archivo.Click += new System.EventHandler(this.Guardar_Como_ArchivoClick);
			// 
			// Cerrar_Archivo
			// 
			this.Cerrar_Archivo.Enabled = false;
			this.Cerrar_Archivo.Name = "Cerrar_Archivo";
			this.Cerrar_Archivo.Size = new System.Drawing.Size(152, 22);
			this.Cerrar_Archivo.Text = "Cerrar";
			this.Cerrar_Archivo.ToolTipText = "Cerrar archivo actual";
			this.Cerrar_Archivo.Click += new System.EventHandler(this.Cerrar_ArchivoClick);
			// 
			// compilarToolStripMenuItem
			// 
			this.compilarToolStripMenuItem.Enabled = false;
			this.compilarToolStripMenuItem.Name = "compilarToolStripMenuItem";
			this.compilarToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
			this.compilarToolStripMenuItem.Text = "Compilar";
			this.compilarToolStripMenuItem.ToolTipText = "Compilar el código actual";
			// 
			// fuenteToolStripMenuItem
			// 
			this.fuenteToolStripMenuItem.Name = "fuenteToolStripMenuItem";
			this.fuenteToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
			this.fuenteToolStripMenuItem.Text = "Fuente";
			this.fuenteToolStripMenuItem.Click += new System.EventHandler(this.FuenteClick);
			// 
			// salirToolStripMenuItem
			// 
			this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
			this.salirToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.salirToolStripMenuItem.Text = "Salir";
			this.salirToolStripMenuItem.ToolTipText = "Salir del programa";
			this.salirToolStripMenuItem.Click += new System.EventHandler(this.SalirClick);
			// 
			// acercaDeToolStripMenuItem
			// 
			this.acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
			this.acercaDeToolStripMenuItem.Size = new System.Drawing.Size(81, 20);
			this.acercaDeToolStripMenuItem.Text = "Acerca De...";
			this.acercaDeToolStripMenuItem.ToolTipText = "Info. sobre el programa y los desarrolladores.";
			this.acercaDeToolStripMenuItem.Click += new System.EventHandler(this.AcercaDeClick);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.splitContainer1);
			this.panel1.Controls.Add(this.menuStrip2);
			this.panel1.Location = new System.Drawing.Point(436, 27);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(443, 361);
			this.panel1.TabIndex = 1;
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 24);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Size = new System.Drawing.Size(443, 337);
			this.splitContainer1.SplitterDistance = 211;
			this.splitContainer1.TabIndex = 1;
			// 
			// menuStrip2
			// 
			this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.léxicoToolStripMenuItem,
									this.sintacticoToolStripMenuItem});
			this.menuStrip2.Location = new System.Drawing.Point(0, 0);
			this.menuStrip2.Name = "menuStrip2";
			this.menuStrip2.Size = new System.Drawing.Size(443, 24);
			this.menuStrip2.TabIndex = 0;
			this.menuStrip2.Text = "menuStrip2";
			// 
			// léxicoToolStripMenuItem
			// 
			this.léxicoToolStripMenuItem.AutoSize = false;
			this.léxicoToolStripMenuItem.Name = "léxicoToolStripMenuItem";
			this.léxicoToolStripMenuItem.Size = new System.Drawing.Size(215, 20);
			this.léxicoToolStripMenuItem.Text = "Léxico";
			// 
			// sintacticoToolStripMenuItem
			// 
			this.sintacticoToolStripMenuItem.AutoSize = false;
			this.sintacticoToolStripMenuItem.Name = "sintacticoToolStripMenuItem";
			this.sintacticoToolStripMenuItem.Size = new System.Drawing.Size(215, 20);
			this.sintacticoToolStripMenuItem.Text = "Sintáctico";
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(12, 401);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(867, 109);
			this.tabControl1.TabIndex = 5;
			// 
			// tabPage1
			// 
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(859, 83);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Resultados";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(859, 83);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Errores";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// Code_Area
			// 
			this.Code_Area.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.Code_Area.Enabled = false;
			this.Code_Area.Location = new System.Drawing.Point(12, 51);
			this.Code_Area.Name = "Code_Area";
			this.Code_Area.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
			this.Code_Area.Size = new System.Drawing.Size(418, 337);
			this.Code_Area.TabIndex = 6;
			this.Code_Area.Text = "";
			this.Code_Area.WordWrap = false;
			this.Code_Area.TextChanged += new System.EventHandler(this.Code_AreaTextChanged);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(883, 522);
			this.Controls.Add(this.Code_Area);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "IDE";
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.menuStrip2.ResumeLayout(false);
			this.menuStrip2.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.RichTextBox Code_Area;
		private System.Windows.Forms.ToolStripMenuItem fuenteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sintacticoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem léxicoToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip2;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ToolStripMenuItem acercaDeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem compilarToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem Cerrar_Archivo;
		private System.Windows.Forms.ToolStripMenuItem Guardar_Como_Archivo;
		private System.Windows.Forms.ToolStripMenuItem Guardar_Archivo;
		private System.Windows.Forms.ToolStripMenuItem Nuevo_Archivo;
		private System.Windows.Forms.ToolStripMenuItem Abrir_Archivo;
		private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
	}
}
