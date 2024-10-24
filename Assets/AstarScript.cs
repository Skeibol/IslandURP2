using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AstarScript : MonoBehaviour
{
    public static AstarScript Instance;
    private GridGraph gg;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
        AstarData data = AstarPath.active.data;
        gg = AstarPath.active.data.gridGraph;

        gg.SetDimensions( 40, 40, 1);
        StartCoroutine(setCenter());
    }

    public void centerPathfindingGraph(Vector3 pos)
    {
        gg.center = pos;
        gg.Scan();
    }

    IEnumerator setCenter()
    {
        yield return new WaitForSeconds(0.2f);
        var plyr = GameObject.Find("Player").transform;
        gg.center = new Vector3(plyr.position.x,plyr.position.y,0);
        gg.Scan();
        yield return 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
