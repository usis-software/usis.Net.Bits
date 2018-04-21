using System;
using System.Globalization;

namespace IZYTRON.IQ
{
    internal class HttpHeaderContentRange
    {
        #region properties

        //  -------------
        //  From property
        //  -------------

        public int From { get; private set; }

        //  -----------
        //  To property
        //  -----------

        public int To { get; private set; }

        #endregion properties

        #region construction

        //  ------------
        //  construction
        //  ------------

        private HttpHeaderContentRange() { }

        #endregion construction

        #region methods

        //  -----------------
        //  FromHeader method
        //  -----------------

        public static HttpHeaderContentRange FromHeader(string headerValue)
        {
            var tokens = headerValue.Split(' ');
            if (tokens.Length > 1 && "bytes".Equals(tokens[0], StringComparison.OrdinalIgnoreCase))
            {
                var values = tokens[1].Split('/');
                if (values.Length == 2)
                {
                    var range = values[0].Split('-');
                    if (range.Length == 2)
                    {
                        return new HttpHeaderContentRange()
                        {
                            From = int.Parse(range[0], NumberStyles.Integer, CultureInfo.InvariantCulture),
                            To = int.Parse(range[1], NumberStyles.Integer, CultureInfo.InvariantCulture)
                        };
                    }
                }
            }
            return null;
        }

        #endregion methods
    }
}
