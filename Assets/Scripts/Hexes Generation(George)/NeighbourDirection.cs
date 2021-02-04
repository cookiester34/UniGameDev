using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NeighbourDirection {
    Above, // position.x greater
    AboveRight, // position.x greater, position.z lower
    BelowRight, // position.x lower, position.z lower
    Below, // position.x lower
    BelowLeft, // position.x lower, position.z greater
    AboveLeft // position.x greater, position.z greater
}

public static class NeighbourDirectionExtension {
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
                inDirection = (comparedPosition.x - position.x) < 0 && comparedPosition.z - position.z > 0;
                break;
            case NeighbourDirection.BelowRight:
                inDirection = (comparedPosition.x - position.x) < 0 && comparedPosition.z - position.z < 0;
                break;
            case NeighbourDirection.AboveLeft:
                inDirection = (comparedPosition.x - position.x) > 0 && comparedPosition.z - position.z > 0;
                break;
            case NeighbourDirection.AboveRight:
                inDirection = (comparedPosition.x - position.x) > 0 && comparedPosition.z - position.z < 0;
                break;
        }

        return inDirection;
    } 
}
