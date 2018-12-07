using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    [TextArea(3, 10)]
    public List<string> option1;

    [TextArea(3, 10)]
    public List<string> option2;

    [TextArea(3, 10)]
    public List<string> option3;
}

