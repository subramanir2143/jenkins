using System;
using System.Collections.Generic;
using System.Text;

namespace Cnx.EarningsAndDeductions.Domain.AggregatesModel.Base
{
    public class AggregateRead  
    {
        private int _version = -1;
        public int Version
        {
            get
            {
                return _version;
            }
            protected set
            {
                _version = value;
            }
        }

    }
}
