using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class ProjectBuilder : Editor
{
    static string[] SCENES = FindEnabledEditorScenes();
    static string TARGET_DIR = "Build";

    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }

    static void GenericBuild(string[] scenes, string target_path, BuildTarget build_target, BuildOptions build_option)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        string res = BuildPipeline.BuildPlayer(scenes, target_path, build_target, build_option);
        if(res.Length > 0)
        {
            throw new System.Exception("BuildPlayer failure: " + res);
        }
    }

    [MenuItem("Custom/CI/Build iOS")]
    static void PerformiOSBuild()
    {
        BuildOptions opt = BuildOptions.None;

        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        PlayerSettings.iOS.targetOSVersion = iOSTargetOSVersion.iOS_4_3;
        PlayerSettings.statusBarHidden = true;

        char sep = System.IO.Path.DirectorySeparatorChar;
        string buildDirectory = System.IO.Path.GetFullPath(".") + sep + TARGET_DIR;
        System.IO.Directory.CreateDirectory(TARGET_DIR + "/iOS");

        string BUILD_TARGET_PATH = buildDirectory + "/iOS";
        GenericBuild(SCENES, BUILD_TARGET_PATH, BuildTarget.iOS, opt);
    }

    [MenuItem("Custom/CI/Build Android")]
    static void PerformAndroidBuild()
    {
        BuildOptions opt = BuildOptions.None;

        char sep = System.IO.Path.DirectorySeparatorChar;
        string BUILD_TARGET_PATH = System.IO.Path.GetFullPath(".") + sep + TARGET_DIR + string.Format("/AndroidBuild_{0}.apk",PlayerSettings.bundleVersion);

        GenericBuild(SCENES, BUILD_TARGET_PATH, BuildTarget.Android, opt);
    }
}
