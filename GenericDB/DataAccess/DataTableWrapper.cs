namespace GenericDB.DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public class DataTableWrapper : IDataRowProvider
    {
        private readonly DataTable _dataTable;

        public DataTableWrapper(DataTable dataTable)
        {
            _dataTable = dataTable;
        }

        public IEnumerator<IDataRow> GetEnumerator()
        {
            return _dataTable.Rows.Cast<DataRow>()
                .Select(dataRow => new DataRowWrapper(dataRow))
                .Cast<IDataRow>()
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string[] ColumnNames
        {
            get
            {
                return _dataTable.Columns.Cast<object>().Select(column => column.ToString()).ToArray();
            }
        }

        public void Add(params object[] values)
        {
            _dataTable.Rows.Add(values);
        }

        public void Add(IDataRow item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(IDataRow item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IDataRow[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IDataRow item)
        {
            throw new NotImplementedException();
        }

        public int Count { get { return _dataTable.Rows.Count; } }

        public bool IsReadOnly { get { return false; } }

        public int IndexOf(IDataRow item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, IDataRow item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public IDataRow this[int index]
        {
            get
            {
                return new DataRowWrapper(_dataTable.Rows[index]);
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}