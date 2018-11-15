using UnityEngine;

[System.Serializable]
public class Dialogue
{
    // NPC name
    public string name;

    // Sentences on neutral state
    [TextArea(3, 10)]
    public string[] neutralsentences;

    // Sentences when player is on a quest and does not have the required items
    [TextArea(3, 10)]
    public string[] inQuestNoItemSentences;

    // Sentences when player is on a quest and has the required items
    [TextArea(3, 10)]
    public string[] inQuestWithItemSentences;

    // Sentences after completing the quest
    [TextArea(3, 10)]
    public string[] afterQuestSentences;
}

