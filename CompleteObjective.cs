using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteObjective : MonoBehaviour
{
    public GameObject WinScreen;    //WIN SCREEN
    [SerializeField]private AudioClip missioncompleteclp;     //MISSION COMPLETE SFX
    //void Awake()=>WinScreen.SetActive(false);
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"){
            StartCoroutine(Win());       //IF PLAYER TRIGGERS THE EXIT COLLIDER, STARTS COROUTINE WIN
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
    IEnumerator Win()        //WIN CONDITION COROUTINE
    {
        WinScreen.SetActive(true);
        AudioSource.PlayClipAtPoint(missioncompleteclp,transform.position);
        Destroy(this.GetComponent<BoxCollider>());
        yield return new WaitForSeconds(8);
        SceneManager.LoadScene("Menu");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
