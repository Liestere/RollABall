﻿using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Text countText;
    public Text announcementText;
    public Text highScoreText;

    private Rigidbody rb;
    private int count;
    private int highScore;
    private GameObject cow;
    private FixedJoint cowJoint;

    private string filePath;
    private bool gamePaused;
    void Awake()
    {
        gamePaused = false;
        filePath = Application.persistentDataPath + "/save.txt";
        rb = GetComponent<Rigidbody>();
        cowJoint = GetComponent<FixedJoint>();
        cow = transform.Find("Cow").gameObject;
        File.Open(filePath, FileMode.OpenOrCreate).Dispose();
        string count = File.ReadAllText(filePath);

        if (string.IsNullOrEmpty(count))
        {
            highScore = 0;
        }
        else
        {
            try
            {
                highScore = int.Parse(count);

            }
            catch (Exception e)
            {
                Debug.LogError(e);
                File.Delete(filePath);
            }
        }


        highScoreText.text = "Highscore: " + highScore.ToString();
    }
    void Start()
    {
        count = 0;
        SetCountText();
        announcementText.text = "";
    }
    private void Save()
    {
        File.Open(filePath, FileMode.OpenOrCreate).Dispose();
        File.WriteAllText(filePath, highScore.ToString());
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            Debug.Log("Application Paused");
            Save();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Application Quit");
        Save();
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

    }
    private void Update()
    {
        if (gamePaused)
        {
            return;
        }
        if (rb.GetPointVelocity(Vector3.zero) == Vector3.zero)
        {
            cow.GetComponent<Animator>().SetBool("isMoving", false);
        }
        else
        {
            cow.GetComponent<Animator>().SetBool("isMoving", true);
        }

        if (cowJoint == null)
        {
            gamePaused = true;
            if (highScore < count)
            {
                announcementText.text = "You got a new Highscore! \n" + count;
                highScoreText.text = "New Highscore! " + count;
                highScore = count;
                Save();
            }
            else
            {
                announcementText.text = "You lose! \n Try Again in 5s!";
            }
            StartCoroutine(RestartGame());
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            ObjectPool.Instance.DespawnObject("pickup", other.gameObject.GetComponent<PooledObject>());
            count = count + 1;
            SetCountText();
        }
    }

    void SetCountText()
    {
        countText.text = "Score: " + count.ToString();
    }

    private IEnumerator RestartGame()
    {

        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}