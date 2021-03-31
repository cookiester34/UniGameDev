using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class ResolutionDropdown : MonoBehaviour {
    private Dropdown _dropdown;
    HashSet<string> _options = new HashSet<string>();

    private void Awake() {
        _dropdown = GetComponent<Dropdown>();
        SetupOptions();
    }

    private void SetupOptions() {
        Resolution[] resolutions = Screen.resolutions;
        _dropdown.ClearOptions();
        _options.Clear();
        
        int currentResolutionIndex = 0;
        foreach (var iterResolution in resolutions) {
            _options.Add(iterResolution.width + "x" + iterResolution.height);
        }
        
        _dropdown.AddOptions(_options.ToList());
        _dropdown.value = currentResolutionIndex;
        _dropdown.RefreshShownValue();
    }

    public void SetToResolution(int width, int height) {
        _dropdown.value = FindResolution(width, height);
    }

    public int FindResolution(int width, int height) {
        string option = width + "x" + height;
        for (int i = 0; i < _options.Count; i++) {
            if (option.Equals(_options.ToList()[i])) {
                return i;
            }
        }

        return 0;
    }

    public Tuple<int, int> GetCurrentResolution() {
        string option = _options.ToList()[_dropdown.value];
        return GetResolution(option);
    }

    private Tuple<int, int> GetResolution(string option) {
        int x = option.IndexOf("x");
        string xPortion = option.Substring(0, x);
        string yPortion = option.Substring(x + 1);
        return new Tuple<int, int>(int.Parse(xPortion), int.Parse(yPortion));
    }
}
