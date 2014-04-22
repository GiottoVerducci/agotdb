// GenericDB - A generic card searcher and deck builder library for CCGs
// Copyright © 2007, 2008, 2009, 2010, 2011, 2012, 2013 Vincent Ripoll
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

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GenericDB.BusinessObjects;
using GenericDB.Services;

namespace GenericDB.Forms
{
    public partial class CardPreviewControl : UserControl
    {
        private Image _currentImage;
        private int _cardUniversalId = -1;
        private Guid[] _cardOctgnIds = new[] { Guid.Empty };

        public IApplicationSettings Settings { get; set; }

        public bool SetIds(int universalId, Guid[] octgnIds)
        {
            bool mustLoadImage = false;
            if (universalId != _cardUniversalId)
            {
                _cardUniversalId = universalId;
                mustLoadImage = true;
            }

            if (octgnIds.Except(_cardOctgnIds).Any())
            {
                _cardOctgnIds = octgnIds.ToArray();
                mustLoadImage = true;
            }

            if (mustLoadImage)
                LoadCardImage();
            return mustLoadImage;
        }

        /// <summary>
        /// The id of the card displayed.
        /// </summary>
        public int CardUniversalId
        {
            get { return _cardUniversalId; }
            set { }
        }

        /// <summary>
        /// The ids of the card displayed.
        /// </summary>
        public Guid[] CardOctgnIds
        {
            get { return _cardOctgnIds; }
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
            return _currentImage != null;
        }

        /// <summary>
        /// Gets the displayed image size, or the size of the control containing the label if no image is displayed.
        /// </summary>
        /// <returns>The size of the image displayed, or the size of the control.</returns>
        public Size GetDesiredSize()
        {
            return _currentImage != null
                ? _currentImage.Size
                : Size;
        }

        private void LoadCardImage()
        {
            if (!Settings.ImagesFolderExists)
            {
                lblUnavailable.Text = Resource1.ImageFolderDoesntExist;
                DisplayImageOrText(false);
                Invalidate();
                return;
            }
            var imageFileNames = CardImageService.GetImageFileNames(Settings.ImagesFolder, _cardUniversalId, _cardOctgnIds);
            int index;
            if (CardImageService.GetImageAvailability(imageFileNames, out index) == ImageAvailability.Available)
            {
                _currentImage = new Bitmap(imageFileNames.ElementAt(index));
                pbImage.Image = _currentImage;
                pbImage.SizeMode = PictureBoxSizeMode.Zoom;
                DisplayImageOrText(true);
                Invalidate();
            }
            else
            {
                _currentImage = null;
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
