using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetNumOfStudentsToSpawn : MonoBehaviour
{

    public int numOfStudToSpawn;
    InputField currText;

    // Start is called before the first frame update
    void Start()
    {
      currText = this.GetComponent<InputField>();
      Debug.Log("NEW " + currText.text);
      currText.text = PlayerPrefs.GetInt("NumOfStudents").ToString();
      Debug.Log("NEW " + currText.text);
    }

    // Update is called once per frame
    void Update()
    {
        if(currText.text != "")
        {
          if(int.Parse(currText.text) != PlayerPrefs.GetInt("NumOfStudents"))
          {
            PlayerPrefs.SetInt("NumOfStudents", int.Parse(currText.text));
            Debug.Log(PlayerPrefs.GetInt("NumOfStudents"));
          }
        }
      }
}
