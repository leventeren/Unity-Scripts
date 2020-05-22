private List<GameObject> AllChilds(GameObject root)
{
    List<GameObject> result = new List<GameObject>();
    if (root.transform.childCount > 0)
    {
        foreach (Transform VARIABLE in root.transform)
        {
            Searcher(result,VARIABLE.gameObject);
        }
    }
    return result;
}

private void Searcher(List<GameObject> list,GameObject root)
{
    list.Add(root);
    if (root.transform.childCount > 0)
    {
        foreach (Transform VARIABLE in root.transform)
        {
            Searcher(list,VARIABLE.gameObject);
        }
    }
}
