using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dummy : MonoBehaviour
{
    public Image hp;
    [SerializeField] GameObject die;

    [SerializeField] float respawnTime;
    [SerializeField] float respawnCd;
    [SerializeField] bool checkDie;

    public void Start()
    {
        hp.fillAmount = 1;
        respawnCd = 2;
    }

    public void Update()
    {

        if(respawnTime > 0 && checkDie)
        {
            respawnTime -= Time.deltaTime;
        }
        else if(respawnTime <= 0 && checkDie)
        {
            hp.fillAmount = 1;
            checkDie = false;
        }

        if(hp.fillAmount > 0)
        {
            die.SetActive(false);
        }
        else
        {
            die.SetActive(true);
            if (!checkDie)
            {
                respawnTime = respawnCd;
                checkDie = true;
            }
        }
    }

    public void atacked()
    {
        hp.fillAmount -= 0.1f;
    }
}
