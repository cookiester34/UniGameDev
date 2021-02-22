using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ApplicationUtil {
    /// <summary>
    /// Used to check if the application is currently quitting, strangely not already supported by application
    /// </summary>
    private static bool _isQuitting;
    public static bool IsQuitting => _isQuitting;
    
    static ApplicationUtil() {
        _isQuitting = false;
        Application.quitting += () => {
            _isQuitting = true;
        };
    }
}
