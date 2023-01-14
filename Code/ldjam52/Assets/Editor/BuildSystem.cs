using System.Collections;
using System.Collections.Generic;
using System.IO;

using Assets.Scripts.Constants;

using Newtonsoft.Json;

using UnityEditor;

using UnityEngine;

public static class BuildSystem
{
    //static string[] sceneList = SceneNames.;
    class BuildInfo
    {
        public string GameVersion = Application.version;
    }

    private static string locationPath = "../../Deployments/web/Deployment";

    public static void BuildProject()
    {
        string prefix = "Assets/Scenes/";
        string[] scenes = new string[] { prefix + SceneNames.MainMenu + ".unity", prefix + SceneNames.World + ".unity" };
        var report = BuildPipeline.BuildPlayer(scenes, locationPath, BuildTarget.WebGL, BuildOptions.Development);
        Debug.Log($"Build result: {report.summary.result}, {report.summary.totalErrors} errors");
        var indexAsJson = GameFrame.Core.Json.Handler.Serialize(new BuildInfo(), Formatting.None, new JsonSerializerSettings());
        File.WriteAllTextAsync(locationPath + "/BuildInfo.json", indexAsJson);
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
