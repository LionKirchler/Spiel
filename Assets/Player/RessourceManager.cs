using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceManager : MonoBehaviour
{
    private int woodAmount = 2000;
    private int ironOreAmount = 1000;
    private Player player;
    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        player.getGuiDisplayer().updateIronOreCount(ironOreAmount);
        player.getGuiDisplayer().updateWoodCount(woodAmount);
    }

    /// <summary>
    /// adds wood
    /// </summary>
    /// <param name="w">amount</param>
    public void addWood(int w)
    {
        woodAmount += w;
        player.getGuiDisplayer().updateWoodCount(woodAmount);
    }

    /// <summary>
    /// removes wood
    /// </summary>
    /// <param name="w">amount</param>
    public void removeWood(int w)
    {
        woodAmount -= w;
        player.getGuiDisplayer().updateWoodCount(woodAmount);
    }

    /// <summary>
    /// removes iron ore
    /// </summary>
    /// <param name="w">amount</param>
    public void removeIronOre(int w)
    {
        ironOreAmount -= w;
        player.getGuiDisplayer().updateIronOreCount(ironOreAmount);
    }

    /// <summary>
    /// removes ressources
    /// </summary>
    /// <param name="res">ressource array</param>
    public void removeRessources(int[] res)
    {
        woodAmount -= res[0];
        ironOreAmount -= res[1];
        player.getGuiDisplayer().updateWoodCount(woodAmount);
        player.getGuiDisplayer().updateIronOreCount(ironOreAmount);
    }

    /// <summary>
    /// adds iron ore
    /// </summary>
    /// <param name="w">amount</param>
    public void addIronOre(int w)
    {
        ironOreAmount += w;
        player.getGuiDisplayer().updateIronOreCount(ironOreAmount);
    }

    /// <summary>
    /// returns the wood amount
    /// </summary>
    /// <returns></returns>
    public int getWood()
    {
        return woodAmount;
    }

    /// <summary>
    /// returns the iron ore amount
    /// </summary>
    /// <returns></returns>
    public int getIronOre()
    {
        return ironOreAmount;
    }

    /// <summary>
    /// returns if the player has enogh ressources to for example build something
    /// </summary>
    /// <param name="res"></param>
    /// <returns></returns>
    public bool hasRessources(int[] res) 
    {
        return woodAmount >= res[0] && ironOreAmount >= res[1];
    }

}
