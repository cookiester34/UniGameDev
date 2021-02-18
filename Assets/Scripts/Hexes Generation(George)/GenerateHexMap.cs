using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;

public class GenerateHexMap : MonoBehaviour
{
	[Range(1, 100)]
	public int width = 1;
	[Range(1, 100)]
	public int length = 1;
	float stepheight = 5f;
	float bigRadius = 1.195f;
	float smallRadius = 1.035f;
	bool shouldOffsetRow = true;
	public GameObject defaultHex;
	public Material baseMat;
	public Color[] colours;
	[SerializeField] private bool hexesVisible;
	
	public InputField widthInputField;
	public InputField lengthInputField;
	public Button generateButton;
	bool isUISetup;
	
    // Start is called before the first frame update
    void Start()
    {
		SetupUI();
    }
	
	void SetupUI() {
		try { //try to find the UI input fields if they have not been manually assigned
			if (!generateButton) {
				generateButton = GameObject.Find("BtnGenerate").GetComponent<Button>();
			}
			if (!widthInputField) {
				widthInputField = GameObject.Find("TxtGridWidthInput").GetComponent<InputField>();
			}
			if (!lengthInputField) {
				lengthInputField = GameObject.Find("TxtGridLengthInput").GetComponent<InputField>();
			}
			
			//add the "GenerateInGame" function to the Generate button
			generateButton.onClick.AddListener(GenerateInGame);
			isUISetup = true;
			
		} catch { //if 1 or more cannot be found then report an error
			Debug.Log("UI Elements for grid generation have not all been assigned! Please set them in the Inspector for play mode generation to be supported.");
			isUISetup = false; //shouldn't need this but just in case
		}
	}
	
	public void Generate() {
		shouldOffsetRow = true; //need to reset this for consistency
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
				//SetRandColour(inst);
			}
		}
		foreach (Transform child in transform) {
			HexPanel HP = child.GetComponent<HexPanel>();
			HP.ToggleVisibility(hexesVisible);
			HP.CalculateNeighbours();
			HP.SetToTerrain();
		}
		foreach (Transform child in transform) { //annoying having to do this loop twice. Trust me, it needs to be here tho
			HexPanel HP = child.GetComponent<HexPanel>();
			HP.TryToMatchHeightWithNeighbour();
		}
		DestroyUnneededHexes();
	}
	
	//"InGame" functions are made to be ideally be called from play mode rather than edit mode
	public void GenerateInGame() {
		if (isUISetup) {
			GetParamsFromUI();
			DestroyGridInGame();
			Generate();
		}
	}
	
	public void ToggleVisibility() {
		hexesVisible = !hexesVisible;
		List<HexPanel> hexes = Object.FindObjectsOfType<HexPanel>().ToArray().ToList();
		for (int i = 0; i < hexes.Count; i++) {
			hexes[i].ToggleVisibility(hexesVisible);
		}
	}
	
	void GetParamsFromUI() {
		string widthStr = widthInputField.text;
		string lengthStr = lengthInputField.text;
		
		if (int.TryParse(widthStr, out width)) {
			width = Mathf.Clamp(int.Parse(widthInputField.text), 1, 100);
		}
		else {
			width = 1;
		}
		
		if (int.TryParse(lengthStr, out length)) {
			length = Mathf.Clamp(int.Parse(lengthInputField.text), 1, 100);
		}
		else {
			length = 1;
		}
		
	}
	
	public void UpdateGrid() {
		foreach (Transform child in transform) {
			HexPanel HP = child.GetComponent<HexPanel>();
			HP.AdjustHeightToNeighbours(stepheight);
		}
	}

	public void DestroyGrid() {
		List<HexPanel> hexes = Object.FindObjectsOfType<HexPanel>().ToArray().ToList();
		for (int i = 0; i < hexes.Count; i++) {
			DestroyImmediate(hexes[i].gameObject);
		}
	}
	
	void DestroyUnneededHexes() {
		List<HexPanel> hexes = Object.FindObjectsOfType<HexPanel>().ToArray().ToList();
		for (int i = 0; i < hexes.Count; i++) {
			if (hexes[i].GetShouldBeDestroyed()) {
				hexes[i].RemoveFromNeighboursList();
				if (!Application.isPlaying) {
					DestroyImmediate(hexes[i].gameObject);
				}
				else {
					Destroy(hexes[i].gameObject);
				}
			}
		}
	}
	
	void DestroyGridInGame() {
		HexPanel[] panels = Object.FindObjectsOfType<HexPanel>().ToArray();
        foreach (HexPanel HP in panels) {
            Object.Destroy(HP.gameObject);
        }
	}

	public void SetRandColour(GameObject go) {
		Material mat = new Material(baseMat);
		mat.color = colours[Random.Range(0, colours.Length)];
		MeshRenderer mr = go.transform.GetChild(0).GetComponent<MeshRenderer>();
		mr.material = mat;
	}
}
