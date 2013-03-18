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
		private LectorSintaxis Sintaxis;//Objeto para manipular el archivo que contiene la sintaxis
		private bool sysColoreando=false;//Indica que el sistema esta coloreando en el área de texto
		private Color ColorComentario=Color.Indigo;
		struct Palabra_y_Posicion
		{
			public string Palabra;
			public int Posicion;
			public int Tamaño;
			public override string ToString()
			{
				string s = "Palabra = " + Palabra + ", Posicion = " + Posicion + ", Tamaño = " + Tamaño + "\n";
				return s;
			}
		};
		
		Palabra_y_Posicion[] buffer = new Palabra_y_Posicion[4000];
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			Sintaxis=new LectorSintaxis("Lenguaje.txt");
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
				Code_Area.Text=new String(data);
				reader.Close();
				sysColoreando=true;
				ColorearSintaxis(true);
				sysColoreando=false;
				//Establecer atributos del archivo actual
				Archivo_Actual.Nombre_de_Archivo=selected_file.FileName;
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
				Archivo_Actual.Nombre_de_Archivo=output_file.FileName;
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
				writer.Write(Code_Area.Text);
				writer.Close();
				//Establecer atributos del archivo actual
				Archivo_Actual.Nombre_de_Archivo=output_file.FileName;
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
			Code_Area.Clear();
			//Limpia las los atributos del objeto Archivo_Actual
			Archivo_Actual.Nombre_de_Archivo="";
			Archivo_Actual.Ubicacion_de_Archivo="";
			//Deshabilitar las opciones para un archivo abierto
			Habilitar_Opciones_Archivo(false);
			//Escribe el título original
			Form.ActiveForm.Text="IDE";
		}
		
		public void Guardar()
		{
			//Se abre un archivo temporal para gurdar el contenido del área de texto
			StreamWriter temporal=new StreamWriter("temp.txt");
			temporal.Write(Code_Area.Text);
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
			Code_Area.Enabled=flag;
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
				Code_Area.Font=Text_Font.Font;
				ColorearSintaxis(true);
			}
		}
		
		void AcercaDeClick(object sender, EventArgs e)
		{
			MessageBox.Show(" Universidad Autónoma de Aguascalientes\n   Ingenieria en Sistemas Computacionales" +
			                "\nMateria:\n    Compiladores I\nAlumnos:\n    María Dolores Delgado Lara\n" +
			                "    José Luis Díaz Montellano\nProfesor:\n    Dr. Eduardo Serna Pérez","Información",
			               MessageBoxButtons.OK,MessageBoxIcon.Information);
		}
		
		//Control de colores para palabras reservadas
			
		void Code_AreaTextChanged(object sender, EventArgs e)
		{
			/*Mientras el sistema colorea se generan eventos que indican cambios en 
			 * el área de texto por lo que para estos eventos no debe haber respuesta
			 * entonces solo se sale del método
			 */
			if(sysColoreando)
				return;
			//Colorear en la linea actual
			
			ColorearSintaxis(false);
			updateNumberLabel();
			
		}
		
		//Retorna el color de acuerdo al tipo de palabra reservada
	    Color AsignarColor(string palabra)
		{
			Color color = Color.Black;
			if (Sintaxis.esFuncion(palabra))
			{
				color = Color.PaleVioletRed;
			}
			if (Sintaxis.esKeyword(palabra))
			{
				color = Color.Blue;
			}
			return color;
		}
