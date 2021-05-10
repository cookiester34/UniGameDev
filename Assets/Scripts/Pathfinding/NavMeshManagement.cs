﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NavMeshManagement : MonoBehaviour
{
    #region Instance
    private static NavMeshManagement _instance = null;
    public static NavMeshManagement Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning("2nd instance of Nav Mesh Management being created, destroy");
            Destroy(gameObject);
            return;
        }
        _instance = this;
        //time = toggleTime;
    }
    #endregion

    //public float toggleTime = 10f;
    private float time;
    [HideInInspector]
    public List<Transform> towersAffectingNavmesh = new List<Transform>();
    private int oldTowerCount = 0;
    private bool canScan = true;
    private bool scanQue = false;
    //private bool toggle = false;

    private void Update()
    {
        time = -Time.deltaTime;
        if(time <= 0)
        {
            CheckTowerList();
            time = 5f;
        }
        //if(time <= 0 && towersAffectingNavmesh.Count > 0)
        //{
        //    time = toggleTime;
        //    ScanGraphs();
        //    toggle = !toggle;
        //    DeactivateBuildings(toggle);
        //}
        if(towersAffectingNavmesh.Count != oldTowerCount)
        {
            oldTowerCount = towersAffectingNavmesh.Count;
            //DeactivateBuildings(true);
            ScanGraphs();
        }
        if (scanQue && canScan)
        {
            ScanGraphs();
            scanQue = false;
        }

    }

    void CheckTowerList()
    {
        List<Transform> temp = towersAffectingNavmesh;
        foreach (Transform i in temp)
        {
            if (i != null)
                towersAffectingNavmesh.Remove(i);
        }
    }

    //void DeactivateBuildings(bool toggle)
    //{
    //    List<Transform> temp = towersAffectingNavmesh;
    //    towersAffectingNavmesh.Clear();
    //    foreach(Transform i in temp)
    //    {
    //        if (i != null)
    //            towersAffectingNavmesh.Add(i);
    //    }

    //    foreach (Transform navObsticle in towersAffectingNavmesh)
    //    {
    //        navObsticle.gameObject.SetActive(toggle);
    //    }
    //}

    private void ScanGraphs()
    {
        ///this will scan all graphs faster but also freezes game for less than a second but noticible.
        //AstarPath.active.Scan();

        ///will scan all graphs without freezing game, but is a lot slower.
        StartCoroutine("ScanAsync");
        
    }
    IEnumerator ScanAsync()
    {
        if (canScan)
        {
            Debug.Log("Scanning Graph");
            foreach (Progress progress in AstarPath.active.ScanAsync())
            {
                //Debug.Log("Scanning... " + progress.description + " - " + (progress.progress * 100).ToString("0") + "%");
                canScan = false;
                yield return null;
            }
            canScan = true;
        }
        else
            scanQue = true;
    }
}
