using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
     public void FinishAtk()
     {
      Destroy(this.gameObject);
     }
}