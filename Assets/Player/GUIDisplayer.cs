using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIDisplayer : MonoBehaviour
{

    public Text unitAmount;
    public Text FPSText;
    public Text woodAmount;
    public Text ironOreAmount;
    int unitCount = 0;
    int unitLimitCount = 10;
    int woodCount = 0;
    int ironOreCount = 0;

    void Start()
    {
        updateUnitAmountDisplay();
    }

    private void Update()
    {
        fpsUpdate();
    }

    /// <summary>
    /// updates the wood count
    /// </summary>
    /// <param name="amount">wood amount</param>
    public void updateWoodCount(int amount)
    {
        woodCount = amount;
        updateWoodCountDisplay();
    }

    /// <summary>
    /// replaces the visual wood count with the internal one
    /// </summary>
    public void updateWoodCountDisplay()
    {
        woodAmount.text = "Wood: " + woodCount;
    }

    /// <summary>
    /// updates the iron ore count
    /// </summary>
    /// <param name="amount">iron ore amount</param>
    public void updateIronOreCount(int amount)
    {
        ironOreCount = amount;
        updateIronOreCountDisplay();
    }

    /// <summary>
    /// replaces the visual iron ore count with the internal one
    /// </summary>
    public void updateIronOreCountDisplay()
    {
        ironOreAmount.text = "IronOre: " + ironOreCount;
    }

    /// <summary>
    /// set the unit count
    /// </summary>
    /// <param name="amount">unit count</param>
    public void updateUnitCount(int amount)
    {
        unitCount = amount;
        updateUnitAmountDisplay();
    }

    /// <summary>
    /// set the unit limit
    /// </summary>
    /// <param name="amount">limit</param>
    public void updateUnitLimit(int amount)
    {
        unitLimitCount = amount;
        updateUnitAmountDisplay();
    }

    /// <summary>
    /// updates the unit amount visuals with the internal ones
    /// </summary>
    void updateUnitAmountDisplay()
    {
        unitAmount.text = unitCount + " / " + unitLimitCount;
    }

    /// <summary>
    /// updates the fps indicator
    /// </summary>
    void fpsUpdate()
    {
        FPSText.text = "" + Mathf.Round(1.0f / Time.smoothDeltaTime);
    }
}
