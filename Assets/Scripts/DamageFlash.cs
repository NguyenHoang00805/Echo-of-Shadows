using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [ColorUsage(true,true)]
    [SerializeField] private Color _flashColor;
    [SerializeField] private float flashTime = .25f;
    [SerializeField] private AnimationCurve _flashCurve;

    //private SpriteRenderer[] _spriteRenderer;
    //private Material[] _materials;
    private SpriteRenderer _spriteRenderer;
    private Material _materials;

    private Coroutine _coroutine; 

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _materials = GetComponent<Material>();
        if (_spriteRenderer != null)
        {
            _materials = _spriteRenderer.material; // Use SpriteRenderer.material to get the instance
        }
        else
        {
            Debug.LogError("Renderer component not found on this GameObject!");
        }
        //Init();
    }

    //private void Init()
    //{
    //    _materials = new Material[_spriteRenderer.Length];
    //    for (int i = 0; i < _spriteRenderer.Length; i++)
    //    {
    //        _materials[i] = _spriteRenderer[i].material;
    //    }
    //} 

    public void CallDamageFlash() 
    {
        _coroutine = StartCoroutine(DamageFlasher()); 
    }

    private IEnumerator DamageFlasher()
    {
        //set color
        SetFlashColor();
        //lerp the flash amount
        float currentFlashAmount = 0f;
        float elaspedTime = 0f; 
        while (elaspedTime < flashTime)
        {
            elaspedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f,_flashCurve.Evaluate(elaspedTime),(elaspedTime/flashTime));
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }

    }

    private void SetFlashColor()
    {
        if (_materials != null)
        {
            _materials.SetColor("_FlashColor", _flashColor);
        }
        else
            Debug.LogError("_material is null in SetFlashColor");
    }
    //for (int i = 0; i < _materials.Length; i++)
        //{
        //    _materials[i].SetColor("_FlashColor", _flashColor);
        //}

    private void SetFlashAmount(float amount)
    {
        //for (int i = 0; i < _materials.Length; i++)
        //{
        //    _materials[i].SetFloat("_FlashAmount", amount);
        //}
        if (_materials != null)
        {
            _materials.SetFloat("_FlashAmount", amount);
        }
        else
            Debug.LogError("_material is null in SetFlashAmount");

    }
}
