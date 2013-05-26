/*
 * Created by SharpDevelop.
 * User:María Dolores Delgado Lara
 * 		Jose Luis Díaz Montellano
 * Date: 19/02/2013
 * Time: 04:29 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
namespace IDE
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		private Archivo Archivo_Actual=new Archivo();//Archivo abierto
		private bool guardado = false;//Bandera que indica si se han guardado los cambios
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		void SalirClick(object sender, EventArgs e)
		{
			CerrarIDE();
		}
		
		void Abrir_ArchivoClick(object sender, EventArgs e)
		{
			//Se abre una ventana para elegir el archivo
			OpenFileDialog selected_file=new OpenFileDialog();
			//Verifica que el usuario haya dado click en aceptar
			if(selected_file.ShowDialog()== DialogResult.OK){
				//Apertura del archivo en modo lectura
				Stream file=selected_file.OpenFile();
				StreamReader reader=new StreamReader(file);
				//El contenido del archivo se muestra en el área de texto
				char[] data=new char[file.Length];
				reader.ReadBlock(data,0,(int)file.Length);
				fastColoredTextBox1.Text=new string(data);
				reader.Close();
				//Establecer atributos del archivo actual
				Archivo_Actual.Nombre_de_Archivo=selected_file.SafeFileName;
				Archivo_Actual.Ubicacion_de_Archivo=Path.GetFullPath(selected_file.FileName);
				//Nombre del archivo en cabecera
				Form.ActiveForm.Text="IDE--"+Archivo_Actual.Ubicacion_de_Archivo;
				//Habilitar opciones para un archivo abierto
				Habilitar_Opciones_Archivo(true);
			}
			
		}
		
		void Nuevo_ArchivoClick(object sender, EventArgs e)
		{
			//Se abre una ventana para elegir la ubicación y el nombre del archivo a guardar
			SaveFileDialog output_file=new SaveFileDialog();
			//Cambio de título del la ventana y establecimiento de la extensíon
			output_file.Title="Nuevo";
			output_file.Filter="Text file (*.txt)|*.txt";
			//Verifica que el usuario haya dado click en aceptar
			if(output_file.ShowDialog()== DialogResult.OK){
				//Apertura del archivo en modo escritura
				Stream file=output_file.OpenFile();
				StreamWriter writer=new StreamWriter(file);
				writer.Close();
				//Establecer atributos del archivo actual
				string [] filePath=output_file.FileName.Split('\\');
				Archivo_Actual.Nombre_de_Archivo=filePath[filePath.Length-1];
				Archivo_Actual.Ubicacion_de_Archivo=Path.GetFullPath(output_file.FileName);
				//Nombre del archivo en cabecera
				Form.ActiveForm.Text="IDE--"+Archivo_Actual.Ubicacion_de_Archivo;
				//Habilitar opciones para un archivo abierto
				Habilitar_Opciones_Archivo(true);
			}
		}
		
		void Guardar_ArchivoClick(object sender, EventArgs e)
		{
			Guardar();
		}
		
		void Guardar_Como_ArchivoClick(object sender, EventArgs e)
		{
			//Se abre una ventana para elegir la ubicación y el nombre del archivo a guardar
			SaveFileDialog output_file=new SaveFileDialog();
			//Se establece una extensión por default
			output_file.Filter="Text file (*.txt)|*.txt";
			//Verifica que el usuario haya dado click en aceptar
			if(output_file.ShowDialog()==DialogResult.OK)
			{
				//Apertura del archivo en modo escritura
				Stream file=output_file.OpenFile();
				StreamWriter writer=new StreamWriter(file);
				//Escritura del archivo con el contenido del área de texto
				writer.Write(fastColoredTextBox1.Text);
				writer.Close();
				//Establecer atributos del archivo actual
				string [] filePath=output_file.FileName.Split('\\');
				Archivo_Actual.Nombre_de_Archivo=filePath[filePath.Length-1];
				Archivo_Actual.Ubicacion_de_Archivo=Path.GetFullPath(output_file.FileName);
				//Nombre del archivo en cabecera
				Form.ActiveForm.Text="IDE--"+Archivo_Actual.Ubicacion_de_Archivo;
				//Habilitar opciones para un archivo abierto
				Habilitar_Opciones_Archivo(true);
				guardado = true;
			}
		}
		
		void Cerrar_ArchivoClick(object sender, EventArgs e)
		{
			if(!guardado){
				DialogResult result;
				result=MessageBox.Show("Deséa guardar los cambios?","Guardar",MessageBoxButtons.YesNoCancel,
				                       MessageBoxIcon.Question,MessageBoxDefaultButton.Button3);
				if(result==DialogResult.Yes)
					Guardar();
				else
					if(result==DialogResult.Cancel)
						return;
			}
			//Limpia el área de texto
			fastColoredTextBox1.Clear();
			//Limpia las los atributos del objeto Archivo_Actual
			Archivo_Actual.Nombre_de_Archivo="";
			Archivo_Actual.Ubicacion_de_Archivo="";
			//Deshabilitar las opciones para un archivo abierto
			Habilitar_Opciones_Archivo(false);
			//Escribe el título original
			Form.ActiveForm.Text="IDE";
			TokenList.Items.Clear();
			ErrorList.Items.Clear();
		}
		
		public void Guardar()
		{
			//Se abre un archivo temporal para gurdar el contenido del área de texto
			StreamWriter temporal=new StreamWriter("temp.txt");
			temporal.Write(fastColoredTextBox1.Text);
			temporal.Close();
			//Se elimina el archivo original en caso de que exista
			if(File.Exists(Archivo_Actual.Ubicacion_de_Archivo))
				File.Delete(Archivo_Actual.Ubicacion_de_Archivo);
			//Se mueve el temporal a la ubicación del archivo original
			File.Move("temp.txt",Archivo_Actual.Ubicacion_de_Archivo);
			guardado = true;
		}
		public void Habilitar_Opciones_Archivo(bool flag)
		{
			fastColoredTextBox1.Enabled=flag;
			Cerrar_Archivo.Enabled=flag;
			Guardar_Archivo.Enabled=flag;
			Guardar_Como_Archivo.Enabled=flag;
			compilarToolStripMenuItem.Enabled=flag;
		}
		public void CerrarIDE()
		{
			if(!Archivo_Actual.Nombre_de_Archivo.Equals("")){
				DialogResult result;
				result=MessageBox.Show("No se guardaran los cambios.\n"+
				                       "Seguro de que quiere salir de la aplicación?","Advertencia",
				                       MessageBoxButtons.YesNo,
				                       MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2);
				if(result==DialogResult.No)
					return;
			}
			this.Close();
		}
		void FuenteClick(object sender, EventArgs e)
		{
			FontDialog Text_Font=new FontDialog();
			Text_Font.MaxSize=17;
			if(Text_Font.ShowDialog() == DialogResult.OK)
			{
				fastColoredTextBox1.Font=Text_Font.Font;
			}
		}
		
		void AcercaDeClick(object sender, EventArgs e)
		{
			MessageBox.Show(" Universidad Autónoma de Aguascalientes\n   Ingenieria en Sistemas Computacionales" +
			                "\nMateria:\n    Compiladores I\nAlumnos:\n    María Dolores Delgado Lara\n" +
			                "    José Luis Díaz Montellano\nProfesor:\n    Dr. Eduardo Serna Pérez","Información",
			                MessageBoxButtons.OK,MessageBoxIcon.Information);
		}
		//Compiling process
		void CompilarToolStripMenuItemClick(object sender, EventArgs e)
		{
			Guardar();//Save the code in the slected file
			guardado=true;//Make a copy of the file in the IDE bin directory
			System.IO.File.Copy(Archivo_Actual.Ubicacion_de_Archivo,"temp"+Archivo_Actual.Nombre_de_Archivo,true);
			string command="";
			ExecuteCMD cmd=new ExecuteCMD();
			command+="Analizador_Lexico.exe temp"+Archivo_Actual.Nombre_de_Archivo;
			cmd.ExecuteCommandSync(command);//Excecute the command to run the Lexicon Analisys
			System.IO.File.Delete("temp"+Archivo_Actual.Nombre_de_Archivo);//Delete the file copy
			FillTokenList();
			FillErrorList("infoLexiconAnalisysTokens.txt");
			command="SyntacticAnalizer.exe";//Execute the command to run the Syntactic Analisys
			cmd.ExecuteCommandSync(command);
			FillTreeView();
			FillErrorList("infoSyntacticAnalisys.txt");
		}
		//Fill the token list in the GUI
		void FillTokenList(){
			TokenList.Items.Clear();//Clear the list avoiding to append the new tokens.
			FileStream file = new FileStream("LexiconAnalisysTokens.txt",FileMode.Open,FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			string [] token;
			string line=reader.ReadLine();
			while(line!=null){
				token=line.Split('\t');
				TokenList.Items.Add(new ListViewItem(token));
				line=reader.ReadLine();
			}
			reader.Close();
		}
		//Fill the TreeView in the GUI
		void FillTreeView(){
			
		}
		//Fill the error list in the GUI
		void FillErrorList(string fileName){
			FileStream file = new FileStream(fileName,FileMode.Open,FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			int i=1;
			string [] error=new string[2];
			string line=reader.ReadLine();
			while(line!=null){
				error[0]=i.ToString();
				if(line.StartsWith("$")){
					error[0]="";
					line=line.Substring(1);
				}
				error[1]=line;
				ErrorList.Items.Add(new ListViewItem(error));
				line=reader.ReadLine();
				i++;
			}
			reader.Close();
		}
		
	}
}
