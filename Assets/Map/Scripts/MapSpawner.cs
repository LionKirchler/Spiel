using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject tree1 = null;
    [SerializeField]
    private GameObject grass1 = null;
    [SerializeField]
    private GameObject mainBuilding = null;
    [SerializeField]
    private GameObject ironOre1 = null;
    private GameObject[] grasses;
    private GameObject[] trees;
    private GameObject[] ironOres;
    private Player player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    /// <summary>
    /// sets the grass array for random grass spawning
    /// </summary>
    public void setGrassArray()
    {
        grasses = new GameObject[] { grass1 };
    }
    /// <summary>
    /// sets the tree array for random tree spawning
    /// </summary>
    public void setTreeArray()
    {
        trees = new GameObject[] { tree1 };
    }

    /// <summary>
    /// sets the iron ore array for random iron ore spawning
    /// </summary>
    public void setIronOreArray()
    {
        ironOres = new GameObject[] { ironOre1 };
    }

    /// <summary>
    /// spawns a random grass at pos
    /// </summary>
    /// <param name="pos">pos to spawn at</param>
    /// <returns>spawned grass</returns>
    public GameObject spawnRandomGrass(Vector2 pos)
    {
        GameObject grass = grasses[Random.Range(0, grasses.Length)];
        GameObject go = Instantiate(grass, pos, tree1.transform.rotation);
        go.transform.parent = transform;
        return go;
    }
    /// <summary>
    /// spawnes a random tree at pos
    /// </summary>
    /// <param name="pos">pos</param>
    /// <returns>spawned tree</returns>
    public GameObject spawnRandomTree(Vector2 pos)
    { 
        GameObject tree = trees[Random.Range(0, trees.Length)];
        GameObject go = Instantiate(tree, new Vector2(pos.x, pos.y + tree.GetComponent<Tree>().getSpawnOffset()), tree.transform.rotation);
        go.transform.parent = transform;
        go.GetComponent<Tree>().setPlayer(player);
        return go;
    }
    /// <summary>
    /// spawns a random iron ore at pos
    /// </summary>
    /// <param name="pos">pos</param>
    /// <returns>spawned iron ore</returns>
    public GameObject spawnRandomIronOre(Vector2 pos)
    {
        GameObject ore = ironOres[Random.Range(0, ironOres.Length)];
        GameObject go = Instantiate(ore, new Vector2(pos.x, pos.y + ore.GetComponent<IronOre>().getSpawnOffset()), ore.transform.rotation);
        go.transform.parent = transform;
        go.GetComponent<IronOre>().setPlayer(player);
        return go;
    }
    /// <summary>
    /// spawns a main building at pos
    /// </summary>
    /// <param name="pos">pos</param>
    /// <returns>spawned main building</returns>
    public GameObject spawnMainBuilding(Vector2 pos)
    {
        GameObject tree = trees[Random.Range(0, trees.Length)];
        GameObject go = Instantiate(tree, new Vector2(pos.x, pos.y + tree.GetComponent<Tree>().getSpawnOffset()), tree.transform.rotation);
        return go;
    }
}
