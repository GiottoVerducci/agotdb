namespace GenericDB.DataAccess
{
    using System.Collections.Generic;

    public interface IDataRowProvider : IList<IDataRow>
    {
        string[] ColumnNames { get; }
        void Add(params object[] values);
    }
}