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
            this.components = new System.ComponentModel.Container();
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
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.TokenList = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.TreeView = new System.Windows.Forms.TreeView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.treeViewSemantic = new System.Windows.Forms.TreeView();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.fastColoredTextBox1 = new FastColoredTextBoxNS.FastColoredTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ErrorList = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Salidas = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.tableSymbolList = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.Salidas.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Pink;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe Print", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.compilarToolStripMenuItem,
            this.fuenteToolStripMenuItem,
            this.salirToolStripMenuItem,
            this.acercaDeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(764, 29);
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
            this.archivoToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("archivoToolStripMenuItem.Image")));
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(84, 25);
            this.archivoToolStripMenuItem.Text = "Archivo";
            // 
            // Abrir_Archivo
            // 
            this.Abrir_Archivo.Image = ((System.Drawing.Image)(resources.GetObject("Abrir_Archivo.Image")));
            this.Abrir_Archivo.Name = "Abrir_Archivo";
            this.Abrir_Archivo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.Abrir_Archivo.Size = new System.Drawing.Size(265, 26);
            this.Abrir_Archivo.Text = "Abrir";
            this.Abrir_Archivo.ToolTipText = "Abrir un archivo";
            this.Abrir_Archivo.Click += new System.EventHandler(this.Abrir_ArchivoClick);
            // 
            // Nuevo_Archivo
            // 
            this.Nuevo_Archivo.Image = ((System.Drawing.Image)(resources.GetObject("Nuevo_Archivo.Image")));
            this.Nuevo_Archivo.Name = "Nuevo_Archivo";
            this.Nuevo_Archivo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.Nuevo_Archivo.Size = new System.Drawing.Size(265, 26);
            this.Nuevo_Archivo.Text = "Nuevo";
            this.Nuevo_Archivo.ToolTipText = "Crear nuevo archivo";
            this.Nuevo_Archivo.Click += new System.EventHandler(this.Nuevo_ArchivoClick);
            // 
            // Guardar_Archivo
            // 
            this.Guardar_Archivo.Enabled = false;
            this.Guardar_Archivo.Image = ((System.Drawing.Image)(resources.GetObject("Guardar_Archivo.Image")));
            this.Guardar_Archivo.Name = "Guardar_Archivo";
            this.Guardar_Archivo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.Guardar_Archivo.Size = new System.Drawing.Size(265, 26);
            this.Guardar_Archivo.Text = "Guardar";
            this.Guardar_Archivo.ToolTipText = "Guardar archivo";
            this.Guardar_Archivo.Click += new System.EventHandler(this.Guardar_ArchivoClick);
            // 
            // Guardar_Como_Archivo
            // 
            this.Guardar_Como_Archivo.Enabled = false;
            this.Guardar_Como_Archivo.Image = ((System.Drawing.Image)(resources.GetObject("Guardar_Como_Archivo.Image")));
            this.Guardar_Como_Archivo.Name = "Guardar_Como_Archivo";
            this.Guardar_Como_Archivo.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.G)));
            this.Guardar_Como_Archivo.Size = new System.Drawing.Size(265, 26);
            this.Guardar_Como_Archivo.Text = "Guardar Como";
            this.Guardar_Como_Archivo.ToolTipText = "Guardar archivo en directorio específico";
            this.Guardar_Como_Archivo.Click += new System.EventHandler(this.Guardar_Como_ArchivoClick);
            // 
            // Cerrar_Archivo
            // 
            this.Cerrar_Archivo.Enabled = false;
            this.Cerrar_Archivo.Image = ((System.Drawing.Image)(resources.GetObject("Cerrar_Archivo.Image")));
            this.Cerrar_Archivo.Name = "Cerrar_Archivo";
            this.Cerrar_Archivo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.Cerrar_Archivo.Size = new System.Drawing.Size(265, 26);
            this.Cerrar_Archivo.Text = "Cerrar";
            this.Cerrar_Archivo.ToolTipText = "Cerrar archivo actual";
            this.Cerrar_Archivo.Click += new System.EventHandler(this.Cerrar_ArchivoClick);
            // 
            // compilarToolStripMenuItem
            // 
            this.compilarToolStripMenuItem.Enabled = false;
            this.compilarToolStripMenuItem.Name = "compilarToolStripMenuItem";
            this.compilarToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.compilarToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.Space)));
            this.compilarToolStripMenuItem.Size = new System.Drawing.Size(76, 25);
            this.compilarToolStripMenuItem.Text = "Compilar";
            this.compilarToolStripMenuItem.ToolTipText = "Compilar el código actual";
            this.compilarToolStripMenuItem.Click += new System.EventHandler(this.CompilarToolStripMenuItemClick);
            // 
            // fuenteToolStripMenuItem
            // 
            this.fuenteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fuenteToolStripMenuItem.Image")));
            this.fuenteToolStripMenuItem.Name = "fuenteToolStripMenuItem";
            this.fuenteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.F)));
            this.fuenteToolStripMenuItem.Size = new System.Drawing.Size(78, 25);
            this.fuenteToolStripMenuItem.Text = "Fuente";
            this.fuenteToolStripMenuItem.Click += new System.EventHandler(this.FuenteClick);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("salirToolStripMenuItem.Image")));
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(65, 25);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.ToolTipText = "Salir del programa";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.SalirClick);
            // 
            // acercaDeToolStripMenuItem
            // 
            this.acercaDeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("acercaDeToolStripMenuItem.Image")));
            this.acercaDeToolStripMenuItem.Name = "acercaDeToolStripMenuItem";
            this.acercaDeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.acercaDeToolStripMenuItem.Size = new System.Drawing.Size(108, 25);
            this.acercaDeToolStripMenuItem.Text = "Acerca De...";
            this.acercaDeToolStripMenuItem.ToolTipText = "Info. sobre el programa y los desarrolladores.";
            this.acercaDeToolStripMenuItem.Click += new System.EventHandler(this.AcercaDeClick);
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Font = new System.Drawing.Font("Segoe Print", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl2.Location = new System.Drawing.Point(402, 51);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(350, 319);
            this.tabControl2.TabIndex = 8;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.TokenList);
            this.tabPage3.Location = new System.Drawing.Point(4, 28);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(342, 287);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "Léxico";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // TokenList
            // 
            this.TokenList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TokenList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.TokenList.FullRowSelect = true;
            this.TokenList.GridLines = true;
            this.TokenList.Location = new System.Drawing.Point(0, 0);
            this.TokenList.Name = "TokenList";
            this.TokenList.Size = new System.Drawing.Size(346, 285);
            this.TokenList.TabIndex = 0;
            this.TokenList.UseCompatibleStateImageBehavior = false;
            this.TokenList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Tipo de Token";
            this.columnHeader1.Width = 165;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Lexema";
            this.columnHeader2.Width = 167;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.TreeView);
            this.tabPage4.Location = new System.Drawing.Point(4, 28);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(342, 287);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Sintáctico";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // TreeView
            // 
            this.TreeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TreeView.Location = new System.Drawing.Point(0, 0);
            this.TreeView.Name = "TreeView";
            this.TreeView.Size = new System.Drawing.Size(342, 311);
            this.TreeView.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.treeViewSemantic);
            this.tabPage5.Location = new System.Drawing.Point(4, 28);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(342, 287);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Semántico";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // treeViewSemantic
            // 
            this.treeViewSemantic.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeViewSemantic.Location = new System.Drawing.Point(0, 0);
            this.treeViewSemantic.Name = "treeViewSemantic";
            this.treeViewSemantic.Size = new System.Drawing.Size(342, 287);
            this.treeViewSemantic.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 28);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(342, 287);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "Código intermedio";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // fastColoredTextBox1
            // 
            this.fastColoredTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fastColoredTextBox1.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.fastColoredTextBox1.BackBrush = null;
            this.fastColoredTextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fastColoredTextBox1.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.fastColoredTextBox1.Enabled = false;
            this.fastColoredTextBox1.FoldingIndicatorColor = System.Drawing.Color.Crimson;
            this.fastColoredTextBox1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fastColoredTextBox1.IsReplaceMode = false;
            this.fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.Mycode;
            this.fastColoredTextBox1.LeftBracket = '(';
            this.fastColoredTextBox1.LineNumberColor = System.Drawing.Color.Indigo;
            this.fastColoredTextBox1.Location = new System.Drawing.Point(0, 0);
            this.fastColoredTextBox1.Name = "fastColoredTextBox1";
            this.fastColoredTextBox1.Paddings = new System.Windows.Forms.Padding(0);
            this.fastColoredTextBox1.RightBracket = ')';
            this.fastColoredTextBox1.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.fastColoredTextBox1.Size = new System.Drawing.Size(380, 319);
            this.fastColoredTextBox1.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.fastColoredTextBox1);
            this.panel1.Location = new System.Drawing.Point(16, 51);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(380, 319);
            this.panel1.TabIndex = 10;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ErrorList);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(728, 111);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Errores";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ErrorList
            // 
            this.ErrorList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ErrorList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.ErrorList.FullRowSelect = true;
            this.ErrorList.GridLines = true;
            this.ErrorList.Location = new System.Drawing.Point(0, 0);
            this.ErrorList.MultiSelect = false;
            this.ErrorList.Name = "ErrorList";
            this.ErrorList.ShowGroups = false;
            this.ErrorList.Size = new System.Drawing.Size(728, 111);
            this.ErrorList.TabIndex = 0;
            this.ErrorList.UseCompatibleStateImageBehavior = false;
            this.ErrorList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Error";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Descripción";
            this.columnHeader4.Width = 663;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(728, 111);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Resultados";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Salidas
            // 
            this.Salidas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Salidas.Controls.Add(this.tabPage1);
            this.Salidas.Controls.Add(this.tabPage2);
            this.Salidas.Controls.Add(this.tabPage7);
            this.Salidas.Font = new System.Drawing.Font("Segoe Print", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Salidas.Location = new System.Drawing.Point(12, 376);
            this.Salidas.Name = "Salidas";
            this.Salidas.SelectedIndex = 0;
            this.Salidas.Size = new System.Drawing.Size(736, 143);
            this.Salidas.TabIndex = 5;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.tableSymbolList);
            this.tabPage7.Location = new System.Drawing.Point(4, 28);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(728, 111);
            this.tabPage7.TabIndex = 2;
            this.tabPage7.Text = "Tabla de simbolos";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // tableSymbolList
            // 
            this.tableSymbolList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableSymbolList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8});
            this.tableSymbolList.FullRowSelect = true;
            this.tableSymbolList.GridLines = true;
            this.tableSymbolList.Location = new System.Drawing.Point(0, 0);
            this.tableSymbolList.MultiSelect = false;
            this.tableSymbolList.Name = "tableSymbolList";
            this.tableSymbolList.ShowGroups = false;
            this.tableSymbolList.Size = new System.Drawing.Size(728, 111);
            this.tableSymbolList.TabIndex = 1;
            this.tableSymbolList.UseCompatibleStateImageBehavior = false;
            this.tableSymbolList.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Nombre";
            this.columnHeader5.Width = 150;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Tipo";
            this.columnHeader6.Width = 80;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Valor";
            this.columnHeader7.Width = 80;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Números de linea";
            this.columnHeader8.Width = 404;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Firebrick;
            this.ClientSize = new System.Drawing.Size(764, 522);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.Salidas);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IDE DD";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fastColoredTextBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.Salidas.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.ListView tableSymbolList;
		private System.Windows.Forms.TabPage tabPage7;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TreeView TreeView;
		private FastColoredTextBoxNS.FastColoredTextBox fastColoredTextBox1;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ListView ErrorList;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ListView TokenList;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabControl tabControl2;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabControl Salidas;
		private System.Windows.Forms.ToolStripMenuItem fuenteToolStripMenuItem;
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
		
		void MainFormLoad(object sender, System.EventArgs e)
		{
			
		}

        private System.Windows.Forms.TreeView treeViewSemantic;
		
	}
}