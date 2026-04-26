using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGetMoney.Common
{
    public class CustomEvent
    {
        public string EventName { get; set; }
        public object Message { get; set; }

        public CustomEvent(string eventName, object message)
        {
            EventName = eventName;
            Message = message;
        }
    }
}
