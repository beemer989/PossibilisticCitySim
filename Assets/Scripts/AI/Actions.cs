using FluidHTN;
using System;
using UnityEngine;
using UnityEngine.AI;

public static class Actions
{
  public static TaskStatus GoingHome(AIContext context)
  {
    context.Agent.navMeshAgent.SetDestination(context.Agent.homePosition.position);
    /*Debug.Log("it goes home");*/
    return TaskStatus.Success;
  }

  public static TaskStatus GoingToWork(AIContext context)
  {
    context.Agent.navMeshAgent.SetDestination(context.Agent.workPosition.position);
    /*Debug.Log("it goes to work");*/
    return TaskStatus.Success;
  }

  public static TaskStatus GoingToSchool(AIContext context)
  {
    context.Agent.navMeshAgent.SetDestination(context.Agent.schoolPosition.position);
    /*Debug.Log("it goes to school");*/
    return TaskStatus.Success;
  }

  public static TaskStatus GoEat(AIContext context)
  {
    var rnd = new System.Random(DateTime.Now.Millisecond);
    var foodChoice = rnd.Next(0, 2);

    if (context.HasEnoughMoney(15))
    {
      float distToFood = 100000;
      float distToHome = 100000;

      NavMeshPath possPath = new NavMeshPath();

      if (NavMesh.CalculatePath(context.Agent.transform.position, context.Agent.foodSelection.transform.position, context.Agent.navMeshAgent.areaMask, possPath))
      {
        distToFood = Vector3.Distance(context.Agent.transform.position, possPath.corners[0]);

        for (int i = 1; i < possPath.corners.Length; i++)
        {
          distToFood += Vector3.Distance(possPath.corners[i - 1], possPath.corners[i]);
        }
      }

      if (NavMesh.CalculatePath(context.Agent.transform.position, context.Agent.homePosition.transform.position, context.Agent.navMeshAgent.areaMask, possPath))
      {
        distToHome = Vector3.Distance(context.Agent.transform.position, possPath.corners[0]);

        for (int i = 1; i < possPath.corners.Length; i++)
        {
          distToHome += Vector3.Distance(possPath.corners[i - 1], possPath.corners[i]);
        }
      }

      Debug.Log(distToFood);
      Debug.Log(distToHome);

      if (distToFood < distToHome)
      {
        context.Agent.navMeshAgent.SetDestination(context.Agent.foodSelection.transform.position);
        context.Agent.foodDecision = 1;
      }
      else if (distToHome <= distToFood)
      {
        context.Agent.navMeshAgent.SetDestination(context.Agent.homePosition.transform.position);
        context.Agent.foodDecision = 0;
      }
    }
    else
    {
      context.Agent.navMeshAgent.SetDestination(context.Agent.homePosition.transform.position);
    }

    /*Debug.Log("it goes to eat");*/
    return TaskStatus.Success;
  }


  public static TaskStatus GoingBowling(AIContext context)
  {
    context.Agent.navMeshAgent.SetDestination(context.Agent.BowlingAlley.transform.position);
    /*Debug.Log("go Bowling");*/
    return TaskStatus.Success;
  }

  public static TaskStatus GoingtoSportsBar(AIContext context)
  {
    if(context.IsTodayInDays(new int[] { 3 }))
    {
      context.Agent.navMeshAgent.SetDestination(context.Agent.SportsBar.transform.position);
      /*Debug.Log("go to sports bar");*/
    }
    else
    {
      switch (context.Agent.cheapActivityIndex)
      {
        case 0:
          context.Agent.navMeshAgent.SetDestination(context.Agent.SportsBar.transform.position);
          /*Debug.Log("go to sports bar");*/
          break;
        case 1:
          context.Agent.navMeshAgent.SetDestination(context.Agent.BowlingAlley.transform.position);
          /*Debug.Log("go to bowling alley");*/
          break;
        default:
          break;
      }
    }
    
    
    return TaskStatus.Success;
  }
  public static TaskStatus GoToStadium(AIContext context)
  {
    context.Agent.navMeshAgent.SetDestination(context.Agent.Stadium.transform.position);
    /*Debug.Log("go to stadium");*/
    return TaskStatus.Success;
  }

  public static TaskStatus Failed(AIContext context)
  {
    /*Debug.Log("Sorry, I don't know how to do that yet!");*/
    return TaskStatus.Success;
  }
}
