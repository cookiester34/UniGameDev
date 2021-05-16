using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Foundation that can be built on, handles whether the tile can be built on for differing sizes
/// </summary>
[RequireComponent(typeof(HexPanel))]
public class BuildingFoundation : MonoBehaviour {
    private HexPanel _hexPanel;
    private Renderer _renderer;

    [SerializeField] private bool _canBuild = true;

    private Color currentColor;
    private static Color canBuildColor = new Color(0.78f, 0.99f, 0.11f, 1f);
    private static Color cannotBuildColor = new Color(0.8f, 0.2f, 0.2f, 0.6f);
    private static Color invisibleColor = new Color(0f, 0f, 0f, 0f);
    private MaterialPropertyBlock _propBlock;
    static List<BuildingFoundation> _foundations = new List<BuildingFoundation>();

    public static Color InvisibleColor => invisibleColor;

    public bool CanBuild {
        get => _canBuild;
        set {
            _canBuild = value;
            if (currentColor != invisibleColor) {
                UpdateVisibleColour();
            }
        }
    }

    private void Awake() {
        _hexPanel = GetComponent<HexPanel>();
        _renderer = GetComponentInChildren<Renderer>();
        _propBlock = new MaterialPropertyBlock();
        _foundations.Add(this);
    }

    public static void Hide() {
        foreach (BuildingFoundation foundation in _foundations) {
            foundation.UpdateVisibleColour(invisibleColor);
        }
    }

    public static void Show() {
        foreach (BuildingFoundation foundation in _foundations) {
            foundation.UpdateVisibleColour();
        }
    }

    public void OnDestroy() {
        _foundations.Remove(this);
    }

    /// <summary>
    /// Gets the central position for the building
    /// </summary>
    /// <param name="shape">The building shape</param>
    /// <returns>The central position for the building</returns>
    public Vector3 BuildingPosition(BuildingShape shape) {
        Vector3 buildingCenter = Vector3.zero;
        List<BuildingFoundation> foundations = GetFoundations(shape);
        foreach (BuildingFoundation foundation in foundations) {
            buildingCenter += foundation._renderer.bounds.center;
        }

        buildingCenter /= foundations.Count;
        return buildingCenter;
    }

    /// <summary>
    /// Checks if the tiles are available for building and changes them to not available if they can
    /// </summary>
    /// <param name="shape">The shape of tiles that the building will take</param>
    /// <param name="updateBuildStatus">Whether the can build flag should be switched</param>
    /// <returns>Whether the building can be built</returns>
    public bool BuildMulti(BuildingShape shape, bool updateBuildStatus = true) {
        bool canBuild = false;
        List<BuildingFoundation> foundations = GetFoundations(shape);

        switch (shape) {
            case BuildingShape.OneTile:
                canBuild = _canBuild;
                if (updateBuildStatus) {
                    CanBuild = false;
                }

                break;

            case BuildingShape.ThreeTile:
                if (foundations.Count == 3) {
                    foreach (BuildingFoundation foundation in foundations) {
                        canBuild = foundation.CanBuild;
                        if (!canBuild) {
                            break;
                        }
                    }

                    if (updateBuildStatus && canBuild) {
                        CanBuild = false;
                        foreach (BuildingFoundation foundation in foundations) {
                            foundation.CanBuild = false;
                        }
                    }
                }
                break;

            case BuildingShape.SevenTile:
                if (foundations.Count == 7) {
                    foreach (BuildingFoundation foundation in foundations) {
                        canBuild = foundation.CanBuild;
                        if (!canBuild) {
                            break;
                        }
                    }

                    if (updateBuildStatus && canBuild) {
                        CanBuild = false;
                        foreach (BuildingFoundation foundation in foundations) {
                            foundation.CanBuild = false;
                        }
                    }
                }
                break;
            
            case BuildingShape.SevenTileJut:
                if (foundations.Count == 8) {
                    foreach (BuildingFoundation foundation in foundations) {
                        canBuild = foundation.CanBuild;
                        if (!canBuild) {
                            break;
                        }
                    }

                    if (updateBuildStatus && canBuild) {
                        CanBuild = false;
                        foreach (BuildingFoundation foundation in foundations) {
                            foundation.CanBuild = false;
                        }
                    }
                }
                break;

            case BuildingShape.ThreeThreeTile:
                if (foundations.Count == 9) {
                    foreach (BuildingFoundation foundation in foundations) {
                        canBuild = foundation.CanBuild;
                        if (!canBuild) {
                            break;
                        }
                    }

                    if (updateBuildStatus && canBuild) {
                        CanBuild = false;
                        foreach (BuildingFoundation foundation in foundations) {
                            foundation.CanBuild = false;
                        }
                    }
                }
                break;

            case BuildingShape.Semicircle: {
                if (foundations.Count == 5) {
                    foreach (BuildingFoundation foundation in foundations) {
                        canBuild = foundation.CanBuild;
                        if (!canBuild) {
                            break;
                        }
                    }

                    if (updateBuildStatus && canBuild) {
                        CanBuild = false;
                        foreach (BuildingFoundation foundation in foundations) {
                            foundation.CanBuild = false;
                        }
                    }
                }
                break;
            }
            
            case BuildingShape.Square:
                if (foundations.Count == 4) {
                    foreach (BuildingFoundation foundation in foundations) {
                        canBuild = foundation.CanBuild;
                        if (!canBuild) {
                            break;
                        }
                    }

                    if (updateBuildStatus && canBuild) {
                        CanBuild = false;
                        foreach (BuildingFoundation foundation in foundations) {
                            foundation.CanBuild = false;
                        }
                    }
                }
                break;
        }

        return canBuild;
    }

