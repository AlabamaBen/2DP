using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineStoryboard cinemachineStoryboard;
    private float shakeTimer;
    private IEnumerator coroutine;


    public void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineStoryboard = cinemachineVirtualCamera.GetComponent<CinemachineStoryboard>();
        cinemachineStoryboard.m_Alpha = 0f;
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
        }
    }

    public void FadeIn()
    {
        coroutine = FadeInRoutine(0.01f);
        StartCoroutine(coroutine);
    }
    public void FadeOut()
    {
        coroutine = FadeOutRoutine(0.01f);
        StartCoroutine(coroutine);
    }

    private IEnumerator FadeInRoutine(float speed)
    {
        cinemachineStoryboard.m_Alpha = 1f;
        while (cinemachineStoryboard.m_Alpha > 0)
        {
            yield return new WaitForSeconds(0.032f);
            cinemachineStoryboard.m_Alpha -= speed;
        }
        cinemachineStoryboard.m_Alpha = 0f;
    }
    private IEnumerator FadeOutRoutine(float speed)
    {
        cinemachineStoryboard.m_Alpha = 0f;
        while (cinemachineStoryboard.m_Alpha < 1)
        {
            yield return new WaitForSeconds(0.032f);
            cinemachineStoryboard.m_Alpha += speed;
        }
        cinemachineStoryboard.m_Alpha = 1f;
    }
}
