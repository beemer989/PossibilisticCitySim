using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentSelector : MonoBehaviour
{
  GameObject studentSelectGO;
  public Dropdown studentSelector;
  public Text studentInfo;
  float timePassed = 0;

  GameObject[] studentGOs;
  List<string> studentNames = new List<string>();
  bool firstCollect = false;

  List<Student> students = new List<Student>();
  Student tempStudent;

  public GameObject studentMarker;

  void Start()
  {
    studentSelectGO = GameObject.FindGameObjectWithTag("StudentSelector");
    studentSelector = studentSelectGO.GetComponent<Dropdown>();
  }

  void Update()
  {
    timePassed += Time.deltaTime;

    if (timePassed > 3 && firstCollect == false)
    {
      studentGOs = GameObject.FindGameObjectsWithTag("Student");
      for (int i = 0; i < studentGOs.Length; i++)
      {
        studentNames.Add(studentGOs[i].name + " " + (i + 1));
        studentGOs[i].name = studentGOs[i].name + " " + (i + 1);

        tempStudent = studentGOs[i].GetComponent<Student>();
        students.Add(tempStudent);
      }
      studentSelector.AddOptions(studentNames);
      firstCollect = true;
    }

    if(studentSelector.options.Count != 0)
    {
      var studentInfoStart = "Student Info:\n\nName: " + students[studentSelector.value].name + "\n\n";
      var studentHunger = "Hunger: " + students[studentSelector.value].hunger + "\n\n";
      var studentMoney = "Money: " + students[studentSelector.value].money + "\n\n";

      var tempEnt = "Entrance";
      var homeChoice = students[studentSelector.value].Agent.homePosition.name;
      homeChoice = homeChoice.Replace(tempEnt, "");
      var workChoice = students[studentSelector.value].Agent.workPosition.name;
      workChoice = workChoice.Replace(tempEnt, "");
      var schoolChoice = students[studentSelector.value].Agent.schoolPosition.name;
      schoolChoice = schoolChoice.Replace(tempEnt, "");


      var studentHome = "Home: " + homeChoice + "\n\tBedtime: " + students[studentSelector.value].sleepStart + "\n\tWake Up: " + students[studentSelector.value].sleepEnd + "\n\n";
      var studentWork = "Work: " + workChoice + "\n\tStart: " + students[studentSelector.value].workStart + "\n\tEnd: " + students[studentSelector.value].workEnd + "\n\n";
      var studentSchool = "School: " + schoolChoice + "\n\tStart: " + students[studentSelector.value].schoolStart + "\n\tEnd: " + students[studentSelector.value].schoolEnd + "\n\n";

      var studentBarPref = "Bar Pref: " + students[studentSelector.value].dispBarPref + "\n\n";
      var studentBowlPref = "Bowl Pref: " + students[studentSelector.value].dispBowlingPref + "\n\n";
      var studentGamePref = "Game Pref: " + students[studentSelector.value].dispGamePref + "\n\n";
      var studentHomePref = "Home Pref: " + students[studentSelector.value].dispHomePref + "\n\n";

      studentInfo.text = studentInfoStart + studentHunger + studentMoney + studentHome + studentWork + studentSchool + studentBarPref + studentBowlPref + studentGamePref + studentHomePref;
      studentMarker.transform.position = students[studentSelector.value].transform.position;
    }
  }
}
