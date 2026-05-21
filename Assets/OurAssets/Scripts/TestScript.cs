using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HashMapADT<string, int> scoreBoard = new HashMapADT<string, int>()
        {
            { "Player0", 0 },
            { "Player1", 1 },
            { "Player2", 2 },
            { "Player3", 3 }
        };
        foreach (KeyValuePair<string, int> entry in scoreBoard)
        {
            Debug.Log($"{entry.Key}: {entry.Value}");
        }
        scoreBoard.Remove("Player1");
        foreach (KeyValuePair<string, int> entry in scoreBoard)
        {
            Debug.Log($"{entry.Key}: {entry.Value}");
        }
    }
}
