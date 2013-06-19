using DeveMazeGenerator.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveMazeGeneratorMonoGame.LineOfSight
{
    public class LineOfSightObject
    {
        public MazePoint CameraPoint { get; set; }
        public List<MazePoint> LosPoints { get; set; }
    }
}
