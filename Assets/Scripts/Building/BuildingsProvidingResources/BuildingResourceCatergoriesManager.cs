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
                if (r.Resource.resourceType == ResourceType.Population)
                {
                    var glowEnabler = r.transform.GetComponentInParent<GlowEnabler>();
                    if (glowEnabler != null)
                    {
                        glowEnabler.EnableGlow(true);
                    }
                }
                else if (r.Resource.GetResourceTickAmount() > 0)
                {
                    var glowEnabler = r.transform.GetComponentInParent<GlowEnabler>();
                    if (glowEnabler != null)
                    {
                        glowEnabler.EnableGlow(true);
                    }
                }
                else
                {
                    //could glow red here
                }
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
            }
        }
    }
}
