using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureCollectionManager : MonoBehaviour
{

    public GameObject treasureOneOnDeck;
    public GameObject treasureTwoOnDeck;
    public GameObject treasureThreeOnDeck;
    public GameObject treasureFourOnDeck;

    private int numTreasuresPlaced;
    private GameObject[] treasuresOnDeck;

    // Start is called before the first frame update
    void Start()
    {
        numTreasuresPlaced = 0;
        treasuresOnDeck = new[] {
            treasureOneOnDeck,
            treasureTwoOnDeck,
            treasureThreeOnDeck,
            treasureFourOnDeck
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (NetworkSyncer.Get())
        {
            int numTreasuresCollected = NetworkSyncer.Get().numTreasuresCollected.Value;
            Debug.Log($"numTreasuresCollected: {numTreasuresCollected}");

            //WIN GAME
            if (numTreasuresCollected > 4)
            {
                NetworkSyncer.Get().SetGameToWonServerRpc();
                return;
            }

            if (numTreasuresCollected > numTreasuresPlaced)
            {
                PlaceTreasure(numTreasuresCollected);
            }

        }
    }

    private void PlaceTreasure(int treasureNumber)
    {
        var treasureOnDeck = treasuresOnDeck[treasureNumber - 1];
        treasureOnDeck.SetActive(true);
        numTreasuresPlaced++;

    }
}
