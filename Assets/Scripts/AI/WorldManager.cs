using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
  public int time;
  public int day;

  public Light sun;

  float timePassed = 0f;

  GameObject[] nightlights;

  GameObject[] people;
  Student person;

  private void Start()
  {
    nightlights = GameObject.FindGameObjectsWithTag("spotlight");
    time = 3;
    day = 1;
  }

  private void Update()
  {
    timePassed += Time.deltaTime;
    if (timePassed > 12f)
    {
      switch (time)
      {
        case 0:
          Debug.Log("1st hour");
          sun.transform.eulerAngles = new Vector3(270, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 1;
          break;
        case 1:
          Debug.Log("2nd hour");
          sun.transform.eulerAngles = new Vector3(-75, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 2;
          break;
        case 2:
          Debug.Log("3rd hour");
          sun.transform.eulerAngles = new Vector3(-60, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 3;
          break;
        case 3:
          Debug.Log("4th hour");
          sun.transform.eulerAngles = new Vector3(-45, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 4;
          break;
        case 4:
          Debug.Log("5th hour");
          sun.transform.eulerAngles = new Vector3(-30, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 5;
          break;
        case 5:
          Debug.Log("6th hour");
          foreach (GameObject spotlight in nightlights)
          {
            Light temp = spotlight.GetComponent<Light>();
            temp.enabled = false;
          }
          sun.transform.eulerAngles = new Vector3(-15, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 6;
          break;
        case 6:
          Debug.Log("7th hour");
          sun.transform.eulerAngles = new Vector3(0, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 7;
          break;
        case 7:
          Debug.Log("8th hour");
          sun.transform.eulerAngles = new Vector3(15, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 8;
          break;
        case 8:
          Debug.Log("9th hour");
          sun.transform.eulerAngles = new Vector3(30, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 9;
          break;
        case 9:
          Debug.Log("10th hour");
          sun.transform.eulerAngles = new Vector3(45, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 10;
          break;
        case 10:
          Debug.Log("11th hour");
          sun.transform.eulerAngles = new Vector3(60, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 11;
          break;
        case 11:
          Debug.Log("12th hour");
          sun.transform.eulerAngles = new Vector3(75, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 12;
          break;
        case 12:
          Debug.Log("13th hour");
          sun.transform.eulerAngles = new Vector3(90, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 13;
          break;
        case 13:
          Debug.Log("14th hour");
          sun.transform.eulerAngles = new Vector3(105, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 14;
          break;
        case 14:
          Debug.Log("15th hour");
          sun.transform.eulerAngles = new Vector3(120, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 15;
          break;
        case 15:
          Debug.Log("16th hour");
          sun.transform.eulerAngles = new Vector3(135, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 16;
          break;
        case 16:
          Debug.Log("17th hour");
          sun.transform.eulerAngles = new Vector3(150, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 17;
          break;
        case 17:
          Debug.Log("18th hour");
          foreach (GameObject spotlight in nightlights)
          {
            Light temp = spotlight.GetComponent<Light>();
            temp.enabled = true;
          }
          sun.transform.eulerAngles = new Vector3(165, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 18;
          break;
        case 18:
          Debug.Log("19th hour");
          sun.transform.eulerAngles = new Vector3(180, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 19;
          break;
        case 19:
          Debug.Log("20th hour");
          sun.transform.eulerAngles = new Vector3(195, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 20;
          break;
        case 20:
          Debug.Log("21st hour");
          sun.transform.eulerAngles = new Vector3(210, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 21;
          break;
        case 21:
          Debug.Log("22nd hour");
          sun.transform.eulerAngles = new Vector3(225, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 22;
          break;
        case 22:
          Debug.Log("23rd hour");
          sun.transform.eulerAngles = new Vector3(240, sun.transform.rotation[1], sun.transform.rotation[2]);
          time = 23;
          break;
        case 23:
          Debug.Log("24th hour");
          people = GameObject.FindGameObjectsWithTag("Student");
          foreach (GameObject temp in people)
          {
            person = temp.GetComponent<Student>();
            person.paidToday = false;
          }
          sun.transform.eulerAngles = new Vector3(255, sun.transform.rotation[1], sun.transform.rotation[2]);

          switch (day)
          {
            case 1:
              Debug.Log("Sunday");
              day = 2;
              break;
            case 2:
              Debug.Log("Monday");
              day = 3;
              break;
            case 3:
              Debug.Log("Tuesday");
              day = 4;
              break;
            case 4:
              Debug.Log("Wednesday");
              day = 5;
              break;
            case 5:
              Debug.Log("Thursday");
              day = 6;
              break;
            case 6:
              Debug.Log("Friday");
              day = 7;
              break;
            case 7:
              Debug.Log("Saturday");
              day = 1;
              break;
            default:
              break;
          }
          time = 0;
          break;
        default:
          break;
      }
      timePassed = 0f;
    }
  }
}
