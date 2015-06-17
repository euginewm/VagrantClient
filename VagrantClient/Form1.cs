using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace VagrantClient
{
    public partial class FormVagrantClient : Form
    {
        public FormVagrantClient()
        {
            InitializeComponent();

            String[] boxList = this.getVagrantBoxList();
            foreach (String value in boxList)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = value;
                item.Tag = value;
                item.Click += new EventHandler(processMenuItem);
                this.toolStripMenuItem1.DropDownItems.Add(item);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // -----------------------
        private String[] getVagrantBoxList()
        {
            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.CreateNoWindow = true;

            cmd.Start();

            cmd.StandardInput.WriteLine("vagrant box list");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            String line = cmd.StandardOutput.ReadToEnd();

            String[] splitResult = line.Split(new String[] { System.Environment.NewLine }, System.StringSplitOptions.RemoveEmptyEntries);
            String vagrantDelimiter = "(virtualbox";
            
            int counter = 0;
            foreach (string value in splitResult)
            {
                int pos = value.IndexOf(vagrantDelimiter);
                if (pos > -1)
                {
                    counter++;
                }
            }
            int i = 0;
            String[] boxList = new string[counter];
            foreach (string value in splitResult)
            {
                int pos = value.IndexOf(vagrantDelimiter);
                if (pos > -1)
                {
                    boxList[i++] = value.Substring(0, pos-1); // -1 to "(" character
                }
            }

            return boxList;
        }

        private void processMenuItem(object sender, EventArgs e)
        {
            MessageBox.Show(sender.ToString());
            // todo: vagrant destroy --force; ... update box
        }

        private String vagrantCommand(String command)
        {
            Process cmd = new Process();

            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.CreateNoWindow = true;

            cmd.Start();

            cmd.StandardInput.WriteLine(command);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            return cmd.StandardOutput.ReadToEnd();
        }

        private void haltToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vagrantCommand("vagrant halt");
        }

        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vagrantCommand("vagrant up");
        }
    }
}
