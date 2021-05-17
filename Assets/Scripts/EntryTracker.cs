using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntryTracker {
    private static bool _visitedMainMenu = false;
    public static bool VisitedMainMenu {
        get => _visitedMainMenu;
        set => _visitedMainMenu = value;
    }
}
