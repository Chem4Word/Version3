using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Chem4WordUpdater
{
    public partial class Updater : Form
    {
        private string _downloadTarget = "";
        private WebClient _webClient;

        private bool _downloadCompleted;
        private string _downloadedFile;
        private string _downloadSource = string.Empty;
        private bool _userCancelledUpdate = false;

        private int _retryCount = 0;

        public Updater(string[] args)
        {
            InitializeComponent();
            if (args.Length > 0)
            {
                RegistryHelper.WriteAction("Update Started");
                _downloadTarget = args[0];
                if (DownloadFile(_downloadTarget))
                {
                    StartTimer();
                }
                else
                {
                    Information.Text = $"Error Staring download of Chem4Word Update; {Information.Text}";
                }
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            RegistryHelper.WriteAction("Update was cancelled by User");
            timer1.Enabled = false;
            _userCancelledUpdate = true;
            _webClient.CancelAsync();
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int wordCount = ShowWordProcesses();
            if (wordCount == 0 && _downloadCompleted)
            {
                RegistryHelper.WriteAction("Conditions right to run Update");
                timer1.Enabled = false;

                Application.DoEvents();
                Thread.Sleep(1000);

                RegistryHelper.WriteAction("Running Chem4Word Update");
                int exitCode = RunProcess(_downloadedFile, "");
                RegistryHelper.WriteAction($"Chem4Word ExitCode: {exitCode}");

                if (exitCode == 0)
                {
                    _userCancelledUpdate = true;
                    Close();
                }
                else
                {
                    MessageBox.Show($"Error {exitCode} while installing {_downloadSource}", "Chem4Word Updater", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Information.Text = $"Error {exitCode} while installing {_downloadSource}";
                    UpdateNow.Enabled = true;
                }
            }
        }

        private int ShowWordProcesses()
        {
            Process[] processes = Process.GetProcessesByName("winword");

            WordInstances.Items.Clear();
            foreach (var proc in processes)
            {
                WordInstances.Items.Add($"[{proc.Id}] - {proc.ProcessName}");
            }

            return processes.Length;
        }

        private void StartTimer()
        {
            timer1.Enabled = true;
        }

        private bool DownloadFile(string url)
        {
            bool started = false;

            try
            {
                RegistryHelper.WriteAction($"Downloading {url}");

                string[] parts = url.Split('/');
                string filename = parts[parts.Length - 1];
                _downloadSource = filename;

                progressBar1.Value = 0;

                _downloadedFile = Path.Combine(Path.GetTempPath(), filename);

                _webClient = new WebClient();
                _webClient.Headers.Add("user-agent", "Chem4Word Bootstrapper");

                _webClient.DownloadProgressChanged += OnDownloadProgressChanged;
                _webClient.DownloadFileCompleted += OnDownloadComplete;

                _webClient.DownloadFileAsync(new Uri(url), _downloadedFile);

                started = true;
            }
            catch (Exception ex)
            {
                RegistryHelper.WriteAction(ex.Message);
                Information.Text = ex.Message;
            }

            return started;
        }

        private void OnDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            RegistryHelper.WriteAction("Download complete");
            progressBar1.Value = 100;

            if (e.Cancelled)
            {
                RegistryHelper.WriteAction($"Downloading of {_downloadSource} was Cancelled");
                Information.Text = $"Downloading of {_downloadSource} was Cancelled";
            }
            else if (e.Error != null)
            {
                _retryCount++;
                if (_retryCount > 3)
                {
                    Information.Text = $"Too many errors downloading {_downloadSource}, please check your internet connection and try again!";
                }
                else
                {
                    DownloadFile(_downloadTarget);
                }
            }
            else
            {
                FileInfo fi = new FileInfo(_downloadedFile);
                if (fi.Length == 0)
                {
                    _retryCount++;
                    if (_retryCount > 3)
                    {
                        Information.Text = $"Too many errors downloading {_downloadSource}, please check your internet connection and try again!";
                    }
                    else
                    {
                        DownloadFile(_downloadTarget);
                    }
                }
                else
                {
                    _downloadCompleted = true;
                    UpdateNow.Enabled = false;
                    Information.Text = "Your update has been downloaded.  It will be automatically installed once all Microsoft Word processes are closed.";
                }
            }
        }

        private void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private int RunProcess(string exePath, string arguments)
        {
            int exitCode = -1;

            // Erase previously stored Update Checks
            RegistryKey key = Registry.CurrentUser.CreateSubKey(Constants.Chem4WordRegistryKey);
            if (key != null)
            {
                try
                {
                    key.DeleteValue(Constants.RegistryValueNameLastCheck);
                    key.DeleteValue(Constants.RegistryValueNameVersionsBehind);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }

            ProcessStartInfo start = new ProcessStartInfo();
            start.Arguments = arguments;
            start.FileName = exePath;
            using (Process proc = Process.Start(start))
            {
                proc.WaitForExit();
                exitCode = proc.ExitCode;
            }

            return exitCode;
        }

        private void Updater_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_userCancelledUpdate)
            {
                e.Cancel = true;
            }
        }
    }
}