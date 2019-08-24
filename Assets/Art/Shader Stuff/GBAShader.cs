﻿using UnityEngine;

[ExecuteInEditMode]
public class GBAShader : MonoBehaviour
{
    public Material gameboyMaterial;
    public Material identityMaterial;

    private RenderTexture _downscaledRenderTexture;

    private void OnEnable()
    {
        var camera = GetComponent<Camera>();
        int height = 360;
        int width = Mathf.RoundToInt(camera.aspect * height);
        _downscaledRenderTexture = new RenderTexture(width, height, 16);
        _downscaledRenderTexture.filterMode = FilterMode.Point;
    }

    private void OnDisable()
    {
        DestroyImmediate(_downscaledRenderTexture);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, _downscaledRenderTexture, gameboyMaterial);
        Graphics.Blit(_downscaledRenderTexture, dst, identityMaterial);
    }
}