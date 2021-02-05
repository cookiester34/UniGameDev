using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for hex neighbours directions
/// </summary>
public enum NeighbourDirection {
    Above, // position.x greater
    AboveRight, // position.x greater, position.z lower
    BelowRight, // position.x lower, position.z lower
    Below, // position.x lower
    BelowLeft, // position.x lower, position.z greater
    AboveLeft // position.x greater, position.z greater
}

public static class NeighbourDirectionExtension {
    /// <summary>
    /// Determines if the neighbour is in the given direction
    /// </summary>
    /// <param name="direction">Direction to test for</param>
    /// <param name="position">position of the central hex</param>
    /// <param name="comparedPosition">position of the neighbour being checked</param>
    /// <returns>True if the neighbour is in the given direction, else false</returns>
    public static bool InDirection(this NeighbourDirection direction, Vector3 position, Vector3 comparedPosition) {
        bool inDirection = false;
        switch (direction) {
            case NeighbourDirection.Above:
                inDirection = comparedPosition.x > position.x && (position.z == comparedPosition.z);
                break;
            case NeighbourDirection.Below:
                inDirection = (comparedPosition.x < position.x) && (position.z == comparedPosition.z);
                break;
            case NeighbourDirection.BelowLeft:
                inDirection = (comparedPosition.x < position.x) && (comparedPosition.z > position.z);
                break;
            case NeighbourDirection.BelowRight:
                inDirection = (comparedPosition.x < position.x) && (comparedPosition.z < position.z);
                break;
            case NeighbourDirection.AboveLeft:
                inDirection = (comparedPosition.x > position.x) && (comparedPosition.z < position.z);
                break;
            case NeighbourDirection.AboveRight:
                inDirection = (comparedPosition.x > position.x) && (comparedPosition.z < position.z);
                break;
        }

        return inDirection;
    } 
}
