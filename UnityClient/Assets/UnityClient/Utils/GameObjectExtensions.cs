using Nashet.UnitsGameLogic;
using UnityEngine;
namespace Nashet.Utils
{
    public static class MonoBehaviourExtensions
    {
        public static Vector3 ScaleToWorldCoords(this MonoBehaviour that, Position position, int mapXSize, int mapYSize, float mapScale)
        {
            return new Vector3(that.transform.position.x + (position.x - mapXSize / 2f) * mapScale + 0.5f, that.transform.position.y + (position.y - mapYSize / 2f) * mapScale + 0.5f, that.transform.position.z);
            //return new Vector3(this.transform.position.x + position.X  * mapScale, this.transform.position.y + position.Y  * mapScale, this.transform.position.z);
        }
    }
}