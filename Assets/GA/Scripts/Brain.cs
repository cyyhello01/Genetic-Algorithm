using System.Runtime.CompilerServices;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public DNA dna;
    public GameObject eyes; //point the agent can see if something is infront
    bool seeWall;
    public float eggsFound = 0;
    LayerMask ignore = 6; //make sure the raycast will not hit any other agent(in layer 6)
    bool canMove = false; //if got a wall infront, will not transform in fixedUpdate

    public void Init()
    {
        dna = new DNA();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("egg"))
        {
            eggsFound++;
            other.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        seeWall = false;
        canMove = true;
        RaycastHit hit;
        Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 1f, Color.red);
        if (Physics.SphereCast(eyes.transform.position, 0.1f, eyes.transform.forward, out hit, 1f, ~ignore)) //use spherecast so that it has some volume(broader), if it is a single ray, the detection is very precise
        {
            if (hit.collider.gameObject.CompareTag("wall"))
            {
                seeWall = true;
                canMove = false;
            }
        }
    }


    void FixedUpdate()
    {
        this.transform.Rotate(0, dna.genes[seeWall], 0); //rotate based on the states of whether a block is infront of the agent
        if(canMove)
        {
            this.transform.Translate(0, 0, 0.1f);
        }
       
    }
}

