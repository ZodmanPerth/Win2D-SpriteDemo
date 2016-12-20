using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteDemo.Extensions
{
    public static class GeometryExtensions
    {
        public static double ToRadians(this int degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
