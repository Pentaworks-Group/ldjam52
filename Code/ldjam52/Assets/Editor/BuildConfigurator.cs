using System.Collections;
using System.Collections.Generic;
using System.IO;

using Assets.Scripts.Constants;

using Newtonsoft.Json;

using UnityEditor;

using UnityEngine;

public static class BuildConfigurator
{
    //static string[] sceneList = SceneNames.;
    class BuildInfo
    {
        public string GameVersion = Application.version;
    }

    private static string locationPath = "../../Deployments/web/Deployment";
    private static string prefix = "Assets/Scenes/";
    private static string postfix = ".unity";

    public static void BuildProjectDevelopment()
    {
        var report = BuildPipeline.BuildPlayer(getSceneNameArray(SceneNames.scenes, SceneNames.scenesDevelopment), locationPath, BuildTarget.WebGL, BuildOptions.Development);
        Debug.Log($"Build result: {report.summary.result}, {report.summary.totalErrors} errors");
        var indexAsJson = GameFrame.Core.Json.Handler.Serialize(new BuildInfo(), Formatting.None, new JsonSerializerSettings());
        File.WriteAllTextAsync(locationPath + "/BuildInfo.json", indexAsJson);
        if (report.summary.totalErrors > 0)
            EditorApplication.Exit(1);
    }

    public static void BuildProjectProduction()
    {
        var report = BuildPipeline.BuildPlayer(getSceneNameArray(SceneNames.scenes, default), locationPath, BuildTarget.WebGL, BuildOptions.None);
        Debug.Log($"Build result: {report.summary.result}, {report.summary.totalErrors} errors");
        var indexAsJson = GameFrame.Core.Json.Handler.Serialize(new BuildInfo(), Formatting.None, new JsonSerializerSettings());
        File.WriteAllTextAsync(locationPath + "/BuildInfo.json", indexAsJson);
        if (report.summary.totalErrors > 0)
            EditorApplication.Exit(1);
    }
    private static string[] getSceneNameArray(List<string> sceneList1, List<string> sceneList2)
    {
        List<string> scenes = new List<string>();
        scenes.AddRange(sceneList1);
        if (sceneList2 != default)
        {
            scenes.AddRange(sceneList2);
        }

        string[] sceneArray = new string[scenes.Count];

        for (int i = 0; i < scenes.Count; i++)
        {
            sceneArray[i] = prefix + scenes[i] + postfix;
        }

        return sceneArray;
    }
}
