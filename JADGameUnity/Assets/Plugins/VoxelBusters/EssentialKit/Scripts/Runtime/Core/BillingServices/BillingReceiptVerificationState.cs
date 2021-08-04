using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// The state of a payment receipt verification. 
    /// </summary>
    public enum BillingReceiptVerificationState
    {
        /// <summary> Receipt verification has not yet been done.</summary>
        NotDetermined,

        /// <summary> Receipt was successfully verified.</summary>
        Success,

        /// <summary> Receipt verification failed for some reason. Possible reasons can be network issue, mismatch of app build details etc.</summary>
        Failed,
    }
}