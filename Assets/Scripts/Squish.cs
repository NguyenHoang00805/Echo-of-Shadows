using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squish : MonoBehaviour
{
    PlayerController playerController;

    [Header("Squish Parameters")]
    public float squishAmountX = 1.2f;
    public float squishAmountY = 0.8f; 
    public float squishDuration = 0.15f;

    private Vector3 originalScale;
    private int isFacingRight;
    private Coroutine squishCoroutine;

    void Awake()
    {
        originalScale = transform.localScale;
        playerController = GetComponent<PlayerController>();
    }

    public void ApplySquish()
    {
        if (squishCoroutine != null)
        {
            StopCoroutine(squishCoroutine);
        }
        squishCoroutine = StartCoroutine(SquishAnimCo());
    }

    public IEnumerator SquishAnimCo()
    {
        // 1 for right, -1 for left
        float direction = playerController.isFacingRight ? 1f : -1f;

        Vector3 startScale = new Vector3(originalScale.x * direction, originalScale.y, originalScale.z);
        Vector3 targetSquishScale = new Vector3(originalScale.x * squishAmountX * direction, originalScale.y * squishAmountY, originalScale.z);

        // Squish
        float timer = 0f;
        float halfDuration = squishDuration / 2f;

        while (timer < halfDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetSquishScale, timer / halfDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetSquishScale;

        // Return to normal
        timer = 0f;
        while (timer < halfDuration)
        {
            transform.localScale = Vector3.Lerp(targetSquishScale, startScale, timer / halfDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        float finalDirection = playerController.isFacingRight ? 1f : -1f;
        Vector3 finalScale = new Vector3(originalScale.x * finalDirection, originalScale.y, originalScale.z);

        //Check 1 last time
        transform.localScale = finalScale;
        squishCoroutine = null;
    }
}
