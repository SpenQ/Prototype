﻿using Mpb.Consensus.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mpb.Consensus.Logic.Exceptions
{
    public class TransactionRejectedException : Exception
    {
        public TransactionRejectedException()
        {
        }

        public TransactionRejectedException(string message) : base(message)
        {
        }

        public TransactionRejectedException(string message, AbstractTransaction t) : base(message)
        {
            Transaction = t;
        }

        public AbstractTransaction Transaction { get; }
    }
}
