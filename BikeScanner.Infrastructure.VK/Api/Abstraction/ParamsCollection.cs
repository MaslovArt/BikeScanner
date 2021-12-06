using System.Collections.Generic;

namespace BikeScanner.Infrastructure.VK.Api.Abstraction
{
    internal struct ParamsCollection<T>
    {
        private IEnumerable<T> _values;

        public ParamsCollection(IEnumerable<T> values)
        {
            _values = values;
        }

        public override string ToString()
        {
            return _values != null
                ? string.Join(',', _values)
                : "";
        }
    }
}
