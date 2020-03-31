using System.Linq;
 
public class ExampleClass : MonoBehaviour {
 
    public Object[] anyTypeArray;
 
    public void Start () {
 
        foreach(Object part in anyTypeArray.Reverse()) {
            //Insert your code Here
        }
 
    }
 
}
