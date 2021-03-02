using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Dissolver : MonoBehaviour {
    private float dissolve;
    private MaterialPropertyBlock _propertyBlock;
    private MeshRenderer _renderer;
    private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");

    public void Setup(GameObject other) {
        var mr = other.GetComponentInChildren<MeshRenderer>();
        if (mr == null) {
            Destroy(gameObject);
            Debug.LogWarning("Attempted to setup dissolver but missing meshrenderer on base object");
            return;
        }

        var mf = other.GetComponentInChildren<MeshFilter>();
        if (mf == null) {
            Destroy(gameObject);
            Debug.LogWarning("Attempted to setup dissolver but missing meshfilter on base object");
            return;
        }

        transform.position = other.transform.position;
        transform.rotation = other.transform.rotation;
        transform.localScale = other.transform.localScale;

        _renderer = gameObject.AddComponent<MeshRenderer>();
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();

        _propertyBlock = new MaterialPropertyBlock();
        _renderer.material = mr.material;
        filter.mesh = mf.mesh;

        dissolve = -1f;
        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetFloat(Dissolve, dissolve);
        _renderer.SetPropertyBlock(_propertyBlock);
        Tween tween = DOTween.To(() => dissolve, x => dissolve = x, 1f, 3f);
        tween.OnUpdate(UpdateAnimate);
        tween.OnComplete(() => Destroy(gameObject));
    }

    private void UpdateAnimate() {
        _renderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetFloat(Dissolve, dissolve);
        _renderer.SetPropertyBlock(_propertyBlock);
    }
}
