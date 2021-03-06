﻿using System.Collections.Generic;

namespace GoNotify
{
    /// <summary>
    /// Message parameters to be used by the generic <see cref="INotification.Send(string, MessageParameterCollection)"/>
    /// </summary>
    public class MessageParameterCollection : Dictionary<string, string>
    {
    }
}
