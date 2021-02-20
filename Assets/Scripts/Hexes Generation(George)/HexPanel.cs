using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class HexPanel : MonoBehaviour
{
	[SerializeField] List<HexPanel> neighbours = new List<HexPanel>();
	bool scaleModifiedByCode;
	bool shouldBeDestroyed = true;
	MeshRenderer mr;
	private BuildingFoundation _buildingFoundation;

	public BuildingFoundation BuildingFoundation => _buildingFoundation;

	private void Awake() {
		_buildingFoundation = GetComponent<BuildingFoundation>();
		mr = GetComponentInChildren<MeshRenderer>();
	}

	public void SetToTerrain() {
		RaycastHit rayHit;
		LayerMask mask = LayerMask.GetMask("Environment");
		if (Physics.Raycast(transform.position, Vector3.down, out rayHit, Mathf.Infinity, mask)) { //can only be fired below since firing upwards does not seem to interact with the Terrain's collider
			AdjustYPos(rayHit.point.y + 0.05f);
			shouldBeDestroyed = false;
			if (CheckForTerrainOverlap()) {
				shouldBeDestroyed = true;
			}
		}

	}
	
	public void TryToMatchHeightWithNeighbour() {
		SortNeighboursByHeight();
		for (int i = 0; i < neighbours.Count; i++) {
			float heightDiff = neighbours[i].transform.position.y - transform.position.y;
			if (heightDiff <= 0.3f && heightDiff > 0f) {
				AdjustYPos(neighbours[i].transform.position.y);
				if (!CheckForTerrainOverlap()) {
					shouldBeDestroyed = false;
					return;
				}
			}
		}
	}
	
	public void ToggleVisibility(bool val) {
		if (!mr) {
			mr = GetComponentInChildren<MeshRenderer>();
		}
		mr.enabled = val;
	}
	
	void SortNeighboursByHeight() {
		neighbours = neighbours.OrderBy(x => x.transform.position.y).ToList();
	}
	
	bool CheckForTerrainOverlap() {
		BoxCollider bc = GetComponent<BoxCollider>();
		LayerMask mask = LayerMask.GetMask("Environment");
		Collider[] hits = Physics.OverlapBox(transform.position + bc.center, bc.size / 2f, Quaternion.identity, mask);
		if (hits.Length > 0) {
			return true;
		}
		return false;
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

	/// <summary>
	/// Removes this panel from the neighbours list of its neighbours
	/// </summary>
	public void RemoveFromNeighboursList() {
		foreach (HexPanel neighbour in neighbours) {
			neighbour.neighbours.Remove(this);
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

	public HexPanel GetNeighbour(NeighbourDirection direction) {
		foreach (HexPanel neighbour in neighbours) {
			if (direction.InDirection(transform.position, neighbour.transform.position)) {
				return neighbour;
			}
		}

		return null;
	}

	private void OnDrawGizmosSelected() {
		foreach (HexPanel neighbour in neighbours) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawCube(neighbour.transform.position, new Vector3(0.5f, 0.5f, 0.5f));
		}
	}
}
