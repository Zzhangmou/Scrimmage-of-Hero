using UnityEditor;

public class BuildEditor
{
    [MenuItem("Tools/打包PC")]
    public static void BuildPC()
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        //获取所有场景名字
        string[] scenePaths = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            scenePaths[i] = EditorBuildSettings.scenes[i].path;
        }
        options.scenes = scenePaths;

        options.target = BuildTarget.StandaloneWindows;
        options.options = BuildOptions.None;
        options.locationPathName = @"E:\Unity\BuildTarget\PC\TestBuild.exe";

        BuildPipeline.BuildPlayer(options);
    }
}
