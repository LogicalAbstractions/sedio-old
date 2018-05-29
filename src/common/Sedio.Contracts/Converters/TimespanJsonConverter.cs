using System;
using System.Xml;
using Newtonsoft.Json;
using Sedio.Core.Converters;

namespace Sedio.Contracts.Converters
{
    public sealed class TimespanJsonConverter : AbstractStringJsonConverter<TimeSpan>
    {
        protected override bool OnFromString(string value, out TimeSpan result)
        {
            try
            {
                result = XmlConvert.ToTimeSpan(value);
                return true;
            }
            catch
            {
                result = default(TimeSpan);
                return false;
            }
        }

        protected override string OnToString(TimeSpan value)
        {
            return XmlConvert.ToString(value);
        }
    }
}