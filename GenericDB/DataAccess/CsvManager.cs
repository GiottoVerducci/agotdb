using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDB.DataAccess
{
    using System.Data;
    using System.IO;

    public class CsvManager
    {
        private readonly string _path;
        private readonly char _delimiter;
        private readonly char _sequenceDelimiter;

        public bool HasHeader { get; private set; }

        public CsvManager(string path, char delimiter, char sequenceDelimiter, bool hasHeader)
        {
            _path = path;
            _delimiter = delimiter;
            _sequenceDelimiter = sequenceDelimiter;
            this.HasHeader = hasHeader;

            if (_delimiter == _sequenceDelimiter)
                throw new Exception("Can't use the same character as field delimiter and sequence delimiter.");
            if (_delimiter == '\r' || _delimiter == '\n')
                throw new Exception("Can't use \\r or \\n as field delimiter.");
            if (_sequenceDelimiter == '\r' || _sequenceDelimiter == '\n')
                throw new Exception("Can't use \\r or \\n as sequence delimiter.");
            this.Data = LoadData();
        }

        public IDataRowProvider Data { get; private set; }

        private string _bulk;
        private int _position;
        private bool _isInSequence;
        private int _lineCount;

        private IDataRowProvider LoadData()
        {
            if (!File.Exists(_path))
                throw new FileNotFoundException(string.Format("Couldn't find file: {0}", _path));

            var csvDataTable = new DataTable();

            using (var streamReader = new StreamReader(_path, Encoding.UTF8))
            {
                _bulk = streamReader.ReadToEnd();
                _position = 0;
                _isInSequence = false;
                _lineCount = 0;
            }

            IList<string> rowItems;
            if (this.HasHeader)
            {
                rowItems = GetRowItems();
                var duplicates = rowItems.GroupBy(s => s).SelectMany(grp => grp.Skip(1));
                if (duplicates.Any())
                {
                    throw new Exception(string.Format("Header contains duplicate columns: {0}", string.Join(", ", duplicates)));
                }
                foreach (string columnName in rowItems)
                    csvDataTable.Columns.Add(new DataColumn((columnName), typeof(string)));
            }

            while ((rowItems = GetRowItems()) != null)
            {
                var csvDataRow = csvDataTable.NewRow();
                csvDataRow.ItemArray = rowItems.ToArray();
                csvDataTable.Rows.Add(csvDataRow);
            }
            return new DataTableWrapper(csvDataTable);
        }

        public IList<string> GetRowItems()
        {
            if (_position >= _bulk.Length)
                return null;

            var result = new List<string>();
            var current = new StringBuilder();
            do
            {
                var character = _bulk[_position];
                if (_isInSequence)
                {
                    if (character == _sequenceDelimiter)
                    {
                        if (_position + 1 == _bulk.Length || _bulk[_position + 1] != _sequenceDelimiter) // no second delimiter (=espace sequence)
                        {
                            _isInSequence = false;
                            result.Add(current.ToString());
                            current.Clear();
                            _position++;
                            if (_position == _bulk.Length || (character = _bulk[_position]) == '\n')
                            {
                                _position++;
                                _lineCount++;
                                return result;
                            }
                            if (character == '\r')
                            {
                                _position++;
                                if (_position == _bulk.Length || _bulk[_position] == '\n')
                                {
                                    _position++;
                                    _lineCount++;
                                    return result;
                                }
                                throw new Exception(string.Format("Invalid line return at line {0}.", _lineCount + 1));
                            }
                            if (character != _delimiter)
                            {
                                throw new Exception(string.Format("Expected delimiter '{0}' after sequence delimiter '{1}' at line {2}", _delimiter, _sequenceDelimiter, _lineCount + 1));
                            }
                            _position++;
                        }
                        else
                        {
                            current.Append(_sequenceDelimiter);
                            _position += 2;
                        }
                    }
                    else
                    {
                        current.Append(character);
                        _position++;
                    }
                }
                else // not in a sequence
                {
                    if (character == _sequenceDelimiter)
                    {
                        if (current.Length == 0)
                        {
                            _isInSequence = true;
                            ++_position;
                            continue;
                        }
                        else if (_position < _bulk.Length && _bulk[_position + 1] != _sequenceDelimiter)
                        {
                            throw new Exception(string.Format("Sequence delimiter '{0}' cannot be used alone in the middle of a sequence at line {1}.", _sequenceDelimiter, _lineCount + 1));
                        }
                        current.Append(_sequenceDelimiter);
                        _position += 2;
                    }
                    else if (character == '\n')
                    {
                        result.Add(current.ToString());
                        _position++;
                        _lineCount++;
                        return result;
                    }
                    else if (character == '\r')
                    {
                        _position++;
                        if (_position == _bulk.Length || _bulk[_position] == '\n')
                        {
                            result.Add(current.ToString());
                            _position++;
                            _lineCount++;
                            return result;
                        }
                        throw new Exception(string.Format("Invalid line return at line {0}.", _lineCount + 1));
                    }
                    else if (character == _delimiter)
                    {
                        _position++;
                        result.Add(current.ToString());
                        current.Clear();
                    }
                    else
                    {
                        current.Append(character);
                        _position++;
                    }
                }
            }
            while (_position < _bulk.Length);

            if (current.Length > 0)
                result.Add(current.ToString());
            return result;
        }
    }
}
