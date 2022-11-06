using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectButton : UseableButton
{
    public Image icon;

    public Color activeColor;
    public Color inactiveColor;

    private List<Collectible> inRange = new();

    private void Start()
    {
    }

    public override void Use()
    {
        DoCollect();
    }

    private void Update()
    {
        icon.color = CanCollect() ? activeColor : inactiveColor;
    }

    public void DoCollect()
    {
        if (!CanCollect())
        {
            return;
        }

        foreach (var collectible in inRange)
        {
            collectible.Collect();
        }
    }

    private bool CanCollect()
    {
        return inRange.Count > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent == null)
        {
            return;
        }
        
        var collectible = other.transform.parent.GetComponent<Collectible>();
        if (collectible == null)
        {
            return;
        }

        inRange.Add(collectible);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent == null)
        {
            return;
        }

        var collectible = other.transform.parent.GetComponent<Collectible>();
        if (collectible == null)
        {
            return;
        }

        inRange.Remove(collectible);
    }
}