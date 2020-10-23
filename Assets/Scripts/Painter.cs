using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

public class Painter : MonoBehaviour
{
    public int resolution = 512;
    private Texture2D _map;
    public float brushSize = 10;
    private uint[,] _intensityMask;
    private Material _material;
    
    public static float MaxX = 25;
    public static float MinX = -25;
    public static float MaxY = 25;
    public static float MinY = -25;
    
    private static float W = (MaxX - MinX);
    private static float H = (MaxY - MinY);

    public List<MonoBehaviour> cPlayers = new List<MonoBehaviour>();
    
    private void Start()
    {
        _material = GetComponent<Renderer>().material;
        
        _intensityMask = new uint[resolution, resolution];
        CreateEmptyBaseColorTexture(resolution, resolution);
        _material.mainTexture = _map;

        foreach (var player in cPlayers)
        {
            Instantiate(GameManager.Instance.predatorCharacters[0],
                player.transform.position, 
                player.transform.rotation,
                player.transform);
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

    private float _timer = 0f;
    private void FixedUpdate()
    {
        // {
        //     UpdateHeatMapValues(cPlayers[0]);
        //
        //     _timer += Time.fixedDeltaTime;
        //     
        //     if (!(_timer > 2)) return;
        //     _timer = 0f;
        //     StartCoroutine(nameof(IntensityMapToTexture2D));
        // }
        
        // { 
        PaintUsingPosition(cPlayers[0]);
        // }
    }

    private void UpdateHeatMapValues(Component player)
    {
        var pos = player.transform.position;
        var x = (int) (Mathf.Abs(pos.x - W / 2) * resolution / W);
        var z = (int) (Mathf.Abs(pos.z - H / 2) * resolution / H);

        ++_intensityMask[x, z];
    }
    
    private IEnumerator IntensityMapToTexture2D()
    {
        var max = 0f;
        for (var i = 0; i < _map.height; ++i)
        {
            for (var j = 0; j < _map.width; ++j)
            {
                if (_intensityMask[i, j] > max)
                {
                    max = _intensityMask[i, j];
                }
            }
        }

        var invMax = 1 / max;
        
        for (var i = 0; i < _map.height; ++i)
        {
            for (var j = 0; j < _map.width; ++j)
            {
                var val = _intensityMask[i, j] * invMax;
                var grayscale = new Color(val, val, val, 1);
                _map.SetPixel(i, j, grayscale);
            }
        }

        _map.Apply();
        
        yield return null;
    }

    // private void HeatMapToTexture2D()
    // {
    //     var max = 0f;
    //     for (var i = 0; i < _map.height; ++i)
    //     {
    //         for (var j = 0; j < _map.width; ++j)
    //         {
    //             if (_intensityMask[i, j] > max)
    //             {
    //                 max = _intensityMask[i, j];
    //             }
    //         }
    //     }
    //
    //     var coeff = 1 / 6;
    //     var blue = max * coeff;
    //     var cyan = max * 2 * coeff;
    //     var green = max * 3 * coeff;
    //     var yellow = max * 4 * coeff;
    //     var red = max * 5 * coeff;
    //     Color newColor;
    //     
    //     for (var i = 0; i < _map.height; ++i)
    //     {
    //         for (var j = 0; j < _map.width; ++j)
    //         {
    //             var val = _intensityMask[i, j] / max;
    //             if (val == 0f)
    //             {
    //                 continue;
    //             }
    //             else if (val < blue)
    //             {
    //                 
    //             }
    //             _map.SetPixel(i, j, Color.black);
    //         }
    //     }
    //     _map.Apply();
    // }

    private void PaintUsingPosition(Component player)
    {
        var pos = player.transform.position;
        var x = (int) (Mathf.Abs(pos.x - W / 2) * resolution / W);
        var z = (int) (Mathf.Abs(pos.z - H / 2) * resolution / H);

        var color = _map.GetPixel(x, z);
        color += new Color(0.001f, 0.001f, 0.001f);
        _map.SetPixel(x, z, color);
        
        _map.Apply();
    }
}