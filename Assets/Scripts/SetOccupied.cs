using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluidHTN;
using System;

public class SetOccupied : MonoBehaviour
{

  private Student agent;
  float timePassed = 0f;
  bool enterFlag = false;
  public float eventDuration;
  public string empType;
  public int cost;
  private void OnTriggerEnter(Collider other)
  {
    agent = other.gameObject.GetComponent<Student>();
   /* if(agent.jobPosition != empType)
    {*/
    /*Debug.Log("RESETTING");*/
    Debug.Log(agent.ctx.GetState(AIWorldState.occupied));
    agent.ctx.SetState(AIWorldState.occupied, true, EffectType.Permanent);
    enterFlag = true;
   /* } */
  }

  private void OnTriggerExit(Collider other)
  {
    agent = other.gameObject.GetComponent<Student>();
    Debug.Log(agent.ctx.GetState(AIWorldState.occupied));
    agent.ctx.SetState(AIWorldState.occupied, false, EffectType.Permanent);
    enterFlag = false;
  }


  private void Update()
  {
    
    if(enterFlag == true)
    {
      Debug.Log("Update From ENTRANCE!");
      timePassed += Time.deltaTime;

      if (timePassed > eventDuration)
      {
        agent.ctx.SetState(AIWorldState.occupied, false, EffectType.Permanent);
        agent.money -= cost;
        timePassed = 0;
        enterFlag = false;
      }
    }
    
  }

 }
