using UnityEngine;

namespace Nashet.UnitSelection
{
    /// <summary>
    /// As component it gives ability to select & deselect some UI element with a selectionMaterial
    /// </summary>
    //public class UISelector : MonoBehaviour, ISelector
    //{
    //    [Header("Gives ability to select & deselect some Object with material")]
    //    [SerializeField] protected Material selectionMaterial;
    //    [SerializeField] protected Material defaultMaterial;

    //    /// <summary>
    //    /// Is forbidden since it's MonoBehaviour
    //    /// </summary>        
    //    protected UISelector() : base()
    //    {

    //    }

    //    /// <summary>
    //    /// Use this instead
    //    /// </summary>        
    //    public static UISelector AddTo(GameObject toWhom, Material selectionMaterial, Material defaultMaterial)
    //    {
    //        var added = toWhom.AddComponent<UISelector>();
    //        added.selectionMaterial = selectionMaterial;
    //        added.defaultMaterial = defaultMaterial;
    //        return added;
    //    }


    //    public virtual void Deselect(ISelectable someObject)
    //    {
    //        var image = (someObject as MonoBehaviour).GetComponent<Renderer>();
    //        if (image == null)
    //        //if there is no render in selected object, find one in childes
    //        {
    //            var children = (someObject as MonoBehaviour).GetComponentsInChildren<Renderer>();
    //            foreach (var item in children)
    //            {
    //                item.material = defaultMaterial;
    //            }
    //        }
    //        else
    //        {
    //            image.material = defaultMaterial;
    //        }
    //    }

    //    public bool IsSelected(ISelectable someObject)
    //    {
    //        var image = (someObject as MonoBehaviour).GetComponent<Renderer>();
    //        if (image == null)
    //        //if there is no render in selected object, find one in childes
    //        {
    //            var children = (someObject as MonoBehaviour).GetComponentsInChildren<Renderer>();
    //            foreach (var item in children)
    //            {
    //                if (item.material == selectionMaterial)
    //                    return true;
    //            }
    //            return false;
    //        }
    //        else
    //        {
    //            return image.material == selectionMaterial;
    //        }
    //    }

    //    public virtual void Select(ISelectable someObject)
    //    {
    //        var image = (someObject as MonoBehaviour).GetComponent<Renderer>();
    //        if (image == null)
    //        //if there is no render in selected object, find one in childes
    //        {
    //            var children = (someObject as MonoBehaviour).GetComponentsInChildren<Renderer>();
    //            foreach (var item in children)
    //            {
    //                item.material = selectionMaterial;
    //            }
    //        }
    //        else
    //        {
    //            image.material = selectionMaterial;
    //        }
    //    }
    //}
}