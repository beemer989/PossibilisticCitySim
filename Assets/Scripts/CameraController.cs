using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CameraController : MonoBehaviour
{
  int camIndex;
  public List<Vector3> positions;
  public List<Vector3> rotations;

  GameObject cameraGO;
  Dropdown cameraOption;
  float timePassed = 0f;

  void Start()
  {
    positions = new List<Vector3>();
    positions.Add(new Vector3(0, 8, -14));
    positions.Add(new Vector3(12, 8, -4));
    positions.Add(new Vector3(0, 8, 9));
    positions.Add(new Vector3(-12, 8, -4));
    positions.Add(new Vector3(0, 16, -4));

    rotations = new List<Vector3>();
    rotations.Add(new Vector3(47.5f, 0, 0));
    rotations.Add(new Vector3(47.5f, -90, 0));
    rotations.Add(new Vector3(47.5f, 180, 0));
    rotations.Add(new Vector3(47.5f, 90, 0));
    rotations.Add(new Vector3(90, 90, 0));
    cameraGO = GameObject.FindGameObjectWithTag("CameraOption");
    cameraOption = cameraGO.GetComponent<Dropdown>();
  }

  void Update()
  {
    switch (cameraOption.value)
    {
      case 0:
        Camera.main.transform.position = positions[1];
        Camera.main.transform.rotation = Quaternion.Euler(rotations[1].x, rotations[1].y, rotations[1].z);
        break;
      case 1:
        Camera.main.transform.position = positions[2];
        Camera.main.transform.rotation = Quaternion.Euler(rotations[2].x, rotations[2].y, rotations[2].z);
        break;
      case 2:
        Camera.main.transform.position = positions[3];
        Camera.main.transform.rotation = Quaternion.Euler(rotations[3].x, rotations[3].y, rotations[3].z);
        break;
      case 3:
        Camera.main.transform.position = positions[0];
        Camera.main.transform.rotation = Quaternion.Euler(rotations[0].x, rotations[0].y, rotations[0].z);
        break;
      case 4:
        Camera.main.transform.position = positions[4];
        Camera.main.transform.rotation = Quaternion.Euler(rotations[4].x, rotations[4].y, rotations[4].z);
        break;
      default:
        break;
    }
  }
}
