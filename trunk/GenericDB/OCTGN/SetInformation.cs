using System;

namespace GenericDB.OCTGN
{
    public class SetInformation
    {
        public string OriginalName;
        public string OctgnName;
        public int SetId;
        public bool ByChapter;
        public string ShortName;
        public string[] ChaptersNames;

        public int GetChapterId(string chapterName)
        {
            for (int i = 0; i < ChaptersNames.Length; ++i)
                if (string.Equals(ChaptersNames[i], chapterName, StringComparison.InvariantCultureIgnoreCase))
                    return i + 1;
            return 0;
        }
    }
}