namespace GenericDB.BusinessObjects
{
    public interface IApplicationSettings
    {
        bool ImagesFolderExists { get; set; }
        string ImagesFolder { get; set; }
        bool IsOctgnReady { get; set; }
    }
}