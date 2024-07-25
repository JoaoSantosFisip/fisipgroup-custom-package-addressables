using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FisipGroup.CustomPackage.Addressables
{
    public static class AddressablesManager
    {
        public static UnityEvent<bool> OnCatalogsUpdate = new();

        private static readonly int RetryWaitTime = 5000; //In miliseconds

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void CheckForUpdate()
        {
            try
            {
                var operationCheckUpdate = UnityEngine.AddressableAssets.Addressables.CheckForCatalogUpdates();

                operationCheckUpdate.Completed += CheckForUpdate_OnCompleted;

                await operationCheckUpdate.Task;
            }
            catch (WebException ex)
            {
                Debug.LogWarning("AddressablesManager.cs: WebException on CheckForUpdate, retrying in 5: "
                    + ex.Status + " - " + ex.Message);

                Thread.Sleep(RetryWaitTime);

                CheckForUpdate();
            }
            catch (Exception ex)
            {
                Debug.LogError("AddressablesManager.cs: Exception during CheckForUpdate: "
                        + ex.Message);

                OnCatalogsUpdate?.Invoke(false);
            }
        }
        private static async void CheckForUpdate_OnCompleted(AsyncOperationHandle<List<string>> handle)
        {
            try
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    if (handle.Result.Count > 0)
                    {
                        Debug.Log("<color=cyan>AddressablesManager.cs: Catalogs updated count" + handle.Result.Count + "</color>");

                        var operationUpdate = UnityEngine.AddressableAssets.Addressables.UpdateCatalogs(handle.Result);
                        
                        operationUpdate.Completed += UpdateCatalogs_OnComplete;

                        await operationUpdate.Task;
                    }
                    else
                    {
                        OnCatalogsUpdate?.Invoke(true);
                    }
                }
                else
                {
                    if(AddressablesExceptionHandler.Handle(handle, "AddressablesManager") == AddressableException.Network)
                    {
                        Thread.Sleep(RetryWaitTime);

                        CheckForUpdate();
                    }
                    else
                    {
                        OnCatalogsUpdate?.Invoke(false);
                    }
                }
            }
            catch (WebException ex)
            {
                Debug.LogWarning("AddressablesManager.cs: WebException on CheckForUpdate_OnCompleted, retrying in 5: "
                    + ex.Status + " - " + ex.Message);

                Thread.Sleep(RetryWaitTime);

                CheckForUpdate();
            }
            catch (Exception ex)
            {
                Debug.LogError("AddressablesManager.cs: Exception during CheckForUpdate_OnCompleted: "
                        + ex.Message);

                OnCatalogsUpdate?.Invoke(false);
            }
        }
        private static void UpdateCatalogs_OnComplete(AsyncOperationHandle<List<IResourceLocator>> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                OnCatalogsUpdate?.Invoke(true);
            }
            else
            {
                if (AddressablesExceptionHandler.Handle(handle, "AddressablesManager") == AddressableException.Network)
                {
                    Thread.Sleep(RetryWaitTime);

                    CheckForUpdate();
                }
                else
                {
                    OnCatalogsUpdate?.Invoke(false);
                }
            }
        }

        /// <summary>
        /// Returns asset with specific labels. Awaitable version
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="labels"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
        public async static Task<T> GetAssetWithLabels<T>(string[] labels, Action<float> progress = null)
        {
            try
            {
                // Get the first one to prevent issues.
                var location = await AddressablesHelper.GetAssetLocation(labels, true);

                if (location != null)
                {
                    var operationHandle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(location);

                    if (progress != null)
                    {
                        while (!operationHandle.IsDone)
                        {
                            progress?.Invoke(operationHandle.PercentComplete);

                            await Task.Yield();
                        }
                    }
                    else
                    {
                        await operationHandle.Task;
                    }

                    // Check if the loading was successful
                    if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        return operationHandle.Result;
                    }
                    else
                    {
                        if (AddressablesExceptionHandler.Handle(operationHandle, "AddressablesManager_Task<T>GetAssetWithLabels<T>: " + string.Join(" ", labels)) 
                            == AddressableException.Network)
                        {
                            Thread.Sleep(RetryWaitTime);

                            return await GetAssetWithLabels<T>(labels);
                        }
                        else
                        {
                            return default;
                        }
                    }
                }
                else
                {
                    Debug.LogError("AddressablesManager.cs: Task<T> GetAssetWithLabels<T> Failed to get asset location: "
                        + string.Join(" ", labels));

                    return default;
                }
            }
            catch (WebException ex)
            {
                Debug.LogWarning("AddressablesManager.cs: WebException on Task<T> GetAssetWithLabels<T>: " + string.Join(" ", labels)
                    + ex.Status + " - " + ex.Message);

                Thread.Sleep(RetryWaitTime);

                return await GetAssetWithLabels<T>(labels);
            }
            catch (Exception ex)
            {
                Debug.LogError("AddressablesManager.cs: Exception during Task<T> GetAssetWithLabels<T>: " + string.Join(" ", labels)
                        + ex.Message);

                return default;
            }
        }
        /// <summary>
        /// Returns asset with specific labels. Callback version
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="labels"></param>
        /// <param name="callback"></param>
        /// <param name="progress"></param>
        public async static void GetAssetWithLabels<T>(string[] labels, Action<bool, T> callback, Action<float> progress = null)
        {
            try
            {
                // Get the first one to prevent issues.
                var location = await AddressablesHelper.GetAssetLocation(labels, true);

                if (location != null)
                {
                    var operationHandle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(location);

                    if (progress != null)
                    {
                        while (!operationHandle.IsDone)
                        {
                            progress?.Invoke(operationHandle.PercentComplete);

                            await Task.Yield();
                        }
                    }
                    else
                    {
                        await operationHandle.Task;
                    }

                    // Check if the loading was successful
                    if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        callback.Invoke(true, operationHandle.Result);
                    }
                    else
                    {
                        if (AddressablesExceptionHandler.Handle(operationHandle, "AddressablesManager_GetAssetWithLabels: " + string.Join(" ", labels))
                            == AddressableException.Network)
                        {
                            Thread.Sleep(RetryWaitTime);

                            GetAssetWithLabels(labels, callback);
                        }
                        else
                        {
                            callback?.Invoke(false, default);
                        }
                    }
                }
                else
                {
                    Debug.LogError("AddressablesManager.cs: GetAssetWithLabels Failed to get asset location: "
                        + string.Join(" ", labels));

                    callback?.Invoke(false, default);
                }
            }
            catch (WebException ex)
            {
                Debug.LogWarning("AddressablesManager.cs: WebException on GetAssetWithLabels: " + string.Join(" ", labels)
                    + ex.Status + " - " + ex.Message);

                Thread.Sleep(RetryWaitTime);

                GetAssetWithLabels(labels, callback);
            }
            catch (Exception ex)
            {
                Debug.LogError("AddressablesManager.cs: Exception during GetAssetWithLabels: " + string.Join(" ", labels)
                        + ex.Message);

                callback.Invoke(false, default);
            }
        }
        /// <summary>
        /// Returns assets with specific key. Callback version
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="callback"></param>
        /// <param name="progress"></param>
        public async static void GetAssetsWithKey<T>(string key, Action<bool, T[]> callback, Action<float> progress = null)
        {
            try
            {
                var operationHandle = UnityEngine.AddressableAssets.Addressables.LoadAssetsAsync<T>(key, null);

                if (progress != null)
                {
                    while (!operationHandle.IsDone)
                    {
                        progress?.Invoke(operationHandle.PercentComplete);

                        await Task.Yield();
                    }
                }
                else
                {
                    await operationHandle.Task;
                }

                // Check if the loading was successful
                if (operationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    callback?.Invoke(true, operationHandle.Result.ToArray());
                }
                else
                {
                    if (AddressablesExceptionHandler.Handle(operationHandle, "AddressablesManager")
                       == AddressableException.Network)
                    {
                        Thread.Sleep(RetryWaitTime);

                        GetAssetsWithKey(key, callback);
                    }
                    else
                    {
                        callback?.Invoke(false, default);
                    }
                }
            }
            catch (WebException ex)
            {
                Debug.LogWarning("AddressablesManager.cs: WebException on GetAssetsWithKey<T>, retrying in 5: "
                    + key + " - "
                    + ex.Status + " - " + ex.Message);

                Thread.Sleep(RetryWaitTime);

                GetAssetsWithKey(key, callback);
            }
            catch (Exception ex)
            {
                Debug.LogError("AddressablesManager.cs: Exception during GetAssetsWithKey<T>: "
                        + ex.Message);

                callback?.Invoke(false, default);
            }
        }
    }
}