using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.Exceptions
{
    public class MissingParameterLessConstructorException : System.Exception
    {
        public MissingParameterLessConstructorException(Type type)
            : base($"{type.FullName} has no constructor without paramerters. This can be either public or private")
        { }
    }
}
