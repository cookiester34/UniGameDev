using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexPanel : MonoBehaviour
{
	List<HexPanel> neighbours = new List<HexPanel>();
	bool scaleModifiedByCode;
    // Start is called before the first frame update
    void Start()
    {
        //CalculateNeighbours();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
