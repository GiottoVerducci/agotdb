﻿// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright © 2007, 2008, 2009, 2010 Vincent Ripoll
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com
// © A Game of Thrones 2005 George R. R. Martin
// © A Game of Thrones CCG 2005 Fantasy Flight Publishing, Inc.
// © A Game of Thrones LCG 2008 Fantasy Flight Publishing, Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl
// © Le Trône de Fer JCE 2008 Edge Entertainment

using System;
using System.Drawing;
using System.Windows.Forms;
using AGoTDB.BusinessObjects;
using AGoTDB.Services;

namespace AGoTDB.Forms
{
	public partial class CardPreviewControl : UserControl
	{
		private Image fCurrentImage = null;
		private int fCardUniversalId = -1;

		/// <summary>
		/// The id of the card displayed.
		/// </summary>
		public int CardUniversalId
		{
			get { return fCardUniversalId; }
			set { if (value != fCardUniversalId && ApplicationSettings.ImagesFolderExists) { fCardUniversalId = value; LoadCardImage(); } }
		}

		public CardPreviewControl()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Indicates whether an image is displayed or not (if the image couldn't be loaded for instance).
		/// </summary>
		/// <returns>True if an image is displayed, false otherwise.</returns>
		public bool IsImageDisplayed()
		{
			return fCurrentImage != null;
		}

		/// <summary>
		/// Gets the displayed image size, or the size of the control containing the label if no image is displayed.
		/// </summary>
		/// <returns>The size of the image displayed, or the size of the control.</returns>
		public Size GetDesiredSize()
		{
			return fCurrentImage != null
				? fCurrentImage.Size
				: Size;
		}

		private void LoadCardImage()
		{
			string imageFileName = CardImageService.GetImageFileName(fCardUniversalId);
			if (CardImageService.GetImageAvailability(imageFileName) == ImageAvailability.Available)
			{
				fCurrentImage = new Bitmap(imageFileName);
				pbImage.Image = fCurrentImage;
				pbImage.SizeMode = PictureBoxSizeMode.Zoom;
				DisplayImageOrText(true);
				Invalidate();
			}
			else
			{
				fCurrentImage = null;
				Size = new Size(60, 50);
				lblUnavailable.Text = Resource1.ImageNotAvailable;
				DisplayImageOrText(false);
				Invalidate();
			}
		}

		private void DisplayImageOrText(bool displayImage)
		{
			pbImage.Visible = displayImage;
			lblUnavailable.Visible = !displayImage;
		}

		private void Control_MouseEnter(object sender, EventArgs e)
		{
			OnMouseEnter(e);
		}

		private void Control_MouseLeave(object sender, EventArgs e)
		{
			OnMouseLeave(e);
		}

		private void Control_MouseCaptureChanged(object sender, EventArgs e)
		{
			// if a sub control still has the mouse capture, we don't raise the event for the outside observers
			if (!pbImage.Capture && !lblUnavailable.Capture && !Capture
				&& !pbImage.Focused && lblUnavailable.Focused && !Focused)
				OnMouseCaptureChanged(e);
		}

		private void Control_MouseClick(object sender, MouseEventArgs e)
		{
			OnMouseClick(e);
		}
	}
}