using UnityEngine;

public class Brain : MonoBehaviour
{

    void Update()
    {

    }


    void FixedUpdate()
    {
        this.transform.Translate(0, 0, 0.1f);
    }
}

