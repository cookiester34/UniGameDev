using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BuildingResourceCatergoriesManager : MonoBehaviour
{
    public static BuildingResourceCatergoriesManager instance;

    private void Start()
    {
        if(instance != null)
        {
            Destroy(this);
            return;
        }
        instance = this;
    }

    public List<ResourceSupplier> catergories = new List<ResourceSupplier>();
    public List<GameObject> housing = new List<GameObject>();
    public List<GameObject> storageBuildings = new List<GameObject>();
    private int selectedResource = -1;

    void CheckList()
    {
        var tempList = catergories;
        foreach(ResourceSupplier r in tempList.ToList())
        {
            if (r == null)
            {
                catergories.Remove(r);
            }
        }
    }

    public void OnResourceHover(int i)
    {
        CheckList();
        foreach(ResourceSupplier r in catergories)
        {
            if((int)r.Resource.resourceType == i)
            {
                if (r.GetProductionAmount() < 0)
                {
                    var glowEnabler = r.transform.GetComponentInParent<GlowEnabler>();
                    if (glowEnabler != null)
                    {
                        glowEnabler.EnableGlow(true);
                        glowEnabler.ChangeGlowColour(Color.red);
                    }
                }
                else if(r.GetProductionAmount() > 0.05f)
                {
                    var glowEnabler = r.transform.GetComponentInParent<GlowEnabler>();
                    if (glowEnabler != null)
                    {
                        glowEnabler.EnableGlow(true);
                        glowEnabler.ChangeGlowColour(Color.green);
                    }
                }
                else
                {
                    var glowEnabler = r.transform.GetComponentInParent<GlowEnabler>();
                    if (glowEnabler != null)
                    {
                        glowEnabler.EnableGlow(true);
                        glowEnabler.ChangeGlowColour(Color.white);
                    }
                }
            }
        }
        foreach(GameObject house in housing)
        {
            if((ResourceType)i == ResourceType.Population)
            {
                var glowEnabler = house.transform.GetComponent<GlowEnabler>();
                if (glowEnabler != null)
                {
                    glowEnabler.EnableGlow(true);
                    glowEnabler.ChangeGlowColour(Color.white);
                }
            }
        }
        foreach(GameObject storage in storageBuildings)
        {
            var glowEnabler = storage.transform.GetComponent<GlowEnabler>();
            if (glowEnabler != null)
            {
                glowEnabler.EnableGlow(true);
                glowEnabler.ChangeGlowColour(Color.white);
            }
        }
    }

    public void OnHoverExit()
    {
        selectedResource = -1;
        foreach (ResourceSupplier r in catergories)
        {
            var glowEnabler = r.transform.GetComponentInParent<GlowEnabler>();
            if (glowEnabler != null)
            {
                glowEnabler.EnableGlow(false);
                glowEnabler.ChangeGlowColour(Color.white);
            }
        }
        foreach (GameObject house in housing)
        {
            var glowEnabler = house.transform.GetComponent<GlowEnabler>();
            if (glowEnabler != null)
            {
                glowEnabler.EnableGlow(false);
                glowEnabler.ChangeGlowColour(Color.white);
            }
        }
        foreach (GameObject storage in storageBuildings)
        {
            var glowEnabler = storage.transform.GetComponent<GlowEnabler>();
            if (glowEnabler != null)
            {
                glowEnabler.EnableGlow(false);
                glowEnabler.ChangeGlowColour(Color.white);
            }
        }
    }
}
