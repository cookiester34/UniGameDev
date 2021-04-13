﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Dissolver : MonoBehaviour {
    private float dissolve;
    private MaterialPropertyBlock _propertyBlock;
    private static readonly int Dissolve = Shader.PropertyToID("_Dissolve");
    private List<Renderer> toDissolve = new List<Renderer>();

    public void Setup(GameObject other) {
        var mrs = other.GetComponentsInChildren<Renderer>();
        if (mrs == null || mrs.Length < 1) {
            Destroy(gameObject);
            Debug.LogWarning("Attempted to setup dissolver but missing meshrenderer on base object");
            return;
        }

        var mfs = other.GetComponentsInChildren<MeshFilter>();
        if (mfs == null || mfs.Length < 1) {
            Destroy(gameObject);
            Debug.LogWarning("Attempted to setup dissolver but missing meshfilter on base object");
            return;
        }

        transform.position = other.transform.position;
        transform.rotation = other.transform.rotation;
        transform.localScale = other.transform.localScale;

        _propertyBlock = new MaterialPropertyBlock();
        dissolve = -1f;
        for(int i = 0; i < mrs.Length; i++) {
            if (mrs[i].gameObject.layer == LayerMask.NameToLayer("Trails")
                || mrs[i].gameObject.layer == LayerMask.NameToLayer("Minimap")) {
                continue;
            }
            GameObject child = new GameObject("dissolvee", typeof(MeshRenderer), typeof(MeshFilter));
            child.transform.parent = gameObject.transform;
            child.transform.position = mrs[i].transform.position;
            child.transform.rotation = mrs[i].transform.rotation;
            child.transform.localScale = mrs[i].transform.localScale;

            var mf = child.GetComponent<MeshFilter>();
            if (mrs[i] is SkinnedMeshRenderer) {
                mf.mesh = ((SkinnedMeshRenderer) mrs[i]).sharedMesh;
            } else {
                if (i < mfs.Length) {
                    mf.mesh = mfs[i].mesh;
                }
            }

            var mr = child.GetComponent<MeshRenderer>();
            mr.materials = mrs[i].materials;
            mr.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(Dissolve, dissolve);
            mr.SetPropertyBlock(_propertyBlock);
            toDissolve.Add(child.GetComponent<MeshRenderer>());
        }

        Tween tween = DOTween.To(() => dissolve, x => dissolve = x, 1f, 3f);
        tween.OnUpdate(UpdateAnimate);
        tween.OnComplete(() => Destroy(gameObject));
    }

    private void UpdateAnimate() {
        foreach (Renderer objRenderer in toDissolve) {
            objRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat(Dissolve, dissolve);
            objRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}
