using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadrax
{
    abstract class GameObject : Object
    {
        int x;
        int y;
        bool solid;
        int weight;
        public GameObject(int x, int y, bool solid, int weight)
        {
            this.x = x;
            this.y = y;
            this.solid = solid;
            this.weight = weight;
        }

        public int X { get { return this.x; } set { this.x = value; } }
        public int Y { get { return this.y; } set { this.y = value; } }

        //is collidable
        public bool isSolid()
        {
            return solid;
        }
        
        public bool canPush(int strength)
        {
            return strength >= weight;
        }

    }
}

