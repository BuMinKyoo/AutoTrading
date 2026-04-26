using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoGetMoney.Common
{
    public class CustomMessage
    {
        public string EventName { get; set; }

        public CustomMessage(string eventName)
        {
            EventName = eventName;
        }
    }
}
