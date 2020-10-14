using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;

public class EditorScripts : MonoBehaviour
{
	[PostProcessBuild (999)]
	public static void OnPostProcessBuild (BuildTarget buildTarget, string path)
	{
#if UNITY_IOS

        if (buildTarget == BuildTarget.iOS)
        {
            string projectPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

            string mainTargetGuid;

            string unityFrameworkTargetGuid;

            PBXProject pbxProject = new PBXProject();

            var unityMainTargetGuidMethod = pbxProject.GetType().GetMethod("GetUnityMainTargetGuid");
            var unityFrameworkTargetGuidMethod = pbxProject.GetType().GetMethod("GetUnityFrameworkTargetGuid");

            pbxProject.ReadFromFile(projectPath);

            if (unityMainTargetGuidMethod != null && unityFrameworkTargetGuidMethod != null)
            {
                mainTargetGuid = (string)unityMainTargetGuidMethod.Invoke(pbxProject, null);
                unityFrameworkTargetGuid = (string)unityFrameworkTargetGuidMethod.Invoke(pbxProject, null);
                pbxProject.SetBuildProperty(unityFrameworkTargetGuid, "ENABLE_BITCODE", "NO");
            }

            else
            {
                mainTargetGuid = pbxProject.TargetGuidByName("Unity-iPhone");
                unityFrameworkTargetGuid = mainTargetGuid;
                pbxProject.SetBuildProperty(mainTargetGuid, "ENABLE_BITCODE", "NO");
                pbxProject.SetBuildProperty(unityFrameworkTargetGuid, "ENABLE_BITCODE", "NO");
            }

            pbxProject.WriteToFile(projectPath);
        }
#endif
    }
}