using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tween
{
    public static IEnumerator ShakeGameObject(GameObject scaleGO, float scaleAmount, float timeScale, LeanTweenType inType, LeanTweenType outType, float waitMiddle) { 
        float newTime = timeScale / 2;
        Vector2 localScale = new Vector2(scaleGO.transform.localScale.x, scaleGO.transform.localScale.y);
        Vector2 scaleVector = new Vector2(scaleGO.transform.localScale.x + scaleAmount, scaleGO.transform.localScale.y + scaleAmount);

        LeanTween.scale(scaleGO, scaleVector, newTime).setEase(inType);
        yield return new WaitForSeconds(newTime + waitMiddle + 0.001f);
        LeanTween.scale(scaleGO, localScale, newTime).setEase(outType);
    }

    public static IEnumerator MoveGameObjectToandBack(GameObject moveGO, Vector2 moveAmount, float timeScale, LeanTweenType inType, LeanTweenType outType, float waitMiddle)
    {
        float newTime = timeScale / 2;
        Vector2 originalPos = moveGO.transform.position;
        LeanTween.move(moveGO, moveAmount, newTime).setEase(inType);
        yield return new WaitForSeconds(newTime + waitMiddle + 0.001f);
        LeanTween.move(moveGO, originalPos, newTime).setEase(outType);
    }

    public static IEnumerator RotateGameObject(GameObject moveGO, Vector3 axis, float rotate, float timeScale, LeanTweenType inType, LeanTweenType outType, float waitMiddle)
    {
        float newTime = timeScale / 2;
        Vector2 originalPos = moveGO.transform.position;
        LeanTween.rotateAround(moveGO, axis, rotate, newTime).setEase(inType);
        yield return new WaitForSeconds(newTime + waitMiddle + 0.001f);
        LeanTween.rotateAround(moveGO, axis, -rotate, newTime).setEase(inType);
    }
}
