using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeTimer;
    private float startIntensity;

    void Awake()
    {
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void OnEnable()
    {
        CharacterEvents.characterDamaged += OnCharacterDamaged;
    }

    void OnDisable()
    {
        CharacterEvents.characterDamaged -= OnCharacterDamaged;
    }

    private void OnCharacterDamaged(GameObject character, int damage)
    {
        if (character.CompareTag("Player"))
        {
            Shake(3f, 0.3f);
        }
        else if (character.CompareTag("Enemy"))
        {
            Shake(1f, 0.15f);
        }
    }

    public void Shake(float intensity, float time)
    {
        noise.m_AmplitudeGain = intensity;
        startIntensity = intensity;
        shakeTimer = time;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            noise.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0, 1 - (shakeTimer / Mathf.Max(shakeTimer, 0.01f)));
            if (shakeTimer <= 0) noise.m_AmplitudeGain = 0;
        }
    }
}
