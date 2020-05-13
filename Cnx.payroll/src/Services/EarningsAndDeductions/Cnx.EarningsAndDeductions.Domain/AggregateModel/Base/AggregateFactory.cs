using Cnx.EarningsAndDeductions.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.AggregatesModel.Base
{
    public static class AggregateFactory
    {
        public static T CreateAggregate<T>()
        {
            try
            {
 
                return (T)Activator.CreateInstance(typeof(T), true);
 
            }
            catch (MissingMethodException)
            {
                throw new MissingParameterLessConstructorException(typeof(T));
            }
        }
    }
}
