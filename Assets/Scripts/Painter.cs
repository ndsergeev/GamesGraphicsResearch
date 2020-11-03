using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Player;
using UnityEngine;

public class Painter : MonoBehaviour
{
    // definition for HSV
    public const float Sat = 1;
    public const float Val = 1;

    public bool renderTexture;
    
    public bool autoThreshold;
    [Range(2, 10000)]
    public float threshold;
    
    public int resolution = 512;
    private Texture2D _map;
    
    [Range(0, 10)]
    public int closePixels = 10;
    
    private uint[,] _intensityMask;
    private Material _material;

    // Right now these are hardcoded, but I can use local scale from GameObjects and translate them to positions
    private const float MaxX = 25;
    private const float MinX = -25;
    private const float MaxY = 25;
    private const float MinY = -25;

    private const float W = (MaxX - MinX);
    private const float H = (MaxY - MinY);
    private const float HalfW = W / 2;
    private const float HalfH = H / 2;
    
    private static float _resW;
    private static float _resH;

    public List<MonoBehaviour> cPlayers = new List<MonoBehaviour>();
    
    private Coroutine _saveTextureCoroutine;

    private void OnDisable()
    {
        if (_saveTextureCoroutine == null) return;
        StopCoroutine(_saveTextureCoroutine);
        Debug.Log("Coroutine was interrupted");
    }

    private void Start()
    {
        _resW = resolution / W;
        _resH = resolution / H;
        
        _material = GetComponent<Renderer>().material;
        
        _intensityMask = new uint[resolution, resolution];
        CreateEmptyBaseColorTexture(resolution, resolution);
        _material.mainTexture = _map;

        foreach (var player in cPlayers)
        {
            var playerTr = player.transform;
            Instantiate(GameManager.Instance.predatorCharacters[0],
                playerTr.position, 
                playerTr.rotation,
                playerTr);
        }
    }

    private void CreateEmptyBaseColorTexture(int w, int h)
    {
        _map = new Texture2D(w, h);
        
        for (var i = 0; i < h; ++i)
        {
            for (var j = 0; j < w; ++j)
            {
                _map.SetPixel(i, j, Color.clear);
            }
        }
        
        _map.Apply();
    }

    private void FixedUpdate()
    {
        foreach (var player in cPlayers)
        {
            UpdateIntensityMapValues(player);
        }
    }

    private void Update()
    {
        if (renderTexture)
        {
            IntensityMapToTexture2D();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            IntensityMapToTexture2D();
            if (_saveTextureCoroutine != null)
            {
                StopCoroutine(_saveTextureCoroutine);
            }
            _saveTextureCoroutine = StartCoroutine(SaveTextureAsPNG(_map,
                "Assets/Textures/Heatmap/",
                threshold)
            );
        }
    }

    private static IEnumerator SaveTextureAsPNG(Texture2D texture, string fullPath, float value)
    {
        var bytes = texture.EncodeToPNG();
        if (!fullPath.EndsWith("/"))
        {
            fullPath += "/";
        }

        var filePath = fullPath + $"Heatmap_threshold_{value}.png";
        System.IO.File.WriteAllBytes(filePath, bytes);
        Debug.Log(bytes.Length / 1024  + "Kb was saved as: " + filePath);
    
        yield return null;
    }

    private void UpdateIntensityMapValues(Component player)
    {
        var pos = player.transform.position;
        var x = Mathf.RoundToInt(Mathf.Abs(pos.x - HalfW) * _resW);
        var z = Mathf.RoundToInt(Mathf.Abs(pos.z - HalfH) * _resH);

        var left  = x - closePixels < 0 ? 0 : x - closePixels;
        var right = x + closePixels > _map.width ? _map.width : x + closePixels;
        var top   = z - closePixels < 0 ? 0 : z - closePixels;
        var bottom= z - closePixels > _map.height ? _map.height : z + closePixels;

        for (var i = left; i < right; ++i)
        {
            for (var j = top; j < bottom; ++j)
            {
                var frac = (uint) Mathf.FloorToInt(closePixels / ((Mathf.Abs(i - x) + Mathf.Abs(j - z)) * 0.5f + 2.5f));
                _intensityMask[i, j] += frac;
            }
        }
    }
    
    private void IntensityMapToTexture2D()
    {
        if (autoThreshold)
        {
            for (var i = 0; i < _map.height; ++i)
            {
                for (var j = 0; j < _map.width; ++j)
                {
                    if (_intensityMask[i, j] > threshold)
                    {
                        threshold = _intensityMask[i, j];
                    }
                }
            }
        }

        var invMax = 1.0f / threshold;
        
        for (var i = 0; i < _map.height; ++i)
        {
            for (var j = 0; j < _map.width; ++j)
            {
                var val = Mathf.Clamp(_intensityMask[i, j] * invMax, 0, 1);
                var rgb = val == 0 ?
                    Color.clear : HSVToRGB(val);
                
                _map.SetPixel(i, j, rgb);
            }
        }

        _map.Apply();
    }

    private static Color HSVToRGB(float intensity)
    {
        var hue = 4 - intensity * 4;
        
        var x = 1 - Mathf.Abs(hue % 2 - 1);
    
        var rgb = Color.clear;
    
        if (hue == 0f) {
            rgb = new Color(1, 1, 1, 1);
        } else if (hue <= 1) {
            rgb = new Color(1, x, 0, 1);
        } else if (hue <= 2) {
            rgb = new Color(x, 1, 0, 1);
        } else if (hue <= 3) {
            rgb = new Color(0, 1, x, 1);
        } else if (hue <= 4) {
            rgb = new Color(0, x, 1, 1);
        }
    
        return rgb;
    }
}