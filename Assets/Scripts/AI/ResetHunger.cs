using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluidHTN;
using System;

public class ResetHunger : MonoBehaviour
{

  private Student student;
  private NPCAgent agent;
  private void OnTriggerEnter(Collider other)
  {
    var tempStudent = other.GetComponent<Student>();
    if (tempStudent != null)
    {
      student = other.gameObject.GetComponent<Student>();
      agent = other.gameObject.GetComponent<NPCAgent>();
      if (student.ctx.GetState(AIWorldState.isHungry) == 1 && agent.foodDecision == 1)
      {
        student.ctx.SetState(AIWorldState.isHungry, false, EffectType.Permanent);

        var rnd = new System.Random(DateTime.Now.Millisecond);
        student.hunger += rnd.Next(50, 70);
      }
    }
  }
}
