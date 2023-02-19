using Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class Record
    {
        public static readonly AppError NotFound = new AppError(
            "Record.NotFound",
            $"Record not found.");
    }
}
