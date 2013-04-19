﻿// NRADB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
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

using System.Drawing;
using System.Windows.Forms;
using NRADB.BusinessObjects;

namespace NRADB.Forms
{
	public partial class CardPreviewForm : Form
	{
		/// <summary>
		/// The id of the card displayed.
		/// </summary>
		public int CardUniversalId
		{
			get { return cardPreviewControl.CardUniversalId; }
			set
			{
				if (value != cardPreviewControl.CardUniversalId && ApplicationSettings.ImagesFolderExists)
				{
					cardPreviewControl.CardUniversalId = value;
				}
				Show();
			}
		}

		public CardPreviewForm()
		{
			InitializeComponent();
			Enabled = false;
			Hide();
		}

		protected override bool ShowWithoutActivation
		{
			get { return true; }
		}

		protected override CreateParams CreateParams
		{
			get
			{
				// to avoid the activation of the window when shown
				var parameters = base.CreateParams;
				parameters.ExStyle |= (int)(
					Win32.ExtendedWindowsStyle.WS_EX_LAYERED
					| Win32.ExtendedWindowsStyle.WS_EX_TOOLWINDOW
				);
				return parameters;
			}
		}

		/// <summary>
		/// The form is hidden by setting its size to 0.
		/// </summary>
		public new void Hide()
		{
			Size = new Size(0, 0);
		}

		/// <summary>
		/// The form is shown by setting its size to the correct size
		/// </summary>
		public new void Show()
		{
			if (cardPreviewControl.IsImageDisplayed())
			{
				var resize = UserSettings.ImagePreviewSize;
				var imageSize = cardPreviewControl.GetDesiredSize();
				Size = new Size(imageSize.Width * resize / 100, imageSize.Height * resize / 100);
			}
			else
				Size = cardPreviewControl.GetDesiredSize();
		}
	}
}
