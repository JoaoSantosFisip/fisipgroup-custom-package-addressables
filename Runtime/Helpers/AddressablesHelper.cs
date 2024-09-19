using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.AddressableAssets;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace FisipGroup.CustomPackage.Addressables.Helpers
{
    /// <summary>
    /// Methods to help on the development of Addressables scripts.
    /// </summary>
    public static class AddressablesHelper
    {
        private static readonly int RetryWaitTime = 5000; //In miliseconds

        /// <summary>
        /// Returns the IResourceLocation of an asset with specific labels.
        /// </summary>
        /// <param name="labels"></param>
        /// <param name="getFirst"></param>
        /// <returns></returns>
        public static async Task<IResourceLocation> GetAssetLocation(string[] labels, bool getFirst = false)
        {
            try
            {
                var operation = UnityEngine.AddressableAssets.Addressables.LoadResourceLocationsAsync(
                    labels.ToList(),
                    UnityEngine.AddressableAssets.Addressables.MergeMode.Intersection);

                await operation.Task;

                // Check if the loading was successful
                if (operation.Status == AsyncOperationStatus.Succeeded)
                {
                    if (operation.Result.Count == 1)
                    {
                        return operation.Result[0];
                    }
                    else if (operation.Result.Count == 0)
                    {
                        Debug.LogError("AddressablesHelper.cs: No Asset found with labels: " + string.Join(", ", labels));

                        return null;
                    }
                    else
                    {
                        if (getFirst)
                        {
                            return operation.Result[0];
                        }
                        else
                        {
                            Debug.LogError("AddressablesHelper.cs: More than one Asset found with labels: " + string.Join(", ", labels));

                            return null;
                        }
                    }
                }
                else
                {
                    if (AddressablesExceptionHandler.Handle(operation, "AddressablesHelper: " + string.Join(" ", labels))
                            == AddressableException.Network)
                    {
                        Thread.Sleep(RetryWaitTime);

                        return await GetAssetLocation(labels, getFirst);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (WebException ex)
            {
                Debug.LogWarning("AddressablesHelper.cs: WebException on GetAssetLocation, retrying in 5: "
                    + string.Join(" ", labels) + " - "
                    + ex.Status + " - " + ex.Message);

                Thread.Sleep(RetryWaitTime);

                return await GetAssetLocation(labels, getFirst);
            }
            catch (Exception ex)
            {
                Debug.LogError("AddressablesHelper.cs: Exception during GetAssetLocation: "
                        + ex.Message);

                return null;
            }
        }
       
        public static void UpdateProfileSettings(string remoteBuildPath, string remoteLoadPath)
        {
            AddressableAssetSettingsDefaultObject.Settings.profileSettings
                .SetValue(AddressableAssetSettingsDefaultObject.Settings.activeProfileId, "Remote.BuildPath", remoteBuildPath);
            AddressableAssetSettingsDefaultObject.Settings.profileSettings
                .SetValue(AddressableAssetSettingsDefaultObject.Settings.activeProfileId, "Remote.LoadPath", remoteLoadPath);
        }

        /// <summary>
        /// Get the url of the cloud content dashboard page.
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="environmentID"></param>
        /// <returns></returns>
        public static string GetCloudContentPageURL(string projectID, string environmentID)
        {
            return $"https://cloud.unity.com/home/organizations/5773479942694/projects/{projectID}" +
                $"/environments/{environmentID}" +
                $"/cloud-content-delivery";
        }
    }
}