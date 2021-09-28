using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabelController : MonoBehaviour
{
    GameObject[] mapLabels;
    bool onFlag = true;
    // Start is called before the first frame update
    void Start()
    {
      mapLabels = GameObject.FindGameObjectsWithTag("Map Label");
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.L) && onFlag == true)
      {
        SetAllLabels(false);
        onFlag = false;
      }
      else if (Input.GetKeyDown(KeyCode.L) && onFlag == false)
      {
        SetAllLabels(true);
        onFlag = true;
      }
    }

    void SetAllLabels(bool state)
    {
      foreach (var label in mapLabels)
      {
        label.SetActive(state);
      }
    }
}
