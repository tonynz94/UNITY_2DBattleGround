using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MultiplayersBuildAndRun
{
	[MenuItem("Tools/Run Multiplayer/1 Players")]
	static void PerformWin64Build1()
	{
		PerformWin64Build(1);
	}

	[MenuItem("Tools/Run Multiplayer/2 Players")]
	static void PerformWin64Build2()
	{
		PerformWin64Build(2);
	}

	[MenuItem("Tools/Run Multiplayer/3 Players")]
	static void PerformWin64Build3()
	{
		PerformWin64Build(3);
	}

	[MenuItem("Tools/Run Multiplayer/4 Players")]
	static void PerformWin64Build4()
	{
		PerformWin64Build(4);
	}

	[MenuItem("Tools/IOS Multi/1 Players")]
	static void PerformIOSBuild1()
	{
		PerformMacOSBuild(1);
	}

	[MenuItem("Tools/IOS Multi/2 Players")]
	static void PerformIOSBuild2()
	{
		PerformMacOSBuild(2);
	}

	[MenuItem("Tools/IOS Multi/3 Players")]
	static void PerformIOSBuild3()
	{
		PerformMacOSBuild(3);
	}

	[MenuItem("Tools/IOS Multi/4 Players")]
	static void PerformIOSBuild4()
	{
		PerformMacOSBuild(4);
	}

	static void PerformWin64Build(int playerCount)
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

		for (int i = 1; i <= playerCount; i++)
		{
			BuildPipeline.BuildPlayer(GetScenePaths(),
				"Builds/Win64/" + GetProjectName() + i.ToString() + "/" + GetProjectName() + i.ToString() + ".exe",
				BuildTarget.StandaloneWindows64, BuildOptions.AutoRunPlayer);
		}
	}

    static void PerformMacOSBuild(int playerCount)
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget(
			BuildTargetGroup.Standalone, BuildTarget.StandaloneOSXIntel64);

		for (int i = 1; i <= playerCount; i++)
		{
			BuildPipeline.BuildPlayer(GetScenePaths(),
				"Builds/IOS/" + GetProjectName() + i.ToString() + "/" + GetProjectName() + i.ToString(),
				BuildTarget.StandaloneOSXIntel64, BuildOptions.AutoRunPlayer);
		}
	}

	static string GetProjectName()
	{
		string[] s = Application.dataPath.Split('/');
		return s[s.Length - 2];
	}

	static string[] GetScenePaths()
	{
		string[] scenes = new string[EditorBuildSettings.scenes.Length];

		for (int i = 0; i < scenes.Length; i++)
		{
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}

		return scenes;
	}
}
