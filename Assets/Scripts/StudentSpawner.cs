using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StudentSpawner : MonoBehaviour
{
  GameObject[] possibleHomes;
  GameObject[] possibleWorkplaces;
  GameObject[] possibleSchoolBuildings;
  public Material[] possibleMats;
  public int numOfStudents = 2;
  public WorldManager worldManager;
  public GameObject studentPrefab;

  List<GameObject> students = new List<GameObject>();
  GameObject instantiatedStudent;
  NPCAgent instAgentRef;
  Student instStudentRef;
  Material instMatRef;
  Renderer instRenderRef;

  List<Tuple<Tuple<int,int>,Tuple<int,int>>> possibleWorkSchoolTimes = new List<Tuple<Tuple<int,int>, Tuple<int,int>>>();

  void Start()
  {
    possibleHomes = GameObject.FindGameObjectsWithTag("Home");
    possibleWorkplaces = GameObject.FindGameObjectsWithTag("Workplace");
    possibleSchoolBuildings = GameObject.FindGameObjectsWithTag("School");

    var rnd = new System.Random(DateTime.Now.Millisecond);


    // ADD MORE TIMES (OR FIGURE OUT WAYS TO MIX THEM) FOR MORE VARIATION
    possibleWorkSchoolTimes.Add(Tuple.Create(Tuple.Create(8, 13), Tuple.Create(14, 16)));
    possibleWorkSchoolTimes.Add(Tuple.Create(Tuple.Create(9, 17), Tuple.Create(18, 20)));
    possibleWorkSchoolTimes.Add(Tuple.Create(Tuple.Create(11, 15), Tuple.Create(8, 10)));
    possibleWorkSchoolTimes.Add(Tuple.Create(Tuple.Create(14, 21), Tuple.Create(11, 13)));

    if(PlayerPrefs.HasKey("NumOfStudents"))
      numOfStudents = PlayerPrefs.GetInt("NumOfStudents");

    for (int i = 0; i < numOfStudents; ++i)
    {
      
      var homeIndex = rnd.Next(0, possibleHomes.Length);
      var workplaceIndex = rnd.Next(0, possibleWorkplaces.Length);
      var schoolIndex = rnd.Next(0, possibleSchoolBuildings.Length);
      var matIndex = rnd.Next(0, possibleMats.Length);
      var workschoolPairIndex = rnd.Next(0, possibleWorkSchoolTimes.Count);

      instantiatedStudent = Instantiate(studentPrefab, possibleHomes[homeIndex].gameObject.transform.position, Quaternion.identity);
      students.Add(instantiatedStudent);

      instMatRef = possibleMats[matIndex];
      instRenderRef = instantiatedStudent.GetComponent<Renderer>();
      instRenderRef.material = instMatRef;

      instAgentRef = instantiatedStudent.GetComponent<NPCAgent>();
      instAgentRef.homePosition = possibleHomes[homeIndex].gameObject.transform;
      instAgentRef.workPosition = possibleWorkplaces[workplaceIndex].gameObject.transform;
      instAgentRef.schoolPosition = possibleSchoolBuildings[schoolIndex].gameObject.transform;
      
      instStudentRef = instantiatedStudent.GetComponent<Student>();
      instStudentRef.worldManager = worldManager;
      IncrementPay tempPay = possibleWorkplaces[workplaceIndex].gameObject.GetComponent<IncrementPay>();
      instStudentRef.jobPosition = tempPay.employeeType;
      instStudentRef.MWFworkTimeFrame.startTime = possibleWorkSchoolTimes[workschoolPairIndex].Item1.Item1;
      instStudentRef.MWFworkTimeFrame.endTime = possibleWorkSchoolTimes[workschoolPairIndex].Item1.Item2;
      instStudentRef.MWFschoolTimeFrame.startTime = possibleWorkSchoolTimes[workschoolPairIndex].Item2.Item1;
      instStudentRef.MWFschoolTimeFrame.endTime = possibleWorkSchoolTimes[workschoolPairIndex].Item2.Item2;

      workschoolPairIndex = rnd.Next(0, possibleWorkSchoolTimes.Count);
      instStudentRef.TTRworkTimeFrame.startTime = possibleWorkSchoolTimes[workschoolPairIndex].Item1.Item1;
      instStudentRef.TTRworkTimeFrame.endTime = possibleWorkSchoolTimes[workschoolPairIndex].Item1.Item2;
      instStudentRef.TTRschoolTimeFrame.startTime = possibleWorkSchoolTimes[workschoolPairIndex].Item2.Item1;
      instStudentRef.TTRschoolTimeFrame.endTime = possibleWorkSchoolTimes[workschoolPairIndex].Item2.Item2;

      instStudentRef.hungerRate = rnd.Next(1, 4);
      instAgentRef.cheapActivityIndex = rnd.Next(0, 2);
      instStudentRef.worldManager = worldManager;
      new WaitForSeconds(.01f);
    }
  }
}
