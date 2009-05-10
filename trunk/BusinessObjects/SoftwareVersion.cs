// AGoTDB - A card searcher and deck builder tool for the CCG "A Game of Thrones"
// Copyright � 2007, 2008, 2009 Vincent Ripoll
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// You can contact me at v.ripoll@gmail.com
// � A Game of Thrones 2005 George R. R. Martin
// � A Game of Thrones CCG 2005 Fantasy Flight Games Inc.
// � Le Tr�ne de Fer JCC 2005-2007 Stratag�mes �ditions / X�nomorphe S�rl

using System;

namespace AGoT.AGoTDB.BusinessObjects
{
  public class SoftwareVersion : IComparable<SoftwareVersion>
  {
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Build { get; set; }
    public bool IsBeta { get { return Major == 0; } }

    public SoftwareVersion(int major, int minor, int build)
    {
      Major = major;
      Minor = minor;
      Build = build;
    }

    public int CompareTo(SoftwareVersion other)
    {
      return Math.Sign(
        Math.Sign(Major.CompareTo(other.Major))*4 +
        Math.Sign(Minor.CompareTo(other.Minor))*2 +
        Math.Sign(Build.CompareTo(other.Build)));
    }

    public override string ToString()
    {
      return String.Format("v. {0}.{1}.{2} {3}", Major, Minor, Build, IsBeta ? "(BETA)" : "");
    }

    /// <summary>
    /// Parse une cha�ne au format v.X.Y.Z ou X.Y.Z (avec Y et Z optionnels : X.Y est valide et Z vaudrait alors 0).
    /// </summary>
    /// <param name="s">La cha�ne repr�sentant la version.</param>
    /// <param name="result">La version repr�sent�e par la cha�ne.</param>
    /// <returns>True if the string was a valid representation of a software version.</returns>
    public static bool TryParse(string s, out SoftwareVersion result)
    {
      var versions = new int[3];
      var items = s.Split('.');
      var shift = 0;
      if(items[0].Trim().CompareTo("v") == 0)
        ++shift;
      bool parseResult = items.Length - shift > 0;
      for (var i = 0; (i < items.Length) && (i <= 3); ++i)
        parseResult &= Int32.TryParse(items[i + shift], out versions[i]);
      result = new SoftwareVersion(versions[0], versions[1], versions[2]);
      return parseResult;
    }
  }
}