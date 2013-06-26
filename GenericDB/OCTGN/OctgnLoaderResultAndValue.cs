namespace GenericDB.OCTGN
{
    public class OctgnLoaderResultAndValue
    {
        public OctgnLoaderResult Result;
        public object Value;
    }

    public enum OctgnLoaderResult
    {
        Undefined,
        Success,
        NoSetsFounds,
        SetNotDefinedInDatabase
    }
}