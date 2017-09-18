using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrashNSaneLoadDetector
{
	public struct ImageCaptureInfo
	{
		public float featureVectorResolutionX;
		public float featureVectorResolutionY;
		public int captureSizeX;
		public int captureSizeY;
		public float cropOffsetX;
		public float cropOffsetY;
		public float captureAspectRatio;
		public float center_of_frame_x;
		public float center_of_frame_y;
		public float actual_offset_x;
		public float actual_offset_y;
		public float actual_crop_size_x;
		public float actual_crop_size_y;
		public float crop_coordinate_left;
		public float crop_coordinate_right;
		public float crop_coordinate_top;
		public float crop_coordinate_bottom;
	}


	//This class contains utilities for capturing from the screen / from a given window.
	class ImageCapture
	{
		/// <summary>
		/// Resize the image to the specified width and height.
		/// </summary>
		/// <param name="image">The image to resize.</param>
		/// <param name="width">The width to resize to.</param>
		/// <param name="height">The height to resize to.</param>
		/// <returns>The resized image.</returns>
		public static Bitmap ResizeImage(Image image, int width, int height)
		{
			var destRect = new Rectangle(0, 0, width, height);
			var destImage = new Bitmap(width, height);

			destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

			using (var graphics = Graphics.FromImage(destImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using (var wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return destImage;
		}

		//Should probably refactor this into some kind of struct, whatever...
		public static void SizeAdjustedCropAndOffset(int device_width, int device_height, ref ImageCaptureInfo info)
		{
			var resolution_factor_x = (device_width / info.featureVectorResolutionX);

			var resolution_factor_y = (device_height / info.featureVectorResolutionY);



			info.actual_crop_size_x = info.captureSizeX * resolution_factor_x;

			info.actual_crop_size_y = info.captureSizeY * resolution_factor_y;

			//Scale offset depending on resolution
			info.actual_offset_x = info.cropOffsetX * resolution_factor_x;

			info.actual_offset_y = info.cropOffsetY * resolution_factor_y;

			//Scale offset and sizes depending on actual vs. needed aspect ratio

			if (((float)device_width / (float)device_height) > (info.captureAspectRatio))
			{

				var image_region = (float)device_height / (1.0f / info.captureAspectRatio);

				//Aspect ratio is larger than original
				var black_bar_width_total = (float)device_width - image_region;

				//Compute space occupied by black border relative to total width
				var adjust_factor = ((float)(device_width - black_bar_width_total) / (float)device_width);
				info.actual_crop_size_x *= adjust_factor;
				info.actual_offset_x *= adjust_factor;
			}
			else
			{

				var image_region = (float)device_width / (info.captureAspectRatio);

				//Aspect ratio is larger than original
				var black_bar_height_total = (float)device_height - image_region;

				//Compute space occupied by black border relative to total width
				var adjust_factor = ((float)(device_height - black_bar_height_total) / (float)device_height);
				info.actual_crop_size_y *= adjust_factor;
				info.actual_offset_y *= adjust_factor;
			}



			info.center_of_frame_x = device_width / 2;

			info.center_of_frame_y = device_height / 2;
		}

		public static Bitmap CaptureFromDisplay(ref ImageCaptureInfo info)
		{
			Bitmap b = new Bitmap((int)info.actual_crop_size_x, (int)info.actual_crop_size_y);
			
			//Full screen capture
			using (Graphics g = Graphics.FromImage(b))
			{
				g.CopyFromScreen((int)(info.center_of_frame_x - info.actual_crop_size_x / 2 + info.actual_offset_x),
				(int)(info.center_of_frame_y - info.actual_crop_size_y / 2 + info.actual_offset_y), 0, 0, new Size((int)info.actual_crop_size_x, (int)info.actual_crop_size_y), CopyPixelOperation.SourceCopy);
			}

			b = ResizeImage(b, info.captureSizeX, info.captureSizeY);

			return b;
		}

		public static Bitmap CaptureForegroundWindow()
		{
			//This function currently uses hardcoded values and is unused, but might be useful
			Rectangle bounds;


			var foregroundWindowsHandle = DLLImportStuff.GetForegroundWindow();
			DLLImportStuff.GetClientRect(foregroundWindowsHandle, out bounds);
			//bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

			if (bounds.Width <= 0)
				return new Bitmap(1, 1);

			var result = new Bitmap(300, 100);

			using (var g = Graphics.FromImage(result))
			{
				g.CopyFromScreen(new Point(bounds.Width / 2, bounds.Height / 2), new Point(bounds.X, bounds.Y), new Size(300, 100));
			}

			return result;
		}

		public static SizeF GetCurrentDpiScale()
		{
			using (Form form = new Form())
			using (Graphics g = form.CreateGraphics())
			{
				var dpi = new SizeF()
				{
					Width = g.DpiX,
					Height = g.DpiY
				};
				// Calc the scale.
				SizeF scale = new SizeF()
				{
					Width = dpi.Width / 96f,
					Height = dpi.Height / 96f
				};

				return scale;
			}
		}

		public static Bitmap PrintWindow(IntPtr hwnd, ref ImageCaptureInfo info, bool full = false, bool useCrop = false, float scalingValueFloat = 1.0f)
		{
			try
			{

			
				Rectangle rc;
				DLLImportStuff.GetClientRect(hwnd, out rc);

				Bitmap ret = new Bitmap(1, 1);

				if (rc.Width < 0)
					return ret;

				IntPtr hdcwnd = DLLImportStuff.GetDC(hwnd);
				IntPtr hdc = DLLImportStuff.CreateCompatibleDC(hdcwnd);

				rc.Width = (int)(rc.Width * scalingValueFloat);
				rc.Height = (int)(rc.Height * scalingValueFloat);



				if (useCrop)
				{
					//Change size according to selected crop
					rc.Width = (int)(info.crop_coordinate_right - info.crop_coordinate_left);
					rc.Height = (int)(info.crop_coordinate_bottom - info.crop_coordinate_top);
				}
				

				

				//Compute crop coordinates and width/ height based on resoution
				ImageCapture.SizeAdjustedCropAndOffset(rc.Width, rc.Height, ref info);


				
				

				float cropOffsetX = info.actual_offset_x;
				float cropOffsetY = info.actual_offset_y;

				if(full)
				{
					info.actual_offset_x = 0;
					info.actual_offset_y = 0;

					info.actual_crop_size_x = 2 * info.center_of_frame_x;
					info.actual_crop_size_y = 2 * info.center_of_frame_y;
				}

				if (useCrop)
				{
					//Adjust for crop offset
					info.center_of_frame_x += info.crop_coordinate_left;
					info.center_of_frame_y += info.crop_coordinate_top;
				}


				IntPtr hbmp = DLLImportStuff.CreateCompatibleBitmap(hdcwnd, (int)info.actual_crop_size_x, (int)info.actual_crop_size_y);

				DLLImportStuff.SelectObject(hdc, hbmp);

				DLLImportStuff.BitBlt(hdc, 0, 0, (int)info.actual_crop_size_x, (int)info.actual_crop_size_y, hdcwnd, (int)(info.center_of_frame_x + info.actual_offset_x - info.actual_crop_size_x / 2),
					(int)(info.center_of_frame_y + info.actual_offset_y - info.actual_crop_size_y / 2), DLLImportStuff.TernaryRasterOperations.SRCCOPY);



				info.actual_offset_x = cropOffsetX;
				info.actual_offset_y = cropOffsetY;


				ret = (Bitmap) Image.FromHbitmap(hbmp).Clone();		

				DLLImportStuff.DeleteObject(hbmp);
				DLLImportStuff.ReleaseDC(hwnd, hdcwnd);
				DLLImportStuff.DeleteDC(hdc);

				return ResizeImage(ret, info.captureSizeX, info.captureSizeY);
			}
			catch (System.Runtime.InteropServices.ExternalException ex)
			{
				
				return new Bitmap(10, 10);
			}
		}

	}
}
