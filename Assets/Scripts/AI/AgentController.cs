using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject resourceLocation;
    public GameObject refinementLocation;
  // Update is called once per frame


    private void Awake()
    {
      agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
      agent.SetDestination(resourceLocation.transform.position);
    }
}
