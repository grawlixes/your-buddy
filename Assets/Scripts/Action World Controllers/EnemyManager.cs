using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<EnemyController> enemiesOnScreen;
    public float timeToSpawnSkeleton = 4f;
    public float timeToSpawnWizard = 5f;
    public bool inTutorial = false;
    public bool movingTutorialEnemies = false;

    private float timeLeftToSpawnSkeleton;
    private float timeLeftToSpawnWizard;
    private bool wizardOnScreen;

    private const string SKELETON_PREFAB_NAME = "Prefabs/Skeleton";
    private const string WIZARD_PREFAB_NAME = "Prefabs/Wizard";
    private Vector3 WIZARD_START_POSITION = new Vector3(700, 250, -1);
    private Vector3 SKELETON_START_POSITION = new Vector3(1100, -517, -1);
    private float WIZARD_TUTORIAL_X = 600f;
    private float SKELETON_TUTORIAL_X = 100f;

    private void Start()
    {
        timeLeftToSpawnSkeleton = timeToSpawnSkeleton;
        timeLeftToSpawnWizard = timeToSpawnWizard;
        wizardOnScreen = enemiesOnScreen.Count == 2;
    }

    public void GetTutorialEnemiesInPosition()
    {
        movingTutorialEnemies = true;
    }

    public void TutorialThunder()
    {
        WizardController wizard = (WizardController) enemiesOnScreen[0];
        StartCoroutine(wizard.Thunder(true));
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
        if (movingTutorialEnemies)
        {
            Transform wizard = enemiesOnScreen[0].GetComponent<Rigidbody2D>().transform;
            Transform skeleton = enemiesOnScreen[1].GetComponent<Rigidbody2D>().transform;

            int inPlace = 0;
            if (wizard.localPosition.x > WIZARD_TUTORIAL_X)
            {
                var lp = wizard.localPosition;
                lp.x -= 10f;
                wizard.localPosition = lp;
            }
            else
                inPlace++;

            if (skeleton.localPosition.x > SKELETON_TUTORIAL_X)
            {
                var lp = skeleton.localPosition;
                lp.x -= 8f;
                skeleton.localPosition = lp;
            }
            else
                inPlace++;

            if (inPlace == 2)
                movingTutorialEnemies = false;
        }
        else if (!inTutorial)
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
}
