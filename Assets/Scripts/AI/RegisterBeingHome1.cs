using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluidHTN;
using System;

public class RegisterBeingHome1 : MonoBehaviour
{
  public Student currStud;
  private void OnTriggerStay(Collider other)
  {
    if (other.name == this.GetComponent<NPCAgent>().homePosition.parent.name + "Entrance")
    {
      Debug.Log("STUDENT AT HOME");
      currStud = this.gameObject.GetComponent<Student>();
      currStud.ctx.SetState(AIWorldState.currentlyAtHome, true, EffectType.Permanent);
    }
  }

  private void OnTriggerExit(Collider other)
  {
    currStud.ctx.SetState(AIWorldState.currentlyAtHome, false, EffectType.Permanent);
  }
}
