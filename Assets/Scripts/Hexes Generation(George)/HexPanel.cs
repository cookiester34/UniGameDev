using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexPanel : MonoBehaviour
{
	List<HexPanel> neighbours = new List<HexPanel>();
	bool scaleModifiedByCode;
	bool shouldBeDestroyed;
    // Start is called before the first frame update
    void Start()
    {
        //CalculateNeighbours();
    }

    // Update is called once per frame
    void Update()
    {
		
    }
	
	public void SetToTerrain() {
		BoxCollider bc = GetComponent<BoxCollider>();
		RaycastHit rayHit;
		LayerMask mask = LayerMask.GetMask("Environment");
		if (Physics.Raycast(transform.position, Vector3.down, out rayHit, Mathf.Infinity, mask)) { //can only be fired below since firing upwards does not seem to interact with the Terrain's collider
			if (rayHit.transform.tag == "Environment") {
				AdjustYPos(rayHit.point.y + 0.05f);
			}
		}
		Collider[] hits = Physics.OverlapBox(transform.position + bc.center, bc.size / 2f, Quaternion.identity, mask);
		if (hits.Length > 0) {
			shouldBeDestroyed = true;
		}
	}
	
	public bool GetShouldBeDestroyed() {
		return shouldBeDestroyed;
	}
	
	
	public void CalculateNeighbours() {
		Collider[] hits = Physics.OverlapSphere(transform.position, 1.035f);
		foreach (Collider hit in hits) {
			Transform parent = hit.transform.parent;
			if (hit.transform.parent != null) {
				HexPanel contender = parent.GetComponent<HexPanel>();
				if (contender != this) {
					neighbours.Add(contender);
				}
			}
		}
	}
	
	public List<HexPanel> GetNeighbours() {
		return neighbours;
	}
	
	public bool GetScaleModifiedByCode() {
		return scaleModifiedByCode;
	}
	
	public void AdjustHeightToNeighbours(float stepHeight) { //this doesn't work properly yet
		if (scaleModifiedByCode) {
			AdjustHeight(1f);
		}
		float highestSteps = 0f;
		foreach (HexPanel HP in neighbours) {
			float numSteps = HP.transform.localScale.y / stepHeight;
			numSteps = Mathf.Floor(numSteps);
			if (numSteps > highestSteps) {
				highestSteps = numSteps;
			}
		}
		if (highestSteps > 0f) {
			AdjustHeight(highestSteps * stepHeight);
			scaleModifiedByCode = true;
		}
	}
	
	void AdjustHeight(float height) {
		Vector3 scale = transform.localScale;
		scale.y = height;
		transform.localScale = scale;
	}
	
	void AdjustYPos(float newY) {
		Vector3 pos = transform.position;
		pos.y = newY;
		transform.position = pos;
	}
}
