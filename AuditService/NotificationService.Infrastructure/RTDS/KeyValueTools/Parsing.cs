﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RTDS.KeyValueTools
{
    public enum Parsing
    {
        key,
        stringValue,
        endString,
        kvValue,
        anyKindOfValue,
    }

}
