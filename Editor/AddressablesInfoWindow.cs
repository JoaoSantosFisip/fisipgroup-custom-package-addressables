using UnityEditor;
//using UnityEngine;
//using FisipGroup.CustomPackage.Tools.Extensions;
using FisipGroup.CustomPackage.Tools.Helpers;
//using FisipGroup.CustomPackage.Tools.EditorTool;

namespace FisipGroup.CustomPackage.Addressables.Editor
{
    /// <summary>
    /// Editor window where the developer can insert the path for the addressables.
    /// </summary>
    public class AddressablesInfoWindow : EditorWindow
    {
        private static AddressablesInfoScriptableObject Info;

        private static readonly string PackageName = "Addressables";

        [MenuItem("FisipGroup/Addressables")]
        public static void ShowWindow()
        {
            HelperCustomPackage.CreateResourcesFolders(PackageName);

            Info = HelperCustomPackage.GetInfoFile<AddressablesInfoScriptableObject>(PackageName) as AddressablesInfoScriptableObject;

            Selection.activeObject = Info;

            //GetWindow<AddressablesInfoWindow>("FisipGroup Addressables");
        }

       //private void OnGUI()
       //{
       //    var textAreaHeight = EditorWindowStyles.InputTextStyle.lineHeight * 1;
       //
       //    // ----- App Info
       //    GUILayout.Label("App Info", EditorWindowStyles.TitleStyle);
       //    GUILayout.BeginHorizontal();
       //    GUILayout.Space(10);
       //    GUILayout.Label("Project ID", EditorWindowStyles.SectionStyle);
       //    Info.projectID = EditorGUILayout.TextArea(Info.projectID, EditorWindowStyles.InputTextStyle, GUILayout.Height(textAreaHeight));
       //    GUILayout.Space(10);
       //    GUILayout.EndHorizontal();
       //    GUILayout.BeginHorizontal();
       //    GUILayout.Space(10);
       //    GUILayout.Label("Environment ID", EditorWindowStyles.SectionStyle);
       //    Info.environmentID = EditorGUILayout.TextArea(Info.environmentID, EditorWindowStyles.InputTextStyle, GUILayout.Height(textAreaHeight));
       //    GUILayout.Space(10);
       //    GUILayout.EndHorizontal();
       //    // -----
       //
       //    // ----- IOS
       //    GUILayout.Label("IOS", EditorWindowStyles.TitleStyle);
       //    GUILayout.BeginHorizontal();
       //    GUILayout.Space(10);
       //    GUILayout.Label("Bucket ID", EditorWindowStyles.SectionStyle);
       //    Info.iosBucketID = EditorGUILayout.TextArea(Info.iosBucketID, EditorWindowStyles.InputTextStyle, GUILayout.Height(textAreaHeight));
       //    GUILayout.Space(10);
       //    GUILayout.EndHorizontal();
       //    GUILayout.BeginHorizontal();
       //    GUILayout.Space(10);
       //    GUILayout.Label("Release ID", EditorWindowStyles.SectionStyle);
       //    Info.iosReleaseID = EditorGUILayout.TextArea(Info.iosReleaseID, EditorWindowStyles.InputTextStyle, GUILayout.Height(textAreaHeight));
       //    GUILayout.Space(10);
       //    GUILayout.EndHorizontal();
       //    GUILayout.BeginHorizontal();
       //    GUILayout.Space(10);
       //    GUILayout.Label(AddressablesHelper.GetPathURL(Info.projectID, Info.iosBucketID, Info.iosReleaseID), EditorWindowStyles.SmallTextStyle);
       //    GUILayout.Space(10);
       //    GUILayout.EndHorizontal();
       //    // -----
       //
       //    // ----- Android
       //    GUILayout.Label("Android", EditorWindowStyles.TitleStyle);
       //    GUILayout.BeginHorizontal();
       //    GUILayout.Space(10);
       //    GUILayout.Label("Bucket ID", EditorWindowStyles.SectionStyle);
       //    Info.androidBucketID = EditorGUILayout.TextArea(Info.androidBucketID, EditorWindowStyles.InputTextStyle, GUILayout.Height(textAreaHeight));
       //    GUILayout.Space(10); 
       //    GUILayout.EndHorizontal();
       //    GUILayout.BeginHorizontal();
       //    GUILayout.Space(10);
       //    GUILayout.Label("Release ID", EditorWindowStyles.SectionStyle);
       //    Info.androidReleaseID = EditorGUILayout.TextArea(Info.androidReleaseID, EditorWindowStyles.InputTextStyle, GUILayout.Height(textAreaHeight));
       //    GUILayout.Space(10);
       //    GUILayout.EndHorizontal();
       //    GUILayout.BeginHorizontal();
       //    GUILayout.Space(10);
       //    GUILayout.Label(AddressablesHelper.GetPathURL(Info.projectID, Info.androidBucketID, Info.androidReleaseID), EditorWindowStyles.SmallTextStyle);
       //    GUILayout.Space(10);
       //    GUILayout.EndHorizontal();
       //    // -----
       //
       //    GUILayout.Space(10);
       //    GUILayout.Label("Tools", EditorWindowStyles.TitleStyle);
       //    if (GUILayout.Button("Save"))
       //    {
       //        var newInfoFile = (AddressablesInfoScriptableObject)CreateInstance(typeof(AddressablesInfoScriptableObject));
       //        newInfoFile.projectID = Info.projectID.RemoveWhitespaceLinesAndTabs();
       //        newInfoFile.environmentID = Info.environmentID.RemoveWhitespaceLinesAndTabs();
       //        newInfoFile.androidBucketID = Info.androidBucketID.RemoveWhitespaceLinesAndTabs();
       //        newInfoFile.androidReleaseID = Info.androidReleaseID.RemoveWhitespaceLinesAndTabs();
       //        newInfoFile.iosBucketID = Info.iosBucketID.RemoveWhitespaceLinesAndTabs();
       //        newInfoFile.iosReleaseID = Info.iosReleaseID.RemoveWhitespaceLinesAndTabs();
       //
       //        Info = HelperCustomPackage.SaveFileChanges(newInfoFile, PackageName) as AddressablesInfoScriptableObject;
       //    }
       //    if (!string.IsNullOrEmpty(Info.projectID) && !string.IsNullOrEmpty(Info.environmentID))
       //    {
       //        if (GUILayout.Button("Open Unity Services Cloud Content Page"))
       //        {
       //            Application.OpenURL(AddressablesHelper.GetCloudContentPageURL(Info.projectID, Info.environmentID));
       //        }
       //    }
       //}
    }
}