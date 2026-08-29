using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEPInspector.Entities
{
    public class CartesianPoint
    {
        public string Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public CartesianPoint(string id, double x, double y, double z)
        {
            Id = id;
            X = x;
            Y = y;
            Z = z;
        }

        public static CartesianPoint ParseCartesianPoint(string line)
        {
            // line : #1247=CARTESIAN_POINT('',(0.,0.,0.));
            // coords : 0.,0.,0.
            string id = line.Split('=')[0];

            int start = line.IndexOf(",");
            int end = line.IndexOf(")");
            string coords = line.Substring(start + 2, end - start - 2);
            string[] values = coords.Split(',');

            double x = Convert.ToDouble(values[0]);
            double y = Convert.ToDouble(values[1]);
            double z = Convert.ToDouble(values[2]);

            return new CartesianPoint(id, x, y, z);
        }
    }
}
