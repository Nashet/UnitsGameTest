using UnityEngine;

namespace Nashet.UnitSelection
{
    /// <summary>
    /// Describes ability to select & deselect some GameObject.
    /// Supposed to be a component
    /// </summary>
    //public interface ISelector
    //{
    //    void Select(ISelectable someObject);
    //    void Deselect(ISelectable someObject);
    //    bool IsSelected(ISelectable someObject);
    //}
    public interface ISelectableObject
    {
        Vector3 Position { get; }
        void Select();
        void Deselect();
        bool IsSelected { get; }
    }
}