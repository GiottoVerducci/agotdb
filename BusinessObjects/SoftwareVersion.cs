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
    /// Parse une chaîne au format v.X.Y.Z ou X.Y.Z (avec Y et Z optionnels : X.Y est valide et Z vaudrait alors 0).
    /// </summary>
    /// <param name="s">La chaîne représentant la version.</param>
    /// <param name="result">La version représentée par la chaîne.</param>
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