﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using Mpb.Consensus.Model;
using System.Security.Cryptography;
using Mpb.Consensus.Logic.Exceptions;
using System.Linq;
using Mpb.Consensus.Logic.TransactionLogic;
using Mpb.Consensus.Logic.MiscLogic;

namespace Mpb.Consensus.Logic.BlockLogic
{
    public class PowBlockValidator : IBlockValidator
    {
        private readonly IBlockHeaderHelper _blockHeaderHelper;
        private readonly ITransactionValidator _transactionValidator;
        private readonly ITimestamper _timestamper;

        public PowBlockValidator(IBlockHeaderHelper blockHeaderHelper, ITransactionValidator transactionValidator, ITimestamper timestamper)
        {
            _blockHeaderHelper = blockHeaderHelper ?? throw new ArgumentNullException(nameof(blockHeaderHelper));
            _transactionValidator = transactionValidator ?? throw new ArgumentNullException(nameof(transactionValidator));
            _timestamper = timestamper ?? throw new ArgumentNullException(nameof(timestamper));
        }

        //! Decorator/composite pattern could be possible here. Only check for PoW things, then call the parent for more generic checks
        public virtual void ValidateBlock(Block block, BigDecimal currentTarget, bool setBlockHash)
        {
            if (setBlockHash)
            {
                block.SetHash(GetBlockHash(block));
            }
            else if (block.Hash == null)
            {
                throw new ArgumentNullException(nameof(block.Hash));
            }
            else if (block.Hash != GetBlockHash(block))
            {
                throw new BlockRejectedException("The hash property of the block is not equal to the calculated hash", block);
            }
            
            BigDecimal hashValue = BigInteger.Parse(block.Hash, NumberStyles.HexNumber);

            // Hash value must be lower than the target and the first byte must be zero
            // because the first byte indidates if the hashValue is a positive or negative number,
            // negative numbers are not allowed.
            if (!block.Hash.StartsWith("0"))
            {
                throw new BlockRejectedException("Hash has no leading zero", block);
            }

            // The hash value must be lower than the given target
            if (hashValue >= currentTarget)
            {
                throw new BlockRejectedException("Hash value is equal or higher than the current target", block);
            }

            // Timestamp must not be lower than UTC - 2 min and not higher than UTC + 2 min
            // Todo refactor 120 seconds to blockchainconstant
            if (_timestamper.GetCurrentUtcTimestamp() - block.Timestamp > 120 || _timestamper.GetCurrentUtcTimestamp() - block.Timestamp < -120)
            {
                throw new BlockRejectedException("Timestamp is not within the acceptable range of -120 seconds and +120 seconds", block);
            }

            // Transaction list may not be empty
            if (block.Transactions.Count() == 0)
            {
                throw new BlockRejectedException("Transaction list cannot be empty", block);
            }

            // Check merkleroot

            // First transaction must be coinbase

            // Check all other transactions

            // Check if the previous hash exists in our blockchain
            // Todo if the previous hash is unknown, let the network module ask the entire 

            // Check if the previous hash isn't used by another block
            // If it is used by another block, and that block is the latest 
        }

        private string GetBlockHash(Block b)
        {
            using (var sha256 = SHA256.Create())
            {
                var blockHash = sha256.ComputeHash(_blockHeaderHelper.GetBlockHeaderBytes(b));
                return BitConverter.ToString(blockHash).Replace("-", "");
            }
        }
    }
}
