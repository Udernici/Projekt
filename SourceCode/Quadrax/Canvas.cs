using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quadrax
{
    class MyCanvas : Panel
    {

        List<GameObject> _objects = new List<GameObject>();
        Timer _gameTimer = new Timer();

        public MyCanvas()
        {

        }

        public void AddObject(GameObject o)
        {
            _objects.Add(o);
        }

        public void RemoveObject(GameObject o)
        {
            _objects.Remove(o);
        }

        public void Redraw()
        {
            //add graphic logic
        }

        public void Clear()
        {
            _objects.Clear();
        }


    }
}
