using System;
using AGoT.AGoTDB.Forms;

namespace AGoT.AGoTDB.BusinessObjects
{
  public class DatabaseInfo
  {
    public int VersionId { get; private set; }
    public DateTime? DateCreation { get; private set; }
    public SoftwareVersion MinimalAgotDbVersion { get; private set; }
    public string Comments { get; private set; }

    public DatabaseInfo(int versionId, DateTime? dateCreation, SoftwareVersion minimalAgotDbVersion, string comments)
    {
      VersionId = versionId;
      DateCreation = dateCreation;
      MinimalAgotDbVersion = minimalAgotDbVersion;
      Comments = comments;
    }
  }
}