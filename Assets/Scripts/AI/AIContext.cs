using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FluidHTN;
using FluidHTN.Contexts;
using FluidHTN.Factory;
using FluidHTN.Debug;

public enum AIWorldState
{
  isHungry,
  headingHome,
  atWork,
  isAsleep,
  currentlyAtHome,
  occupied,
  offWork,
  atSchool,
  wantToSpendMoney
}

public class AIContext : BaseContext
{
  private byte[] _worldState = new byte[Enum.GetValues(typeof(AIWorldState)).Length];
  public override IFactory Factory { get; set; } = new DefaultFactory();
  public override List<string> MTRDebug { get; set; }
  public override List<string> LastMTRDebug { get; set; }
  public override bool DebugMTR { get; } = false;
  public override Queue<IBaseDecompositionLogEntry> DecompositionLog { get; set; }
  public override bool LogDecomposition { get; } = true;
  public override byte[] WorldState => _worldState;

  public int money = 0;
  public int timeOfDay;
  public int dayOfWeek;

  public bool Done { get; set; } = false;

  public NPCAgent Agent { get; set; }

  public AIContext(NPCAgent agent)
  {
    Agent = agent;
  }

  public override void Init()
  {
    base.Init();

    SetState(AIWorldState.isHungry, false, EffectType.Permanent);
    SetState(AIWorldState.headingHome, true, EffectType.Permanent);
    SetState(AIWorldState.atWork, false, EffectType.Permanent);
    SetState(AIWorldState.occupied, false, EffectType.Permanent);
  }

  public bool HasState(AIWorldState state, bool value)
  {
    return HasState((int)state, (byte)(value ? 1 : 0));
  }

  public bool HasState(AIWorldState state)
  {
    return HasState((int)state, 1);
  }
  public void SetState(AIWorldState state, bool value, EffectType type)
  {
    SetState((int)state, (byte)(value ? 1 : 0), true, type);
  }

  public byte GetState(AIWorldState state)
  {
    return GetState((int)state);
  }

  public bool HasEnoughMoney(int amount)
  {
    return (money > amount);
  }

  public bool TimeInWindow(int startTime, int endTime)
  {
    bool temp = false;
    if (endTime < startTime)
    {
      if(timeOfDay <= endTime || startTime <= timeOfDay)
      {
        temp = true;
      }
    }
    else
    {
      if(startTime <= timeOfDay && timeOfDay <= endTime)
      {
        temp = true;
      }
    }
    return temp;
  }
  
  public bool IsTodayInDays(int[] days)
  {
    foreach(int x in days)
    {
      if (x == dayOfWeek)
        return true;
    }
    return false;
  }
}
