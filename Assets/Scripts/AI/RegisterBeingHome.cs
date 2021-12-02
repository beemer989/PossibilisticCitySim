using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluidHTN;
using System;

public class RegisterBeingHome : MonoBehaviour
{

  private Student student;
  private NPCAgent agent;

  bool hasEntered = false;
  private void OnTriggerEnter(Collider other)
  {
    var tempStudent = other.GetComponent<Student>();
    if (tempStudent != null && hasEntered == false)
    {
      student = other.gameObject.GetComponent<Student>();
      agent = student.Agent;
      var agentHomeEntrance = agent.homePosition.parent.name + "Entrance";
      if (agentHomeEntrance == this.name)
      {
        student.ctx.SetState(AIWorldState.currentlyAtHome, true, EffectType.Permanent);
        hasEntered = true;
      }

    }
  }

  private void OnTriggerExit(Collider other)
  {
    var tempStudent = other.GetComponent<Student>();
    if (tempStudent != null && hasEntered == true)
    {
      student = other.gameObject.GetComponent<Student>();
      Debug.Log(student.ctx.GetState(AIWorldState.currentlyAtHome));
      student.ctx.SetState(AIWorldState.currentlyAtHome, false, EffectType.Permanent);
      hasEntered = false;
    }
  }
}
