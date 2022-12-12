using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TagFlagField
{
    [SerializeField]
    private List<string> _tags;
    public List<string> tags => _tags;

    TagFlagField ()
    {
        _tags = new List<string> ();
    }

    public void AddTag(string tag)
    {
        if (!_tags.Contains(tag))
            _tags = _tags.Append(tag).ToList();
    }
}
