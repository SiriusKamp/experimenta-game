using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Bonfire : MonoBehaviour
{
    public GameObject KeyE;
    public void OnTriggerStay2D(Collider2D collider){
        if(collider.tag == "Player"){
        KeyE.SetActive(true);
        if(Input.GetKeyDown(KeyCode.E))
        SceneManager.LoadScene("Fim");
        }
    }
    public void OnTriggerExit2D(Collider2D collider){
if(collider.tag == "Player")
        KeyE.SetActive(false);
    }

}