//	    private bool esComentario(string s)
//		{
//			string testString = s.Trim();
//			if ( (testString.Length >= 2) &&
//				 (testString[0] == '/')	  &&
//				 (testString[1] == '/')	  
//				)
//				return true;
//
//			return false;
//		}
	    
	    private int AnalizarPalabras(string s)
		{
			buffer.Initialize();
			int contador = 0;
			Regex r = new Regex(@"\w+|[^A-Za-z0-9_ \f\t\v]", RegexOptions.IgnoreCase|RegexOptions.Compiled);
			Match m;

			for (m = r.Match(s); m.Success ; m = m.NextMatch()) 
			{
				buffer[contador].Palabra = m.Value;
				buffer[contador].Posicion = m.Index;
				buffer[contador].Tamaño = m.Length;
				contador++;
			}
			return contador++;
		}
	    
	    private void ColorearSintaxis(bool todo)
	    {
	    	IDE.LiberadorDeParpadeoCodeArea.paint=false;
	    	string Texto=null;
	    	if(todo)
	    	{
	    		Code_Area.SelectAll();
	    		Texto=Code_Area.Text;
	    	}
	    		
	    	//Como no hay nada seleccionado devuelven la ubicación del cursor
	    	int InicioSeleccionActual=Code_Area.SelectionStart;
	    	int TamañoSeleccionActual=Code_Area.SelectionLength;
	    	
	    	//Encontrar inicio de linea
	    	int posicionInicial=InicioSeleccionActual;
	    	
	    	
	    	while((posicionInicial>0) && 
	    	      (Code_Area.Text[posicionInicial-1]!='\n')&&!todo)
	    	{
	    		posicionInicial--;
	    	}
	    	
	    	//Fin de Linea
	    	int posicionfinal=InicioSeleccionActual;
	    	while(posicionfinal<Code_Area.Text.Length &&
	    	      Code_Area.Text[posicionfinal]!='\n' && !todo)
	    	{
	    		posicionfinal++;
	    	}
	    	if(!todo)
	    	//Se extrae la linea final
	    	Texto=Code_Area.Text.Substring(posicionInicial,posicionfinal-posicionInicial);
	    	
	    		string palabraAnterior = "";
				int contador = AnalizarPalabras(Texto);
				
				for  (int i = 0; i < contador; i++)
				{
					Palabra_y_Posicion pp=buffer[i];

					if (pp.Palabra == "/" && palabraAnterior == "/")
					{
						// color until end of line
						int inicioComentario = pp.Posicion - 1;
						int finComentario = todo?i:posicionfinal;
						while (pp.Palabra != "\n" && i < contador)
						{
							pp=buffer[i];
							i++;
						}

						i--;

						inicioComentario=posicionfinal;
						if(todo)
							Code_Area.Select(inicioComentario, finComentario - inicioComentario);
						else
							Code_Area.Select(inicioComentario + posicionInicial, finComentario - (inicioComentario + posicionInicial));
						
						Code_Area.SelectionColor = ColorComentario;

					}
					else
					{
							
							
						Color color = AsignarColor(pp.Palabra);
						Code_Area.Select(pp.Posicion + posicionInicial, pp.Tamaño);
						 Code_Area.SelectionColor = color;
					}

					palabraAnterior = pp.Palabra;
					
	    	}
				
	    	Code_Area.Select(InicioSeleccionActual, TamañoSeleccionActual);
	    	
	    	IDE.LiberadorDeParpadeoCodeArea.paint=true;
	    }
		
		void codeArea_vScroll(object sender, EventArgs e)
		{
			updateNumberLabel();
		}
		
		void codeArea_resize(object sender, EventArgs e)
		{
			 codeArea_vScroll(null, null);
		}
		
		void codeArea_fontChanged(object sender, EventArgs e)
		{
			if(Code_Area.Font.Size < 18){
				numberedLabel.Font = Code_Area.Font;
			}
			updateNumberLabel();
			codeArea_vScroll(null, null);
		}
		
		private void updateNumberLabel()
		{
			//we get index of first visible char and number of first visible line
			Point pos = new Point(0, 0);
			int firstIndex = Code_Area.GetCharIndexFromPosition(pos);
			int firstLine = Code_Area.GetLineFromCharIndex(firstIndex);

			//now we get index of last visible char and number of last visible line
			pos.X = ClientRectangle.Width;
			pos.Y = ClientRectangle.Height;
			int lastIndex = Code_Area.GetCharIndexFromPosition(pos);
			int lastLine = Code_Area.GetLineFromCharIndex(lastIndex);

			//this is point position of last visible char, we'll use its Y value for calculating numberLabel size
			pos = Code_Area.GetPositionFromCharIndex(lastIndex);

			
			//finally, renumber label
			numberedLabel.Text = "";
			for (int i = firstLine; i <= lastLine + 1; i++)
			{
				numberedLabel.Text += i + 1 + "\n";
			}

		}

	}
}
