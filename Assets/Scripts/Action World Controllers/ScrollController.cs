using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    // Transform of the object to scroll. This will usually be a Rigidbody transform.
    public new Transform transform;
    public bool isBackgroundSprite;

    private const float SCROLL_SPEED = 5f;
    private const string PATH_TO_BACKGROUND_PREFAB = "Prefabs/BG";
    private ActionWorldController playerAwc;
    private EnemyManager manager;

    private GameObject CANVAS;

    void Start()
    {
        CANVAS = GameObject.Find("Canvas");
        playerAwc = GameObject.Find("Canvas/Player")
                              .GetComponent<ActionWorldController>();
        manager = GameObject.Find("Canvas/Enemy Manager")
                            .GetComponent<EnemyManager>();
    }

    // Call this when the current sprite becomes invisible, so we have infinite scroll.
    private void CreateNewBackgroundSprite()
    {
        GameObject newBackgroundSprite = Resources.Load(PATH_TO_BACKGROUND_PREFAB) as GameObject;
        GameObject.Instantiate(newBackgroundSprite, CANVAS.transform);
    }

    private void OnBecameInvisible()
    {
        if (isBackgroundSprite)
            CreateNewBackgroundSprite();
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (manager.inTutorial)
            return;

        if (playerAwc.dead)
            this.enabled = false;

        Vector3 lp = transform.localPosition;
        lp.x -= SCROLL_SPEED;
        transform.localPosition = lp;
    }
}
