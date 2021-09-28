using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurnOffStudentCollis : MonoBehaviour
{
  private NavMeshAgent agent;
  private Collider studCollider;
  private void OnTriggerEnter(Collider other)
  {
    studCollider = other.gameObject.GetComponent<Collider>();
    studCollider.gameObject.layer = 8;
    studCollider.attachedRigidbody.gameObject.layer = 8;

    agent = other.gameObject.GetComponent<NavMeshAgent>();
    agent.radius = 0.05f;
  }

  private void OnTriggerExit(Collider other)
  {
    studCollider = other.gameObject.GetComponent<Collider>();
    studCollider.gameObject.layer = 9;
    studCollider.attachedRigidbody.gameObject.layer = 9;

    agent = other.gameObject.GetComponent<NavMeshAgent>();
    agent.radius = 0.5f;
  }
}
