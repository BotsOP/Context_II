using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class AcidCloudSpawner : MonoBehaviour
{
    //Should be from 0 to 1
    [SerializeField] private float spawnTimerGood = 50;
    [SerializeField] private float spawnTimerBad = 30;
    [SerializeField] private List<PaintSpawner> paintSpawners;
    [SerializeField] private float boycottProgression = 0.5f;
    private float startTime = 0;

    private void Update()
    {
        float timeTillNextSpawn = Mathf.Lerp(spawnTimerBad, spawnTimerGood, Mathf.Clamp01(boycottProgression));
        if (Time.timeSinceLevelLoad > startTime + timeTillNextSpawn )
        {
            startTime = Time.timeSinceLevelLoad;
            paintSpawners[Random.Range(0, paintSpawners.Count - 1)].parentCloud.SetActive(true);
        }
    }

    public void AddBoycottProgress(float boycottValue)
    {
        boycottProgression += boycottValue;
        if (boycottProgression > 1 && Time.timeSinceLevelLoad > 30 && PaintTargetManager.GetWorldTaintedness() < 0.1f)
        {
            SceneManager.LoadScene("GoodEndScene");
        }
        if (boycottProgression < 1 && Time.timeSinceLevelLoad > 30 && PaintTargetManager.GetWorldTaintedness() > 0.9f)
        {
            SceneManager.LoadScene("BadEndScene");
        }
        boycottProgression = Mathf.Clamp01(boycottProgression);
    }

}
