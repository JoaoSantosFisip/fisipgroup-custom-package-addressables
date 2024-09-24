using UnityEditor;
using UnityEngine;
using FisipGroup.CustomPackage.Tools.Helpers;
using FisipGroup.CustomPackage.Addressables.Helpers;
using UnityEditor.AddressableAssets;

namespace FisipGroup.CustomPackage.Addressables.Editor
{
    /// <summary>
    /// Popup when building APK. 
    /// Select's the app's asset source -> Cloud or local.
    /// </summary>
    public static class AddressablesBadgePopup
    {
        [InitializeOnLoadMethod]
        private static void RegisterBuildHandler()
        {
            BuildPlayerWindow.RegisterBuildPlayerHandler(OpenMainCategoryPopup);
        }
        
        private static void OpenMainCategoryPopup(BuildPlayerOptions options)
        {
            var info = HelperCustomPackage.GetInfoFile<AddressablesInfoScriptableObject>("Addressables") as AddressablesInfoScriptableObject;
            var mainChoice = EditorUtility.DisplayDialogComplex(
                "Cloud content path",
                "What path do you want for the addressables?",
                "Cloud",
                "Local",
                "Cancel"
            );

            SetVersionName();

            switch (mainChoice)
            {
                // Cloud
                case 0:
#if UNITY_ANDROID
                    SetAddressableSettings(options, AddressablesPaths.RemoteBuildPath, AddressablesPaths.GetPathURL(info.projectID, info.androidBucketID, info.androidBadge));
#elif UNITY_IOS
                    SetAddressableSettings(options, AddressablesPaths.RemoteBuildPath, AddressablesPaths.GetPathURL(info.projectID, info.iosBucketID, info.iosBadge));
#endif
                    break;

                // Local
                case 1:
                    SetAddressableSettings(options, AddressablesPaths.LocalBuildPath, AddressablesPaths.LocalLoadPath);
                    break;

                // Cancel
                case 2:
                    return;

                default:
                    Debug.LogError("AddressablesSetRemoteURL.cs: Unrecognized option.");
                    break;
            }
        }

        private static void SetVersionName()
        {
#if UNITY_ANDROID
            PlayerSettings.bundleVersion = PlayerSettings.Android.bundleVersionCode.ToString();

#elif UNITY_IOS
            PlayerSettings.bundleVersion = PlayerSettings.iOS.buildNumber;
#endif
        }
        private static void SetAddressableSettings(BuildPlayerOptions options, string buildPath, string loadPath)
        {
            AddressableAssetSettingsDefaultObject.Settings.profileSettings
                .SetValue(AddressableAssetSettingsDefaultObject.Settings.activeProfileId, "Remote.BuildPath", buildPath);
            AddressableAssetSettingsDefaultObject.Settings.profileSettings
                .SetValue(AddressableAssetSettingsDefaultObject.Settings.activeProfileId, "Remote.LoadPath", loadPath);

            BuildPipeline.BuildPlayer(options);
        }
    }
}