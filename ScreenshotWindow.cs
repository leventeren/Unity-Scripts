using System.IO;
using UnityEditor;
using UnityEngine;

public class ScreenshotWindow : EditorWindow
{
	private int superSize;
	private string folder;

	[MenuItem("Tools/Create Screenshot")]
	private static void Init()
	{
		var window = GetWindow<ScreenshotWindow>(false,"Screenshots", true);
		if (string.IsNullOrEmpty(window.folder))
		{
			window.folder = Directory.GetCurrentDirectory();
		}
	}

	private void OnGUI()
	{
		GUILayout.Label("Settings", EditorStyles.boldLabel);
		superSize = (int)EditorGUILayout.Slider("Super Size", superSize, 1, 20);

		GUILayout.BeginHorizontal();

		EditorGUILayout.TextField("Folder", folder);
		if (GUILayout.Button("...", GUILayout.MaxWidth(50)))
		{
			folder = EditorUtility.SaveFolderPanel("Save screenshots to folder", folder, "");
			if (string.IsNullOrEmpty(folder))
			{
				folder = Directory.GetCurrentDirectory();
			}
		}

		GUILayout.EndHorizontal();

		GUILayout.Space(10);

		if (GUILayout.Button("Capture"))
		{
			if (!Directory.Exists(folder))
			{
				EditorUtility.DisplayDialog("Folder not found", "Could not find the folder, does it exist?", "Ok");
			}
			else
			{
				int counter = 0;
				string filename = GetFilename(counter);
				while (File.Exists(filename))
				{
					filename = GetFilename(++counter);
				}
				ScreenCapture.CaptureScreenshot(filename, superSize);
			}
		}
	}

	private string GetFilename(int index)
	{
		return Path.Combine(folder, string.Format("Screenshot_{0:000}.png",index));
	}
}
