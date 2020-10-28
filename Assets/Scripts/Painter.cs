using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using Player;
using UnityEngine;

public class Painter : MonoBehaviour
{
    // definition for HSV
    // public float Hue = 0;
    public const float Sat = 1;
    public const float Val = 1;

    [Range(0, 10000)]
    public float threshold;
    
    public int resolution = 512;
    private Texture2D _map;
    
    [Range(0, 10)]
    public int closePixels = 10;
    
    private uint[,] _intensityMask;
    private Material _material;
    
    public static float MaxX = 25;
    public static float MinX = -25;
    public static float MaxY = 25;
    public static float MinY = -25;
    
    private static float W = (MaxX - MinX);
    private static float H = (MaxY - MinY);
    private static float halfW = W/2;
    private static float halfH = H/2;
    private static float resW;
    private static float resH;

    public List<MonoBehaviour> cPlayers = new List<MonoBehaviour>();
    
    private void Start()
    {
        resW = resolution / W;
        resH = resolution / H;
        
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
        UpdateIntensityMapValues(cPlayers[0]);

        // { 
        // PaintUsingPosition(cPlayers[0]);
        // }
    }

    private void LateUpdate()
    {
        // if (!Input.GetKeyDown(KeyCode.Space)) return;
        IntensityMapToTexture2D();
        // SaveTextureAsPNG(_map, "Assets/Textures/Heatmap/Image.png");
    }

    public static void SaveTextureAsPNG(Texture2D texture, string fullPath)
    {
        var bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath, bytes);
        Debug.Log(bytes.Length / 1024  + "Kb was saved as: " + fullPath);
    }
    
    private void UpdateIntensityMapValues(Component player)
    {
        var pos = player.transform.position;
        var x = Mathf.RoundToInt(Mathf.Abs(pos.x - halfW) * resW);
        var z = Mathf.RoundToInt(Mathf.Abs(pos.z - halfH) * resH);

        // _intensityMask[x, z] += 1;
        // ++_intensityMask[x, z];
        
        var L = x - closePixels < 0 ? 0 : x - closePixels;
        var R = x + closePixels > _map.width ? _map.width : x + closePixels;
        var U = z - closePixels < 0 ? 0 : z - closePixels;
        var B = z - closePixels > _map.height ? _map.height : z + closePixels;
        
        for (var i = L; i < R; ++i)
        {
            for (var j = U; j < B; ++j)
            {
                var frac = (uint) Mathf.FloorToInt(closePixels / ((Mathf.Abs(i - x) + Mathf.Abs(j - z)) * 0.5f + 3));
                _intensityMask[i, j] += frac;
            }
        }
    }
    
    private void IntensityMapToTexture2D()
    {
        if (threshold < 1)
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

        // var invMax = threshold > 1 ? 1/threshold : 1;
        var invMax = 1.0f / threshold;
        
        for (var i = 0; i < _map.height; ++i)
        {
            for (var j = 0; j < _map.width; ++j)
            {
                var val = Mathf.Clamp(_intensityMask[i, j] * invMax, 0, 1);
                // var grayscale = val == 0 ?
                //     _emptyColor : new Color(val, val, val, 1);
                // _map.SetPixel(i, j, grayscale);
                
                var rgb = val == 0 ? Color.clear : HSVToRGB2(val);
                
                _map.SetPixel(i, j, rgb);
            }
        }

        _map.Apply();
    }
    
    private Color HSVToRGB2(float intensity)
    {
        var hue = 6 - intensity * 6;
        
        var x = (1 - Mathf.Abs(hue % 2 - 1));

        var rgb = Color.clear;

        if (hue == 0f) {
            rgb = new Color(1, 1, 1, 1);
        } else if (hue <= 1) {
            rgb = new Color(1, x, 0, 1);
        } else if (hue <= 2) {
            rgb = new Color(x, 1, 0, 1);
        } else if (hue <= 6) {
            rgb = new Color(0, 1, x, 1);
        } else if (hue <= 6) {
            rgb = new Color(0, x, 1, 1);
        }
        // else if (hue <= 6) {
        //     rgb = new Color(x, 0, 1, 1);
        // }

        return rgb;
    }

    // private Color HSVToRGB(float intensity)
    // {
    //     var hue = 360 * intensity;
    //     var r = ToColorCh(5, hue);
    //     var g = ToColorCh(3, hue);
    //     var b = ToColorCh(1, hue);
    //     Debug.Log(hue + " " + r + " " + g + " " + b);
    //     return new Color(r, g, b, 1);
    // }
    //
    // private int K(int n, float hue)
    // {
    //     return Mathf.RoundToInt(n + hue / 60) % 6;
    // }
    //
    // private int ToColorCh(int n, float hue)
    // {
    //     var k = K(n, hue);
    //     return (1 - Mathf.Max(0, Mathf.Min(k, 4 - k, 1)));
    // }
    
    // private void PaintUsingPosition(Component player)
    // {
    //     var pos = player.transform.position;
    //     var x = (int) (Mathf.Abs(pos.x - halfW) * resW);
    //     var z = (int) (Mathf.Abs(pos.z - halfH) * resH);
    //
    //     var color = _map.GetPixel(x, z);
    //     color += new Color(0.001f, 0.001f, 0.001f);
    //     _map.SetPixel(x, z, color);
    //     
    //     _map.Apply();
    // }
}