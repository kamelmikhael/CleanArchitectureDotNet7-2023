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
        public static AppError NotFound(string entityName, object id) => new AppError(
            "Record.NotFound",
            $"{entityName} with [{id}] not found.");
    }
}