    public List<BuildingFoundation> GetFoundations(BuildingShape shape) {
        List<BuildingFoundation> foundations = new List<BuildingFoundation>();
        switch (shape) {
            case BuildingShape.OneTile:
                foundations.Add(this);
                break;

            case BuildingShape.ThreeTile:
                foundations.Add(this);
                HexPanel br = _hexPanel.GetNeighbour(NeighbourDirection.BelowRight);
                HexPanel ar = _hexPanel.GetNeighbour(NeighbourDirection.AboveRight);
                if (ar != null) {
                    foundations.Add(ar.BuildingFoundation);
                }

                if (br != null) {
                    foundations.Add(br.BuildingFoundation);
                }

                break;

            case BuildingShape.SevenTile:
                foundations.Add(this);
                foreach (HexPanel neighbour in _hexPanel.GetNeighbours()) {
                    foundations.Add(neighbour.BuildingFoundation);
                }
                break;

            case BuildingShape.SevenTileJut: {
                foundations.Add(this);
                foreach (HexPanel neighbour in _hexPanel.GetNeighbours()) {
                    foundations.Add(neighbour.BuildingFoundation);
                }

                var below = _hexPanel.GetNeighbour(NeighbourDirection.Below);
                if (below != null) {
                    var belowRight = below.GetNeighbour(NeighbourDirection.BelowRight);
                    if (belowRight != null) {
                        foundations.Add(belowRight.BuildingFoundation);
                    }
                }

                break;
            }

            case BuildingShape.ThreeThreeTile: {
                foundations.Add(this);
                var above = _hexPanel.GetNeighbour(NeighbourDirection.Above);
                var below = _hexPanel.GetNeighbour(NeighbourDirection.Below);

                var belowLeft = _hexPanel.GetNeighbour(NeighbourDirection.BelowLeft);
                if (belowLeft != null) {
                    foundations.Add(belowLeft.BuildingFoundation);
                }

                var aboveLeft = _hexPanel.GetNeighbour(NeighbourDirection.AboveLeft);
                if (aboveLeft != null) {
                    foundations.Add(aboveLeft.BuildingFoundation);
                }

                if (above != null) {
                    foundations.Add(above.BuildingFoundation);
                    var twoAbove = above.GetNeighbour(NeighbourDirection.Above);
                    if (twoAbove != null) {
                        foundations.Add(twoAbove.BuildingFoundation);
                    }

                    var aboveAboveRight = above.GetNeighbour(NeighbourDirection.AboveRight);
                    if (aboveAboveRight) {
                        foundations.Add(aboveAboveRight.BuildingFoundation);
                    }
                }

                if (below != null) {
                    foundations.Add(below.BuildingFoundation);
                    var twoBelow = below.GetNeighbour(NeighbourDirection.Below);
                    if (twoBelow != null) {
                        foundations.Add(twoBelow.BuildingFoundation);
                    }

                    var belowBelowRight = below.GetNeighbour(NeighbourDirection.BelowRight);
                    if (belowBelowRight != null) {
                        foundations.Add(belowBelowRight.BuildingFoundation);
                    }
                }

                break;
            }

            case BuildingShape.Square: {
                foundations.Add(this);
                var above = _hexPanel.GetNeighbour(NeighbourDirection.Above);
                var aboveRight = _hexPanel.GetNeighbour(NeighbourDirection.AboveRight);
                if (aboveRight != null) {
                    foundations.Add(aboveRight.BuildingFoundation);
                }

                if (above != null) {
                    foundations.Add(above.BuildingFoundation);
                    var aboveAboveRight = above.GetNeighbour(NeighbourDirection.AboveRight);
                    if (aboveAboveRight != null) {
                        foundations.Add(aboveAboveRight.BuildingFoundation);
                    }
                }

                break;
            }

            case BuildingShape.Semicircle: {
                foundations.Add(this);
                var above = _hexPanel.GetNeighbour(NeighbourDirection.Above);
                if (above != null) {
                    foundations.Add(above.BuildingFoundation);
                }
                
                var below = _hexPanel.GetNeighbour(NeighbourDirection.Below);
                if (below != null) {
                    foundations.Add(below.BuildingFoundation);
                }
                
                var aboveRight = _hexPanel.GetNeighbour(NeighbourDirection.AboveRight);
                if (aboveRight != null) {
                    foundations.Add(aboveRight.BuildingFoundation);
                }
                
                var belowRight = _hexPanel.GetNeighbour(NeighbourDirection.BelowRight);
                if (belowRight != null) {
                    foundations.Add(belowRight.BuildingFoundation);
                }
                break;
            }
        }

        return foundations;
    }

    public void UpdateVisibleColour(float distance = 1, float maxDistance = 1) {
        Color usedColor = _canBuild ? canBuildColor : cannotBuildColor;
        Color lerpedColor = Color.Lerp(invisibleColor, usedColor, distance / maxDistance);
        UpdateVisibleColour(lerpedColor);
    }

    public void UpdateVisibleColour(Color color) {
        currentColor = color;
        if (ApplicationUtil.IsQuitting) {
            return;
        }

        if (_renderer == null) {
            _renderer = GetComponentInChildren<Renderer>();
        }

        if (_propBlock == null) {
            _propBlock = new MaterialPropertyBlock();
        }

        _renderer.GetPropertyBlock(_propBlock);
        _propBlock.SetColor("_Color", color);
        _renderer.SetPropertyBlock(_propBlock);
    }
}
