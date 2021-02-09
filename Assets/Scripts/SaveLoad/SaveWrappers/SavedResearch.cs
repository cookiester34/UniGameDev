using System;
using System.Collections;
using System.Collections.Generic;
using Research;
using UnityEngine;

[Serializable]
public class SavedResearch {
    public bool researched;
    public string name;

    public SavedResearch(ResearchObject research) {
        researched = research.Researched;
        name = research.ResearchName;
    }
}
