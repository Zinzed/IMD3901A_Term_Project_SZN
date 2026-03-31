using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class progressBar : MonoBehaviour
{
    public SceneLoader sceneLoader;

    public int maxProgress = 30;

    private int targetProgress;

    public Slider slider;

    void Start()
    {
        slider.maxValue = maxProgress;
        targetProgress = 0;
    }
    public void UpdateProgress(int amount)
    {
        targetProgress = Mathf.Clamp(targetProgress + amount, 0, maxProgress);


        StartCoroutine(SmoothUpdate());
    }

    IEnumerator SmoothUpdate()
    {
        float startValue = slider.value;
        float elapsed = 0;

        while (elapsed < 0.3f)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Lerp(startValue, targetProgress, elapsed / 0.3f);
            yield return null;
        }

        slider.value = targetProgress;

        if (slider.value >= maxProgress)
            Win();
    }

    void Win()
    {
        sceneLoader.next("WinScene");
        Debug.Log($"you won!");
    }

    public void setMaxProgress(int progress)
    {
        slider.value = progress;
    }
}
