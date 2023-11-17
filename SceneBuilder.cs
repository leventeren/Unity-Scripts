using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using com.zibra.liquid.Manipulators;
using com.zibra.liquid.Solver;
using UnityEngine;
using SimpleFileBrowser;
public class SceneBuilder : MonoBehaviour
{

    // Ability to Create Objects
    // Ability to Destroy Objects
    // Ability to Save Levels
    // Ability to Load Levels

    public GameObject cubePrefab;

    public int poolCount = 20;
    
    
    private List<GameObject> GameObjectsToTrack;

    private List<GameObject> pool;

    private string loadedLevel;
    
    private ZibraLiquid zibraLiquidVolume;

    
    // Reference from the https://github.com/yasirkula/UnitySimpleFileBrowser
    // public static bool ShowSaveDialog( OnSuccess onSuccess, OnCancel onCancel, PickMode pickMode, bool allowMultiSelection = false, string initialPath = null, string initialFilename = null, string title = "Save", string saveButtonText = "Save" );
    // public static bool ShowLoadDialog( OnSuccess onSuccess, OnCancel onCancel, PickMode pickMode, bool allowMultiSelection = false, string initialPath = null, string initialFilename = null, string title = "Load", string loadButtonText = "Select" );
    //
    // public delegate void OnSuccess( string[] paths );
    // public delegate void OnCancel();
    
    void Start()
    {
        GameObjectsToTrack = new List<GameObject>();
        pool = new List<GameObject>();
        zibraLiquidVolume = FindObjectOfType<ZibraLiquid>();

        for (int i = 0; i < poolCount; i++)
        {
            GameObject go = Instantiate(cubePrefab);
            go.SetActive(false);
            zibraLiquidVolume.AddCollider(go.GetComponent<ZibraLiquidCollider>());
            pool.Add(go);
        }

        zibraLiquidVolume.enabled = true;

    }
    
    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: both, Allow multiple selection: true
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Load File", Submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load" );

        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        Debug.Log( FileBrowser.Success );

        if( FileBrowser.Success )
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            for( int i = 0; i < FileBrowser.Result.Length; i++ )
                Debug.Log( FileBrowser.Result[i] );

            // Read the bytes of the first file via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well

            loadedLevel = FileBrowserHelpers.ReadTextFromFile(FileBrowser.Result[0]);

            LoadLevel();
            // Or, copy the first file to persistentDataPath
            // string destinationPath = Path.Combine( Application.persistentDataPath, FileBrowserHelpers.GetFilename( FileBrowser.Result[0] ) );
            // FileBrowserHelpers.CopyFile( FileBrowser.Result[0], destinationPath );
        }
    }

    void LoadLevel()
    {
        ClearAllObjects();

        // split into objects
        string[] TRS = loadedLevel.Split('~');
        foreach (string trs in TRS)
        {
            Debug.Log($"TRS Component is {trs}");
            // split into a single transform, rotation, or scale component
            // the order is trans, rot, scale
            string [] trsComponent = trs.Split("|");
            if (trsComponent[0] != "")
            {
                GameObject go = RequestCubeFromPool();
                
                go.transform.position = StringToVector3(trsComponent[0]);
            
                go.transform.rotation = StringToQuaternion(trsComponent[1]);
            
                go.transform.localScale = StringToVector3(trsComponent[2]);
                
                go.SetActive(true);
                GameObjectsToTrack.Add(go);
            }
            
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
        {
            SaveLevel();
        }
        
        // Coroutine example
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine( ShowLoadDialogCoroutine() );
        }
        
    }

    public bool SaveLevel() // TODO: https://github.com/yasirkula/UnitySimpleFileBrowser
    {
        Debug.Log($"Level Saved as {Application.dataPath}/WaveWorld-{DateTimeOffset.Now.ToUnixTimeSeconds()}.txt"); 
        StringBuilder stringBuilder = new StringBuilder();
            ;        string saveString = "";
        foreach (GameObject go in GameObjectsToTrack)
        {
            stringBuilder.Append(go.transform.position.ToString());
            stringBuilder.Append("|");
            stringBuilder.Append(go.transform.rotation.ToString());
            stringBuilder.Append("|");
            stringBuilder.Append(go.transform.localScale.ToString());
            stringBuilder.Append("~");
            
        }
        
        
        //FileBrowser.ShowSaveDialog( onSuccess: , onCancel: null, FileBrowser.PickMode.Files, allowMultiSelection: false, initialPath:null,  initialFilename:$"{DateTime.Now}", title :"Save", saveButtonText: "Save" );

        StreamWriter writer = new StreamWriter($"./WaveWorld-{DateTimeOffset.Now.ToUnixTimeSeconds()}.txt", true);
        writer.Write(stringBuilder.ToString());
        writer.Close();
        
        return true;

    }

    
    
    
    public void AddCube()
    {
        GameObject go = RequestCubeFromPool();
        go .transform.position = Camera.main.transform.position +
            10.5f * Camera.main.transform.forward * cubePrefab.transform.localScale.z / 2.0f;
        go.transform.rotation = Quaternion.identity;
        go.transform.localScale = cubePrefab.transform.localScale;
        go.SetActive(true);
        GameObjectsToTrack.Add(go);
    }

    public void ClearAllObjects()
    {
        
        foreach (GameObject go in GameObjectsToTrack)
        {
            ReturnCubeToPool(go);
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = cubePrefab.transform.localScale;
        }
        GameObjectsToTrack.Clear();
        
    }

    public GameObject RequestCubeFromPool()
    {
        return pool.Find(go => go.activeSelf == false );
    }

    public void ReturnCubeToPool(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
    
    public static Vector3 StringToVector3(string sVector)
    {
        Debug.Log($"S2V3: {sVector}");
        // Remove the parentheses
        if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
            sVector = sVector.Substring(1, sVector.Length-2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        try
        {
            
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]));
            
            return result;
        } 
        catch (FormatException formatException)
        {
            Debug.Log($"issue with input string: {sVector}");
            return Vector3.zero;

        }

    }
    
    public static Quaternion StringToQuaternion(string sQuat)
    {
        Debug.Log($"S2V3: {sQuat}");
        // Remove the parentheses
        if (sQuat.StartsWith ("(") && sQuat.EndsWith (")")) {
            sQuat = sQuat.Substring(1, sQuat.Length-2);
        }

        // split the items
        string[] sArray = sQuat.Split(',');

        // store as a Vector3
        try
        {
            
            Quaternion result = new Quaternion(
                float.Parse(sArray[0]),
                float.Parse(sArray[1]),
                float.Parse(sArray[2]),
                float.Parse(sArray[3]));
            
            return result;
        } 
        catch (FormatException formatException)
        {
            Debug.Log($"issue with input string: {sQuat}");
            return Quaternion.identity;

        }

    }
}
