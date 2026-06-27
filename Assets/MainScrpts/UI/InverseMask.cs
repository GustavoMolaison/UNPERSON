using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[AddComponentMenu("UI/Inverse Mask")]
public class InverseMask : Image
{
    private Material _customMaterial;

    public override Material materialForRendering
    {
        get
        {
            if (_customMaterial == null)
            {
                // Tworzymy kopiê domyœlnego materia³u UI
                _customMaterial = new Material(base.materialForRendering);
            }

            // Odwracamy test stencila: renderuj tylko tam, gdzie NIE MA tego obiektu
            _customMaterial.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            _customMaterial.SetInt("_Stencil", 1);
            _customMaterial.SetInt("_StencilOp", (int)StencilOp.Keep);
            _customMaterial.SetInt("_StencilWriteMask", 1);
            _customMaterial.SetInt("_StencilReadMask", 1);

            return _customMaterial;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_customMaterial != null)
        {
            DestroyImmediate(_customMaterial);
        }
    }
}
