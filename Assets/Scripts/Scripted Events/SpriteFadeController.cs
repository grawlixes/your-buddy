using UnityEngine;
using UnityEngine.SceneManagement;


// controls fade in and out (to and from black) for a sprite.
public class SpriteFadeController : MonoBehaviour
{
    public int numFramesPerFade;
    public bool fadeInAtSceneStart;
    public int sceneToLoadAfterFadeOut;

    // 0 if not doing anything, 1 if fading in, 2 if fading out
    // this is public so other components can see whether it's fading in or out
    public int fadeIndex = 0;

    private int framesToNextFade;
    private byte scrolled = 0;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        framesToNextFade = numFramesPerFade;

        // We expect the fade object to be transparent (0 alpha) at the start, so I can see stuff in the inspector.
        // So, if we want to fade in at the start, make it completely visible (1 alpha) and start fading in.
        if (fadeInAtSceneStart)
        {
            Color32 originalColor = sprite.color;
            sprite.color = new Color32(originalColor.r, originalColor.g, originalColor.b, 255);
            fadeIndex = 1;
        }
    }

    public void StartFadingIn()
    {
        fadeIndex = 1;
    }

    public void StartFadingOut()
    {
        fadeIndex = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIndex == 0)
            return;

        framesToNextFade -= 1;

        // fading in
        if (fadeIndex == 1)
        {
            Color32 color = sprite.color;

            if (framesToNextFade == 0)
            {
                scrolled++;
                color.a = (byte)(255 - scrolled);
                sprite.color = color;
                framesToNextFade = numFramesPerFade;

                if (color.a == 0)
                {
                    fadeIndex = 0;
                    scrolled = 0;
                }
            }
        }
        // fading out
        else if (fadeIndex == 2)
        {
            Color32 color = sprite.color;

            if (framesToNextFade == 0)
            {
                color.a += 1;
                sprite.color = color;
                framesToNextFade = numFramesPerFade;

                if (color.a == 255)
                    if (sceneToLoadAfterFadeOut != -1)
                        SceneManager.LoadScene(sceneToLoadAfterFadeOut);
                    else
                        Destroy(this.gameObject);
            }
        }
    }
}
