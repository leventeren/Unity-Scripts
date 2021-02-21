public static Transform FindDescendentTransform(Transform searchTransform, string descendantName)
	{
		Transform result = null;

		int childCount = searchTransform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			Transform childTransform = searchTransform.GetChild(i);

        // Not it, but has children? Search the children.
			if (childTransform.name != descendantName
				&& childTransform.childCount > 0)
			{
				Transform grandchildTransform = FindDescendentTransform(childTransform, descendantName);
				if (grandchildTransform == null)
				continue;

				result = grandchildTransform;
				break;
			}
        // Not it, but has no children?  Go on to the next sibling.
			else if (childTransform.name != descendantName
				&& childTransform.childCount == 0)
			{
				continue;
			}

        // Found it.
			result = childTransform;
			break;
		}

		return result;
	}
