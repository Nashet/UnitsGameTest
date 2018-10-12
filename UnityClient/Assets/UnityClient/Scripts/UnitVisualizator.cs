using Nashet.UnitsGameLogic;
using Nashet.UnitSelection;
using Nashet.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nashet.UnitsGameClient
{
    /// <summary>
    /// Visualizes units, updates visualization from server data
    /// </summary>
    public class UnitVisualizator : MonoBehaviour, ISelectableObject
    {
        [SerializeField] protected float lineRendererHeight = 0.4f;
        [SerializeField] protected Material selectionMaterial;
        [SerializeField] protected Material defaultMaterial;

        protected LineRenderer lineRenderer;
        protected new Renderer renderer;
        protected new Animation animation;

        public IUnitView serverUnit;

        public Vector3 Position { get { return transform.position; } }
        public bool IsSelected { get; protected set; }

        protected void Start()
        {
            renderer = GetComponent<Renderer>();
            lineRenderer = GetComponent<LineRenderer>();
            animation = GetComponent<Animation>();
        }

        public void Select()
        {
            renderer.material = selectionMaterial;
            IsSelected = true;
        }

        public void Deselect()
        {
            GetComponent<Renderer>().material = defaultMaterial;
            IsSelected = false;
        }

        /// <summary>
        /// Synchronizes, called from callback
        /// </summary>        
        internal void Synchronize(IUnitView item, MapVisualisator map)
        {
            serverUnit = item;
            transform.position = map.ScaleToWorldCoords(item.Position, map.XSize, map.YSize, map.MapScale);

            if (animation != null)
            {
                if (item.IsWalking)
                    animation.Play("UnitWalking");
                else
                    animation.Play("UnitStaying");
            }

            if (lineRenderer != null)
            {
                if (serverUnit.Path == null)//!IsSelected || 
                {
                    lineRenderer.positionCount = 0;
                }
                else
                {
                    var lineRendererPath = ScaleToWotldCoords(map, serverUnit.Path);
                    lineRenderer.positionCount = lineRendererPath.Length;
                    lineRenderer.SetPositions(lineRendererPath);
                }

            }
        }

        private Vector3[] ScaleToWotldCoords(MapVisualisator map, IEnumerable<Position> positions)
        {
            Vector3[] res = new Vector3[positions.Count() + 1]; // adds self as a point
            res[0] = this.Position;
            // todo optimize it
            for (int i = 0; i < res.Length - 1; i++)
            {
                res[i + 1] = map.ScaleToWorldCoords(positions.ElementAt(i), map.XSize, map.YSize, map.MapScale);
                res[i + 1].z -= lineRendererHeight;
            }
            return res;
        }
    }
}