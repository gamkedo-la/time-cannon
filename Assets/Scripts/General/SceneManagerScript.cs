using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript Instance { get; private set; } = null;

    [SerializeField] private FloatSO timePassed;
    [SerializeField] private IntSO totalNumberOfHits;
    [SerializeField] private IntListSO numberOfHitsList;
    [SerializeField] private IntListSO scoreFromEachShot;

    //[SerializeField] private GameObject RoundSummaryUIVR; //TODO
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void RestartScene()
    {
        this.ResetUIValues();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ResetUIValues()
    {
        timePassed.value = 0.0f;
        totalNumberOfHits.value = 0;
        numberOfHitsList.value.Clear();
        scoreFromEachShot.value.Clear();
    }
}
