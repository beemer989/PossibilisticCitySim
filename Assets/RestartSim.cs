using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartSim : MonoBehaviour
{
    public GameObject heatMap;
    public StudentSelector studSelect;
    float timePassed = 0;
    bool heatMapOn = true;
    void Update()
    {
      timePassed += Time.deltaTime;

      if (Input.GetKeyDown(KeyCode.R))
      {
        SceneManager.LoadScene("SampleScene");
      }
      if (Input.GetKeyDown(KeyCode.H))
      {
        if (heatMapOn == true)
        {
          Renderer[] a = heatMap.GetComponentsInChildren<Renderer>();
          foreach (Component b in a)
          {
            Renderer c = (Renderer)b;
            c.enabled = false;
          }
          heatMapOn = false;
        }
        else
        {
          Renderer[] a = heatMap.GetComponentsInChildren<Renderer>();
          foreach (Component b in a)
          {
            Renderer c = (Renderer)b;
            c.enabled = true;
          }
          heatMapOn = true;
        }
      }
      
      if (timePassed > 0.25f)
      {
        
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow))
        {
          studSelect.studentSelector.value -= 1;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow))
        {
          studSelect.studentSelector.value += 1;
        }
        timePassed = 0;
      }
        
    }
}
