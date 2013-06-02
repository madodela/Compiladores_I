/*
 * Created by SharpDevelop.
 * User: Jose Luis
 * Date: 19/02/2013
 * Time: 04:29 p.m.
 */
using System;
using System.Windows.Forms;

namespace IDE
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program {
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
	}
}