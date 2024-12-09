using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HeartRateManager : MonoBehaviour
{
    private float time = 0;
    private float hider1Pulse = 0;
    private float hider2Pulse = 0;
    private float hider3Pulse = 0;
    private int hider1Bpm = 0;
    private int hider2Bpm = 0;
    private int hider3Bpm = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;   
    }
    
    void OnMessageArrived(string message)
    {
        int bpm = int.Parse(message);
        if (GameManager.Instance.hiders[0] != null && !GameManager.Instance.hiders[0].tag.Equals("Dead"))
        {
            if (bpm > 45 && bpm < 150)
            {
                GameManager.Instance.updateBpm(1, message);
                if (bpm  > 120)
                {
                    GameManager.Instance.hiders[0].transform.GetChild(2).GetComponent<AudioSource>().volume = 1;
                    GameManager.Instance.hiders[0].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                    GameManager.Instance.HeartHighAnimation(1);
                    for (int i = 2; i < 5; i++)
                    {
                        GameManager.Instance.hiders[0].transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                    }
                }

            }
        }
        if (GameManager.Instance.hiders[1] != null && !GameManager.Instance.hiders[1].tag.Equals("Dead"))
        {
            if (bpm > 45 && bpm < 150)
            {
                GameManager.Instance.updateBpm(2, message);
                if (bpm > 120)
                {
                    GameManager.Instance.hiders[1].transform.GetChild(2).GetComponent<AudioSource>().volume = 1;
                    GameManager.Instance.hiders[1].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                    GameManager.Instance.HeartHighAnimation(2);
                    for (int i = 2; i < 5; i++)
                    {
                        GameManager.Instance.hiders[1].transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                    }
                }

            }
        }
        if (GameManager.Instance.hiders[2] != null && !GameManager.Instance.hiders[2].tag.Equals("Dead"))
        {
            GameManager.Instance.updateBpm(3, message);
            if (bpm > 120)
            {
                GameManager.Instance.hiders[2].transform.GetChild(2).GetComponent<AudioSource>().volume = 1;
                GameManager.Instance.hiders[2].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                GameManager.Instance.HeartHighAnimation(2);
                for (int i = 2; i < 5; i++)
                {
                    GameManager.Instance.hiders[2].transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                }
            }

        }
    }

    void OnMessageArrived2(string message)
    {
        Debug.Log(message);
        string[] hearRate = message.Split(',');
        if (GameManager.Instance.hiders[0] != null && !GameManager.Instance.hiders[0].tag.Equals("Dead"))
        {
            if (int.Parse(hearRate[0]) > 600)
            {
                Debug.Log("hider1: " + (time - hider1Pulse).ToString());
                int bpm1 = (int)( 60 / (time - hider1Pulse));
                Debug.Log("hider1 bpm =" + bpm1);
                if(bpm1 > 45 && bpm1 < 150)
                {
                    GameManager.Instance.updateBpm(1, bpm1.ToString());
                    if (bpm1 - hider1Bpm > 20)
                    {
                        GameManager.Instance.hiders[0].transform.GetChild(2).GetComponent<AudioSource>().volume = 1;
                        GameManager.Instance.hiders[0].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                        GameManager.Instance.HeartHighAnimation(1);
                        for (int i = 2; i < 5; i++)
                        {
                            GameManager.Instance.hiders[0].transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                        }
                    }

                    hider1Bpm = bpm1;
                }
                
                //if (hider1Pulse != 0 && time - hider1Pulse < 0.5f)
                //{
                //    GameManager.Instance.hiders[0].transform.GetChild(2).GetComponent<AudioSource>().volume = 1;
                //    GameManager.Instance.hiders[0].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                //    GameManager.Instance.HeartHighAnimation(1);
                //    for (int i = 2; i < 5; i++)
                //    {
                //        GameManager.Instance.hiders[0].transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                //    }
                //}
                //else
                //{
                //    //GameManager.Instance.hiders[0].transform.GetChild(2).GetComponent<AudioSource>().volume = 0.5f;
                //}
                hider1Pulse = time;
            }
        }

        if (GameManager.Instance.hiders[1] != null && !GameManager.Instance.hiders[1].tag.Equals("Dead"))
        {
            if (int.Parse(hearRate[1]) > 550)
            {
                int bpm2 = (int)(60 / (time - hider2Pulse));
                Debug.Log("hider2 bpm =" + bpm2);
                if (bpm2 > 45 && bpm2 < 150)
                {
                    GameManager.Instance.updateBpm(2, bpm2.ToString());
                    if (bpm2 - hider2Bpm > 20)
                    {
                        GameManager.Instance.hiders[1].transform.GetChild(2).GetComponent<AudioSource>().volume = 1;
                        GameManager.Instance.hiders[1].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                        GameManager.Instance.HeartHighAnimation(2);
                        for (int i = 2; i < 5; i++)
                        {
                            GameManager.Instance.hiders[1].transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                        }
                    }

                    hider2Bpm = bpm2;
                }
                
                //if (hider2Pulse != 0 && time - hider2Pulse < 0.5f)
                //{
                //    GameManager.Instance.hiders[1].transform.GetChild(2).GetComponent<AudioSource>().volume = 1;
                //    GameManager.Instance.hiders[1].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                //    GameManager.Instance.HeartHighAnimation(2);
                //    for (int i = 2; i < 5; i++)
                //    {
                //        GameManager.Instance.hiders[1].transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                //    }
                //}
                //else
                //{
                //    //GameManager.Instance.hiders[1].transform.GetChild(2).GetComponent<AudioSource>().volume = 0.5f;
                //}
                hider2Pulse = time;
            }
        }

        if (GameManager.Instance.hiders[2] != null && !GameManager.Instance.hiders[2].tag.Equals("Dead"))
        {
            if (int.Parse(hearRate[2]) > 550)
            {
                int bpm3 = (int)(60 / (time - hider3Pulse));
                Debug.Log("hider3 bpm =" + bpm3);
                if (bpm3 > 45 && bpm3 < 150)
                {
                    GameManager.Instance.updateBpm(3, bpm3.ToString());
                    if (bpm3 - hider3Bpm > 20)
                    {
                        GameManager.Instance.hiders[2].transform.GetChild(2).GetComponent<AudioSource>().volume = 1;
                        GameManager.Instance.hiders[2].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                        GameManager.Instance.HeartHighAnimation(3);
                        for (int i = 2; i < 5; i++)
                        {
                            GameManager.Instance.hiders[2].transform.GetChild(1).GetChild(i).gameObject.SetActive(false);
                        }
                    }

                    hider3Bpm = bpm3;
                }
                
                //if (hider3Pulse != 0 && time - hider3Pulse < 0.5f)
                //{
                //    GameManager.Instance.hiders[2].transform.GetChild(2).GetComponent<AudioSource>().volume = 1;
                //    GameManager.Instance.hiders[2].transform.GetChild(1).GetChild(1).gameObject.SetActive(true);
                //    GameManager.Instance.HeartHighAnimation(3);
                //    for (int i = 2; i < 5; i++)
                //    {
                //        GameManager.Instance.hiders[2].transform.GetChild(i).GetChild(1).gameObject.SetActive(false);
                //    }
                //}
                //else
                //{
                //    //GameManager.Instance.hiders[2].transform.GetChild(2).GetComponent<AudioSource>().volume = 0.5f;
                //}
                hider3Pulse = time;
            }
        }

            
        //Debug.Log(hearRate[0]);
        //Debug.Log(hearRate[1]);
        //Debug.Log(hearRate[2]);
    }

}
