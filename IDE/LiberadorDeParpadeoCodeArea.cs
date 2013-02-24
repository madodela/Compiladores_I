/*
 * Created by SharpDevelop.
 * User: Jose Luis
 * Date: 23/02/2013
 * Time: 06:38 p.m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
namespace IDE
{
	/// <summary>
	/// Description of LiberadorDeParpadeoCodeArea.
	/// </summary>
	public class LiberadorDeParpadeoCodeArea:RichTextBox
	{
		const short  WM_PAINT = 0x00f;
		public static bool paint = true;
		public LiberadorDeParpadeoCodeArea()
		{
			
		}
		protected override void WndProc(ref System.Windows.Forms.Message m) 
		{

			// sometimes we want to eat the paint message so we don't have to see all the 
			//  flicker from when we select the text to change the color.

			if (m.Msg == WM_PAINT) 
			{

				if (paint)

					base.WndProc(ref m);

				else

					m.Result = IntPtr.Zero;

			}

			else

				base.WndProc (ref m);

		}
	}
}
