using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/*
public class BuildAssetBundle : Editor
{
	private static string assetPath = "AllAssets";

	private static string AssetBundleOutPath = "Assets/StreamingAssets";

	private static List<string> assetPathList = new List<string>();

	private static Dictionary<string, string> asExtensionDic = new Dictionary<string, string>();

	[MenuItem("Assets/BuildAssetBundle")]
	private static void BuildAssetBundleSource()
	{
		assetPathList.Clear ();
		SetAsExtensionDic();

		string outPath = Path.Combine(AssetBundleOutPath, Platform.GetPlatformFolder(EditorUserBuildSettings.activeBuildTarget));

		GetDirs(Application.dataPath + "/" + assetPath);

		BuildAsset(outPath);
	}

	private static void SetAsExtensionDic()
	{
		asExtensionDic.Clear();

		asExtensionDic.Add(".prefab", ".unity3d");
		asExtensionDic.Add(".mat", ".unity3d");
		asExtensionDic.Add(".png", ".unity3d");
	}

	private static void GetDirs(string dirPath)
	{
		foreach(string path in Directory.GetFiles(dirPath))
		{
			if (asExtensionDic.ContainsKey(System.IO.Path.GetExtension(path)))
			{
				Debug.Log("path:" + path);
				assetPathList.Add (path);
			}
		}

		foreach(string path in Directory.GetDirectories(dirPath))
		{
			GetDirs(path);
		}
	}

	private static void BuildAsset(string outPath)
	{
		for(int i = 0; i < assetPathList.Count; i++)
		{
			string asPath = assetPathList[i];

			Debug.Log("asPath:" + asPath);
			AssetImporter assetImporter = AssetImporter.GetAtPath(asPath);

			if (assetImporter == null)
			{
							Debug.Log("here");
				continue;
			}

			string assetName = asPath.Substring(asPath.IndexOf(assetPath));

			Debug.Log ("assetName:" + assetName);
			assetName = assetName.Replace(Path.GetExtension(assetName), ".unity3d");
			Debug.Log ("assetName:" + assetName);
			assetImporter.assetBundleName = assetName;
		}

		if (!Directory.Exists(outPath))
		{
			Directory.CreateDirectory(outPath);
		}

		BuildPipeline.BuildAssetBundles(outPath, 0, EditorUserBuildSettings.activeBuildTarget);

		AssetDatabase.Refresh();
	}
}

public class Platform
{
	public static string GetPlatformFolder(BuildTarget target)
	{
		switch(target)
		{
		case BuildTarget.Android:
			return "Android";
		case BuildTarget.iOS:
			return "IOS";
		case BuildTarget.WebPlayer:
			return "WebPlayer";
		case BuildTarget.StandaloneWindows:
		case BuildTarget.StandaloneWindows64:
			return "Windows";
		case BuildTarget.StandaloneOSXIntel:
		case BuildTarget.StandaloneOSXIntel64:
		case BuildTarget.StandaloneOSXUniversal:
			return "OSX";
		default:
			return null;
		}
	}
}
*/
