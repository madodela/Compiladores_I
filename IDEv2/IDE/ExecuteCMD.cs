using System;
using System.Threading;
using System.Drawing;
namespace IDE
{
	public class ExecuteCMD {
		string CommandOutput;
		bool blackWindow = true;
		
		public bool BlackWindow {
			get { return blackWindow; }
			set { blackWindow = value;}
		}
		public ExecuteCMD () {
			CommandOutput="vacio";
		}
		
		public string CmdOutput{
			get{
				return CommandOutput;
			}
			set{
				CommandOutput = value;
			}
		}
		//Execute the command Synchronously
		public void ExecuteCommandSync(Object command) {
			try {
				// create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as the parameters.
				// Incidentally, /c tells cmd that we want it to execute the command that follows, and then exit.
				System.Diagnostics.ProcessStartInfo procStartInfo =
					new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);
				// The following commands are needed to redirect the standard output.
				//This means that it will be redirected to the Process.StandardOutput StreamReader.
				procStartInfo.RedirectStandardOutput = false;
				procStartInfo.UseShellExecute = false;
				// Do not create the black window.
				procStartInfo.CreateNoWindow = false;
				// Now we create a process, assign its ProcessStartInfo and start it
				System.Diagnostics.Process proc = new System.Diagnostics.Process();
				proc.StartInfo = procStartInfo;
				proc.Start();

				// Get the output into a string
				//if(blackWindow){
					string result = proc.StandardOutput.ReadToEnd();
					CmdOutput = result;
				//}
			} catch (Exception e) {
				// Log the exception
			}
		}

		/// Execute the command Asynchronously.
		public void ExecuteCommandAsync(string command) {
			try {
				//Asynchronously start the Thread to process the Execute command request.
				Thread objThread = new Thread(new ParameterizedThreadStart(ExecuteCommandSync));
				//Make the thread as background thread.
				objThread.IsBackground = true;
				//Set the Priority of the thread.
				objThread.Priority = ThreadPriority.AboveNormal;
				//Start the thread.
				objThread.Start(command);
			} catch (ThreadStartException objException) {
				// Log the exception
			} catch (ThreadAbortException objException) {
				// Log the exception
			} catch (Exception objException) {
				// Log the exception
			}
		}
	}
}