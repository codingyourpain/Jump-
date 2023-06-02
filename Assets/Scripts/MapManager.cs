using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject Pool;
    public List<GameObject> Platforms;

    public Vector2 spawnPoint;

    [SerializeField] GameObject lastPlatform;
    GameObject savedLast;


    private void Start()
    {
        savedLast = lastPlatform;
    }

    public void generateMap(GameObject player)
    {
        Vector2 p;
        if (Pool.transform.childCount > 0)
        {
            int sc = 3;
            foreach (Transform platform in Pool.GetComponentInChildren<Transform>())
            {
                if (sc > 0)
                {
                    if (platform.GetComponent<PlatformElement>().id == 0)
                    {
                        platform.parent = transform.parent;
                        if (lastPlatform != null)
                        {
                            if (lastPlatform != null)
                            {
                                platform.position = (Vector2)lastPlatform.transform.position + new Vector2(lastPlatform.transform.localScale.x / 2, 0);
                                lastPlatform = platform.gameObject;
                            }
                            else
                            {
                                platform.position = new Vector2(0, -3);
                                lastPlatform = platform.gameObject;
                            }
                        }
                        sc--;
                    }
                }
                else break;
            }
        }
        else
        {
            foreach (GameObject platform in Platforms)
            {
                if (!platform.GetComponent<PlatformElement>()) platform.AddComponent<PlatformElement>();
                platform.GetComponent<PlatformElement>().player = player;
                platform.GetComponent<PlatformElement>().Pool = Pool;
                platform.GetComponent<PlatformElement>().controller = GetComponent<GameController>();
                if (lastPlatform != null)
                {
                    GameObject newPlatform = Instantiate(
                        platform,
                        (Vector2)lastPlatform.transform.position + new Vector2(lastPlatform.transform.localScale.x / 2, 0),// + Random.Range(0,1),0),//Random.Range(0,2)),
                        Quaternion.identity
                    );
                    lastPlatform = newPlatform;
                }
                else
                {
                    GameObject newPlatform = Instantiate(platform, new Vector2(0, -3), Quaternion.identity);
                    lastPlatform = newPlatform;
                }
                p = (Vector2)platform.transform.position + new Vector2(platform.transform.localScale.x / 2, 0);
            }
            //foreach (GameObject platform in Platforms)
            //{
            //    if (!platform.GetComponent<PlatformElement>()) platform.AddComponent<PlatformElement>();
            //    platform.GetComponent<PlatformElement>().player = player;
            //    platform.GetComponent<PlatformElement>().Pool = Pool;
            //    platform.GetComponent<PlatformElement>().controller = GetComponent<GameController>();
            //    if (lastPlatform != null)
            //    {
            //        GameObject newPlatform = Instantiate(
            //            platform,
            //            (Vector2)lastPlatform.transform.position + new Vector2(lastPlatform.transform.localScale.x / 2, 0),// + Random.Range(0,1),0),//Random.Range(0,2)),
            //            Quaternion.identity
            //        );
            //        lastPlatform = newPlatform;
            //    }
            //    else
            //    {
            //        GameObject newPlatform = Instantiate(platform, new Vector2(0, -3), Quaternion.identity);
            //        lastPlatform = newPlatform;
            //    }

            //}
        }
    }
    public void updateMap(GameObject player)
    {
        //Debug.Log(Mathf.Abs((lastPlatform.transform.position.x + lastPlatform.transform.localScale.x/2) - spawnPoint.x));
        if (Mathf.Abs((lastPlatform.transform.position.x + lastPlatform.transform.localScale.x/2) - spawnPoint.x) > Random.Range(3,6))
        {
            if (Pool.transform.childCount < 3)
            {
                lastPlatform = Instantiate(Platforms[Random.Range(0, Platforms.Count)], new Vector2(spawnPoint.x, lastPlatform.transform.position.y + Random.Range(-3, 3)), Quaternion.identity);
            }
            else
            {
                //lastPlatform = Instantiate(Platforms[Random.Range(0, Platforms.Count)], new Vector2(spawnPoint.x, lastPlatform.transform.position.y + Random.Range(-3, 3)), Quaternion.identity);
                Vector2 newTr = new Vector2(spawnPoint.x, lastPlatform.transform.position.y + Random.Range(-3, 3));
                lastPlatform = Pool.transform.GetChild(Random.Range(0, Pool.transform.childCount)).gameObject;
                lastPlatform.transform.position = newTr;
                lastPlatform.transform.parent = transform.parent;
            }
            if (!lastPlatform.GetComponent<PlatformElement>()) lastPlatform.AddComponent<PlatformElement>();
            lastPlatform.GetComponent<PlatformElement>().player = player;
            lastPlatform.GetComponent<PlatformElement>().Pool = Pool;
            lastPlatform.GetComponent<PlatformElement>().controller = GetComponent<GameController>();
        }
    }
    public void clearMap()
    {
        foreach (GameObject platform in GameObject.FindGameObjectsWithTag("Ground"))
        {
            platform.transform.parent = Pool.transform;
        }
        //lastPlatform = null;
        lastPlatform.transform.position = new Vector2(0, -3);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(spawnPoint, new Vector2(1, 1));
    }
}
