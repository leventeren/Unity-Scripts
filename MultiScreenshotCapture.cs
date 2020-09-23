using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MultiScreenshotCaptureNamespace
{
	internal static class ReflectionExtensions
	{
		internal static object FetchField( this Type type, string field )
		{
			return type.GetFieldRecursive( field, true ).GetValue( null );
		}

		internal static object FetchField( this object obj, string field )
		{
			return obj.GetType().GetFieldRecursive( field, false ).GetValue( obj );
		}

		internal static object FetchProperty( this Type type, string property )
		{
			return type.GetPropertyRecursive( property, true ).GetValue( null, null );
		}

		internal static object FetchProperty( this object obj, string property )
		{
			return obj.GetType().GetPropertyRecursive( property, false ).GetValue( obj, null );
		}

		internal static object CallMethod( this Type type, string method, params object[] parameters )
		{
			return type.GetMethod( method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static ).Invoke( null, parameters );
		}

		internal static object CallMethod( this object obj, string method, params object[] parameters )
		{
			return obj.GetType().GetMethod( method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance ).Invoke( obj, parameters );
		}

		internal static object CreateInstance( this Type type, params object[] parameters )
		{
			Type[] parameterTypes;
			if( parameters == null )
				parameterTypes = null;
			else
			{
				parameterTypes = new Type[parameters.Length];
				for( int i = 0; i < parameters.Length; i++ )
					parameterTypes[i] = parameters[i].GetType();
			}

			return CreateInstance( type, parameterTypes, parameters );
		}

		internal static object CreateInstance( this Type type, Type[] parameterTypes, object[] parameters )
		{
			return type.GetConstructor( parameterTypes ).Invoke( parameters );
		}

		private static FieldInfo GetFieldRecursive( this Type type, string field, bool isStatic )
		{
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | ( isStatic ? BindingFlags.Static : BindingFlags.Instance );
			do
			{
				FieldInfo fieldInfo = type.GetField( field, flags );
				if( fieldInfo != null )
					return fieldInfo;

				type = type.BaseType;
			} while( type != null );

			return null;
		}

		private static PropertyInfo GetPropertyRecursive( this Type type, string property, bool isStatic )
		{
			BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | ( isStatic ? BindingFlags.Static : BindingFlags.Instance );
			do
			{
				PropertyInfo propertyInfo = type.GetProperty( property, flags );
				if( propertyInfo != null )
					return propertyInfo;

				type = type.BaseType;
			} while( type != null );

			return null;
		}
	}

	public class MultiScreenshotCapture : EditorWindow
	{
		private enum TargetCamera { GameView = 0, SceneView = 1 };

		private class CustomResolution
		{
			public readonly int width, height;
			private int originalIndex, newIndex;

			private bool m_isActive;
			public bool IsActive
			{
				get { return m_isActive; }
				set
				{
					if( m_isActive != value )
					{
						m_isActive = value;

						int resolutionIndex;
						if( m_isActive )
						{
							originalIndex = (int) GameView.FetchProperty( "selectedSizeIndex" );

							object customSize = GetFixedResolution( width, height );
							SizeHolder.CallMethod( "AddCustomSize", customSize );
							newIndex = (int) SizeHolder.CallMethod( "IndexOf", customSize ) + (int) SizeHolder.CallMethod( "GetBuiltinCount" );
							resolutionIndex = newIndex;
						}
						else
						{
							SizeHolder.CallMethod( "RemoveCustomSize", newIndex );
							resolutionIndex = originalIndex;
						}

						GameView.CallMethod( "SizeSelectionCallback", resolutionIndex, null );
						GameView.Repaint();
					}
				}
			}

			public CustomResolution( int width, int height )
			{
				this.width = width;
				this.height = height;
			}
		}

		[Serializable]
		private class SaveData
		{
			public List<Vector2> resolutions;
			public List<bool> resolutionsEnabled;
			public bool currentResolutionEnabled;
		}

		[Serializable]
		private class SessionData
		{
			public List<Vector2> resolutions;
			public List<bool> resolutionsEnabled;
			public bool currentResolutionEnabled;
			public float resolutionMultiplier;
			public TargetCamera targetCamera;
			public bool captureOverlayUI;
			public bool setTimeScaleToZero;
			public bool saveAsPNG;
			public bool allowTransparentBackground;
			public string saveDirectory;
		}

		private const string SESSION_DATA_PATH = "Library/MSC_Session.json";
		private const string TEMPORARY_RESOLUTION_LABEL = "MSC_temp";
		private readonly GUILayoutOption GL_WIDTH_25 = GUILayout.Width( 25f );
		private readonly GUILayoutOption GL_EXPAND_WIDTH = GUILayout.ExpandWidth( true );

		private static object SizeHolder { get { return GetType( "GameViewSizes" ).FetchProperty( "instance" ).FetchProperty( "currentGroup" ); } }
		private static EditorWindow GameView { get { return GetWindow( GetType( "GameView" ) ); } }
		//private static EditorWindow GameView { get { return (EditorWindow) GetType( "GameView" ).CallMethod( "GetMainGameView" ); } }

		private List<Vector2> resolutions = new List<Vector2>() { new Vector2( 1024, 768 ) }; // Not readonly to support serialization
		private List<bool> resolutionsEnabled = new List<bool>() { true }; // Same as above
		private bool currentResolutionEnabled = true;
		private float resolutionMultiplier = 1f;

		private TargetCamera targetCamera = TargetCamera.GameView;
		private bool captureOverlayUI = false;
		private bool setTimeScaleToZero = true;
		private float prevTimeScale;
		private bool saveAsPNG = true;
		private bool allowTransparentBackground = false;
		private string saveDirectory;

		private Vector2 scrollPos;

		private readonly List<CustomResolution> queuedScreenshots = new List<CustomResolution>();

		[MenuItem( "Window/Multi Screenshot Capture" )]
		private static void Init()
		{
			MultiScreenshotCapture window = GetWindow<MultiScreenshotCapture>();
			window.titleContent = new GUIContent( "Screenshot" );
			window.minSize = new Vector2( 325f, 150f );
			window.Show();
		}

		private void Awake()
		{
			if( File.Exists( SESSION_DATA_PATH ) )
			{
				SessionData sessionData = JsonUtility.FromJson<SessionData>( File.ReadAllText( SESSION_DATA_PATH ) );
				resolutions = sessionData.resolutions;
				resolutionsEnabled = sessionData.resolutionsEnabled;
				currentResolutionEnabled = sessionData.currentResolutionEnabled;
				resolutionMultiplier = sessionData.resolutionMultiplier > 0f ? sessionData.resolutionMultiplier : 1f;
				targetCamera = sessionData.targetCamera;
				captureOverlayUI = sessionData.captureOverlayUI;
				setTimeScaleToZero = sessionData.setTimeScaleToZero;
				saveAsPNG = sessionData.saveAsPNG;
				allowTransparentBackground = sessionData.allowTransparentBackground;
				saveDirectory = sessionData.saveDirectory;
			}
		}

		private void OnDestroy()
		{
			SessionData sessionData = new SessionData()
			{
				resolutions = resolutions,
				resolutionsEnabled = resolutionsEnabled,
				currentResolutionEnabled = currentResolutionEnabled,
				resolutionMultiplier = resolutionMultiplier,
				targetCamera = targetCamera,
				captureOverlayUI = captureOverlayUI,
				setTimeScaleToZero = setTimeScaleToZero,
				saveAsPNG = saveAsPNG,
				allowTransparentBackground = allowTransparentBackground,
				saveDirectory = saveDirectory
			};

			File.WriteAllText( SESSION_DATA_PATH, JsonUtility.ToJson( sessionData ) );
		}

		private void OnGUI()
		{
			// In case resolutionsEnabled didn't exist when the latest SessionData was created
			if( resolutionsEnabled == null || resolutionsEnabled.Count != resolutions.Count )
			{
				resolutionsEnabled = new List<bool>( resolutions.Count );
				for( int i = 0; i < resolutions.Count; i++ )
					resolutionsEnabled.Add( true );
			}

			scrollPos = EditorGUILayout.BeginScrollView( scrollPos );

			GUILayout.BeginHorizontal();

			GUILayout.Label( "Resolutions:", GL_EXPAND_WIDTH );

			if( GUILayout.Button( "Save" ) )
				SaveSettings();

			if( GUILayout.Button( "Load" ) )
				LoadSettings();

			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();

			GUI.enabled = currentResolutionEnabled;
			GUILayout.Label( "Current Resolution", GL_EXPAND_WIDTH );
			GUI.enabled = true;

			currentResolutionEnabled = EditorGUILayout.Toggle( GUIContent.none, currentResolutionEnabled, GL_WIDTH_25 );

			if( GUILayout.Button( "+", GL_WIDTH_25 ) )
			{
				resolutions.Insert( 0, new Vector2() );
				resolutionsEnabled.Insert( 0, true );
			}

			GUI.enabled = false;
			GUILayout.Button( "-", GL_WIDTH_25 );
			GUI.enabled = true;

			GUILayout.EndHorizontal();

			for( int i = 0; i < resolutions.Count; i++ )
			{
				GUILayout.BeginHorizontal();

				GUI.enabled = resolutionsEnabled[i];
				resolutions[i] = EditorGUILayout.Vector2Field( GUIContent.none, resolutions[i] );
				GUI.enabled = true;
				resolutionsEnabled[i] = EditorGUILayout.Toggle( GUIContent.none, resolutionsEnabled[i], GL_WIDTH_25 );

				if( GUILayout.Button( "+", GL_WIDTH_25 ) )
				{
					resolutions.Insert( i + 1, new Vector2() );
					resolutionsEnabled.Insert( i + 1, true );
				}

				if( GUILayout.Button( "-", GL_WIDTH_25 ) )
				{
					resolutions.RemoveAt( i );
					resolutionsEnabled.RemoveAt( i );
					i--;
				}

				GUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();

			resolutionMultiplier = EditorGUILayout.FloatField( "Resolution Multiplier", resolutionMultiplier );
			targetCamera = (TargetCamera) EditorGUILayout.EnumPopup( "Target Camera", targetCamera );

			EditorGUILayout.Space();

			if( targetCamera == TargetCamera.GameView )
			{
				captureOverlayUI = EditorGUILayout.ToggleLeft( "Capture Overlay UI", captureOverlayUI );
				if( captureOverlayUI && EditorApplication.isPlaying )
				{
					EditorGUI.indentLevel++;
					setTimeScaleToZero = EditorGUILayout.ToggleLeft( "Set timeScale to 0 during capture", setTimeScaleToZero );
					EditorGUI.indentLevel--;
				}
			}

			saveAsPNG = EditorGUILayout.ToggleLeft( "Save as PNG", saveAsPNG );
			if( saveAsPNG && !captureOverlayUI && targetCamera == TargetCamera.GameView )
			{
				EditorGUI.indentLevel++;
				allowTransparentBackground = EditorGUILayout.ToggleLeft( "Allow transparent background", allowTransparentBackground );
				if( allowTransparentBackground )
					EditorGUILayout.HelpBox( "For transparent background to work, you may need to disable post-processing on the Main Camera.", MessageType.Info );
				EditorGUI.indentLevel--;
			}

			EditorGUILayout.Space();

			saveDirectory = PathField( "Save to:", saveDirectory );

			EditorGUILayout.Space();

			GUI.enabled = queuedScreenshots.Count == 0 && resolutionMultiplier > 0f;
			if( GUILayout.Button( "Capture Screenshots" ) )
			{
				if( string.IsNullOrEmpty( saveDirectory ) )
					saveDirectory = Environment.GetFolderPath( Environment.SpecialFolder.DesktopDirectory );

				if( currentResolutionEnabled )
					CaptureScreenshot( ( targetCamera == TargetCamera.GameView ? Camera.main : SceneView.lastActiveSceneView.camera ).pixelRect.size );

				for( int i = 0; i < resolutions.Count; i++ )
				{
					if( resolutionsEnabled[i] )
						CaptureScreenshot( resolutions[i] );
				}

				if( !captureOverlayUI || targetCamera == TargetCamera.SceneView )
					Debug.Log( "<b>Saved screenshots:</b> " + saveDirectory );
				else
				{
					if( EditorApplication.isPlaying && setTimeScaleToZero )
					{
						prevTimeScale = Time.timeScale;
						Time.timeScale = 0f;
					}

					EditorApplication.update -= CaptureQueuedScreenshots;
					EditorApplication.update += CaptureQueuedScreenshots;
				}
			}
			GUI.enabled = true;

			EditorGUILayout.EndScrollView();
		}

		private void CaptureScreenshot( Vector2 resolution )
		{
			int width = Mathf.RoundToInt( resolution.x * resolutionMultiplier );
			int height = Mathf.RoundToInt( resolution.y * resolutionMultiplier );

			if( width <= 0 || height <= 0 )
				Debug.LogWarning( "Skipped resolution: " + resolution );
			else if( !captureOverlayUI || targetCamera == TargetCamera.SceneView )
				CaptureScreenshotWithoutUI( width, height );
			else
				queuedScreenshots.Add( new CustomResolution( width, height ) );
		}

		private void CaptureQueuedScreenshots()
		{
			if( queuedScreenshots.Count == 0 )
			{
				EditorApplication.update -= CaptureQueuedScreenshots;
				return;
			}

			CustomResolution resolution = queuedScreenshots[0];
			if( !resolution.IsActive )
			{
				resolution.IsActive = true;

				if( EditorApplication.isPlaying && EditorApplication.isPaused )
					EditorApplication.Step(); // Necessary to refresh overlay UI
			}
			else
			{
				try
				{
					CaptureScreenshotWithUI();
				}
				catch( Exception e )
				{
					Debug.LogException( e );
				}

				resolution.IsActive = false;

				queuedScreenshots.RemoveAt( 0 );
				if( queuedScreenshots.Count == 0 )
				{
					if( EditorApplication.isPlaying && EditorApplication.isPaused )
						EditorApplication.Step(); // Necessary to restore overlay UI

					if( EditorApplication.isPlaying && setTimeScaleToZero )
						Time.timeScale = prevTimeScale;

					Debug.Log( "<b>Saved screenshots:</b> " + saveDirectory );
					Repaint();
				}
				else
				{
					// Activate the next resolution immediately
					CaptureQueuedScreenshots();
				}
			}
		}

		private void CaptureScreenshotWithoutUI( int width, int height )
		{
			Camera camera = targetCamera == TargetCamera.GameView ? Camera.main : SceneView.lastActiveSceneView.camera;

			RenderTexture temp = RenderTexture.active;
			RenderTexture temp2 = camera.targetTexture;

			RenderTexture renderTex = RenderTexture.GetTemporary( width, height, 24 );
			Texture2D screenshot = null;

			bool allowHDR = camera.allowHDR;
			if( saveAsPNG && allowTransparentBackground )
				camera.allowHDR = false;

			try
			{
				RenderTexture.active = renderTex;

				camera.targetTexture = renderTex;
				camera.Render();

				screenshot = new Texture2D( renderTex.width, renderTex.height, saveAsPNG && allowTransparentBackground ? TextureFormat.RGBA32 : TextureFormat.RGB24, false );
				screenshot.ReadPixels( new Rect( 0, 0, renderTex.width, renderTex.height ), 0, 0, false );
				screenshot.Apply( false, false );

				File.WriteAllBytes( GetUniqueFilePath( renderTex.width, renderTex.height ), saveAsPNG ? screenshot.EncodeToPNG() : screenshot.EncodeToJPG( 100 ) );
			}
			finally
			{
				camera.targetTexture = temp2;
				if( saveAsPNG && allowTransparentBackground )
					camera.allowHDR = allowHDR;

				RenderTexture.active = temp;
				RenderTexture.ReleaseTemporary( renderTex );

				if( screenshot != null )
					DestroyImmediate( screenshot );
			}
		}

		private void CaptureScreenshotWithUI()
		{
			RenderTexture temp = RenderTexture.active;

			RenderTexture renderTex = (RenderTexture) GameView.FetchField( "m_TargetTexture" );
			Texture2D screenshot = null;

			int width = renderTex.width;
			int height = renderTex.height;

			try
			{
				RenderTexture.active = renderTex;

				screenshot = new Texture2D( width, height, saveAsPNG && allowTransparentBackground ? TextureFormat.RGBA32 : TextureFormat.RGB24, false );
				screenshot.ReadPixels( new Rect( 0, 0, width, height ), 0, 0, false );

				if( SystemInfo.graphicsUVStartsAtTop )
				{
					Color32[] pixels = screenshot.GetPixels32();
					for( int i = 0; i < height / 2; i++ )
					{
						int startIndex0 = i * width;
						int startIndex1 = ( height - i - 1 ) * width;
						for( int x = 0; x < width; x++ )
						{
							Color32 color = pixels[startIndex0 + x];
							pixels[startIndex0 + x] = pixels[startIndex1 + x];
							pixels[startIndex1 + x] = color;
						}
					}

					screenshot.SetPixels32( pixels );
				}

				screenshot.Apply( false, false );

				File.WriteAllBytes( GetUniqueFilePath( width, height ), saveAsPNG ? screenshot.EncodeToPNG() : screenshot.EncodeToJPG( 100 ) );
			}
			finally
			{
				RenderTexture.active = temp;

				if( screenshot != null )
					DestroyImmediate( screenshot );
			}
		}

		private string PathField( string label, string path )
		{
			GUILayout.BeginHorizontal();
			path = EditorGUILayout.TextField( label, path );
			if( GUILayout.Button( "o", GL_WIDTH_25 ) )
			{
				string selectedPath = EditorUtility.OpenFolderPanel( "Choose output directory", "", "" );
				if( !string.IsNullOrEmpty( selectedPath ) )
					path = selectedPath;

				GUIUtility.keyboardControl = 0; // Remove focus from active text field
			}
			GUILayout.EndHorizontal();

			return path;
		}

		private void SaveSettings()
		{
			string savePath = EditorUtility.SaveFilePanel( "Choose destination", "", "resolutions", "json" );
			if( !string.IsNullOrEmpty( savePath ) )
			{
				SaveData saveData = new SaveData()
				{
					resolutions = resolutions,
					resolutionsEnabled = resolutionsEnabled,
					currentResolutionEnabled = currentResolutionEnabled
				};

				File.WriteAllText( savePath, JsonUtility.ToJson( saveData, false ) );
			}
		}

		private void LoadSettings()
		{
			string loadPath = EditorUtility.OpenFilePanel( "Choose save file", "", "json" );
			if( !string.IsNullOrEmpty( loadPath ) )
			{
				SaveData saveData = JsonUtility.FromJson<SaveData>( File.ReadAllText( loadPath ) );
				resolutions = saveData.resolutions ?? new List<Vector2>();
				resolutionsEnabled = saveData.resolutionsEnabled ?? new List<bool>();
				currentResolutionEnabled = saveData.currentResolutionEnabled;
			}
		}

		private string GetUniqueFilePath( int width, int height )
		{
			string filename = string.Concat( width, "x", height, " {0}", saveAsPNG ? ".png" : ".jpeg" );
			int fileIndex = 0;
			string path;
			do
			{
				path = Path.Combine( saveDirectory, string.Format( filename, ++fileIndex ) );
			} while( File.Exists( path ) );

			return path;
		}

		private static object GetFixedResolution( int width, int height )
		{
			object sizeType = Enum.Parse( GetType( "GameViewSizeType" ), "FixedResolution" );
			return GetType( "GameViewSize" ).CreateInstance( sizeType, width, height, TEMPORARY_RESOLUTION_LABEL );
		}

		private static Type GetType( string type )
		{
			return typeof( EditorWindow ).Assembly.GetType( "UnityEditor." + type );
		}
	}
}
