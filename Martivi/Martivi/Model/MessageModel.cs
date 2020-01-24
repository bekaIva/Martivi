﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Martivi.Model
{
    public class MessageModel
    {
        public string User { get; set; }
        public string Message { get; set; }
        public bool IsOwnMessage { get; set; }
        public bool IsSystemMessage { get; set; }
    }
}
