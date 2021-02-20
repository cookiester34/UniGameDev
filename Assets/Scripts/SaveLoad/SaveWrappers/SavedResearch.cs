using System;
using System.Collections;
using System.Collections.Generic;
using Research;
using UnityEngine;
using Util;

[Serializable]
public class SavedResearch {
    public bool researched;
    public string name;
    public float progress;
    public Timer timer;

    public SavedResearch(ResearchObject research) {
        researched = research.Researched;
        name = research.ResearchName;
        progress = research.ResearchProgress;
        timer = research.Timer;
    }
}
