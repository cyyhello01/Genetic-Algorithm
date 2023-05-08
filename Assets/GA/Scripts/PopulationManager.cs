using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {

	public GameObject botPrefab;
	public GameObject[] startingPos;
	public int populationSize = 50;
	List<GameObject> population = new List<GameObject>();
	public static float elapsed = 0;
	public float trialTime = 10;
	public float timeScale = 2;
	int generation = 1;
	public GenerateMaze maze;

	GUIStyle guiStyle = new GUIStyle();
	void OnGUI()
	{
		guiStyle.fontSize = 25;
		guiStyle.normal.textColor = Color.white;
		GUI.BeginGroup (new Rect (10, 10, 250, 150));
		GUI.Box (new Rect (0,0,140,140), "Stats", guiStyle);
		GUI.Label(new Rect (10,25,200,30), "Gen: " + generation, guiStyle);
		GUI.Label(new Rect (10,50,200,30), string.Format("Time: {0:0.00}",elapsed), guiStyle);
		GUI.Label(new Rect (10,75,200,30), "Population: " + population.Count, guiStyle);
		GUI.EndGroup ();
	}


	// Use this for initialization
	void Start () {
		for(int i = 0; i < populationSize; i++)
		{
			int starti = Random.Range(0, startingPos.Length);
			GameObject b = Instantiate(botPrefab, startingPos[starti].transform.position, this.transform.rotation);
			b.transform.Rotate(0, Mathf.Round(Random.Range(-90, 91) / 90) * 90, 0);
			b.GetComponent<Brain>().Init(); //agent got the brain to have random angles for going forward
			population.Add(b);
		}
		Time.timeScale = timeScale;
	}

	GameObject Breed(GameObject parent1, GameObject parent2)
	{
		int starti = Random.Range(0, startingPos.Length); //set up new starting position(1-4) for the new offspring
		GameObject offspring = Instantiate(botPrefab, startingPos[starti].transform.position, this.transform.rotation);
        offspring.transform.Rotate(0, Mathf.Round(Random.Range(-90, 91) / 90) * 90, 0);
		Brain b = offspring.GetComponent<Brain>();//initialize a brain for the offspring -> //to combine the dna with parents
        if (Random.Range(0, 100) == 1) { // (1/100 chance to get mutation)
			b.Init(); //initialize "mutated" brain 
		}
		else
		{
			b.Init();
			b.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
		}
        return offspring;
	}

	void BreedNewPopulation() //figure out who is the fittest in each generation to create a new population
	{
        //organize the population in the sortedList based on the eggs found
        //most eggs found will be on the top of list
        List<GameObject> sortedList = population.OrderByDescending(o=> o.GetComponent<Brain>().eggsFound).ToList();
		//Debug code: see the eggs found in list
		string eggsCollected = "Generation: " + generation;
		foreach (GameObject g in sortedList)
		{
			eggsCollected += ", " + g.GetComponent<Brain>().eggsFound;
		}
		Debug.Log("Eggs: " + eggsCollected);
		population.Clear(); //population = 0


		//breed new population from sortedList into population list
		while (population.Count < populationSize)
		{
			int bestParentCutoff = sortedList.Count / 4; //keep the top half of the parents
			for (int i=0; i < bestParentCutoff -1; i++) //first parent, put -1 so that we wont take the last parent
			{
				for (int j = 1; j < bestParentCutoff; j++) //second parent in the list,
				{
					population.Add(Breed(sortedList[i], sortedList[j]));//breed offspsring based on the sorted parent list
					if (population.Count == populationSize)
					{
						break;
					}
                    population.Add(Breed(sortedList[j], sortedList[i])); //reverse breed
					if (population.Count == populationSize)
					{
						break;
					}
                }
                if (population.Count == populationSize)
                {
                    break;
                }
            }
		}
		//delete old population from sortedList
		for (int i=0; i < sortedList.Count; i++)
		{
			Destroy(sortedList[i]);
		}
		generation++; //move to next generation
	}
	
	void Update () //keep track of the time trial, after the time is finished
	{
		elapsed += Time.deltaTime;
		if (elapsed >= trialTime)
		{
			maze.Reset();
			BreedNewPopulation();
			elapsed = 0;
		}
	}
}
