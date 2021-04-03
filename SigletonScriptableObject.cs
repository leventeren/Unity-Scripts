using System.Collection;
using System.Collection.Generic;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
  private static T instance;
  public static T Instance {
    get {
      if (instance == null){
        T[] = Resources.LoadAll<T>("");
        if (assets == null || assets.Length < 1){
          throw new System.Exception("Could not find and singleton scriptable object instance in the resource");
        }
        else if (assets.Length > 1){
          Debug.LoagWarning("Multiple instance of the singleton scriptable object fount in the resources");
        }
        instance = assets[0];
      }
      return instance;
    }
  }
  
}


/////////////////////// GameSettings.cs

[CreateAssetMenu(filename="Game Settings", menuName="Scriptable Objects/Game Settings")]
public class GameSettings : SingletonScriptableObject<GameSettings>
{
  public string gameName;
  public int gameVersion;
}
