/*
 * Created by SharpDevelop.
 * User: Jose Luis
 * Date: 22/02/2013
 * Time: 06:13 p.m.
 */
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
namespace IDE
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class LectorSintaxis {
		private string ArchivoSintaxis; //Archivo que contiene las palabras reservadas
		private ArrayList Keywords = new ArrayList(); //Guarda las keywords indicadas en el archivo
		private ArrayList Funciones = new ArrayList(); //Guarda las funciones indicadas en el archivo
		
		//Constructor de la clase, recibe path del archivo a abrir
		public LectorSintaxis(string archivo) {
			//Se abre el archivo en modo lectura
			FileStream file = new FileStream(archivo, FileMode.Open, FileAccess.Read);
			StreamReader reader = new StreamReader(file);
			//Extrae todo el contenido del archivo en un String
			ArchivoSintaxis = reader.ReadToEnd();
			reader.Close();
			file.Close();
			LLenarArreglos();
		}
		
		//Metodo para guardar las keywords y las funciones en su respectivo arreglo
		public void LLenarArreglos() {
			//Se guarda el texto extraido del archivo en un objeto StringReader para su manipulación
			StringReader reader = new StringReader(ArchivoSintaxis);
			//Una linea es extraida
			string siguienteLinea;
			siguienteLinea = reader.ReadLine();

			//Buscar las cabeceras de [Funciones] y [Keywords]
			while (siguienteLinea != null) {
				if (siguienteLinea == "[Funciones]") {
					//Se extraen todas las palabras referentes a funciones y se almacenan en el arreglo correspondiente
					siguienteLinea = reader.ReadLine();
					//siguienteLinea = siguienteLinea.Trim();
					while (siguienteLinea != "[Keywords]") {
						Funciones.Add(siguienteLinea);
						siguienteLinea = reader.ReadLine();
					}

					//En este punto ya se ha encontrado la cabecera [Keywords]
					siguienteLinea = reader.ReadLine();
					//Se extraen todas las palabras referentes a Keywords y se almacenan en el arreglo correspondiente
					while (siguienteLinea != String.Empty && siguienteLinea != null) {
						Keywords.Add(siguienteLinea);
						siguienteLinea = reader.ReadLine();
					}
				}
			}
			Funciones.Sort();
			Keywords.Sort();
		}
		
		public bool esFuncion(string palabra) {
			if(Funciones.Contains(palabra))
				return true;
			return false;
		}
		
		public bool esKeyword(string palabra) {
			if(Keywords.Contains(palabra))
				return true;
			return false;
		}
	}
}