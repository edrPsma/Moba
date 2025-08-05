using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ModelViewer : MonoBehaviour
{
    public int TextureWidth => _textureWidth;
    [SerializeField] int _textureWidth;
    public int TextureHeight => _textureHeight;
    [SerializeField] int _textureHeight;
    [SerializeField] Bounds _modelBounds;
    [SerializeField] Vector3 _offset;
    [SerializeField] Vector3 _eulerAngles;
    RenderTexture _texture;
    RawImage _rawImage;
    [SerializeField] Camera _camera;
    [SerializeField] Transform _modelPlace;

    void Awake()
    {
        _rawImage = GetComponent<RawImage>();
        _texture = new RenderTexture(_textureWidth, _textureHeight, 24, RenderTextureFormat.Default);
        _texture.Create();
        _camera.targetTexture = _texture;
        _rawImage.texture = _texture;
    }

    [Button]
    public void ShowModel(GameObject model)
    {
        model.transform.SetParent(_modelPlace);
        model.transform.localPosition = _offset;
        model.transform.eulerAngles = _eulerAngles;

        Vector3 modelBottomCenter = new Vector3(_modelPlace.position.x, _modelPlace.position.y, _modelPlace.position.z) + _modelBounds.center;
        float maxDimension = Mathf.Max(_modelBounds.size.x, _modelBounds.size.y, _modelBounds.size.z);
        float distance = maxDimension / (2 * Mathf.Tan(Mathf.Deg2Rad * _camera.fieldOfView / 2));

        _camera.transform.position = modelBottomCenter - distance * _camera.transform.forward + new Vector3(0, maxDimension / 2, 0);
        _camera.transform.LookAt(modelBottomCenter + new Vector3(0, maxDimension / 2, 0));
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        if (_modelPlace == null)
        {
            _modelPlace = transform.Find<Transform>("modelPlace");
        }

        Gizmos.DrawWireCube(_modelPlace.position + Vector3.up * +_modelBounds.size.y * 0.5f + _modelBounds.center, _modelBounds.size);
    }

    void OnDestroy()
    {
        if (_texture != null)
        {
            _texture.Release();
            _texture = null;
        }
    }
}
