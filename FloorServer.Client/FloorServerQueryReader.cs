#region

using System;
using System.Collections.Specialized;
using FloorServer.Client.Tools;

#endregion

namespace FloorServer.Client
{
    public class FloorServerQueryReader : IQueryReader
    {
        #region Member

        private int? _index;
        private readonly FloorQueryResponse _result;
        private readonly IQueryReaderHelper _helper = new FloorServerQueryReaderHelper();

        #endregion

        #region Properties

        public StringDictionary Current
        {
            get
            {
                if (_result.Rows.Count == 0)
                    throw new InvalidOperationException("Result has no value");

                if (!_index.HasValue)
                    throw new InvalidOperationException("You have to read first before getting data");

                return _result.Rows[_index.Value];
            }
        }

        #endregion

        #region Constructor

        public FloorServerQueryReader(FloorQueryResponse result)
        {
            if (result == null)
                throw new ArgumentNullException("result");

            _result = result;
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _result.Rows.Clear();
        }

        #endregion

        #region Implementation of IQueryReader

        public bool Read()
        {
            if (_result.Rows.Count == 0)
                return false;

            if (!_index.HasValue)
            {
                _index = 0;
                return true;
            }

            _index++;

            return _index < _result.Rows.Count;
        }

        public IQueryReaderHelper GetHelper()
        {
            return _helper;
        }

        #endregion
    }
}
