﻿using System.Collections.Generic;
using System.Threading;
using Mpb.Consensus.Model;

namespace Mpb.Consensus.Logic.BlockLogic
{
    /// <summary>
    /// Create Proof-of-Work blocks
    /// </summary>
    public interface IPowBlockCreator
    {
        /// <summary>
        /// Mine a Proof-of-Work block by following the current consensus rules.
        /// </summary>
        /// <param name="transactions">The transactions that will be included in the new block</param>
        /// <param name="difficulty">The difficulty to start with. Must be atleast 1</param>
        /// <returns>A valid block that meets the consensus conditions</returns>
        Block CreateValidBlock(IEnumerable<AbstractTransaction> transactions, BigDecimal difficulty);

        /// <summary>
        /// Mine a Proof-of-Work block by following the current consensus rules.
        /// Throws OperationCanceledException when a cancellation was requested.
        /// </summary>
        /// <param name="transactions">The transactions that will be included in the new block</param>
        /// <param name="difficulty">The difficulty to start with. Must be atleast 1</param>
        /// <param name="ct">Optional cancellationtoken to be able to stop the mining process</param>
        /// <returns>A valid block that meets the consensus conditions</returns>
        Block CreateValidBlock(IEnumerable<AbstractTransaction> transactions, BigDecimal difficulty, CancellationToken ct);

        /// <summary>
        /// Mine a Proof-of-Work block with custom parameters.
        /// Throws OperationCanceledException when a cancellation was requested.
        /// </summary>
        /// <param name="netIdentifier">The net identifier for this block</param>
        /// <param name="protocolVersion">The current protocol version</param>
        /// <param name="transactions">The transactions that will be included in the new block</param>
        /// <param name="difficulty">The difficulty to start with. Must be atleast 1</param>
        /// <param name="maximumtarget">The maximum (easiest) target possible</param>
        /// <param name="ct">Optional cancellationtoken to be able to stop the mining process</param>
        /// <returns>A valid block that meets the consensus conditions, unless a different maximumTarget was given!</returns>
        Block CreateValidBlock(string netIdentifier, uint protocolVersion, IEnumerable<AbstractTransaction> transactions, BigDecimal difficulty, BigDecimal maximumTarget, CancellationToken ct);
    }
}