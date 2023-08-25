using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Extensions;

public static class Check
{
    public static string NotNullOrWhiteSpace(string value, string argName, int? maxLength = null, int? minLength = null)
    {
        if(string.IsNullOrWhiteSpace(value)) 
            throw new ArgumentNullException($"{argName} is null or white-space");

        if (minLength is not null && value.Length < minLength)
            throw new ArgumentException($"{argName} min length must be {minLength}");

        if (maxLength is not null && value.Length > maxLength)
            throw new ArgumentException($"{argName} exceed the max length: {maxLength}");

        return value;
    }
}
