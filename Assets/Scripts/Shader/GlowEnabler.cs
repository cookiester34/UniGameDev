using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowEnabler : MonoBehaviour {
    private Renderer[] _renderers;

    private MaterialPropertyBlock _propertyBlock;

    private static readonly int GlowOn = Shader.PropertyToID("_GlowOn");

    // Start is called before the first frame update
    void Awake() {
        _renderers = GetComponentsInChildren<Renderer>();
    }

    public void EnableGlow(bool enable = true) {
        if (_propertyBlock == null) {
            _propertyBlock = new MaterialPropertyBlock();
        }

        foreach (Renderer r in _renderers) {
            r.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(GlowOn, enable ? 1f : 0f);
            r.SetPropertyBlock(_propertyBlock);
        }
    }
}
