﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public class MongoDBSettings : DatabaseSettings
    {
        public string DatabaseName { get; set; }

    }
}
