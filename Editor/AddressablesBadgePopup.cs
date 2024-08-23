using UnityEditor;
using UnityEngine;
using UnityEditor.AddressableAssets;
using FisipGroup.CustomPackage.Tools.Helpers;

namespace FisipGroup.CustomPackage.Addressables.Editor
{
    /// <summary>
    /// Popup when building APK. 
    /// Select's the app's asset source -> Cloud or local.
    /// </summary>
    public static class AddressablesBadgePopup
    {
        private static readonly string RemoteBuildPath = "ServerData/[BuildTarget]";
        private static readonly string LocalBuildPath = "[UnityEngine.AddressableAssets.Addressables.BuildPath]/[BuildTarget]";
        private static readonly string LocalLoadPath = "{UnityEngine.AddressableAssets.Addressables.RuntimePath}/[BuildTarget]";

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
                    SetAddressableSettings(options, RemoteBuildPath, AddressablesHelper.GetPathURL(info.projectID, info.androidBucketID, info.androidReleaseID));
#elif UNITY_IOS
                    SetAddressableSettings(options, RemoteBuildPath, AddressablesHelper.GetPathURL(info.projectID, info.iosBucketID, info.iosReleaseID));
#endif
                    break;

                // Local
                case 1:
                    SetAddressableSettings(options, LocalBuildPath, LocalLoadPath);
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
            PlayerSettings.bundleVersion = GetVersionName();
        }
        private static void SetAddressableSettings(BuildPlayerOptions options, string buildPath, string loadPath)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            settings.profileSettings.SetValue(settings.activeProfileId, "Remote.BuildPath", buildPath);
            settings.profileSettings.SetValue(settings.activeProfileId, "Remote.LoadPath", loadPath);

            BuildPipeline.BuildPlayer(options);
        }

        private static string GetVersionName()
        {
#if UNITY_ANDROID
            return PlayerSettings.Android.bundleVersionCode.ToString();

#elif UNITY_IOS
            return PlayerSettings.iOS.buildNumber;
#endif
        }
    }
}