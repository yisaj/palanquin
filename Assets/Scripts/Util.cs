using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    public Vector3[] points = new Vector3[3];

    public Triangle()
    {
        points[0] = points[1] = points[2] = new Vector3();
    }

    public Triangle(Vector3 a, Vector3 b, Vector3 c)
    {
        points[0] = a;
        points[1] = b;
        points[2] = c;
    }
}

public class Util
{
    // So technically this is not quite triangle intersection. Intersection of triangle edges don't count.
    // See http://fileadmin.cs.lth.se/cs/Personal/Tomas_Akenine-Moller/pubs/tritri.pdf
    public static bool IntersectTriangles(Triangle a, Triangle b)
    {
        Vector3 aNormal = Vector3.Normalize(Vector3.Cross(a.points[1] - a.points[0], a.points[2] - a.points[0]));
        Vector3 bNormal = Vector3.Normalize(Vector3.Cross(b.points[1] - b.points[0], b.points[2] - b.points[0]));
        float aOffset = -Vector3.Dot(aNormal, a.points[0]);
        float bOffset = -Vector3.Dot(bNormal, b.points[0]);
        float[] distToAPlane = new float[3];
        float[] distToBPlane = new float[3];
        int numAZeros = 0;
        int numBZeros = 0;
        for (int i = 0; i < 3; i++)
        {
            distToAPlane[i] = Vector3.Dot(aNormal, b.points[i]) + aOffset;
            distToBPlane[i] = Vector3.Dot(bNormal, a.points[i]) + bOffset;
            if (AlmostZero(distToAPlane[i])) { numAZeros++; }
            if (AlmostZero(distToBPlane[i])) { numBZeros++; }
        }

        // if exactly two points of a triangle are on the line of intersection, they can't possibly intersect
        if (numAZeros == 2 || numBZeros == 2)
        {
            return false;
        }

        bool coplanar = numAZeros == 3;
        bool aSameSign = ((distToAPlane[0] < 0) == (distToAPlane[1] < 0) || AlmostZero(distToAPlane[0]) || AlmostZero(distToAPlane[1])) &&
                         ((distToAPlane[1] < 0) == (distToAPlane[2] < 0) || AlmostZero(distToAPlane[1]) || AlmostZero(distToAPlane[2])) &&
                         ((distToAPlane[2] < 0) == (distToAPlane[0] < 0) || AlmostZero(distToAPlane[2]) || AlmostZero(distToAPlane[0]));
        bool bSameSign = ((distToBPlane[0] < 0) == (distToBPlane[1] < 0) || AlmostZero(distToBPlane[0]) || AlmostZero(distToBPlane[1])) &&
                         ((distToBPlane[1] < 0) == (distToBPlane[2] < 0) || AlmostZero(distToBPlane[1]) || AlmostZero(distToBPlane[2])) &&
                         ((distToBPlane[2] < 0) == (distToBPlane[0] < 0) || AlmostZero(distToBPlane[2]) || AlmostZero(distToBPlane[0]));

        /*
        Debug.Log("coplane testing");
        Debug.Log(coplanar);
        Debug.Log(aSameSign);
        Debug.Log(distToAPlane[0]);
        Debug.Log(distToAPlane[1]);
        Debug.Log(distToAPlane[2]);
        Debug.Log(distToBPlane[0]);
        Debug.Log(distToBPlane[1]);
        Debug.Log(distToBPlane[2]);
        Debug.Log(bSameSign);
        */

        if (!coplanar && aSameSign && bSameSign)
        {
            return false;
        }

        if (coplanar)
        {
            Vector2[] aFlat = new Vector2[3];
            Vector2[] bFlat = new Vector2[3];
            float max = Mathf.Abs(aNormal.x);
            int index = 0;
            if (Mathf.Abs(aNormal.y) > max)
            {
                max = aNormal.y;
                index = 1;
            } else if (Mathf.Abs(aNormal.z) > max)
            {
                max = aNormal.z;
                index = 2;
            }

            if (index == 0)
            {
                aFlat[0] = new Vector2(a.points[0].y, a.points[0].z);
                aFlat[1] = new Vector2(a.points[1].y, a.points[1].z);
                aFlat[2] = new Vector2(a.points[2].y, a.points[2].z);
                bFlat[0] = new Vector2(b.points[0].y, b.points[0].z);
                bFlat[1] = new Vector2(b.points[1].y, b.points[1].z);
                bFlat[2] = new Vector2(b.points[2].y, b.points[2].z);
            } else if (index == 1)
            {
                aFlat[0] = new Vector2(a.points[0].x, a.points[0].z);
                aFlat[1] = new Vector2(a.points[1].x, a.points[1].z);
                aFlat[2] = new Vector2(a.points[2].x, a.points[2].z);
                bFlat[0] = new Vector2(b.points[0].x, b.points[0].z);
                bFlat[1] = new Vector2(b.points[1].x, b.points[1].z);
                bFlat[2] = new Vector2(b.points[2].x, b.points[2].z);
            } else if (index == 2)
            {
                aFlat[0] = new Vector2(a.points[0].x, a.points[0].y);
                aFlat[1] = new Vector2(a.points[1].x, a.points[1].y);
                aFlat[2] = new Vector2(a.points[2].x, a.points[2].y);
                bFlat[0] = new Vector2(b.points[0].x, b.points[0].y);
                bFlat[1] = new Vector2(b.points[1].x, b.points[1].y);
                bFlat[2] = new Vector2(b.points[2].x, b.points[2].y);
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (IntersectLines(aFlat[i], aFlat[(i + 1) % 3], bFlat[j], bFlat[(j + 1) % 3])) { return true; }
                }
            }
            Vector2 aCenter = new Vector2((aFlat[0].x + aFlat[1].x + aFlat[2].x) / 3f, (aFlat[0].y + aFlat[1].y + aFlat[2].y) / 3f);
            Vector2 bCenter = new Vector2((bFlat[0].x + bFlat[1].x + bFlat[2].x) / 3f, (bFlat[0].y + bFlat[1].y + bFlat[2].y) / 3f);

            float[] aAngles = new float[3] { Cross2D(aFlat[1] - aFlat[0], bCenter - aFlat[0]),
                                             Cross2D(aFlat[2] - aFlat[1], bCenter - aFlat[1]),
                                             Cross2D(aFlat[0] - aFlat[2], bCenter - aFlat[2]) };
            float[] bAngles = new float[3] { Cross2D(bFlat[1] - bFlat[0], aCenter - bFlat[0]),
                                             Cross2D(bFlat[2] - bFlat[1], aCenter - bFlat[1]),
                                             Cross2D(bFlat[0] - bFlat[2], aCenter - bFlat[2]) };
    /*
            Debug.Log("passed");
            Debug.Log("flats");
            Debug.Log(aFlat[0]);
            Debug.Log(aFlat[1]);
            Debug.Log(aFlat[2]);
            Debug.Log(bCenter);

            Debug.Log(aAngles[0]);
            Debug.Log(aAngles[1]);
            Debug.Log(aAngles[2]);
            */

            if (((aAngles[0] < 0 == aAngles[1] < 0) || AlmostZero(aAngles[0]) || AlmostZero(aAngles[1])) &&
                ((aAngles[1] < 0 == aAngles[2] < 0) || AlmostZero(aAngles[1]) || AlmostZero(aAngles[2])) &&
                ((aAngles[2] < 0 == aAngles[0] < 0) || AlmostZero(aAngles[2]) || AlmostZero(aAngles[0])))
            {
                return true;
            }
            if (((bAngles[0] < 0 == bAngles[1] < 0) || bAngles[0] == 0 || bAngles[1] == 0) &&
                ((bAngles[1] < 0 == bAngles[2] < 0) || bAngles[1] == 0 || bAngles[2] == 0) &&
                ((bAngles[2] < 0 == bAngles[0] < 0) || bAngles[2] == 0 || bAngles[0] == 0))
            {
                return true;
            }

            return false;
        } else
        {
            //Debug.Log("not coplanar");
            Vector3 intersectionDir = Vector3.Cross(aNormal, bNormal);
            //Debug.Log(intersectionDir);
            float max = Mathf.Abs(intersectionDir.x);
            int index = 0;
            if (Mathf.Abs(intersectionDir.y) > max)
            {
                max = intersectionDir.y;
                index = 1;
            }
            if (Mathf.Abs(intersectionDir.z) > max)
            {
                max = intersectionDir.z;
                index = 2;
            }

            float[] aProjections = new float[3];
            float[] bProjections = new float[3];
            if (index == 0) // x max
            {
                for (int i = 0; i < 3; i++)
                {
                    aProjections[i] = a.points[i].x;
                    bProjections[i] = b.points[i].x;
                }
            } else if (index == 1) // y max
            {
                for (int i = 0; i < 3; i++)
                {
                    aProjections[i] = a.points[i].y;
                    bProjections[i] = b.points[i].y;
                }
            } else if (index == 2) // z max
            {
                for (int i = 0; i < 3; i++)
                {
                    aProjections[i] = a.points[i].z;
                    bProjections[i] = b.points[i].z;
                }
            }
            /*
            Debug.Log("before");
            Debug.Log(distToAPlane[0]);
            Debug.Log(distToAPlane[1]);
            Debug.Log(distToAPlane[2]);
            Debug.Log(distToBPlane[0]);
            Debug.Log(distToBPlane[1]);
            Debug.Log(distToBPlane[2]);
            */
            aProjections = sort(aProjections[0], aProjections[1], aProjections[2], ref distToBPlane);
            bProjections = sort(bProjections[0], bProjections[1], bProjections[2], ref distToAPlane);

            float aLeft = aProjections[0] + (aProjections[1] - aProjections[0]) * distToBPlane[0] / (distToBPlane[0] - distToBPlane[1]);
            float aRight = aProjections[1] + (aProjections[2] - aProjections[1]) * -distToBPlane[1] / (distToBPlane[2] - distToBPlane[1]);
            float bLeft = bProjections[0] + (bProjections[1] - bProjections[0]) * distToAPlane[0] / (distToAPlane[0] - distToAPlane[1]);
            float bRight = bProjections[1] + (bProjections[2] - bProjections[1]) * -distToAPlane[1] / (distToAPlane[2] - distToAPlane[1]);
            /*
            Debug.Log("after");
            Debug.Log(distToAPlane[0]);
            Debug.Log(distToAPlane[1]);
            Debug.Log(distToAPlane[2]);
            Debug.Log(distToBPlane[0]);
            Debug.Log(distToBPlane[1]);
            Debug.Log(distToBPlane[2]);
            
            Debug.Log("left right");
            Debug.Log(aLeft);
            Debug.Log(aRight);
            Debug.Log(bLeft);
            Debug.Log(bRight);
            */

            if (aRight < aLeft)
            {
                float temp = aRight;
                aRight = aLeft;
                aLeft = temp;
            }
            if (bRight < bLeft)
            {
                float temp = bRight;
                bRight = bLeft;
                bLeft = temp;
            }

            if (AlmostZero(aRight - aLeft) || AlmostZero(bRight - bLeft))
            {
                //Debug.Log("early");
                return false;
            }
            //Debug.Log("not early");
            return (aLeft <= bLeft && aRight > bLeft) || (bLeft <= aLeft && bRight > aLeft);
        }
    }

    static float[] sort(float a, float b, float c, ref float[] dists)
    {
        float temp;
        if (dists[0] > dists[1])
        {
            temp = dists[0];
            dists[0] = dists[1];
            dists[1] = temp;
            temp = a;
            a = b;
            b = temp;
        }

        if (dists[0] > dists[2])
        {
            temp = dists[0];
            dists[0] = dists[2];
            dists[2] = dists[1];
            dists[1] = temp;
            temp = a;
            a = c;
            c = b;
            b = temp;
        } else if (dists[1] > dists[2])
        {
            temp = dists[1];
            dists[1] = dists[2];
            dists[2] = temp;
            temp = b;
            b = c;
            c = temp;
        }

        if (AlmostZero(dists[0]) && AlmostZero(dists[1])) // 0 0 1
        {
            temp = dists[1];
            dists[1] = -dists[2];
            dists[2] = temp;
            return new float[] { a, c, b };
        } else if (AlmostZero(dists[1]) && AlmostZero(dists[2])) // -1 0 0
        {
            temp = dists[0];
            dists[0] = dists[1];
            dists[1] = temp;
            return new float[] { b, a, c };
        } else if (AlmostZero(dists[1])) // -1 0 1
        {
            temp = dists[0];
            dists[0] = dists[1];
            dists[1] = temp;
            return new float[] { b, a, c };
        } else if (dists[1] < 0) // -1 -1 1
        {
            temp = dists[1];
            dists[1] = -dists[2];
            dists[2] = -temp;
            dists[0] = -dists[0];
            return new float[] { a, c, b };
        } else // -1 1 1
        {
            temp = dists[0];
            dists[0] = dists[1];
            dists[1] = temp;
            return new float[] { b, a, c };
        }
    }

    static bool IntersectLines(Vector2 aStart, Vector2 aEnd, Vector2 bStart, Vector2 bEnd)
    {
        Vector2 aDir = aEnd - aStart;
        Vector2 bDir = bEnd - bStart;

        float denom = Cross2D(aDir, bDir);
        float tNumer = Cross2D(bStart - aStart, bDir);
        float uNumer = Cross2D(bStart - aStart, aDir);
        // colinear lines don't count as intersecting here
        if (denom == 0)
        {
            return false;
        }

        float t = tNumer / denom;
        float u = uNumer / denom;

        return 0 < t && t < 1 && 0 < u && u < 1;
    }

    static float Cross2D(Vector2 a, Vector2 b)
    {
        return a.x * b.y - a.y * b.x;
    }

    public static Mesh GenerateConvexMesh(Vector3[] points)
    {
        Mesh mesh = new Mesh();
        List<int> triangles = new List<int>();

        Vector3 center = Vector3.zero;
        for (int i = 0; i < points.Length; i++)
        {
            for (int j = i + 1; j < points.Length; j++)
            {
                for (int k = j + 1; k < points.Length; k++)
                {
                    Triangle newTri = new Triangle(points[i], points[j], points[k]);
                    bool conflict = false;
                    // remove all conflicting triangles
                    for (int l = 0; l < triangles.Count; l += 3)
                    {
                        Triangle oldTri = new Triangle(points[triangles[l]], points[triangles[l + 1]], points[triangles[l + 2]]);
                        if (Util.IntersectTriangles(newTri, oldTri))
                        {
                            /*
                            Debug.Log("INTERSECT");
                            Debug.Log(oldTri.points[0]);
                            Debug.Log(oldTri.points[1]);
                            Debug.Log(oldTri.points[2]);
                            Debug.Log(newTri.points[0]);
                            Debug.Log(newTri.points[1]);
                            Debug.Log(newTri.points[2]);
                            */
                            conflict = true;
                            break;
                        }
                    }
                    // add the new triangle
                    if (!conflict)
                    {
                        triangles.Add(i);
                        triangles.Add(j);
                        triangles.Add(k);
                    }
                }
            }
            center += points[i];
        }
        center /= points.Length;

        // reconcile all the normals so that they point outwards
        for (int i = 0; i < triangles.Count; i += 3)
        {
            Vector3 a = points[triangles[i + 1]] - points[triangles[i]];
            Vector3 b = points[triangles[i + 2]] - points[triangles[i + 1]];
            Vector3 fromCenter = points[triangles[i + 1]] - center;
            if (Vector3.Dot(Vector3.Cross(a, b), fromCenter) < 0) {
                // reverse the chirality of the triangle
                int temp = triangles[i];
                triangles[i] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
        }

        List<Vector3> vertices = new List<Vector3>();
        for (int i = 0; i < triangles.Count; i += 3)
        {
            vertices.Add(points[triangles[i]]);
            vertices.Add(points[triangles[i + 1]]);
            vertices.Add(points[triangles[i + 2]]);
            triangles[i] = i;
            triangles[i + 1] = i + 1;
            triangles[i + 2] = i + 2;
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        return mesh;
        
    }

    public static bool AlmostZero(float num)
    {
        float epsilon = 0.0000001f;
        return num > -epsilon && num < epsilon;
    }
}
