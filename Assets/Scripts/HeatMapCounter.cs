using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HeatMapCounter : MonoBehaviour
{
  public int interactionCount = 0;

  public int coldInt = 40;
  public int coolInt;
  public int medInt;
  public int warmInt;
  public int hotInt;

  public Material coldCol;
  public Material coolCol;
  public Material medCol;
  public Material warmCol;
  public Material hotCol;

  private Renderer rend;


  private void OnTriggerEnter(Collider other)
  {
    if(other.tag == "Student")
    {
      var tempStud = other.GetComponent<Student>();
      if (!tempStud.ctx.TimeInWindow(tempStud.sleepStart, tempStud.sleepEnd))
      {
        interactionCount += 1;
        var scaleChange = new Vector3(0f, 0f, 100f);
        this.gameObject.transform.localScale += scaleChange;
      }
    }
  }

  private void Start()
  {
    rend = this.gameObject.GetComponent<Renderer>();
  }

  private void Update()
  {
    if (interactionCount < coldInt)
    {
      rend.material = coldCol;
    }
    else if (interactionCount < coolInt)
    {
      rend.material = coolCol;
    }
    else if (interactionCount < medInt)
    {
      rend.material = medCol;
    }
    else if (interactionCount < warmInt)
    {
      rend.material = warmCol;
    }
    else if (interactionCount < hotInt)
    {
      rend.material = hotCol;
    }
  }
}
