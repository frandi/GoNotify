using System;
using System.Collections.Generic;
using System.Text;

namespace GoNotify
{
    public class NotificationProviderOptions
    {
        public NotificationProviderOptions()
        {
            Parameters = new Dictionary<string, object>();
        }

        protected Dictionary<string, object> Parameters { get; }
    }
}
