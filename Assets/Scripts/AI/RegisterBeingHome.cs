using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluidHTN;
using System;

public class RegisterBeingHome : MonoBehaviour
{

  private Student agent;
  private void OnTriggerStay(Collider other)
  {
    agent = other.gameObject.GetComponent<Student>();
    Debug.Log("CURRENTLY AT HOME");
    Debug.Log(agent.ctx.GetState(AIWorldState.currentlyAtHome));
    agent.ctx.SetState(AIWorldState.currentlyAtHome, true, EffectType.Permanent);
  }

  private void OnTriggerExit(Collider other)
  {
    agent = other.gameObject.GetComponent<Student>();
    Debug.Log(agent.ctx.GetState(AIWorldState.currentlyAtHome));
    agent.ctx.SetState(AIWorldState.currentlyAtHome, false, EffectType.Permanent);
  }
}
