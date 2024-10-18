using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WaitIntro : MonoBehaviour
{

    public float wait_time = 7f;

    void Start()
    {
        StartCoroutine(Wait_intro());
    }

    IEnumerator Wait_intro()
    {
        yield return new WaitForSeconds(wait_time);
        SceneManager.LoadScene("Main");

    }

    
}
