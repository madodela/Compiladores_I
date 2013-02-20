/*
 * Created by SharpDevelop.
 * User: Jose Luis
 * Date: 19/02/2013
 * Time: 11:17 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace IDE
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	public class Archivo
	{
		private String Nombre;
		private String Ubicacion;
		public Archivo()
		{
			
		}
		public Archivo(String Nombre, String Ubicacion)
		{
			this.Nombre=Nombre;
			this.Ubicacion=Ubicacion;
		}
		public String Nombre_de_Archivo
		{
			get
			{
				return Nombre;
			}
			set
			{
				Nombre=value;
			}
		}
		public String Ubicacion_de_Archivo
		{
			get
			{
				return Ubicacion;
			}
			set
			{
				Ubicacion=value;
			}
		}
	}
}
