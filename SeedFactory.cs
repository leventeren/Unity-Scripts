using UnityEngine;

public class SeedFactory {

    private Random.State state;

    public SeedFactory (int seed) {
        Random.InitState(seed);
        state = Random.state;
    }

    // Set Unity's global Random state with this SeedFactory's state, get a random int,
    // then set our SeedFactory's state with the new state.
    // (this allows us to use multiple SeedFactories for multiple paths of determinism
    // if desired)
    public int GetRandomInt (int minInclusive, int maxExclusive) {
        Random.state = state;
        int randomInt = Random.Range(minInclusive, maxExclusive);
        state = Random.state;
        return randomInt;
    }

}

MonoBehaviour to run the test

public class SeedTest : MonoBehaviour {

    void Start () {
        SeedFactory seedFactory = new SeedFactory(123456789);
        string result = "";
        for (int i = 0; i < 20; i++) {
            result += seedFactory.GetRandomInt(int.MinValue, int.MaxValue) + ", ";
        }
        Debug.Log(result);
    }
}

Windows Editor:
217814258, 711215697, 1793372675, -1318111305, -513578644, 1776128467, -1503243711, -285471819, -1800526065, -1845985472, -2061970588, 188207569, 1858341351, -1139513088, 2136219157, 1255727479, -2070068486, 459175680, 1151694536, 1232856178, 

Windows Standalone:
217814258, 711215697, 1793372675, -1318111305, -513578644, 1776128467, -1503243711, -285471819, -1800526065, -1845985472, -2061970588, 188207569, 1858341351, -1139513088, 2136219157, 1255727479, -2070068486, 459175680, 1151694536, 1232856178,

macOS Standalone:
217814258, 711215697, 1793372675, -1318111305, -513578644, 1776128467, -1503243711, -285471819, -1800526065, -1845985472, -2061970588, 188207569, 1858341351, -1139513088, 2136219157, 1255727479, -2070068486, 459175680, 1151694536, 1232856178,

Android:
217814258, 711215697, 1793372675, -1318111305, -513578644, 1776128467, -1503243711, -285471819, -1800526065, -1845985472, -2061970588, 188207569, 1858341351, -1139513088, 2136219157, 1255727479, -2070068486, 459175680, 1151694536, 1232856178,
