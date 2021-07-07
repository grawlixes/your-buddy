using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyController> enemiesOnScreen;
    public float timeToSpawnSkeleton = 3f;
    public float timeToSpawnWizard = 7f;

    private float timeLeftToSpawnSkeleton;
    private float timeLeftToSpawnWizard;
    private bool wizardOnScreen;

    private const string SKELETON_PREFAB_NAME = "Prefabs/Skeleton";
    private const string WIZARD_PREFAB_NAME = "Prefabs/Wizard";
    private Vector3 WIZARD_START_POSITION = new Vector3(700, 250, -1);
    private Vector3 SKELETON_START_POSITION = new Vector3(1100, -517, -1);

    private void Start()
    {
        timeLeftToSpawnSkeleton = timeToSpawnSkeleton;
        timeLeftToSpawnWizard = timeToSpawnWizard;
        wizardOnScreen = true;
    }

    // Kills all enemies on screen. 
    public void KillAllEnemies(string animEffect)
    {
        wizardOnScreen = false;
        while (enemiesOnScreen.Count > 0) {
            EnemyController enemy = enemiesOnScreen[0];
            if (animEffect != null)
                enemy.PlayAnimation(animEffect, true);
            enemy.Die();
        }

        enemiesOnScreen.Clear();
    }

    private void SpawnSkeleton()
    {
        GameObject skeleton = Resources.Load(SKELETON_PREFAB_NAME) as GameObject;
        GameObject newSkeleton = GameObject.Instantiate(skeleton, gameObject.transform);
        newSkeleton.transform.localPosition = SKELETON_START_POSITION;

        enemiesOnScreen.Add(newSkeleton.GetComponent<SkeletonController>());
    }

    private void SpawnWizard()
    {
        wizardOnScreen = true;

        GameObject wizard = Resources.Load(WIZARD_PREFAB_NAME) as GameObject;
        GameObject newWizard = GameObject.Instantiate(wizard, gameObject.transform);
        newWizard.transform.localPosition = WIZARD_START_POSITION;

        enemiesOnScreen.Add(newWizard.GetComponent<WizardController>());
    }

    // Update is called once per frame
    void Update()
    {
        timeLeftToSpawnSkeleton -= Time.deltaTime;
        if (timeLeftToSpawnSkeleton <= 0)
        {
            SpawnSkeleton();
            timeToSpawnSkeleton -= .1f;
            timeLeftToSpawnSkeleton = timeToSpawnSkeleton;
        }

        if (!wizardOnScreen)
        {
            timeLeftToSpawnWizard -= Time.deltaTime;
            if (timeLeftToSpawnWizard <= 0)
            {
                SpawnWizard();
                timeToSpawnWizard -= .1f;
                timeLeftToSpawnWizard = timeToSpawnWizard;
            }
        }
    }
}
