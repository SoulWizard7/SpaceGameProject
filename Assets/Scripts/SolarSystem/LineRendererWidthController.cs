using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererWidthController : MonoBehaviour
{
    
    private GameObject _mainCamPos;
    private float _lineWidth;
    private LineRenderer _lineRenderer;
    

    public void Settings(float width, GameObject mainCameraPos, LineRenderer lineRenderer)
    {
        _lineRenderer = lineRenderer;
        _mainCamPos = mainCameraPos;
        _lineWidth = width;
    }
    
    void Update()
    {
        if (_lineRenderer)
        {
            float newWidth = _lineWidth * _mainCamPos.transform.position.y / 100;
            
            _lineRenderer.startWidth = newWidth;
            _lineRenderer.endWidth = newWidth;
        }
    }
}
