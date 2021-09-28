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
      Debug.Log("HELLO: TIME: " + world.time);
      Debug.Log("HELLO: DAY: " + world.day);
      currText.text = "Day:" + world.day + "\tTime: " + world.time;
    }
}
