namespace GenericDB.DataAccess
{
    using System.Data;

    public class DataRowWrapper : IDataRow
    {
        private readonly DataRow _dataRow;

        public DataRowWrapper(DataRow dataRow)
        {
            _dataRow = dataRow;
        }

        public object this[int index]
        {
            get
            {
                return _dataRow[index];
            }
            set
            {
                _dataRow[index] = value;
            }
        }

        public object this[string columnName]
        {
            get
            {
                return _dataRow[columnName];
            }
            set
            {
                _dataRow[columnName] = value;
            }
        }
    }
}