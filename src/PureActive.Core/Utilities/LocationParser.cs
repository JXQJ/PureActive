
using PureActive.Core.Extensions;

namespace PureActive.Core.Utilities
{
    public class LocationParser
    {
        private string _location;

        public LocationParser(string location)
        {
            Location = location;
        }

        public string Location
        {
            get => _location;
            set
            {
                _location = value?.Trim().Replace(",,", ",");

                if (string.IsNullOrEmpty(_location))
                {
                    City = null;
                    State = null;
                }
                else
                {
                    var cityStateParsed = _location.SplitOnFirstDelim(',');

                    City = cityStateParsed[0];
                    State = cityStateParsed[1];
                }
            }
        }

        public string City { get; private set; }

        public string State { get; private set; }
    }
}