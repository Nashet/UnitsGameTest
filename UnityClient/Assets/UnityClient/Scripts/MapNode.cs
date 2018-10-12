using Nashet.UnitGame;
using Nashet.UnitSelection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Nashet.NTClient
{
    /// <summary>
    /// Represents place where unit can be
    /// </summary>
    public class MapNode : MonoBehaviour, IMovementTarget
    {
        public Position Position { get; protected set; }

        protected void Start()
        {
         
        }
        public void Set(Position position)
        {
            Position = position;
        }
    }
}
