using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleWorldClock : MonoBehaviour
{
    Text currText; 
    public WorldManager world;

    void Start()
    {
      currText = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
      currText.text = "Day:" + world.day + "\tTime: " + world.time + "\n\n";
      currText.text += "Bar Traffic:" + world.barTraffic + "\n\n";
      currText.text += "Bowl Traffic:" + world.bowlingTraffic + "\n\n";
      currText.text += "Game Traffic:" + world.gameTraffic + "\n\n";
    }
}
