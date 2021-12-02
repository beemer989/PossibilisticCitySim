using FluidHTN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterBeingOccupied : MonoBehaviour
{
    Student student;
    public WorldManager worldManager;
    float eventDuration;
    int cost;
    bool hasEntered = false;
    float timePassed = 0f;

    int activityType = 0;

    // Start is called before the first frame update
    void Start()
    {
      student = this.GetComponent<Student>();
      GameObject worldRef = GameObject.FindGameObjectWithTag("WorldManager");
      worldManager = worldRef.GetComponent<WorldManager>();
      
    }

    // Update is called once per frame
    void Update()
    {
      if(hasEntered == true && activityType == 0)
      {
        timePassed += Time.deltaTime;

        if (timePassed > eventDuration)
        {
          student.ctx.SetState(AIWorldState.occupied, false, EffectType.Permanent);
          student.ctx.SetState(AIWorldState.wantToSpendMoney, false, EffectType.Permanent);
          student.money -= cost;
          student.readyForActivity = false;
          hasEntered = false;
          timePassed = 0;
        }
      }

      else if(hasEntered == true && activityType == 1)
      {
        if(worldManager.time > 22)
        {
          student.ctx.SetState(AIWorldState.occupied, false, EffectType.Permanent);
          student.ctx.SetState(AIWorldState.wantToSpendMoney, false, EffectType.Permanent);
          student.money -= cost;
          student.readyForActivity = false;
          hasEntered = false;
        }
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (hasEntered == false && student.ctx.GetState(AIWorldState.wantToSpendMoney) == 1)
      {
        switch (other.tag)
        {
          case "BowlingAlley":
            student.ctx.SetState(AIWorldState.occupied, true, EffectType.Permanent);
            eventDuration = 10.5f;
            cost = 20;
            hasEntered = true;
            activityType = 0;
            break;
          case "Stadium":
            student.ctx.SetState(AIWorldState.occupied, true, EffectType.Permanent);
            eventDuration = 14f;
            cost = 40;
            hasEntered = true;
            activityType = 1;
            break;
          case "SportsBar":
            student.ctx.SetState(AIWorldState.occupied, true, EffectType.Permanent);
            eventDuration = 9f;
            cost = 10;
            hasEntered = true;
            activityType = 0;
            break;
          default:
            break;
        }
      }
    }
}
