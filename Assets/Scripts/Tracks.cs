using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracks : MonoBehaviour
{
    public static Tracks instance;

    public bool active;
    [SerializeField] private GameObject trackPrefab;
    private List<GameObject> trackPool = new List<GameObject>();
    private List<GameObject> currentList = new List<GameObject>();
    private List<float> initX = new List<float>() { -12f, -8.4f, -4.8f, -1.2f, 2.4f, 6f, 9.6f }; //3.6
    private float startX = -12f;
    private float endX = 13.2f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        active = true;

        for (int i = 0; i < initX.Count; i++)
        {
            if (currentList.Count < initX.Count)
            {
                GameObject go = Spawn(initX[i]);
            }
        }
    }

    void LateUpdate()
    {
        if (active)
        {
            for (int i = 0; i < currentList.Count; i++)
            {
                if (currentList[i].transform.position.x >= endX)
                {
                    currentList[i].SetActive(false);
                    trackPool.Add(currentList[i]);
                    currentList.RemoveAt(i);
                    Spawn(startX);
                    break;
                }
            }
        }
    }

    GameObject Spawn(float _x)
    {
        GameObject go = null;
        if (trackPool.Count == 0)
        {
            go = Instantiate(trackPrefab);
        }
        else
        {
            go = trackPool[0];
            trackPool.RemoveAt(0);
            go.SetActive(true);
        }
        currentList.Add(go);
        go.transform.SetParent(this.transform);
        go.transform.position = new Vector3(_x, -3.52f, 2.5f);
        return go;
    }
}
