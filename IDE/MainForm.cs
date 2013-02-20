/*
 * Created by SharpDevelop.
 * User: Jose Luis
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
namespace IDE
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		Archivo Archivo_Actual=new Archivo();
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
			
			this.Close();
		}
		
		
		
		void Abrir_ArchivoClick(object sender, EventArgs e)
		{
			OpenFileDialog Selected_File=new OpenFileDialog();
			if(Selected_File.ShowDialog()== DialogResult.OK){
				Stream file=Selected_File.OpenFile();
				StreamReader reader=new StreamReader(file);
				char[] data=new char[file.Length];
				reader.ReadBlock(data,0,(int)file.Length);
				Code_Area.Text=new String(data);
				reader.Close();
				//Establecer atributos del archivo actual
				Archivo_Actual.Nombre_de_Archivo=Selected_File.SafeFileName;
				Archivo_Actual.Ubicacion_de_Archivo=Path.GetFullPath(Selected_File.SafeFileName);
			}
			
		}
		
		void Nuevo_ArchivoClick(object sender, EventArgs e)
		{
			
		}
		
		void Guardar_ArchivoClick(object sender, EventArgs e)
		{
			Guardar();
		}
		
		void Guardar_Como_ArchivoClick(object sender, EventArgs e)
		{
			SaveFileDialog Output_File=new SaveFileDialog();
			Output_File.Filter="Text file (*.txt)|*.txt";
			if(Output_File.ShowDialog()==DialogResult.OK)
			{
				Stream file=Output_File.OpenFile();
				StreamWriter writer=new StreamWriter(file);
				writer.Write(Code_Area.Text);
				writer.Close();
			}
		}
		
		void Cerrar_ArchivoClick(object sender, EventArgs e)
		{
			Code_Area.Clear();
		}
		
		public void Guardar()
		{
			
		}
		
		void FuenteClick(object sender, EventArgs e)
		{
			FontDialog Text_Font=new FontDialog();
			if(Text_Font.ShowDialog() == DialogResult.OK)
			{
				
				Code_Area.Font=Text_Font.Font;
			}
		}
		
		void AcercaDeClick(object sender, EventArgs e)
		{
			MessageBox.Show(" Universidad Autónoma de Aguascalientes\n   Ingenieria en Sistemas Computacionales" +
			                "\nMateria:\n    Compiladores I\nAlumnos:\n    María Dolores Delgado Lara\n" +
			                "    José Luis Díaz Montellano\nProfesor:\n    Dr. Eduardo Serna Pérez");
		}
	}
}
