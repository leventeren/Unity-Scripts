using UnityEngine;
using System.Collections;

public static class TransformDeepChildExtension
{
	public static Transform FindDeepChild(this Transform aParent, string aName)
	{
		var result = aParent.Find(aName);
		if (result != null)
			return result;
		foreach (Transform child in aParent)
		{
			result = child.FindDeepChild(aName);
			if (result != null)
				return result;
		}
		return null;
	}
}
