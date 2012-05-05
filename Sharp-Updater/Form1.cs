using System;
using System.ComponentModel;
using System.Drawing;
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
            button3.Visible = false;
            XmlDocument current = new XmlDocument();
	        current.Load(Environment.CurrentDirectory + @"\Info.xml");
	        XmlNodeList ver1 = current.GetElementsByTagName("Installedversion");
	        XmlNodeList update = current.GetElementsByTagName("updatexml");
	        XmlNodeList pro = current.GetElementsByTagName("ProgramName");
	        string prog = pro[0].InnerText;
	        string up1 = update[0].InnerText;
            double cversion = double.Parse(ver1[0].InnerText);
	        XmlDocument updater = new XmlDocument();
	        updater.Load(up1);
	        XmlNodeList ver2 = updater.GetElementsByTagName("LatestVersion");
        	double newversion = double.Parse(ver2[0].InnerText);
            this.Text = prog + " Updater";
            if (cversion < newversion)
            {
                label1.Text = "There is a new update for " + prog + ".";
                label2.Text = "Would you like to download it?";
                label3.Text = "Installed Version: " + cversion + " Latest Version: " + newversion;
            }
            else if (cversion >= newversion)
            {
                label1.Text = "There are no new updates for " + prog + ".";
                label2.Text = "Installed Version: " + cversion;
                label3.Visible = false;
                button2.Visible = false;
                button1.Text = "Close";
            }
        }
        
        public double bytesIn
        {
        	get;
        	set;
        }
        
        public double totalBytes
        {
        	get;
        	set;
        }

        public void download_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            bytesIn = double.Parse(e.BytesReceived.ToString());
            totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            double downloaded = bytesIn / 1024;
            double max = totalBytes / 1024;
            progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
            label3.Text = Math.Round(downloaded, 2) + "KB of " + Math.Round(max, 2) +"KB downloaded.";
        }

        public void download_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {   
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            XmlDocument current = new XmlDocument();
           	current.Load(Environment.CurrentDirectory + @"\Info.xml");
           	XmlNodeList pro = current.GetElementsByTagName("ProgramName");
            XmlNodeList update = current.GetElementsByTagName("updatexml");
            string prog = pro[0].InnerText;
           	string updateurl = update[0].InnerText;
           	XmlDocument updater = new XmlDocument();
           	updater.Load(updateurl);
           	XmlNodeList load = updater.GetElementsByTagName("InstallerNAME");
           	string installer = load[0].InnerText;
            if (bytesIn == totalBytes)
            {
            	MessageBox.Show("Update has been downloaded sucessfully. Please Close all instances of " + prog + " while update is in session.", "Update Complete");
            	System.Diagnostics.Process.Start(path + @"\" + installer);
            }
            else if (bytesIn != totalBytes)
            {
            	MessageBox.Show("Update Canceled", "!WARNING!");
        		System.IO.File.Delete(path + @"\" + installer);
            }
            Application.Exit();
        }
		WebClient download = new WebClient();
        private void button2_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            button3.Visible = true;
            button1.Enabled = false;
            XmlDocument current = new XmlDocument();
            current.Load(Environment.CurrentDirectory + @"\Info.xml");
            XmlNodeList update = current.GetElementsByTagName("updatexml");
            XmlNodeList pro = current.GetElementsByTagName("ProgramName");
            XmlNodeList ver1 = current.GetElementsByTagName("Installedversion");
            string inversion = ver1[0].InnerText;
            string updateurl = update[0].InnerText;
            string prog = pro[0].InnerText;
            XmlDocument updater = new XmlDocument();
            updater.Load(updateurl);
            XmlNodeList down = updater.GetElementsByTagName("InstallerURL");
            XmlNodeList load = updater.GetElementsByTagName("InstallerNAME");
            XmlNodeList ver2 = updater.GetElementsByTagName("LatestVersion");
            string newversion = ver2[0].InnerText;
            string installer = load[0].InnerText;
            string install = down[0].InnerText + load[0].InnerText;
            label1.Text = "Downloading version "+ newversion +" update for " + prog + ".";
            label2.Text = "Please wait...";
            label3.Text = "";
            download.DownloadProgressChanged += new DownloadProgressChangedEventHandler(download_DownloadProgressChanged);
            download.DownloadFileCompleted += new AsyncCompletedEventHandler(download_DownloadFileCompleted);
            button2.Enabled = false;
            if (System.IO.File.Exists(path + @"\" + installer) == false)
            {
                download.DownloadFileAsync(new Uri(install), (path  + @"\" + installer));
            }
            else if (System.IO.File.Exists(path + @"\" + installer) == true)
            {
                System.IO.File.Delete(path + @"\" + installer);
                download.DownloadFileAsync(new Uri(install), (path + @"\" + installer));
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
        	download.CancelAsync();
        }

    }
}
