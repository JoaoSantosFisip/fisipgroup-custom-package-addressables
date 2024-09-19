using UnityEngine;

namespace FisipGroup.CustomPackage.Addressables
{
    /// <summary>
    /// Settings for the addressables custom package
    /// </summary>
    public class AddressablesInfoScriptableObject : ScriptableObject
    {
        [Header("App")]
        public string projectID;
        public string environmentID;
        [Header("IOS")]
        public string iosBucketID;
        public string iosBadge;
        [Header("ANDROID")]
        public string androidBucketID;
        public string androidBadge;
    }
}