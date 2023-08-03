#if UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;
#if Analytics
using Facebook.Unity;
#endif

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;

namespace Baruch
{
    public class PostProcess
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.iOS)
            {
                string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
                PBXProject project = new PBXProject();
                project.ReadFromString(File.ReadAllText(projectPath));

                string xcodeTarget = project.GetUnityFrameworkTargetGuid();


                EditInfoPlist(pathToBuiltProject);

                var targetGuid = project.GetUnityMainTargetGuid();

                project.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
                project.SetBuildProperty(xcodeTarget, "ENABLE_BITCODE", "NO");
                project.SetBuildProperty(xcodeTarget, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");

                // Write.
                File.WriteAllText(projectPath, project.WriteToString());
            }
        }

        private static void EditInfoPlist(string pathToBuiltProject)
        {
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;

            // ATT
            rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);
#if Analytics
            rootDict.SetString("FacebookClientToken", Facebook.Unity.Settings.FacebookSettings.ClientToken);
#endif


            File.WriteAllText(plistPath, plist.WriteToString());
        }

    }
}
#endif
