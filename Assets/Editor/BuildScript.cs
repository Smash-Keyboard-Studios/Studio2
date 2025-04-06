using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO.Compression;
using System;

public class BuildScript
{
    public static void BuildWindows()
    {
        string path = "Builds/Windows";
        CreateDirectory(path);

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = GetEnabledScene(),
            locationPathName = $"{path}/{Application.productName} v{Application.version}.exe",
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);

        ZipBuild(path);
    }

    private static void ZipBuild(string path)
    {
        string zipPath = path + ".zip";
        if (File.Exists(zipPath))
        {
            File.Delete(zipPath);
        }

        ZipFile.CreateFromDirectory(path, zipPath);
    }

    private static string[] GetEnabledScene()
    {
        return EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();
    }

    private static void CreateDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
