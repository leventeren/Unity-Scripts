using UnityEngine;
using System.Collections;
 
// Can use it on:
// VECTOR2 / VECTOR2INT
// FLOAT
// you put the attribute above/before the max value!
// then you specify in the MinTo arguments (if the minName template doesn't work)
// the name of the min float variable
public class MinToAttribute : PropertyAttribute
{
    // $ becomes the name of the max property
    // example: [MinTo] float duration; float durationMin;
    public string minName = "$Min";
    public float? max;
    public float min;
 
    public MinToAttribute(string minName = null)
    {
        if (minName != null)
            this.minName = minName;
    }
    public MinToAttribute(float max, string minName = null) : this(0, max, minName) { }
    public MinToAttribute(float min, float max, string minName = null) : this(minName)
    {
        this.max = max;
        this.min = min;
    }
}




MinToDrawer.cs (in editor folder)

using UnityEditor;
using UnityEngine;
 
[CustomPropertyDrawer(typeof(MinToAttribute))]
public class MinToDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        var att = (MinToAttribute)attribute;
        var type = property.propertyType;
 
        string minName = att.minName.Replace("$", property.name);
        int lastDot = property.propertyPath.LastIndexOf('.');
        if (lastDot > -1)
            minName = property.propertyPath.Substring(0, lastDot) + '.' + minName;
        //Debug.Log("minName=" + minName);
 
        if (type == SerializedPropertyType.Float)
            //label.text = string.Format("({1}-{0})", property.name, minName);
            label.text = " ";
        var ctrlRect = EditorGUI.PrefixLabel(position, label);
        Rect[] r = SplitRectIn3(ctrlRect, 36, 5);
        if (type == SerializedPropertyType.Vector2)
        {
            EditorGUI.BeginChangeCheck();
            var vec = property.vector2Value;
            float min = vec.x;
            float to = vec.y;
            min = EditorGUI.FloatField(r[0], min);
            to = EditorGUI.FloatField(r[2], to);
            EditorGUI.MinMaxSlider(r[1], ref min, ref to, att.min, att.max ?? to);
            vec = new Vector2(min < to ? min : to, to);
            if (EditorGUI.EndChangeCheck())
                property.vector2Value = vec;
        }
        else if (type == SerializedPropertyType.Vector2Int)
        {
            EditorGUI.BeginChangeCheck();
            var vec = property.vector2IntValue;
            float min = vec.x;
            float to = vec.y;
            min = EditorGUI.IntField(r[0], (int)min);
            to = EditorGUI.IntField(r[2], (int)to);
            EditorGUI.MinMaxSlider(r[1], ref min, ref to, att.min, att.max ?? to);
            vec = new Vector2Int(Mathf.RoundToInt(min < to ? min : to), Mathf.RoundToInt(to));
            if (EditorGUI.EndChangeCheck())
                property.vector2IntValue = vec;
        }
        else if (type == SerializedPropertyType.Float)
        {
            EditorGUI.BeginChangeCheck();
            // Line setup
            var line2 = position;
            line2.y += EditorGUIUtility.singleLineHeight;
 
            // Swap lines
            // Comment these 3 lines if you want the slider above
            // Or uncomment them if you want the slider sandwiched between min and max
            //var y = line2.y;
            //line2.y = r[0].y;
            //r[0].y = r[1].y = r[2].y = y;
 
            // First we draw the float below/above as normal
            EditorGUI.PropertyField(line2, property);
 
            // Then the slider
            var minProperty = property.serializedObject.FindProperty(minName);
            if (minProperty?.propertyType != SerializedPropertyType.Float)
            {
                EditorGUI.HelpBox(ctrlRect, "Min float not found!!", MessageType.Info);
                return;
            }
            float minVal = minProperty.floatValue;
            float maxVal = property.floatValue;
 
            EditorGUI.MinMaxSlider(r[1], ref minVal, ref maxVal, att.min, att.max ?? maxVal);
            EditorGUI.LabelField(r[0], att.min.ToString());
 
            if (att.max.HasValue && maxVal > att.max.Value)
            {
                // Shows that the max value overflowed the slider
                // So if you just wanna try infinite range and stuff you just put 999
                // and it shows clearly that it is a big test value with the color
                // This is only if you specify a max value in the attribute
                Color c = GUI.contentColor;
                GUI.contentColor = overflowColor;
                EditorGUI.LabelField(r[2], maxVal.ToString());
                GUI.contentColor = c;
            }
            else
                EditorGUI.LabelField(r[2], (att.max ?? maxVal).ToString());
 
            // Rounding you lose a tiny bit of precision but I don't mind
            // it is just to show 0.84 instead of ugly 0.840041..
            // unless you're in very small values (<0.1) then it doesn't round
            minVal = FRound(minVal);
            maxVal = FRound(maxVal);
 
            // Proofcheck
            maxVal = Mathf.Max(att.min, maxVal);
            minVal = Mathf.Clamp(minVal, att.min, maxVal);
 
            // And finally update the variables
            if (EditorGUI.EndChangeCheck())
            {
                minProperty.floatValue = minVal;
                property.floatValue = maxVal;
            }
        }
        else
            EditorGUI.HelpBox(ctrlRect, "MinTo is for Vector2 or float!!", MessageType.Error);
    }
 
    const float threshold = .1f;
    const float precision = .01f;
    float FRound(float f) => f > threshold ? Mathf.Floor(f / precision) * precision : f;
 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int lines = 1;
        if (property.propertyType == SerializedPropertyType.Float)
            lines = 2;
        return lines * EditorGUIUtility.singleLineHeight;
    }
 
    // That's orange
    private Color overflowColor = new Color(1f, .55f, .1f, 1);
 
    public static Rect[] SplitRectIn3(Rect rect, int bordersSize, int space = 0)
    {
        var r = SplitRect(rect, 3);
        int pad = (int)r[0].width - bordersSize;
        int ps = pad + space;
        r[0].width = r[2].width -= ps;
        r[1].width += pad * 2;
        r[1].x -= pad;
        r[2].x += ps;
        return r;
    }
    public static Rect[] SplitRect(Rect a, int n)
    {
        Rect[] r = new Rect[n];
        for (int i = 0; i < n; ++i)
            r[I] = new Rect(a.x + a.width / n * i, a.y, a.width / n, a.height);
        return r;
    }
}

/*

usage 

[MinTo(100)]
public float aiZorluk = 100;
public float aiZorlukMin = 0;
        

*/
 
