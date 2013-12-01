/*
 * Created by SharpDevelop.
 * User:María Dolores Delgado Lara
 * 		Jose Luis Díaz Montellano
 * Date: 19/02/2013
 * Time: 04:29 p.m.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace IDE
{
	public partial class MainForm : Form
	{
		private Archivo archivo_actual = new Archivo(); //Archivo abierto
		private bool guardado = false; //Bandera que indica si se han guardado los cambios
		
		public MainForm() {
			InitializeComponent();
		}
		
		void SalirClick(object sender, EventArgs e) {
			CerrarIDE();
		}
		
		void Abrir_ArchivoClick(object sender, EventArgs e) {
			//Se abre una ventana para elegir el archivo
			OpenFileDialog selected_file = new OpenFileDialog();
			//Verifica que el usuario haya dado click en aceptar

			if (selected_file.ShowDialog() == DialogResult.OK) {
				//Apertura del archivo en modo lectura
				Stream file = selected_file.OpenFile();
				StreamReader reader = new StreamReader(file);
				//El contenido del archivo se muestra en el área de texto
				char[] data = new char[file.Length];
				reader.ReadBlock(data, 0, (int) file.Length);
				fastColoredTextBox1.Text = new string(data);
				reader.Close();
				//Establecer atributos del archivo actual
				archivo_actual.Nombre_de_Archivo = selected_file.SafeFileName;
				archivo_actual.Ubicacion_de_Archivo = Path.GetFullPath(selected_file.FileName);
				Nombre_y_opciones_de_archivo();
			}
		}
		
		void Nuevo_ArchivoClick(object sender, EventArgs e) {
			//Se abre una ventana para elegir la ubicación y el nombre del archivo a guardar
			SaveFileDialog output_file = new SaveFileDialog();
			//Cambio de título del la ventana y establecimiento de la extensíon
			output_file.Title = "Nuevo";
			output_file.Filter = "Text file (*.txt)|*.txt";
			//Verifica que el usuario haya dado click en aceptar
			if (output_file.ShowDialog() == DialogResult.OK) {
				//Apertura del archivo en modo escritura
				Stream file = output_file.OpenFile();
				StreamWriter writer = new StreamWriter(file);
				writer.Close();
				//Establecer atributos del archivo actual
				string[] filePath = output_file.FileName.Split('\\');
				archivo_actual.Nombre_de_Archivo = filePath[filePath.Length - 1];
				archivo_actual.Ubicacion_de_Archivo = Path.GetFullPath(output_file.FileName);
				Nombre_y_opciones_de_archivo();
			}
		}
		
		void Nombre_y_opciones_de_archivo() {
			//Nombre del archivo en cabecera
			Form.ActiveForm.Text = "IDE--"+archivo_actual.Ubicacion_de_Archivo;
			//Habilitar opciones para un archivo abierto
			Habilitar_Opciones_Archivo(true);
		}
		
		void Guardar_ArchivoClick(object sender, EventArgs e) {
			Guardar();
		}
		
		void Guardar_Como_ArchivoClick(object sender, EventArgs e) {
			//Se abre una ventana para elegir la ubicación y el nombre del archivo a guardar
			SaveFileDialog output_file = new SaveFileDialog();
			//Se establece una extensión por default
			output_file.Filter = "Text file (*.txt)|*.txt";
			//Verifica que el usuario haya dado click en aceptar
			if(output_file.ShowDialog() == DialogResult.OK) {
				//Apertura del archivo en modo escritura
				Stream file = output_file.OpenFile();
				StreamWriter writer = new StreamWriter(file);
				//Escritura del archivo con el contenido del área de texto
				writer.Write(fastColoredTextBox1.Text);
				writer.Close();
				//Establecer atributos del archivo actual
				string [] filePath = output_file.FileName.Split('\\');
				archivo_actual.Nombre_de_Archivo = filePath[filePath.Length - 1];
				archivo_actual.Ubicacion_de_Archivo = Path.GetFullPath(output_file.FileName);
				Nombre_y_opciones_de_archivo();
				guardado = true;
			}
		}
		
		void Cerrar_ArchivoClick(object sender, EventArgs e) {
			if (!guardado) {
				DialogResult result;
				result = MessageBox.Show("Deséa guardar los cambios?", "Guardar",
				                         MessageBoxButtons.YesNoCancel,
				                         MessageBoxIcon.Question,
				                         MessageBoxDefaultButton.Button3);
				if (result == DialogResult.Yes) {
					Guardar();
				} else if (result == DialogResult.Cancel) {
					return;
				}
			}
			//Limpia el área de texto
			fastColoredTextBox1.Clear();
			//Limpia las los atributos del objeto Archivo_Actual
			archivo_actual.Nombre_de_Archivo = "";
			archivo_actual.Ubicacion_de_Archivo = "";
			//Deshabilitar las opciones para un archivo abierto
			Habilitar_Opciones_Archivo(false);
			//Escribe el título original
			Form.ActiveForm.Text = "IDE";
			TokenList.Items.Clear();
			ErrorList.Items.Clear();
			TreeView.Nodes.Clear();
			interCode.Text = "";
		}
		
		public void Guardar() {
			//Se abre un archivo temporal para gurdar el contenido del área de texto
			StreamWriter temporal = new StreamWriter("temp.txt");
			temporal.Write(fastColoredTextBox1.Text);
			temporal.Close();
			//Se elimina el archivo original en caso de que exista
			if(File.Exists(archivo_actual.Ubicacion_de_Archivo))
				File.Delete(archivo_actual.Ubicacion_de_Archivo);
			//Se mueve el temporal a la ubicación del archivo original
			File.Move("temp.txt",archivo_actual.Ubicacion_de_Archivo);
			guardado = true;
		}
		
		public void Habilitar_Opciones_Archivo(bool flag) {
			fastColoredTextBox1.Enabled = flag;
			Cerrar_Archivo.Enabled = flag;
			Guardar_Archivo.Enabled = flag;
			Guardar_Como_Archivo.Enabled = flag;
			compilarToolStripMenuItem.Enabled = flag;
		}
		
		public void CerrarIDE() {
			if (!archivo_actual.Nombre_de_Archivo.Equals("")) {
				DialogResult result;
				result = MessageBox.Show("No se guardaran los cambios.\n"
				                         + "Seguro de que quiere salir de la aplicación?", "Advertencia",
				                         MessageBoxButtons.YesNo,
				                         MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
				if (result == DialogResult.No) {
					return;
				}
			}
			this.Close();
		}
		
		void FuenteClick(object sender, EventArgs e) {
			FontDialog Text_Font = new FontDialog();
			Text_Font.MaxSize = 17;
			if (Text_Font.ShowDialog() == DialogResult.OK) {
				fastColoredTextBox1.Font = Text_Font.Font;
			}
		}
		
		void AcercaDeClick(object sender, EventArgs e) {
			MessageBox.Show(" Universidad Autónoma de Aguascalientes\n   Ingenieria en Sistemas Computacionales"
			                + "\nMateria:\n    Compiladores I\nAlumnos:\n    María Dolores Delgado Lara\n"
			                + "    José Luis Díaz Montellano", "Información",
			                MessageBoxButtons.OK, MessageBoxIcon.Information);
		}
		
		//Compiling process
		void CompilarToolStripMenuItemClick(object sender, EventArgs e) {
			Guardar();//Save the code in the slected file
			guardado = true;
			ErrorList.Items.Clear(); // clean the error's area
			//Make a copy of the file in the \IDE\bin directory
			System.IO.File.Copy(archivo_actual.Ubicacion_de_Archivo, "temp" + archivo_actual.Nombre_de_Archivo, true);
			string command = "";
			ExecuteCMD cmd = new ExecuteCMD();
			command += "Analizador_Lexico.exe temp" + archivo_actual.Nombre_de_Archivo;
			cmd.ExecuteCommandSync(command);//Excecute the command to run the Lexicon Analisys
			System.IO.File.Delete("temp" + archivo_actual.Nombre_de_Archivo);//Delete the file copy
			FillTokenList();
			FillErrorList("infoLexiconAnalisysTokens.txt");
			command = "SyntacticAnalizer.exe";//Execute the command to run the Syntactic Analisys
			cmd.ExecuteCommandSync(command);
            FillTreeView(TreeView , "SyntacticTree.xml");
            FillTreeView(treeViewSemantic , "SemanticTree.xml");
			FillErrorList("infoSyntacticAnalisys.txt");
            FillErrorList("infoSemanticAnalisys.txt");
            FillSymbolList();
            FillIntermediateCode();
		}
		
		//Fill the token list in the GUI
		void FillTokenList() {
			TokenList.Items.Clear();//Clear the list avoiding to append the new tokens.
			FileStream file = new FileStream("LexiconAnalisysTokens.txt", FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			string[] token;
			string line = reader.ReadLine();
			while (line != null) {
				token = line.Split('\t');
				TokenList.Items.Add(new ListViewItem(token));
				line = reader.ReadLine();
			}
			reader.Close();
		}
		
		//Fill the TreeView in the GUI
		void FillTreeView(TreeView TreeView, string file) {
			try {
				// SECTION 1. Create a DOM Document and load the XML data into it.
				XmlDocument dom = new XmlDocument();
				dom.Load(file);
				string name = "";
				int x, x2;
				name = dom.DocumentElement.InnerText.Substring(1);
				x = name.IndexOf('"');
				x2 = name.Length;
				name = name.Remove(x, x2 - x);

				// SECTION 2. Initialize the TreeView control.
				TreeView.Nodes.Clear();
				TreeView.Nodes.Add(name);
				TreeNode tNode = new TreeNode();
				tNode = TreeView.Nodes[0];
				
				// SECTION 3. Populate the TreeView with the DOM nodes.
				addNode(dom.DocumentElement, tNode);
				TreeView.ExpandAll();
			} catch (XmlException xmlEx) {
				MessageBox.Show(xmlEx.Message);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
		
		void addNode(XmlNode inXmlNode, TreeNode inTreeNode) {
			XmlNode xNode;
			TreeNode tNode;
			XmlNodeList nodeList;
			string name = "";
			int i, x, x2;

			// Loop through the XML nodes until the leaf is reached.
			// Add the nodes to the TreeView during the looping process.
			if (inXmlNode.HasChildNodes) {
				nodeList = inXmlNode.ChildNodes;
				for(i = 1; i<=nodeList.Count - 1; i++) {
					xNode = inXmlNode.ChildNodes[i];
					name = xNode.InnerText.Substring(1);
					x = name.IndexOf('"');
					x2 = name.Length;
					name = name.Remove(x,x2-x);
					inTreeNode.Nodes.Add(name);
					tNode = inTreeNode.Nodes[i-1];
					addNode(xNode, tNode);
				}
			} else {
				/* Here you need to pull the data from the XmlNode based on the
				 * type of node, whether attribute values are required, and so forth.
				 * inTreeNode.Text = (inXmlNode.OuterXml).Trim();
				 * inTreeNode.Text=null;*/
			}
		}
		
		//Fill the error list in the GUI
		void FillErrorList(string fileName) {
			FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			int i = 1;
			string[] error = new string[2];
			string line = reader.ReadLine();
			while (line != null) {
				error[0] = i.ToString();
				if (line.StartsWith("$")) {
					error[0] = "";
					line = line.Substring(1);
				}
				error[1] = line;
				ErrorList.Items.Add(new ListViewItem(error));
				line = reader.ReadLine();
				i++;
			}
			reader.Close();
		}
		
		void FillIntermediateCode(){
            FileStream file = new FileStream("middleCode.tm" , FileMode.Open , FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            string line = reader.ReadLine();
            while (line != null)
            {
          		interCode.Text += line + '\n';
                line = reader.ReadLine();
            }
            reader.Close();
		}

        //Fill the symbol table list in the GUI
        void FillSymbolList()
        {
            tableSymbolList.Items.Clear();//Clear the list avoiding to append the new tokens.
            FileStream file = new FileStream("tableSymbolFile.txt" , FileMode.Open , FileAccess.Read);
            StreamReader reader = new StreamReader(file);
            string[] variable;
            string line = reader.ReadLine();
            while (line != null)
            {
                variable = line.Split('\t');
                tableSymbolList.Items.Add(new ListViewItem(variable));
                line = reader.ReadLine();
            }
            reader.Close();
        }
		
	}
}