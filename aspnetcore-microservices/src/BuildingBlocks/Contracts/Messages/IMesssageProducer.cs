﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Messages
{
    public interface IMesssageProducer
    {
        void SendMessage<T>(T message);
    }
}
