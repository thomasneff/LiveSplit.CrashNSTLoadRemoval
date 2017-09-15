using System;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using CrashNSaneLoadDetector;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LiveSplit.UI.Components
{
    public partial class CrashNSTLoadRemovalSettings : UserControl {

		//-1 -> full screen, otherwise index process list
        private int processCaptureIndex = -1;
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

		private Rectangle selectionRectanglePreviewBox;
		private Point selectionTopLeft = new Point(0, 0);
		private Point selectionBottomRight = new Point(0, 0);

		private void initImageCaptureInfo()
		{
			imageCaptureInfo = new ImageCaptureInfo();

			selectionTopLeft = new Point(0, 0);
			selectionBottomRight = new Point(previewPictureBox.Width, previewPictureBox.Height);
			selectionRectanglePreviewBox = new Rectangle(selectionTopLeft.X, selectionTopLeft.Y, selectionBottomRight.X - selectionTopLeft.X, selectionBottomRight.Y - selectionTopLeft.Y);


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
			if (processCaptureIndex < 0)
			{
				Screen selected_screen = Screen.AllScreens[-processCaptureIndex - 1];
				Rectangle screenRect = selected_screen.Bounds;

				Point screenCenter = new Point(screenRect.Width / 2, screenRect.Height / 2);


				//Change size according to selected crop
				screenRect.Width = (int)(imageCaptureInfo.crop_coordinate_right - imageCaptureInfo.crop_coordinate_left);
				screenRect.Height = (int)(imageCaptureInfo.crop_coordinate_bottom - imageCaptureInfo.crop_coordinate_top);

				//Compute crop coordinates and width/ height based on resoution
				ImageCapture.SizeAdjustedCropAndOffset(screenRect.Width, screenRect.Height, ref imageCaptureInfo);

				//Adjust for crop offset
				imageCaptureInfo.center_of_frame_x += imageCaptureInfo.crop_coordinate_left;
				imageCaptureInfo.center_of_frame_y += imageCaptureInfo.crop_coordinate_top;

				//Adjust for selected screen offset
				imageCaptureInfo.center_of_frame_x += selected_screen.Bounds.X;
				imageCaptureInfo.center_of_frame_y += selected_screen.Bounds.Y;




				b = ImageCapture.CaptureFromDisplay(ref imageCaptureInfo);
			}
			else
			{
				IntPtr handle = new IntPtr(0);

				if (processCaptureIndex >= processList.Length)
					return b;

				if (processCaptureIndex != -1)
				{
					handle = processList[processCaptureIndex].MainWindowHandle;
				}
				//Capture from specific process
				processList[processCaptureIndex].Refresh();
				if ((int)handle == 0)
					return b;

				b = ImageCapture.PrintWindow(handle, ref imageCaptureInfo, useCrop: true);
			}


			return b;
		}

		public Bitmap CaptureImageFullPreview(ref ImageCaptureInfo imageCaptureInfo, bool useCrop = false)
		{
			Bitmap b = new Bitmap(1, 1);

			//Full screen capture
			if (processCaptureIndex < 0)
			{
				Screen selected_screen = Screen.AllScreens[-processCaptureIndex - 1];
				Rectangle screenRect = selected_screen.Bounds;

				Point screenCenter = new Point(screenRect.Width / 2, screenRect.Height / 2);

				if (useCrop)
				{
					//Change size according to selected crop
					screenRect.Width = (int)(imageCaptureInfo.crop_coordinate_right - imageCaptureInfo.crop_coordinate_left);
					screenRect.Height = (int)(imageCaptureInfo.crop_coordinate_bottom - imageCaptureInfo.crop_coordinate_top);
				}


				//Compute crop coordinates and width/ height based on resoution
				ImageCapture.SizeAdjustedCropAndOffset(screenRect.Width, screenRect.Height, ref imageCaptureInfo);


				imageCaptureInfo.actual_crop_size_x = 2 * imageCaptureInfo.center_of_frame_x;
				imageCaptureInfo.actual_crop_size_y = 2 * imageCaptureInfo.center_of_frame_y;

				if (useCrop)
				{
					//Adjust for crop offset
					imageCaptureInfo.center_of_frame_x += imageCaptureInfo.crop_coordinate_left;
					imageCaptureInfo.center_of_frame_y += imageCaptureInfo.crop_coordinate_top;
				}

				//Adjust for selected screen offset
				imageCaptureInfo.center_of_frame_x += selected_screen.Bounds.X;
				imageCaptureInfo.center_of_frame_y += selected_screen.Bounds.Y;

				imageCaptureInfo.actual_offset_x = 0;
				imageCaptureInfo.actual_offset_y = 0;

				b = ImageCapture.CaptureFromDisplay(ref imageCaptureInfo);

				imageCaptureInfo.actual_offset_x = cropOffsetX;
				imageCaptureInfo.actual_offset_y = cropOffsetY;



			}
			else
			{
				IntPtr handle = new IntPtr(0);

				if (processCaptureIndex >= processList.Length)
					return b;

				if (processCaptureIndex != -1)
				{
					handle = processList[processCaptureIndex].MainWindowHandle;
				}
				//Capture from specific process
				processList[processCaptureIndex].Refresh();
				if ((int)handle == 0)
					return b;



				b = ImageCapture.PrintWindow(handle, ref imageCaptureInfo, full: true, useCrop: useCrop);
			}


			return b;
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {

			processCaptureIndex = processListComboBox.SelectedIndex - numScreens;

			selectionTopLeft = new Point(0, 0);
			selectionBottomRight = new Point(previewPictureBox.Width, previewPictureBox.Height);
			selectionRectanglePreviewBox = new Rectangle(selectionTopLeft.X, selectionTopLeft.Y, selectionBottomRight.X - selectionTopLeft.X, selectionBottomRight.Y - selectionTopLeft.Y);


			DrawPreview();

		}


		private void DrawPreview()
		{


			ImageCaptureInfo copy = imageCaptureInfo;
			copy.captureSizeX = previewPictureBox.Width;
			copy.captureSizeY = previewPictureBox.Height;

			//Show something in the preview
			Bitmap capture_image = CaptureImageFullPreview(ref copy);
			float crop_size_x = copy.actual_crop_size_x;
			float crop_size_y = copy.actual_crop_size_y;


			//Draw selection rectangle
			using (Graphics g = Graphics.FromImage(capture_image))
			{
				Pen drawing_pen = new Pen(Color.Magenta, 8.0f);
				drawing_pen.Alignment = PenAlignment.Inset;
				g.DrawRectangle(drawing_pen, selectionRectanglePreviewBox);

			}

			previewPictureBox.Image = capture_image;




			//Compute image crop coordinates according to selection rectangle

			//Get raw image size from imageCaptureInfo.actual_crop_size to compute scaling between raw and rectangle coordinates

			//Console.WriteLine("SIZE X: {0}, SIZE Y: {1}", imageCaptureInfo.actual_crop_size_x, imageCaptureInfo.actual_crop_size_y);

			imageCaptureInfo.crop_coordinate_left = selectionRectanglePreviewBox.Left * (crop_size_x / previewPictureBox.Width);
			imageCaptureInfo.crop_coordinate_right = selectionRectanglePreviewBox.Right * (crop_size_x / previewPictureBox.Width);
			imageCaptureInfo.crop_coordinate_top = selectionRectanglePreviewBox.Top * (crop_size_y / previewPictureBox.Height);
			imageCaptureInfo.crop_coordinate_bottom = selectionRectanglePreviewBox.Bottom * (crop_size_y / previewPictureBox.Height);

			copy.crop_coordinate_left = selectionRectanglePreviewBox.Left * (crop_size_x / previewPictureBox.Width);
			copy.crop_coordinate_right = selectionRectanglePreviewBox.Right * (crop_size_x / previewPictureBox.Width);
			copy.crop_coordinate_top = selectionRectanglePreviewBox.Top * (crop_size_y / previewPictureBox.Height);
			copy.crop_coordinate_bottom = selectionRectanglePreviewBox.Bottom * (crop_size_y / previewPictureBox.Height);



			croppedPreviewPictureBox.Image = CaptureImageFullPreview(ref copy, useCrop: true);


			copy.captureSizeX = captureSize.Width;
			copy.captureSizeY = captureSize.Height;


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

		private void previewPictureBox_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left
				&& (selectionRectanglePreviewBox.Left + selectionRectanglePreviewBox.Width) - e.Location.X > 0
				&& (selectionRectanglePreviewBox.Top + selectionRectanglePreviewBox.Height) - e.Location.Y > 0)
			{
				selectionTopLeft = e.Location;
			}
			else if (e.Button == MouseButtons.Right && e.Location.X - selectionRectanglePreviewBox.Left > 0 && e.Location.Y - selectionRectanglePreviewBox.Top > 0)
			{
				selectionBottomRight = e.Location;
			}

			selectionRectanglePreviewBox = new Rectangle(selectionTopLeft.X, selectionTopLeft.Y, selectionBottomRight.X - selectionTopLeft.X, selectionBottomRight.Y - selectionTopLeft.Y);
			DrawPreview();
		}
	}
}
