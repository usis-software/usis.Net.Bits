using System.Collections.Generic;
using usis.FinTS.Base;
using usis.FinTS.DataElements;

namespace usis.FinTS.DataElementGroups
{
    internal sealed class Feedback : DataElementGroup
    {
        private DigitsDataElement code = new DigitsDataElement(4, LengthQualifier.Exact);
        //private AlphanumericDataElement reference = new AlphanumericDataElement(7, LengthQualifier.Maximum);
        private AlphanumericDataElement text = new AlphanumericDataElement(80, LengthQualifier.Maximum);
        private DataElementType parameterType = new DataElementType(BaseDataType.Alphanumeric, 35, LengthQualifier.Maximum);
        private List<DataElement> parameters = new List<DataElement>();
    }
}
