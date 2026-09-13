
using System;
using UnityEngine.Localization.Metadata;

[Metadata(AllowedTypes = MetadataType.SharedStringTableEntry)]
[Serializable]
public class SlowedMetaData : IMetadata
{
    public int isSlowed;
}