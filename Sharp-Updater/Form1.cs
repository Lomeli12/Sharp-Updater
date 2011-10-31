using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;

namespace Sharp_Updater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            XmlDocument current = new XmlDocument();
            current.Load(Environment.CurrentDirectory + @"\Info.xml");
            XmlNodeList ver1 = current.GetElementsByTagName("Installedversion");
            XmlNodeList update = current.GetElementsByTagName("updatexml");
            XmlNodeList pro = current.GetElementsByTagName("ProgramName");
            string prog = pro[0].InnerText;
            string up1 = update[0].InnerText;
            string cversion = ver1[0].InnerText;
            XmlDocument updater = new XmlDocument();
            updater.Load(up1);
            XmlNodeList ver2 = updater.GetElementsByTagName("LatestVersion");
            string newversion = ver2[0].InnerText;
            this.Text = prog + " Updater";
            if (cversion != newversion)
            {
                label1.Text = "There is a new update for " + prog + ".";
                label2.Text = "Would you like to download it?";
            }
            else if (cversion == newversion)
            {
                label1.Text = "There are no new updates for " + prog + ".";
                label2.Text = "";
                button2.Visible = false;
                button1.Text = "Close";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XmlDocument current = new XmlDocument();
            current.Load(Environment.CurrentDirectory + @"\Info.xml");
            XmlNodeList pro = current.GetElementsByTagName("ProgramName");
            string prog = pro[0].InnerText;
            XmlDocument updater = new XmlDocument();
            updater.Load("http://anthony-lomeli.co.cc/NotepadS/Update1.xml");
            XmlNodeList down = updater.GetElementsByTagName("InstallerURL");
            string install = down[0].InnerText;
            WebClient download = new WebClient();
            label1.Text = "Downloading update for " + prog + ".";
            label2.Text = "Please wait...";
            download.DownloadFile(install, @"\update.exe");
            MessageBox.Show("Please Close all instances of " + prog + " while update is in session.");
            System.Diagnostics.Process.Start(@"\update.exe");
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
