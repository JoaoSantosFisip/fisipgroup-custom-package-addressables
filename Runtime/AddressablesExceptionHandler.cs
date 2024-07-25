using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FisipGroup.CustomPackage.Addressables
{
    /// <summary>
    /// Contains methods related to AsyncOperationHandle handling.
    /// </summary>
    public static class AddressablesExceptionHandler
    {
        /// <summary>
        /// Handles the exception responses from the addressable methods.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static AddressableException Handle(AsyncOperationHandle operation, string origin)
        {
            if(operation.OperationException.Message.Contains("RemoteProviderException : Unable to load asset bundle"))
            {
                Debug.LogWarning("RemoteProviderException on " + origin + ": "
                        + operation.OperationException.Message + " - " + operation.DebugName);

                return AddressableException.Network;
            }
            if(operation.OperationException.Message.Contains("ConnectionError :"))
            {
                Debug.LogWarning("Connection error exception on " + origin + ": "
                        + operation.OperationException.Message + " - " + operation.DebugName);

                return AddressableException.Network;
            }
            if (operation.OperationException.Message.Contains("ChainOperation failed because dependent operation failed"))
            {
                Debug.LogWarning("ChainOperation error exception on " + origin + ": "
                        + operation.OperationException.Message + " - " + operation.DebugName);

                return AddressableException.Network;
            }
            if (operation.OperationException.Message.Contains("Dependency Exception"))
            {
                Debug.LogWarning("Depedency error exception on " + origin + ": "
                        + operation.OperationException.Message + " - " + operation.DebugName);

                return AddressableException.Network;
            }
            else
            {
                Debug.LogError("Unhandled Exception on " + origin + ": "
                       + operation.OperationException.Message + " - " + operation.DebugName);

                return AddressableException.Unhandled;
            }
        }
    }
}