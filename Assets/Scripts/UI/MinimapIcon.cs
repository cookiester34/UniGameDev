using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
	private float _height = 40f;
    public bool shouldBeHiddenInFog;
    private MeshRenderer _mr;
    private Transform _parent;
    // Start is called before the first frame update
    void Start()
    {
        _height = Random.Range(40f, 45f);
        _mr = GetComponent<MeshRenderer>();
        if (transform.parent)
        {
            _parent = transform.parent;
        }
        else
        {
            _parent = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
		pos.y = _height;
		transform.position = pos;
        if (shouldBeHiddenInFog)
        {
            _mr.enabled = !FogOfWarBounds.instance.IsInFog(_parent.position);
        }
    }
}
