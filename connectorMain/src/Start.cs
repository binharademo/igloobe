using System;
using System.Windows.Forms;
using Br.Com.IGloobe.Connector.Windows;
using System.Diagnostics;

namespace Br.Com.IGloobe.Connector {

    static class Start {

        [STAThread]
        static void Main() {
            try {
                Process[] processlist = Process.GetProcesses();
                Process current = Process.GetCurrentProcess();
                foreach(Process process in processlist){
                    if(process.Id == current.Id) 
                        continue;
                    if(process.ProcessName == current.ProcessName) 
                        process.Kill();
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Connector.Prototype = typeof (ConnectorImpl);
                Application.Run(Apps.Main.Gui.FormAppsMain.Singleton());
                //Application.Run(Gui.FormFindAndConnect.Singleton());
                
            } catch(Exception x) {
                Console.WriteLine(x.StackTrace);
                //MessageBox.Show(@"ERROR: " + x.Message + @"\n" + x.StackTrace);
            }
        }
    }
}
