using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/FieldMapper", fileName = "FieldMapper", order = 10)]
public class FieldMapper : ScriptableObject
{
    public MapperInfo[] infos;

    [System.Serializable]
    public struct MapperInfo
    {
        public string From;
        public string To;
    }

    public string Map(string type)
    {
        if (infos == null) return type;

        for (int i = 0; i < infos.Length; i++)
        {
            if (infos[i].From == type)
            {
                return infos[i].To;
            }
        }

        return type;
    }
}
