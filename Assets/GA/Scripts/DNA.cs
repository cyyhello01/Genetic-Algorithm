using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    public Dictionary <bool, float> genes; //bool= to detect if there is a block infront of it; float=direction to turn if there is something blocking 
    int dnaLength; //for swaping individual gene values

    public DNA()
    {
        genes = new Dictionary <bool, float>();
        SetRandom(); //to fill up dictionary with random values  //--Genetic Algorithm-- starts(all agents get random genes)
    }

    //best one kept their gene and breed together, others put aside because of bad quality

    public void SetRandom() //set all possible states that the agent is going - block infront? angle of turning
    {
        genes.Clear();
        genes.Add(false, Random.Range(-90, 91)); //no wall blocking -> - 90 degree to +90 (91 exclusive); angle to turn when there is no walls blocking infront
        genes.Add(true, Random.Range(-90, 91)); // a block infront, turning angle remain the same as well
        dnaLength = genes.Count;
    }

    public void Combine(DNA d1, DNA d2) //combine DNA
    {
        int i = 0; //keep track of each gene
        Dictionary<bool, float> newGenes = new Dictionary<bool, float>();//temporary holding spot for genes
        foreach (KeyValuePair<bool, float> g in genes)
        {
            if (i < dnaLength/2)
            {
                newGenes.Add(g.Key, d1.genes[g.Key]);
            }
            else
            {
                newGenes.Add(g.Key, d2.genes[g.Key]);
            }
            i++;
        }
        genes = newGenes;
    }

    public float GetGene(bool front)
    {
        return genes[front]; //return angles based on true or false
    }
}
