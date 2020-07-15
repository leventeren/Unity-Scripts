GameObject[] npcs = GameObject.FindGameObjectsWithTag("NPC");
foreach(GameObject npc in npcs)
{
  npc.GetComponent<NPCController>().run = false; // Or true
}
