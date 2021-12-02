using FluidHTN;
using FluidHTN.PrimitiveTasks;
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
  static WorldManager statWM;

  public int hunger = 100;
  public int hungerRate = 2;
  public int money = 0;
  public int activityFrequency = 0;
  public int activityCount = 0;
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

  
  public float activityPref = 0;
  public float localHomePref = 0;
  public float bowlingMod = 0;
  public float barMod = 0;
  public float gameMod = 0;
  public float homeMod = 0;

  
  public float dispBarPref;
  public float dispBowlingPref = 0;
  public float dispGamePref = 0;
  public float dispHomePref = 0;

  protected static float utilBarPref;
  protected static float utilBowlingPref = 0;
  protected static float utilGamePref = 0;
  protected static float utilHomePref = 0;

  public float[] highPrefVals = new float[] { 0.65f, 0.7f, 0.75f };
  public float[] medPrefVals = new float[] { 0.4f, 0.5f, 0.6f };
  public float[] lowPrefVals = new float[] { 0.1f, 0.2f, 0.3f };
  public float[] highHomePrefVals = new float[] { 0.65f, 0.7f, 0.75f };
  public float[] medHomePrefVals = new float[] { 0.3f, 0.4f, 0.5f };
  public float[] lowHomePrefVals = new float[] { 0.1f, 0.15f, 0.2f };
  public float[] actModVals = new float[] { 0.2f, 0.25f, 0.3f, 0.35f, 0.4f };
  public float[] homeModVals = new float[] { 0.2f, 0.25f, 0.3f, 0.35f, 0.4f };
  public bool readyForActivity = true;
  bool changesMadeToday = false;


  ////////////////////////////////
  // Possibilistic Domain
  ////////////////////////////////
  Domain<AIContext> domain = new DomainBuilder<AIContext>("StudentDomain")
    .Select("Go eat")
        .Condition("Is currently hungry", (ctx => ctx.HasState(AIWorldState.isHungry, true) && !ctx.HasState(AIWorldState.isAsleep, true) && !ctx.HasState(AIWorldState.occupied, true) && !ctx.HasState(AIWorldState.atSchool, true)))
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
        .Condition("Time to go to school", (ctx => ctx.HasState(AIWorldState.atSchool, true)))
        .Action("Go to school")
            .Do(Actions.GoingToSchool)
        .End()
    .End()
    .UtilitySelect<DomainBuilder<AIContext>, AIContext>("Occupy Time")
    .Condition("Needs to Occupy Free Time", (ctx => !ctx.HasState(AIWorldState.isAsleep, true) && ctx.HasState(AIWorldState.offWork, true) && !ctx.HasState(AIWorldState.occupied, true) && !ctx.HasState(AIWorldState.atSchool, true) && ctx.ReadyForActivity(ctx.readyForActivity) && !ctx.TimeInWindow(0, 12)))
        .UtilityAction<DomainBuilder<AIContext>, AIContext, UtilityActionBowlUtility>("bowling Utility")
            .Condition("bowling is open and affordable", ctx => ctx.HasEnoughMoney(20) && !ctx.IsTodayInDays(new int[] { 3 }))  // bowling costs 20 and is closed on wednesdays due to league nights
            .Do(Actions.GoingBowling)
            .Effect("spending money", EffectType.PlanAndExecute, (ctx, type) => ctx.SetState(AIWorldState.wantToSpendMoney, true, type))
        .End()
        .UtilityAction<DomainBuilder<AIContext>, AIContext, UtilityActionBarUtility>("bar utility")
            .Condition("the bar is open and affordable", ctx => ctx.HasEnoughMoney(15))  // sports bar costs 15 and is open every night
            .Do(Actions.GoingtoSportsBar)
            .Effect("spending money", EffectType.PlanAndExecute, (ctx, type) => ctx.SetState(AIWorldState.wantToSpendMoney, true, type))
        .End()
        .UtilityAction<DomainBuilder<AIContext>, AIContext, UtilityActionGameUtility>("game utility")
            .Condition("a sports game is open and affordable", ctx => ctx.HasEnoughMoney(60) && ctx.IsTodayInDays(new int[] { 2, 4, 6 }))  // sports games cost 60 but they only occur on Tuesdays, Thursdays, and Saturdays 
            .Do(Actions.GoToStadium)
            .Effect("spending money", EffectType.PlanAndExecute, (ctx, type) => ctx.SetState(AIWorldState.wantToSpendMoney, true, type))
        .End()
        .UtilityAction<DomainBuilder<AIContext>, AIContext, UtilityActionHomeUtility>("home Utility")
            .Do(Actions.GoingHome)
        .End()
    .End()
    .Select("Go back home")
        .Condition("has free time but too early to go out", (ctx => ctx.HasState(AIWorldState.offWork, true) && !ctx.HasState(AIWorldState.occupied, true) && !ctx.HasState(AIWorldState.atSchool, true)))
        .Action("Go home")
            .Do(Actions.GoingHome)
        .End()
    .End()
    .Build();


  ///////////////////////////////////////////////////////////////////////////////////////////////////////////
  // Basic Domain - kept here to comment out and replace the brains of the students easily
  ///////////////////////////////////////////////////////////////////////////////////////////////////////////
  /*Domain<AIContext> domain = new DomainBuilder<AIContext>("StudentDomain")
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
        .Condition("GOAL: ", (ctx => ctx.HasState(AIWorldState.offWork, true) && ctx.HasEnoughMoney(20) && !ctx.HasState(AIWorldState.occupied, true) && !ctx.HasState(AIWorldState.isAsleep) && ctx.TimeInWindow(17, 21) && !ctx.HasState(AIWorldState.atSchool)))
        .Action("Go to SportsBar")
            .Do(Actions.GoToCheapActivity)
            .Effect("spending money", EffectType.PlanAndExecute, (ctx, type) => ctx.SetState(AIWorldState.wantToSpendMoney, true, type))
        .End()
    .End()
    .Select("Go back home")
        .Condition("has free time, but no option but to go home", (ctx => ctx.HasState(AIWorldState.offWork, true) && !ctx.HasState(AIWorldState.occupied, true) && !ctx.HasState(AIWorldState.atSchool)))
        .Action("Go home")
            .Do(Actions.GoingHome)
        .End()
    .End()
    .Build();*/


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
    activityFrequency = rnd.Next(0, 3);
    homeMod = homeModVals[rnd.Next(0, homeModVals.Length)];
    barMod = actModVals[rnd.Next(0, actModVals.Length)];
    bowlingMod = actModVals[rnd.Next(0, actModVals.Length)];
    gameMod = actModVals[rnd.Next(0, actModVals.Length)];

    workStart = MWFworkTimeFrame.startTime;
    workEnd = MWFworkTimeFrame.endTime;
    schoolStart = MWFschoolTimeFrame.startTime;
    schoolEnd = MWFschoolTimeFrame.endTime;
    HandlePreferences(TTRschoolTimeFrame, TTRworkTimeFrame);
    sleepTimeFrame.startTime = sleepStart;
    sleepTimeFrame.endTime = sleepEnd;
  }


  void Update()
  {
    timePassed += Time.deltaTime;
    ctx.money = money;
    ctx.readyForActivity = readyForActivity;
    ctx.timeOfDay = worldManager.time;
    ctx.dayOfWeek = worldManager.day;
    statWM = worldManager;

    utilHomePref = dispHomePref;
    utilBarPref = dispBarPref;
    utilBowlingPref = dispBowlingPref;
    utilGamePref = dispGamePref;


    if (timePassed > 3f)
    {
      ////////////
      // SLEEP
      ////////////
      HandleSleep();

      ////////////
      // HUNGER
      ////////////
      HandleHunger();

      ////////////
      // WORK
      ////////////
      HandleWork();

      ////////////
      // SCHOOL
      ////////////
      HandleSchool();

      Agent.Think(domain);
      timePassed = 0f;
    }

    
    if(worldManager.time == 1 && changesMadeToday == false)
    {
      HandleScheduleChange();
      HandleFoodSelection();
      if (readyForActivity == false && activityCount == activityFrequency)
      {
        readyForActivity = true;
        activityCount = 0;
      }
      else if(readyForActivity == false && activityCount < activityFrequency)
      {
        activityCount += 1;
      }
      changesMadeToday = true;
    }
    if(worldManager.time == 2)
      changesMadeToday = false;
  }

  private void HandleSleep()
  {
    if (ctx.TimeInWindow(sleepTimeFrame.startTime, sleepTimeFrame.endTime) && ctx.HasState(AIWorldState.currentlyAtHome))
    {
      ctx.SetState(AIWorldState.isAsleep, true, EffectType.Permanent);
      sleepLabel.gameObject.SetActive(true);
    }
    else
    {
      ctx.SetState(AIWorldState.isAsleep, false, EffectType.Permanent);
      sleepLabel.gameObject.SetActive(false);
    }
  }

  private void HandleHunger()
  {
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
  }

  private void HandleWork()
  {
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
  }

  private void HandleSchool()
  {
    if (ctx.TimeInWindow(schoolStart, schoolEnd))
    {
      ctx.SetState(AIWorldState.atSchool, true, EffectType.Permanent);
      Actions.GoingToSchool(ctx);
    }
    else if (ctx.TimeInWindow(schoolEnd, schoolStart))
    {
      ctx.SetState(AIWorldState.atSchool, false, EffectType.Permanent);
    }
  }

  private void HandleScheduleChange()
  {
    switch (worldManager.day)
    {
      case 1:
      case 3:
      case 5:
        workStart = MWFworkTimeFrame.startTime;
        workEnd = MWFworkTimeFrame.endTime;
        schoolStart = MWFschoolTimeFrame.startTime;
        schoolEnd = MWFschoolTimeFrame.endTime;
        HandlePreferences(TTRschoolTimeFrame, TTRworkTimeFrame);
        break;
      case 2:
      case 4:
        workStart = TTRworkTimeFrame.startTime;
        workEnd = TTRworkTimeFrame.endTime;
        schoolStart = TTRschoolTimeFrame.startTime;
        schoolEnd = TTRschoolTimeFrame.endTime;
        HandlePreferences(MWFschoolTimeFrame, MWFworkTimeFrame);
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
  }

  public void HandleFoodSelection()
  {
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

  public void HandlePreferences(timeFrame nextDaySchool, timeFrame nextDayWork)
  {
    var rnd = new System.Random(DateTime.Now.Millisecond);
    if(worldManager.day != 5 || worldManager.day != 6)
    {
      if (workEnd > 19 || schoolEnd > 19 || nextDaySchool.startTime < 9 || nextDayWork.startTime < 9)
      {
        activityPref = lowPrefVals[rnd.Next(0, 3)];
        localHomePref = highHomePrefVals[rnd.Next(0, 3)];
      }
      else if (workEnd > 15 || schoolEnd > 15 || nextDaySchool.startTime < 10 || nextDayWork.startTime < 10)
      {
        activityPref = medPrefVals[rnd.Next(0, 3)];
        localHomePref = medHomePrefVals[rnd.Next(0, 3)];
      }
      else if (workEnd > 12 || schoolEnd > 12 || nextDaySchool.startTime < 11 || nextDayWork.startTime < 11)
      {
        activityPref = highPrefVals[rnd.Next(0, 3)];
        localHomePref = lowHomePrefVals[rnd.Next(0, 3)];
      }
    }
    else
    {
      activityPref = highPrefVals[rnd.Next(0, 3)];
      localHomePref = lowHomePrefVals[rnd.Next(0, 3)];
    }


    dispHomePref = homeMod + localHomePref;
    dispBarPref = barMod + activityPref;
    dispBowlingPref = bowlingMod + activityPref;
    dispGamePref = gameMod + activityPref;
  }

  class UtilityActionHomeUtility : PrimitiveTask, IUtilityTask  // Utility function for home option
  {
    public float Score(IContext ctx)
    {
      return utilHomePref;    // simply return the home preference value that's been calculated previously in HandlePreferences
    }
  }

  class UtilityActionBarUtility : PrimitiveTask, IUtilityTask // Utility function for Bar option
  {
    public float Score(IContext ctx)
    {
      var possofEnjoyingBar = Mathf.Max((1 - statWM.barTraffic), utilBarPref);  // compare the calculated Bar preference against the current bar traffic
      return possofEnjoyingBar;
    }
  }

  class UtilityActionBowlUtility : PrimitiveTask, IUtilityTask  // Utility function for Bowling Alley option
  {
    public float Score(IContext ctx)
    {
      var possofEnjoyingBowling = Mathf.Max((1 - statWM.bowlingTraffic), utilBowlingPref); // compare the calculated Bowling Alley preference against the current Bowling Alley traffic
      return possofEnjoyingBowling;
    }
  }

  class UtilityActionGameUtility : PrimitiveTask, IUtilityTask  // Utility function for Sports Game option
  {
    public float Score(IContext ctx)
    {
      var possofEnjoyingGame = Mathf.Max((1 - statWM.gameTraffic), utilGamePref); // compare the calculated Sports Game preference against the current Sports Game traffic
      return possofEnjoyingGame;
    }
  }
}



