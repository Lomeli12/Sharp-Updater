using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Xml;
using System.ComponentModel;
using System.Net;
using System.Text;
using Gtk;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnButton3Clicked (object sender, System.EventArgs e)
	{
		Application.Quit ();
	}

	protected void OnButton4Clicked (object sender, System.EventArgs e)
	{
		button4.Visible = false;
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
		if (cversion < newversion)
		{
			label2.Text = "There is a new update for " + prog + ".";
			label3.Text = "Would you like to download it?";
			label4.Text = "Installed Version: " + cversion + " Latest Version: " + newversion;
			button2.Visible = true;
			button3.Label = "No";
		}
		else if (cversion >= newversion)
		{
			label2.Text = "There are no new updates for " + prog + ".";
			button4.Visible = false;
		}
	}

	public void download_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
	{
		double bytesIn = double.Parse(e.BytesReceived.ToString());
		double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
		double percentage = bytesIn / totalBytes * 100;
		double downloaded = bytesIn / 1024;
		double max = totalBytes / 1024;
		label1.Text = Math.Round(downloaded, 2) + "KB of " + Math.Round(max, 2) +"KB downloaded.";
	}
	
	public void download_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
	{
		string path = System.IO.Path.GetTempPath();
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
		MessageDialog complete = new MessageDialog(this,DialogFlags.NoSeparator,MessageType.Warning,ButtonsType.Ok,"Update has been downloaded sucessfully. Please Close all instances of " + prog + " while update is in session.");
		System.Diagnostics.Process.Start(path + installer);
		Application.Quit();
	}

	private void OnButton2Clicked (object sender, System.EventArgs e)
	{
		button1.Visible = true;
		button3.Visible = false;
		button4.Visible = false;
		button2.Visible = false;
		string path = System.IO.Path.GetTempPath();
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
		WebClient download = new WebClient();
		label2.Text = "Downloading update for " + prog + ".";
		label3.Text = "Please wait...";
		label4.Text = "Downloading Version " + newversion + " installer. ";
		download.DownloadProgressChanged += new DownloadProgressChangedEventHandler(download_DownloadProgressChanged);
		download.DownloadFileCompleted += new AsyncCompletedEventHandler(download_DownloadFileCompleted);
		if (System.IO.File.Exists(path + installer) == false)
		{
			download.DownloadFileAsync(new Uri(install), (path + installer));
		}
		else if (System.IO.File.Exists(path + installer) == true)
		{
			System.IO.File.Delete(path + installer);
			download.DownloadFileAsync(new Uri(install), (path + installer));
		}
	}

	protected void OnButton1Clicked (object sender, System.EventArgs e)
	{
		Application.Quit();
	}
}
