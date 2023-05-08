using System.Collections.Generic;
using UnityEngine;

public class DNA//move freely the maze to explore, not stuck in any corner 
{
    public Dictionary<(bool left, bool forward, bool right), float> genes; //bool= to detect if there is a block infront of it (true == blocked); float=direction to turn if there is something blocking 
    int dnaLength; //for swaping individual gene values

    public DNA()
    {
        genes = new Dictionary<(bool left, bool forward, bool right), float>();
        SetRandom(); //to fill up dictionary with random values  //--Genetic Algorithm-- starts(all agents get random genes)
    }

    //best one kept their gene and breed together, others put aside because of bad quality

    public void SetRandom() //set all possible states that the agent is going - whether a block infront? angle of turning
    {
        genes.Clear();// need to call back more few times - reset the genes
        genes.Add((false, false, false), Random.Range(-90, 91)); //if no wall blocking -> - 90 degree to +90 (91 exclusive); angle to turn when there is no walls blocking infront
        genes.Add((false, false, true), Random.Range(-90, 91)); //if a block infront, turning angle remain the same as well
        genes.Add((false, true, true), Random.Range(-90, 91));
        genes.Add((true, true, true), Random.Range(-90, 91));
        genes.Add((true, false, false), Random.Range(-90, 91));
        genes.Add((true, false, true), Random.Range(-90, 91));
        genes.Add((false, true, false), Random.Range(-90, 91));
        genes.Add((true, true, false), Random.Range(-90, 91));
        dnaLength = genes.Count;
    }

    public void Combine(DNA d1, DNA d2) //combine DNA with another gene - parents
    {
        int i = 0; //keep track of each gene
        Dictionary<(bool left, bool forward, bool right), float> newGenes = 
            new Dictionary<(bool left, bool forward, bool right), float>();//temporary holding spot for genes
        foreach (KeyValuePair<(bool left, bool forward, bool right), float> g in genes) //loop through genes in the sequence of dna
        {
            if (i < dnaLength / 2)  //first parent
            {
                newGenes.Add(g.Key, d1.genes[g.Key]);
            }
            else //second parent
            {
                newGenes.Add(g.Key, d2.genes[g.Key]);
            }
            i++;
        }
        genes = newGenes;
    }

    public float GetGene((bool left, bool forward, bool right) seeWall)//how the agent should turn based on the states - (whatever infornt of them, true or false)
    {
        return genes[seeWall]; //return angles based on true or false
    }
}
