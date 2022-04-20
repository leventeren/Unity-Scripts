using UnityEngine;
using Unity.Collections;
using Unity.Burst;
using Unity.Jobs;
namespace nameOf
{
    public partial struct LineRenderUtility
    {
        /// <summary>
        /// interpolates a rasterized line between point 0 and point 1 using bresenhams line algorithm
        /// </summary>
        /// <param name="pos0">the beginning position for the line</param>
        /// <param name="pos1">the ending position for the line</param>
        /// <returns>the list of points that make up the line </returns>
        public static Vector2Int[] InterpolateLine(Vector2Int pos0, Vector2Int pos1)
        {
            NativeList<Vector2Int> native = new NativeList<Vector2Int>(0, Allocator.TempJob); ;
            Job Job = new Job()
            {
                pos0 = pos0,
                pos1 = pos1,
                @return = native,
            };
            JobHandle handle = Job.Schedule();
            handle.Complete();
            Vector2Int[] @return = native.ToArray();
            native.Dispose();
            return @return;
        }

        [BurstCompile]
        struct Job : IJob
        {
            public Vector2Int pos0, pos1;
            public NativeList<Vector2Int> @return;
            public void Execute()
            {
                LineRenderVars v = LineRenderPreProcessing(pos0, pos1);
                for (int x = v.pos0.x; x <= v.pos1.x; x++)
                {
                    @return.Add(new Vector2Int(v.steep ? v.y : x, v.steep ? x : v.y));
                    v.error -= v.dy;
                    if (v.error < 0)
                    {
                        v.y += v.ystep;
                        v.error += v.dx;
                    }
                }
            }
        }
    }
}

LineRenderPreprocessingVars.cs

using UnityEngine;
namespace nameOf
{
    /// <summary>
    /// a small struct for storing the parameters needed to compute the line algorithm
    /// </summary>
    public struct LineRenderVars
    {
        // input positions
        public Vector2Int pos0;
        public Vector2Int pos1;
        // line parameters
        public bool steep;
        public int error;
        public int ystep;
        public int y;
        public int dx;
        public int dy;
    }
}

LineRenderUtility.cs

using UnityEngine;
using System.Linq;
using nameOf.Math;
namespace nameOf
{
    /// <summary>
    /// struct responsible for containing the code that renders a rasterized line according to bresenhams line algorithm
    /// Source for the algorithm: http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html
    /// </summary>
    public partial struct LineRenderUtility
    {
        /// <summary>
        /// the line renderer preprocessing used to generate the parameters for computing the line
        /// </summary>
        /// <param name="pos0">beginning position for a line</param>
        /// <param name="pos1">ending position for the line</param>
        /// <returns></returns>
        public static LineRenderVars LineRenderPreProcessing(Vector2Int pos0, Vector2Int pos1)
        {
            bool steep = Mathf.Abs(pos1.y - pos0.y) > Mathf.Abs(pos1.x - pos0.x);
            if (steep)
            {
                int t;
                t = pos0.x; // swap startPos.x and startPos.y
                pos0.x = pos0.y;
                pos0.y = t;
                t = pos1.x; // swap endPos.x and endPos.y
                pos1.x = pos1.y;
                pos1.y = t;
            }
            if (pos0.x > pos1.x)
            {
                int t;
                t = pos0.x; // swap startPos.x and endPos.x
                pos0.x = pos1.x;
                pos1.x = t;
                t = pos0.y; // swap startPos.y and endPos.y
                pos0.y = pos1.y;
                pos1.y = t;
            }
            int dx = pos1.x - pos0.x;
            int dy = Mathf.Abs(pos1.y - pos0.y);
            int error = dx / 2;
            int ystep = (pos0.y < pos1.y) ? 1 : -1;
            int y = pos0.y;
            return new LineRenderVars()
            {
                // render params
                steep = steep,
                error = error,
                ystep = ystep,
                y = y,
                dx = dx,
                dy = dy,
                // input positions
                pos0 = pos0,
                pos1 = pos1,
            };
        }
        /// <summary>
        /// interpolates a line with a given shape copied to every positon
        /// </summary>
        /// <param name="pos0">beginning position for the line</param>
        /// <param name="pos1">ending position for the line</param>
        /// <param name="shape">the shape that is copied over the line</param>
        /// <returns>a rasterized line</returns>
        public static Vector2Int[] InterpolateLineWithShapeApplied(Vector2Int pos0, Vector2Int pos1, Vector2Int[] shape) => 
            VectorMath.TranslateVectorsToArray(InterpolateLine(pos0, pos1).ToArray(), shape);
    }
}


MathUtility.cs

namespace nameOf.Math
{
    /// <summary>
    /// provides multiple basic math functions, not included in UnityEngine.mathf
    /// </summary>
    public partial struct MathUtility
    {
        /// <summary>
        /// function for computing positive modulus
        /// </summary>
        /// <param name="value">the value to apply this operator to</param>
        /// <param name="m">the value to modulus by</param>
        /// <returns>the modified value</returns>
        public static uint PositiveMod(int value, uint m)
        {
            int mod = value % (int)m;
            if (mod < 0)
                mod += (int)m;
            return (uint)mod;
        }
        /// <summary>
        /// function for computing positive modulus
        /// </summary>
        /// <param name="value">the value to apply this operator to</param>
        /// <param name="m">the value to modulus by</param>
        /// <returns>the modified value</returns>
        public static int PositiveMod(int value, int m)
        {
            int mod = value % m;
            if (mod < 0)
                mod += m;
            return mod;
        }
        /// <summary>
        /// function for computing positive modulus
        /// </summary>
        /// <param name="value">the value to apply this operator to</param>
        /// <param name="m">the value to modulus by</param>
        /// <returns>the modified value</returns>
        public static float PositiveMod(float value, float m)
        {
            float mod = value % m;
            if (mod < 0)
                mod += m;
            return mod;
        }
    }
}
