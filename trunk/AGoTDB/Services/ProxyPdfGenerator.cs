// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
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
// © A Game of Thrones 2005 George R. R. Martin
// © A Game of Thrones CCG 2005 Fantasy Flight Publishing, Inc.
// © A Game of Thrones LCG 2008 Fantasy Flight Publishing, Inc.
// © Le Trône de Fer JCC 2005-2007 Stratagèmes éditions / Xénomorphe Sàrl
// © Le Trône de Fer JCE 2008 Edge Entertainment

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AGoTDB.BusinessObjects;
using GenericDB.Services;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AGoTDB.Services
{
    public static class ProxyPdfGenerator
    {
        private class ProxyInfo
        {
            public string ImagePath { get; set; }
            public int Quantity { get; set; }
            public bool MustRotate { get; set; }
        }

        /// <summary>
        /// Creates a pdf file containing the images of all the cards
        /// in the deck with the appropriate quantities.
        /// </summary>
        /// <returns>The list of unprinted items because no image was found.</returns>
        public static IList<string> CreateProxiesPdf(string proxyFileName, AgotDeck deck)
        {
            const float inch = 72; // number of points per inch
            const float cardWidth = 2.3225f * inch;
            const float cardHeight = 3.307f * inch;
            const float leftPageMargin = 0.5f * inch;
            const float bottomPageMargin = 0.5f * inch;

            var doc = new Document(PageSize.A4);

            // collect the card images to add to the pdf file
            var printList = new List<ProxyInfo>();
            var unprintedList = new List<string>();
            AddProxiesToPrintList(printList, unprintedList, deck.CardLists[1]);
            if (deck.Agenda != null)
                AddProxiesToPrintList(printList, unprintedList, deck.Agenda);
            if (printList.Count == 0)
                return unprintedList;

            // create the pdf document
            PdfWriter.GetInstance(doc, new FileStream(proxyFileName, FileMode.Create));
            doc.Open();

            // display the images on a 3x3 grid on each page
            int imageIndex = 0;
            foreach (var proxy in printList)
            {
                // prepare the image that may used many times (once per copy)
                var image = Image.GetInstance(proxy.ImagePath);
                if (proxy.MustRotate)
                {
                    image.Rotation = (float)Math.PI / 2;
                    image.ScaleAbsolute(cardHeight, cardWidth);
                }
                else
                    image.ScaleAbsolute(cardWidth, cardHeight);
                image.Border = Image.LEFT_BORDER | Image.RIGHT_BORDER | Image.TOP_BORDER | Image.BOTTOM_BORDER;
                image.BorderColor = BaseColor.BLACK;
                image.BorderWidth = 1;

                // add the copies to the page(s)
                for (var q = 0; q < proxy.Quantity; ++q)
                {
                    var x = (imageIndex % 3) * cardWidth + leftPageMargin;
                    var y = (((imageIndex % 9)) / 3) * cardHeight + bottomPageMargin;

                    image.SetAbsolutePosition(x, y);
                    doc.Add(image);

                    ++imageIndex;
                    if (imageIndex % 9 == 0 && (q < proxy.Quantity || proxy != printList.Last()))
                        doc.NewPage();
                }
            }
            doc.Close();
            return unprintedList;
        }

        /// <summary>
        /// Fills a "to print" list of cards with the path to the image, either it must be rotated or not (for plots)
        /// and the quantity of each card and an unprinted list for cards with no image found.
        /// </summary>
        /// <param name="printList">The list filled with cards to print.</param>
        /// <param name="unprintedList">The list filled with cards that can't be printed.</param>
        /// <param name="cardList">The list of cards to add to either list.</param>
        private static void AddProxiesToPrintList(IList<ProxyInfo> printList, IList<string> unprintedList, IEnumerable<AgotCard> cardList)
        {
            foreach (var card in cardList)
            {
                var imageFileNames = CardImageService.GetImageFileNames(ApplicationSettings.ImagesFolder, card.UniversalId, card.OctgnId);
                int index;
                if (CardImageService.GetImageAvailability(imageFileNames, out index) == ImageAvailability.Available)
                    printList.Add(new ProxyInfo
                    {
                        ImagePath = imageFileNames[index],
                        Quantity = card.Quantity,
                        MustRotate = card.Type != null && card.Type.Value == (int)AgotCard.CardType.Plot
                    });
                else
                {
                    var unprintedName = String.Format("{0} ({1})", card.Name);
                    if (!unprintedList.Contains(unprintedName))
                        unprintedList.Add(unprintedName);
                }
            }
        }
    }
}
