using System.Collections.Generic;
using UnityEngine;

public class DNA//move freely the maze to explore, not stuck in any corner 
{
    public Dictionary<bool, float> genes; //bool= to detect if there is a block infront of it (true == blocked); float=direction to turn if there is something blocking 
    int dnaLength; //for swaping individual gene values

    public DNA()
    {
        genes = new Dictionary<bool, float>();
        SetRandom(); //to fill up dictionary with random values  //--Genetic Algorithm-- starts(all agents get random genes)
    }

    //best one kept their gene and breed together, others put aside because of bad quality

    public void SetRandom() //set all possible states that the agent is going - whether a block infront? angle of turning
    {
        genes.Clear();// need to call back more few times - reset the genes
        genes.Add(false, Random.Range(-90, 91)); //if no wall blocking -> - 90 degree to +90 (91 exclusive); angle to turn when there is no walls blocking infront
        genes.Add(true, Random.Range(-90, 91)); //if a block infront, turning angle remain the same as well
        dnaLength = genes.Count;
    }

    public void Combine(DNA d1, DNA d2) //combine DNA with another gene - parents
    {
        int i = 0; //keep track of each gene
        Dictionary<bool, float> newGenes = new Dictionary<bool, float>();//temporary holding spot for genes
        foreach (KeyValuePair<bool, float> g in genes) //loop through genes in the sequence of dna
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

    public float GetGene(bool front)//how the agent should turn based on the states - (whatever infornt of them, true or false)
    {
        return genes[front]; //return angles based on true or false
    }
}
