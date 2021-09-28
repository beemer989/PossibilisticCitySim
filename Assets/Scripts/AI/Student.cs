using FluidHTN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Student : MonoBehaviour
{
  public float period = 0.0f;
  public NPCAgent Agent;
  public AIContext ctx;
  Planner<AIContext> planner;

  float timePassed = 0f;

  public WorldManager worldManager;

  public int hunger = 100;
  public int hungerRate = 2;
  public int money = 0;
  public string jobPosition;
  public bool paidToday = false;

  Transform hungerLabel;
  Transform sleepLabel;

  public timeFrame sleepTimeFrame = new timeFrame();
  public timeFrame MWFworkTimeFrame = new timeFrame();
  public timeFrame MWFschoolTimeFrame = new timeFrame();
  public timeFrame MWFsleepTimeFrame = new timeFrame();
  public timeFrame TTRworkTimeFrame = new timeFrame();
  public timeFrame TTRschoolTimeFrame = new timeFrame();
  public timeFrame TTRsleepTimeFrame = new timeFrame();
  public timeFrame WKNDworkTimeFrame = new timeFrame();
  public timeFrame WKNDschoolTimeFrame = new timeFrame();
  public timeFrame WKNDsleepTimeFrame = new timeFrame();

  public int workStart = 14;
  public int workEnd = 19;
  public int schoolStart = 7;
  public int schoolEnd = 11;
  public int sleepStart = 22;
  public int sleepEnd = 5;

  Domain<AIContext> domain = new DomainBuilder<AIContext>("StudentDomain")
    .Select("Go eat")
        .Condition("Is currently hungry", (ctx => ctx.HasState(AIWorldState.isHungry, true) && !ctx.HasState(AIWorldState.isAsleep) && !ctx.HasState(AIWorldState.occupied) && !ctx.HasState(AIWorldState.atSchool)))
        .Action("Go eat")
            .Do(Actions.GoEat)
        .End()
    .End()
    .Select("Go back to work")
        .Condition("Time to go to work", (ctx => ctx.HasState(AIWorldState.atWork, true)))
        .Action("Go to work")
            .Do(Actions.GoingToWork)
        .End()
    .End()
    .Select("Go to school")
        .Condition("Time to go to work", (ctx => ctx.HasState(AIWorldState.atSchool, true)))
        .Action("Go to school")
            .Do(Actions.GoingToSchool)
        .End()
    .End()
    .Select("Go See a Game")
        .Condition("GOAL: ", (ctx => ctx.HasState(AIWorldState.offWork, true) && ctx.HasEnoughMoney(60) && !ctx.HasState(AIWorldState.occupied, true) && !ctx.HasState(AIWorldState.isAsleep) && ctx.TimeInWindow(17, 21) && !ctx.HasState(AIWorldState.atSchool) && ctx.IsTodayInDays(new int[] { 2, 4, 6 })))
        .Action("Go See a Game")
            .Do(Actions.GoToStadium)
            .Effect("spending money", EffectType.PlanAndExecute, (ctx, type) => ctx.SetState(AIWorldState.wantToSpendMoney, true, type))
        .End()
    .End()
    .Select("Go to Cheap Activity")
        .Condition("GOAL: ", (ctx => ctx.HasState(AIWorldState.offWork, true) && ctx.HasEnoughMoney(30) && !ctx.HasState(AIWorldState.occupied, true) && !ctx.HasState(AIWorldState.isAsleep) && ctx.TimeInWindow(17, 21) && !ctx.HasState(AIWorldState.atSchool)))
        .Action("Go to SportsBar")
            .Do(Actions.GoingtoSportsBar)
            .Effect("spending money", EffectType.PlanAndExecute, (ctx, type) => ctx.SetState(AIWorldState.wantToSpendMoney, true, type))
        .End()
    .End()
    .Select("Go back home")
        .Condition("has free time, but no option but to go home", (ctx => ctx.HasState(AIWorldState.offWork, true) && !ctx.HasState(AIWorldState.occupied, true) && !ctx.HasState(AIWorldState.atSchool)))
        .Action("Go home")
            .Do(Actions.GoingHome)
        .End()
    .End()
    .Build();


  void Start()
  {
    hungerLabel = this.transform.GetChild(0).GetChild(0);
    sleepLabel = this.transform.GetChild(1).GetChild(0);

    Agent = GetComponent<NPCAgent>();
    planner = Agent.Planner;
    ctx = Agent.Context;
    ctx.Init();
    ctx.timeOfDay = worldManager.time;
    ctx.dayOfWeek = worldManager.day;
    ctx.SetState(AIWorldState.isHungry, false, EffectType.Permanent);
    ctx.SetState(AIWorldState.headingHome, true, EffectType.Permanent);
    ctx.SetState(AIWorldState.atWork, false, EffectType.Permanent);
    ctx.SetState(AIWorldState.offWork, true, EffectType.Permanent);

    var rnd = new System.Random(DateTime.Now.Millisecond);
    hunger = rnd.Next(75, 100);

    workStart = MWFworkTimeFrame.startTime;
    workEnd = MWFworkTimeFrame.endTime;
    schoolStart = MWFschoolTimeFrame.startTime;
    schoolEnd = MWFschoolTimeFrame.endTime;

    sleepTimeFrame.startTime = sleepStart;
    sleepTimeFrame.endTime = sleepEnd;
  }


  void Update()
  {
    timePassed += Time.deltaTime;
    ctx.money = money;
    ctx.timeOfDay = worldManager.time;
    ctx.dayOfWeek = worldManager.day;

    if (timePassed > 3f)
    {
      ///SLEEPING
      if (ctx.TimeInWindow(sleepTimeFrame.startTime, sleepTimeFrame.endTime) && ctx.HasState(AIWorldState.currentlyAtHome))
      {
        ctx.SetState(AIWorldState.isAsleep, true, EffectType.Permanent);
        /*Debug.Log("Sleeping!");*/
        sleepLabel.gameObject.SetActive(true);
      }
      else
      {
        ctx.SetState(AIWorldState.isAsleep, false, EffectType.Permanent);
        /*Debug.Log("NOT Sleeping!");*/
        sleepLabel.gameObject.SetActive(false);
      }

      ////////////
      // HUNGER
      ////////////
      hunger -= hungerRate;
      if (hunger < 20)
      {
        ctx.SetState(AIWorldState.isHungry, true, EffectType.Permanent);
        hungerLabel.gameObject.SetActive(true);
        hunger += hungerRate;
      }
      if (ctx.HasState(AIWorldState.currentlyAtHome) && ctx.HasState(AIWorldState.isHungry) && !ctx.HasState(AIWorldState.isAsleep))
      {
        ctx.SetState(AIWorldState.isHungry, false, EffectType.Permanent);
        hungerLabel.gameObject.SetActive(false);
        System.Random temprand = new System.Random(DateTime.Now.Millisecond);
        hunger += temprand.Next(50, 70);
      }
      if (!ctx.HasState(AIWorldState.isHungry))
      {
        hungerLabel.gameObject.SetActive(false);
      }

      ////////////
      // WORK
      ////////////
      if (ctx.TimeInWindow(workStart, workEnd))
      {
        ctx.SetState(AIWorldState.atWork, true, EffectType.Permanent);
        ctx.SetState(AIWorldState.offWork, false, EffectType.Permanent);
        Actions.GoingToWork(ctx);
      }
      else if (ctx.TimeInWindow(workEnd, workStart))
      {
        ctx.SetState(AIWorldState.offWork, true, EffectType.Permanent);
        ctx.SetState(AIWorldState.atWork, false, EffectType.Permanent);
      }

      ////////////
      // SCHOOL
      ////////////
      if (ctx.TimeInWindow(schoolStart, schoolEnd))
      {
        ctx.SetState(AIWorldState.atSchool, true, EffectType.Permanent);
        Actions.GoingToSchool(ctx);
      }
      else if (ctx.TimeInWindow(schoolEnd, schoolStart))
      {
        ctx.SetState(AIWorldState.atSchool, false, EffectType.Permanent);
      }

      Agent.Think(domain);
      timePassed = 0f;
    }

    switch (worldManager.day)
    {
      case 1:
      case 3:
      case 5:
        workStart = MWFworkTimeFrame.startTime;
        workEnd = MWFworkTimeFrame.endTime;
        schoolStart = MWFschoolTimeFrame.startTime;
        schoolEnd = MWFschoolTimeFrame.endTime;
        break;
      case 2:
      case 4:
        workStart = TTRworkTimeFrame.startTime;
        workEnd = TTRworkTimeFrame.endTime;
        schoolStart = TTRschoolTimeFrame.startTime;
        schoolEnd = TTRschoolTimeFrame.endTime;
        break;
      case 6:
      case 7:
        workStart = -1;
        workEnd = -1;
        schoolStart = -1;
        schoolEnd = -1;
        break;
      default:
        break;
    }

    ////////////
    ////abstract this out
    ////////////
    if (worldManager.time == 1)
    {
      var rnd = new System.Random(DateTime.Now.Millisecond);
      var tempRand = UnityEngine.Random.Range(0, 100);
      if (tempRand % 2 == 0)
      {
        Agent.cheapActivityIndex = 1;
      }
      else
      {
        Agent.cheapActivityIndex = 0;
      }

      if (tempRand % 2 == 0)
      {
        Agent.foodSelection = Agent.foodLocations[0];
      }
      else
      {
        Agent.foodSelection = Agent.foodLocations[1];
      }
    }
  }
}

