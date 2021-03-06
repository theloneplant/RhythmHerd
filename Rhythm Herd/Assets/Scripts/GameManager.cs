﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraController camera;
    [SerializeField] private Herd controller;
    [SerializeField] private GameObject soundPrefab;
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject barTarget;
    [SerializeField] private AudioSource music;
    [SerializeField] private float offset;
    [SerializeField] private int bpm;

    public delegate void BeatAction();
    public static event BeatAction OnBeat;

    private float startTime;
    private float previousBeat;
    private float beatInterval;

    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Use this for initialization
    private void Start()
    {
        controller = GetComponent<Herd>();
        startTime = Time.time;
        beatInterval = 60.0f / bpm;
        previousBeat = startTime - beatInterval;
        music.time = offset;
        music.Play();
    }

    // Update is called once per frame
    private void Update()
    {
        // Determine if a beat hit, notify everyone listening for the beat
        if (Time.time - previousBeat >= beatInterval)
        {
            playBeat();
            previousBeat = getNextBeatTime() - beatInterval;
        }
    }

    public void playBeat()
    {
        // Create UI bars
        var barOffset = new Vector3(1000, 0, 0);
        if (barPrefab)
        {
            Vector3 leftBarPosition = barTarget.transform.position - barOffset;
            Vector3 rightBarPosition = barTarget.transform.position + barOffset;
            GameObject leftBar = Instantiate(barPrefab, leftBarPosition, Quaternion.identity);
            GameObject rightBar = Instantiate(barPrefab, rightBarPosition, Quaternion.identity);
            leftBar.transform.SetParent(canvas.transform);
            rightBar.transform.SetParent(canvas.transform);
        }

        OnBeat?.Invoke();
    }

    public float getNextBeatTime(float numberOfBeatsAhead = 1)
    {
        float elapsed = Time.time - startTime;
        return elapsed + (beatInterval * numberOfBeatsAhead) - (elapsed % beatInterval);
    }

    public float getBeatScore()
    {
        float elapsed = Time.time - startTime;
        float beatPosition = elapsed % beatInterval;
        float center = beatInterval / 2.0f;
        return (Mathf.Abs(beatPosition - center) / beatInterval) * 2;
    }

    public static void PlaySound(AudioClip source, float pitch = 1f, bool randomPitch = false, float volume = 0.5f)
    {
        if (randomPitch)
        {
            pitch *= Random.Range(0.9f, 1.1f);
        }
        GameObject sound = Instantiate(instance.soundPrefab, Vector3.zero, Quaternion.identity);
        AudioSource audio = sound.GetComponent<AudioSource>();
        audio.clip = source;
        audio.volume = volume;
        audio.pitch = pitch;
        audio.Play();
    }


    public GameObject getBarTarget()
    {
        return barTarget;
    }
}
