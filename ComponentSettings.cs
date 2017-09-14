using System;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using CrashNSaneLoadDetector;
using System.Drawing;

namespace LiveSplit.UI.Components
{
    public partial class CrashNSTLoadRemovalSettings : UserControl {

		//-1 -> full screen, otherwise index process list
        private int captureIndex = -1;
		private int numScreens = 1;
		Process[] processList;
		ImageCaptureInfo imageCaptureInfo;
		private Size captureSize = new Size(300, 100);
		private float featureVectorResolutionX = 1920.0f;
		private float featureVectorResolutionY = 1080.0f;

		private float captureAspectRatioX = 16.0f;
		private float captureAspectRatioY = 9.0f;

		private float cropOffsetX = 0.0f;
		private float cropOffsetY = -40.0f;

		private void initImageCaptureInfo()
		{
			imageCaptureInfo = new ImageCaptureInfo();

			imageCaptureInfo.featureVectorResolutionX = featureVectorResolutionX;
			imageCaptureInfo.featureVectorResolutionY = featureVectorResolutionY;
			imageCaptureInfo.captureSizeX = captureSize.Width;
			imageCaptureInfo.captureSizeY = captureSize.Height;
			imageCaptureInfo.cropOffsetX = cropOffsetX;
			imageCaptureInfo.cropOffsetY = cropOffsetY;
			imageCaptureInfo.captureAspectRatio = captureAspectRatioX / captureAspectRatioY;

			
		}


		public CrashNSTLoadRemovalSettings() {
            InitializeComponent();
			initImageCaptureInfo();
			processListComboBox.SelectedIndex = 0;
            lblVersion.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);


			Process[] processListtmp = Process.GetProcesses();
			List<Process> processes_with_name = new List<Process>();
			processListComboBox.Items.Clear();
			numScreens = 0;
			foreach (var screen in Screen.AllScreens)
			{
				// For each screen, add the screen properties to a list box.
				processListComboBox.Items.Add("Screen: " + screen.DeviceName + ", " + screen.Bounds.ToString());
				numScreens++;
			}
			foreach (Process process in processListtmp)
			{
				if (!String.IsNullOrEmpty(process.MainWindowTitle))
				{
					Console.WriteLine("Process: {0} ID: {1} Window title: {2} HWND PTR {3}", process.ProcessName, process.Id, process.MainWindowTitle, process.MainWindowHandle);
					processListComboBox.Items.Add(process.ProcessName + ": " + process.MainWindowTitle);
					processes_with_name.Add(process);
				}

			}
			processListComboBox.SelectedIndex = 0;
			processList = processes_with_name.ToArray();

			

		}

        public XmlNode GetSettings(XmlDocument document) {
           

            var settingsNode = document.CreateElement("Settings");

            //settingsNode.AppendChild(ToElement(document, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));

            //settingsNode.AppendChild(ToElement(document, "AutoReset", AutoReset.ToString()));
            //settingsNode.AppendChild(ToElement(document, "Category", category.ToString()));


            return settingsNode;
        }

        public void SetSettings(XmlNode settings) {
            var element = (XmlElement)settings;
            if (!element.IsEmpty) {
                Version version;
               /* if (element["Version"] != null) {
                    version = Version.Parse(element["Version"].InnerText);
                } else {
                    version = new Version(1, 0, 0);
                }

                if (element["AutoReset"] != null) {
                    AutoReset = Convert.ToBoolean(element["AutoReset"].InnerText);
                   
                }

              

                if (element["Category"] != null) {
                    category = Convert.ToInt32(element["Category"].InnerText);
                }

                processListComboBox.SelectedIndex = category;
                */

            }
        }

        private void checkAutoReset_CheckedChanged(object sender, EventArgs e) {
          
        }


		public Bitmap CaptureImage()
		{
			Bitmap b = new Bitmap(1, 1);

			//Full screen capture
			if (captureIndex < 0)
			{
				Screen selected_screen = Screen.AllScreens[-captureIndex - 1];
				Rectangle screenRect = selected_screen.Bounds;

				Point screenCenter = new Point(screenRect.Width / 2, screenRect.Height / 2);

				//Compute crop coordinates and width/ height based on resoution
				ImageCapture.SizeAdjustedCropAndOffset(screenRect.Width, screenRect.Height, ref imageCaptureInfo);

				//Adjust for selected screen offset
				imageCaptureInfo.center_of_frame_x += selected_screen.Bounds.X;
				imageCaptureInfo.center_of_frame_x += selected_screen.Bounds.Y;

				b = ImageCapture.CaptureFromDisplay(ref imageCaptureInfo);
			}
			else
			{
				IntPtr handle = new IntPtr(0);

				if (captureIndex >= processList.Length)
					return b;

				if (captureIndex != -1)
				{
					handle = processList[captureIndex].MainWindowHandle;
				}
				//Capture from specific process
				processList[captureIndex].Refresh();
				if ((int)handle == 0)
					return b;

				b = ImageCapture.PrintWindow(handle, ref imageCaptureInfo);
			}


			return b;
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {

			captureIndex = processListComboBox.SelectedIndex - numScreens;

			imageCaptureInfo.captureSizeX = previewPictureBox.Width;
			imageCaptureInfo.captureSizeY = previewPictureBox.Height;

			//Show something in the preview
			previewPictureBox.Image = CaptureImage();

			imageCaptureInfo.captureSizeX = captureSize.Width;
			imageCaptureInfo.captureSizeY = captureSize.Height;

		}

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e) {
           
        }

        private XmlElement ToElement<T>(XmlDocument document, String name, T value)
        {
            var element = document.CreateElement(name);
            element.InnerText = value.ToString();
            return element;
        }

		private void processListComboBox_DropDown(object sender, EventArgs e)
		{
			Process[] processListtmp = Process.GetProcesses();
			List<Process> processes_with_name = new List<Process>();
			processListComboBox.Items.Clear();

			numScreens = 0;
			foreach (var screen in Screen.AllScreens)
			{
				// For each screen, add the screen properties to a list box.
				processListComboBox.Items.Add("Screen: " + screen.DeviceName + ", " + screen.Bounds.ToString());
				numScreens++;
			}
			foreach (Process process in processListtmp)
			{
				if (!String.IsNullOrEmpty(process.MainWindowTitle))
				{
					Console.WriteLine("Process: {0} ID: {1} Window title: {2} HWND PTR {3}", process.ProcessName, process.Id, process.MainWindowTitle, process.MainWindowHandle);
					processListComboBox.Items.Add(process.ProcessName + ": " + process.MainWindowTitle);
					processes_with_name.Add(process);
				}

			}

			processList = processes_with_name.ToArray();
		}
	}
}
