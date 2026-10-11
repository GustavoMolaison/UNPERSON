using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Zastêpuje GraphicRaycaster na wirtualnym monitorze
public class VirtualScreenRaycasterOnly : GraphicRaycaster
{
    // Flaga: zezwalaj na raycast TYLKO wtedy, gdy pyta o to nasz skrypt monitora
    public static bool AllowVirtualRaycast = false;

    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
    {
        // Jeœli to zwyk³e klikniêcie z gry – CA£KOWICIE IGNORUJ i nie blokuj dialogów!
        if (!AllowVirtualRaycast)
        {
            return;
        }

        // Jeœli to nasz skrypt – przepuœæ normalny raycast do monitora
        base.Raycast(eventData, resultAppendList);
    }
}