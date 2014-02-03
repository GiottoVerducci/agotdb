namespace GenericDB.DataAccess
{
    public interface IDataRow   
    {
        object this[int index] { get; set; }
        object this[string columnName] { get; set; }
    }
}