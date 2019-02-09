using Microsoft.Xna.Framework;
using MorpheusInTheUnderworld.Collisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorpheusInTheUnderworld.Classes.Components
{
    public class Tile
    {
        private Color color = Color.White;
        public Color Color { get { return color;  } set { color = value; } }
    }
}
