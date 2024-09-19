namespace FisipGroup.CustomPackage.Addressables.Helpers
{
    public static class AddressablesPaths
    {
        public static readonly string RemoteBuildPath = "ServerData/[BuildTarget]";
        public static readonly string LocalBuildPath = "[UnityEngine.AddressableAssets.Addressables.BuildPath]/[BuildTarget]";
        public static readonly string LocalLoadPath = "{UnityEngine.AddressableAssets.Addressables.RuntimePath}/[BuildTarget]";

        /// <summary>
        /// Get the url path of the cloud content bucket.
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="bucketID"></param>
        /// <param name="badgeID"></param>
        /// <returns></returns>
        public static string GetPathURL(string projectID, string bucketID, string badgeID)
        {
            return $"https://{projectID}" +
                $".client-api.unity3dusercontent.com/client_api/v1/environments/production/buckets/{bucketID}" +
                $"/release_by_badge/{badgeID}" +
                $"/entry_by_path/content/?path=";
        }
    }
}