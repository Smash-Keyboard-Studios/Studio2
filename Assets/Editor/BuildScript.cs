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

        string appName = $"{Application.productName} v{Application.version}.exe";


        string build = $"{path}/{appName}";


        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = GetEnabledScene(),
            locationPathName = build,
            target = BuildTarget.StandaloneWindows64,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(buildPlayerOptions);

        ZipBuild(path);

        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
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
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
