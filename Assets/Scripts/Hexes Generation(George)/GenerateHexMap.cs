using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateHexMap : MonoBehaviour
{
	public int width = 1;
	public int length = 1;
	public float stepHeight = 5f;
	float bigRadius = 1.195f;
	float smallRadius = 1.035f;
	bool shouldOffsetRow = true;
	//public Vector3 centre;
	public GameObject defaultHex;
	public Material baseMat;
	public Color[] colours;
    // Start is called before the first frame update
    void Start()
    {

    }
	
	public void Generate() {
		//float fWidth = width * bigRadius;
		//transform.position = centre;
		float spawnBoundsX = ((float)width * (bigRadius / 2)) - (bigRadius / 2f);
		float spawnBoundsY = ((float)length * (smallRadius / 2)) - (smallRadius / 2f);
		for (float i = -spawnBoundsY; i <= (spawnBoundsY + 0.01f); i += smallRadius) {
			shouldOffsetRow = !shouldOffsetRow;
			for (float j = -spawnBoundsX; j <= (spawnBoundsX + 0.01f); j += bigRadius) {
				Vector3 pos = transform.position;
				pos.x += j;
				pos.z += i;
				if (shouldOffsetRow) {
					pos.x += bigRadius / 2f;
				}
				GameObject inst = Instantiate(defaultHex, pos, Quaternion.identity);
				inst.transform.parent = transform;
				SetRandColour(inst);
			}
		}
		foreach (Transform child in transform) { //annoying having to do this loop twice
			HexPanel HP = child.GetComponent<HexPanel>();
			HP.CalculateNeighbours();
		}
	}
	
	public void UpdateGrid() {
		foreach (Transform child in transform) {
			HexPanel HP = child.GetComponent<HexPanel>();
			HP.AdjustHeightToNeighbours(stepHeight);
		}
	}

	public void DestroyGrid() {
		// For some reason only deletes half
		foreach (Transform child in transform) {
			DestroyImmediate(child.gameObject);
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	
	void SetRandColour(GameObject go) {
		Material mat = new Material(baseMat);
		mat.color = colours[Random.Range(0, colours.Length)];
		MeshRenderer mr = go.transform.GetChild(0).GetComponent<MeshRenderer>();
		mr.material = mat;
	}
}
