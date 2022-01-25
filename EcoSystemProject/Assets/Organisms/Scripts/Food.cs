using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private bool m_Eaten = false;

    public void EatFood()
    {
        m_Eaten = true;
    }

    public bool IsEaten() => m_Eaten;
    

}
