using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.Constants;

using UnityEditor;

using UnityEngine;

public static class BuildSystem
{
    //static string[] sceneList = SceneNames.;

    public static void BuildProject()
    {
        string prefix = "Assets/Scenes/";
        string[] scenes = new string[] { prefix + SceneNames.MainMenu + ".unity", prefix + SceneNames.World + ".unity" };
        var report = BuildPipeline.BuildPlayer(scenes, "../../Deployments/web/Deployment", BuildTarget.WebGL, BuildOptions.None);
        Debug.Log($"Build result: {report.summary.result}, {report.summary.totalErrors} errors");
        if (report.summary.totalErrors > 0)
            EditorApplication.Exit(1);
    }

    public static void BuildWebGLHeadless()
    {
        var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions()
        {
            locationPathName = "WEBGLDEPLOYMENT",
            //scenes = sceneList,
            target = BuildTarget.WebGL,


        });

        Debug.Log($"Build result: {report.summary.result}, {report.summary.totalErrors} errors");
        if (report.summary.totalErrors > 0)
            EditorApplication.Exit(1);
    }
}
