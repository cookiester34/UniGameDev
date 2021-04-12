using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extension of radial progress where it will then use a resource supplier to set its speed and color
/// </summary>
public class SupplierRadialProgress : RadialProgress {
    [SerializeField] private ResourceSupplier supplier;
    private bool previouslyCapped;

    public void Awake() {
        if (supplier == null) {
            Debug.LogWarning("Supplier radial progress has no supplier set in inspector, the radial progress will not function");
            return;
        }
        supplier.Resource.OnCapReached += ActivateProgressBar; // Event from resource in supplier when cap is reached.
        supplier.Resource.OnCurrentValueChanged += delegate { ActivateProgressBar(); };
        supplier.ProductionChanged += ActivateProgressBar;
        previouslyCapped = false;
    }
    
    public void ActivateProgressBar() {
        bool capped = supplier.Resource.ResourceCapReached();
        progressBar.color = capped ? Color.red : Color.green;
        if (previouslyCapped && !capped) {
            currentValue = 0;
            progressBar.fillAmount = 0;
        }
        Activate(supplier.ProductionAmount / supplier.ProductionTime / ResourceManagement.Instance.resourceTickTime);
        previouslyCapped = capped;
    }
}
