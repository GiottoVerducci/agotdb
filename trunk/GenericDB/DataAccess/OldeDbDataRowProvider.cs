namespace GenericDB.DataAccess
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public class OldeDbDataRowProvider : IDataRowProvider, ICollection<IDataRow>
    {
        private readonly DataRowCollection _dataRowCollection;

        public OldeDbDataRowProvider(DataRowCollection dataRowCollection)
        {
            _dataRowCollection = dataRowCollection;
        }

        public string[] ColumnNames { get; private set; }

        public void Add(params object[] values)
        {
            _dataRowCollection.Add(values);
        }

        public IEnumerator<IDataRow> GetEnumerator()
        {
            return _dataRowCollection.Cast<DataRow>()
                .Select(dataRow => new DataRowWrapper(dataRow))
                .Cast<IDataRow>()
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

        public int Count { get { return _dataRowCollection.Count; } }

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
                return new DataRowWrapper(_dataRowCollection[index]);
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}