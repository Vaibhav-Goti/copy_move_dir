using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NWP;
using System.Threading;
using System.Diagnostics;

namespace Send_to_Release
{
    public partial class Form1 : Form
    {
        ModifyRegistry mr = new ModifyRegistry();
        
        string ok3dpath = "";
        string capokpath = "";
        string stonepass = Path.Combine(Path.GetTempPath(), "StoneRelease");
        
        
        public Form1()
        {
            InitializeComponent();
        }
        public static string SelectFolder()
        {
            string folderPath = null;

            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    folderPath = dialog.SelectedPath;
                }
            }

            return folderPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string folderPath = SelectFolder();
                mr.Write("3dokpath", folderPath);
                if (ok3dpath != null && capokpath != null)
                    if (ok3dpath != "" && capokpath != "")
                    {
                        timer1.Start();
                    }
            }
            catch
            {

            }
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string folderPath = SelectFolder();
                mr.Write("capokpath", folderPath);
                if (ok3dpath != null && capokpath != null)
                    if (ok3dpath != "" && capokpath != "")
                    {
                        timer1.Start();
                    }
            }
            catch
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            string[] directories = Directory.GetDirectories(ok3dpath);
            if(directories.Length > 0)
            {
                timer1.Stop();
            }
            for(int i = 0; i < directories.Length; i++)
            {
                
                var path3 = Path.Combine(directories[i], "CapPath.txt");
                using (StreamWriter sw = new StreamWriter(path3, false))
                {
                    var pathfg = capokpath;
                    var strReadLine = pathfg.Trim().Replace("\t", "").Replace("\n", "");
                    if (!String.IsNullOrWhiteSpace(strReadLine))
                    {
                        sw.Write(strReadLine);

                    }
                }
                string[] temp = directories[i].Split('\\');
                MoveDirectory(directories[i], Path.Combine(stonepass, temp[temp.Length - 1]));
            }
            timer1.Start();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
               ok3dpath= mr.Read("3dokpath");
               capokpath = mr.Read("capokpath");
                if (!Directory.Exists(stonepass))
                    Directory.CreateDirectory(stonepass);

                if(ok3dpath != null && capokpath != null)
                if(ok3dpath != "" && capokpath != "")
                {
                    timer1.Start();
                }
                try
                {
                    Process proc = new Process();
                    proc.StartInfo.FileName = @"C:\Program Files\Sarin Technologies\Advisor\CapGen.exe";
                    proc.Start();
                }
                catch
                {
                    MessageBox.Show("Not start advisor..! please start manually");
                }

            }
            catch
            {

            }
        }

        public static void MoveDirectory(string source, string target)
        {
            var stack = new Stack<Folders>();
            stack.Push(new Folders(source, target));

            while (stack.Count > 0)
            {
                var folders = stack.Pop();
                Directory.CreateDirectory(folders.Target);
                foreach (var file in Directory.GetFiles(folders.Source, "*.*"))
                {
                    string targetFile = Path.Combine(folders.Target, Path.GetFileName(file));
                    if (File.Exists(targetFile)) File.Delete(targetFile);
                    File.Move(file, targetFile);
                }

                foreach (var folder in Directory.GetDirectories(folders.Source))
                {
                    stack.Push(new Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
                }
            }
            Directory.Delete(source, true);
        }

    }
    public class Folders
    {
        public string Source { get; private set; }
        public string Target { get; private set; }

        public Folders(string source, string target)
        {
            Source = source;
            Target = target;
        }
    }

}
