using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoopManager : MonoBehaviour
{
    public List<Image> poopImages;
    private Stack<Image> _imageStack;
    private Stack<Image> _imageStackUsed;
    
    // Start is called before the first frame update
    void Start()
    {
        _imageStack = new Stack<Image>(poopImages.Count);
        _imageStackUsed = new Stack<Image>(poopImages.Count);
        
        foreach (Image poopImage in poopImages)
        {
            _imageStack.Push(poopImage);
        }
    }

    public void UsePoop()
    {
        var img = _imageStack.Pop();
        img.gameObject.SetActive(false);
        
        _imageStackUsed.Push(img);
    }

    public void ReloadPoop()
    {
        var img = _imageStackUsed.Pop();
        img.gameObject.SetActive(true);
        
        _imageStack.Push(img);
    }

    public void ReloadAllPoop()
    {
        foreach (Image image in _imageStackUsed)
        {
            image.gameObject.SetActive(true);
            _imageStack.Push(image);
        }
    }

    public Image GetLastUsedPoop()
    {
        if (_imageStack.Count == 0)
            return null;
        return _imageStack.Pop();
    }
}
