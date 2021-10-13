using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Exceptions
{
    public class SchedulerException : Exception
    {
        public SchedulerException() { }

        public SchedulerException(string message)
            : base(message)
        { }
    }
}
