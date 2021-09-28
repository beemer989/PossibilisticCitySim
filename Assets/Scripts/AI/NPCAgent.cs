using System;
using System.Diagnostics;
using FluidHTN;
using UnityEngine;
using UnityEngine.AI;

public class NPCAgent : MonoBehaviour
{
  private Planner<AIContext> _planner;
  private AIContext _context;
  public AIContext Context => _context;
  public Planner<AIContext> Planner => _planner;
  public Transform homePosition;
  public Transform workPosition;
  public Transform schoolPosition;
  public GameObject[] foodLocations;
  public NavMeshAgent navMeshAgent;

  public GameObject foodSelection;
  public GameObject BowlingAlley;
  public GameObject Stadium;
  public GameObject SportsBar;
  public int cheapActivityIndex;
  public int foodDecision = 0;

  public NPCAgent()
  {
    _planner = new Planner<AIContext>();
    _context = new AIContext(this);
    _context.Init();
  }

  private void Start()
  {
    foodLocations = GameObject.FindGameObjectsWithTag("Food");
    var rnd = new System.Random(DateTime.Now.Millisecond);
    int selection = rnd.Next(0, foodLocations.Length);
    foodSelection = foodLocations[selection];

    BowlingAlley = GameObject.FindGameObjectWithTag("BowlingAlley");
    Stadium = GameObject.FindGameObjectWithTag("Stadium");
    SportsBar = GameObject.FindGameObjectWithTag("SportsBar");
    navMeshAgent = GetComponent<NavMeshAgent>();
    cheapActivityIndex = rnd.Next(0, 2);
  }

  public void Think(Domain<AIContext> domain)
  {
    _planner.Tick(domain, _context);

    if (_context.LogDecomposition)
    {
      Console.WriteLine("---------------------- DECOMP LOG --------------------------");
      if (_context.DecompositionLog?.Count > 0)
      {
        var entry = _context.DecompositionLog.Dequeue();
        var depth = FluidHTN.Debug.Debug.DepthToString(entry.Depth);
        Console.ForegroundColor = entry.Color;
        Console.WriteLine($"{depth}{entry.Name}: {entry.Description}");
      }
      Console.ResetColor();
      Console.WriteLine("-------------------------------------------------------------");
    }
  }
}
