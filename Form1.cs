using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FNFLoader_v1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listBox1.Items.Clear();
            try
            {
                Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "mods"));
            }
            catch (Exception ex)
            {
                
            }
            string[] dirs = Directory.GetDirectories(Path.Combine(Environment.CurrentDirectory, "mods"), "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i <= dirs.Length - 1; i++)
            {
                string[] splitFolders = dirs[i].Split('\\');
                listBox1.Items.Add(splitFolders[splitFolders.Length - 1]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                string fnf_path = read_ini();
                if (!File.Exists(fnf_path) && fnf_path != "")
                {
                    string selectedItem = listBox1.Items[listBox1.SelectedIndex].ToString();
                    DirectoryCopy(Path.Combine(Environment.CurrentDirectory, "mods", selectedItem, "assets"), Path.Combine(fnf_path, "assets"), true);
                }
                else
                {
                    MessageBox.Show("ERROR: FNF \"Funkin\" Folder is not found, click Browse and open it up!", "Error");
                }

            }
            else
            {
                MessageBox.Show("ERROR: No mod selected!", "Error");
            }

        }
        private static int DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs) //Lazy-ly copied from Microsoft article on copying dirs, and only slightly edited to replace files.
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                MessageBox.Show("ERROR: File cannot be found", "Error");
                return -1;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
   
            Directory.CreateDirectory(destDirName);

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
            return 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            string[] dirs = Directory.GetDirectories(Path.Combine(Environment.CurrentDirectory, "mods"), "*", SearchOption.TopDirectoryOnly);
            for (int i = 0;i<= dirs.Length-1; i++)
            {
                string[] splitFolders = dirs[i].Split('\\');
                listBox1.Items.Add(splitFolders[splitFolders.Length-1]);
            }
        }
        private void make_ini()
        {
            using (FileStream fs = File.Create(Path.Combine(Environment.CurrentDirectory, "fnf_path.ini")))
            {
                byte[] info = new UTF8Encoding(true).GetBytes("path=#");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }
        }
        private void write_ini(string path)
        {
            using (StreamWriter outputFile = new StreamWriter((Path.Combine(Environment.CurrentDirectory, "fnf_path.ini"))))
            {
                outputFile.Write("path=" + path);
            }
        }
        private string read_ini()
        {
            try
            {
                System.IO.File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "fnf_path.ini"));
            }
            catch(Exception e)
            {
                return "";
            }
            string text = System.IO.File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "fnf_path.ini"));
            return text.Split('=')[1];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    write_ini(fbd.SelectedPath);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
