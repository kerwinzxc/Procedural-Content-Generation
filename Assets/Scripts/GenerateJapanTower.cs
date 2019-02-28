﻿using System.Collections.Generic;
using UnityEngine;

public class GenerateJapanTower : MonoBehaviour
{
    [System.Serializable]
    public class TowerParts
    {
        public GameObject Empty;
        public GameObject Base;
        public GameObject Connector;
        public GameObject Top;
    };

    public TowerParts parts;

    private GameObject tempEmpty;
    private GameObject tempBase;
    private GameObject tempConnector;
    private GameObject tempTop;

    public int maxLevel = 2;
    public int level;

    private float height;
    public float offset = 0.1f;

    public void Generate()
    {
        level = Random.Range(1, maxLevel + 1);

        GenerateBase();
        GenerateTop();
    }

    void GenerateBase()
    {
        DestroyObject();

        tempEmpty = Instantiate(parts.Empty, this.transform);
        if (tempEmpty)
        {

            this.name = "JapanTower";
            tempEmpty.name = "Tower";
            tempEmpty.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
            Debug.LogError("An empty gameobject has not been set to empty on the game manager object.");

        tempBase = Instantiate(parts.Base, tempEmpty.transform);
        if (tempBase)
            tempBase.name = "TowerBase";
        else
            Debug.LogError("The Base gameobject has not been set on the game manager object.");
    }

    void GenerateTop()
    {
        height = tempBase.GetComponent<MeshFilter>().sharedMesh.bounds.extents.z * 2;

        for(int i = 0; i < level; i++)
        {
            tempConnector = Instantiate(parts.Connector, tempEmpty.transform);
            if (tempConnector)
            {
                tempConnector.name = "TowerConnector";
                tempConnector.transform.localPosition = new Vector3(0, height, 0);
                height += tempConnector.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh.bounds.extents.z * 3;
            }
            else
                Debug.LogError("The tower gameobject has not been set on the game manager object.");

            tempTop = Instantiate(parts.Top, tempEmpty.transform);
            if (tempTop)
            {
                float rand = Random.Range(0.5f, 2.0f);

                tempTop.name = "TowerTop";
                tempTop.transform.localScale = new Vector3(rand, GenerateTopScale(rand), rand);
                tempTop.transform.localPosition = new Vector3(0, height - offset, 0);
                height += (tempTop.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh.bounds.extents.z * tempTop.transform.localScale.y);
            }
            else
                Debug.LogError("The tower gameobject has not been set on the game manager object.");
        }
    }

    private float GenerateTopScale(float x)
    {
        float scaleY = 1.0f;

        if (x < 1.0f)
            scaleY = 1.0f;
        else
            scaleY = scaleY - (x - 1.0f);

        if (scaleY < 0.5f)
            scaleY = 0.5f;

        return scaleY;
    }

    public void DestroyObject()
    {
        List<GameObject> objectsToDelete = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
            objectsToDelete.Add(transform.GetChild(i).gameObject);

        foreach (GameObject item in objectsToDelete)
            DestroyImmediate(item);

        this.name = "SpawnJapanTower";
    }
}
