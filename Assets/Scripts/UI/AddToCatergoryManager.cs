using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToCatergoryManager : MonoBehaviour
{
    public bool housing = false;
    public bool storage = false;
    // Start is called before the first frame update
    void Start()
    {
        if(housing)
            BuildingResourceCatergoriesManager.instance.housing.Add(this.gameObject);
        if(storage)
            BuildingResourceCatergoriesManager.instance.storageBuildings.Add(this.gameObject);
    }
}
