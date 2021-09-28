using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementPay : MonoBehaviour
{
  public int dailyPay;
  public string employeeType;
  private Student agent;
  public WorldManager worldManager;


  private void OnTriggerEnter(Collider other)
  {
    agent = other.gameObject.GetComponent<Student>();

    if(agent.paidToday == false && agent.jobPosition == employeeType)
    {
      agent.money += dailyPay;
      agent.paidToday = true;
    }
  }

}
