using System;
using System.Collections;
using System.Collections.Generic;
using Research;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ResearchModel : MonoBehaviour {
    private Renderer _renderer;
    private MaterialPropertyBlock _materialPropertyBlock;
    [SerializeField] private Color activeColor;
    private Color inactiveColor = Color.black;
    private static readonly int EmissiveColourMultiplier = Shader.PropertyToID("_EmissiveColourMultiplier");

    private void Start() {
        _renderer = GetComponent<Renderer>();
    }

    public void Update() {
        SetEmissive(ResearchManager.Instance.OngoingResearch.Count > 0 ? activeColor : inactiveColor);
    }

    private void SetEmissive(Color color) {
        if (_materialPropertyBlock == null) {
            _materialPropertyBlock = new MaterialPropertyBlock();
        }
        
        _renderer.GetPropertyBlock(_materialPropertyBlock);
        _materialPropertyBlock.SetColor(EmissiveColourMultiplier, color);
        _renderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
