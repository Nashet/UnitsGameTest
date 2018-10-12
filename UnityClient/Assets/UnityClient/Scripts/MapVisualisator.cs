using Nashet.GameLogicAbstraction;
using Nashet.UnitsGameLogic;
using Nashet.Utils;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using UnityEngine;

namespace Nashet.UnitsGameClient
{
    /// <summary>
    /// Visualizes map, and update units..
    /// </summary>
    public class MapVisualisator : MonoBehaviour
    {
        protected IMapView serverMap;

        [SerializeField] protected float mapScale;
        public float MapScale { get { return mapScale; } }

        [SerializeField] protected GameObject mapNodePrefab, unitPrefab;
        [SerializeField] protected bool builtMap;  
       
        public int XSize => serverMap.XSize;
        public int YSize => serverMap.YSize;

        /// <summary>
        /// Called from callback
        /// </summary>        
        internal void ReceiveNetData(string text)
        {
            IFormatter formatter = new SoapFormatter();
            byte[] byteArray = Encoding.ASCII.GetBytes(text);
            MemoryStream stream = new MemoryStream(byteArray);
            var game = (IGameLogic)formatter.Deserialize(stream);
            serverMap = game.World.MapView;

            if (!builtMap)
            {
                for (int x = 0; x < serverMap.XSize; x++)
                {
                    for (int y = 0; y < serverMap.YSize; y++)
                    {
                        var newNode = Instantiate(mapNodePrefab, this.transform);
                        var newNodePosition = this.ScaleToWorldCoords(new Position(x, y), serverMap.XSize, serverMap.YSize, mapScale);
                        newNode.transform.position = newNodePosition;
                        newNode.GetComponent<MapNode>().Set(new Position(x, y));
                    }
                }
                builtMap = true;
            }

            foreach (var serverUnits in serverMap.AllUnits)
            {
                var foundLocal = GetComponentsInChildren<UnitVisualizator>().FirstOrDefault(x => x.serverUnit.UID == serverUnits.UID);
                if (foundLocal == null)
                {
                    var newUnitVisualizator = Instantiate(unitPrefab, this.transform);
                    var c = newUnitVisualizator.GetComponent<UnitVisualizator>();
                    c.Synchronize(serverUnits, this);
                }
                else
                    foundLocal.Synchronize(serverUnits, this);
                //todo Destroy units which aren't presented on server anymore
            }
        }
    }
}
