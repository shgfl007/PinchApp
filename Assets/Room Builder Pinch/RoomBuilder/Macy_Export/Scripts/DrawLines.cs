using UnityEngine;
using System.Collections;

public class DrawLines : MonoBehaviour {

    public Material lineMat;
    bool mDrew;
    Vector3[] points = new Vector3[] {new Vector3(-150,-30,-150), new Vector3(150,-30,-150),
    new Vector3(150,120,-150), new Vector3(-150,120,-150),new Vector3(-150,-30,-150),
    new Vector3(-150,-30,150),new Vector3(-150,120,150),new Vector3(-150,120,-150)
    ,new Vector3(150,120,-150) ,new Vector3(150,120,150) ,new Vector3(150,-30,150),new Vector3(150,-30,-150)
    ,new Vector3(150,-30,150),new Vector3(-150,-30,150),new Vector3(150,-30,150),new Vector3(150,120,150)
    ,new Vector3(-150,120,150)};

    // Connect all of the `points` to the `mainPoint`
    void DrawConnectingLines()
    {
        if (points.Length > 0)
        {
            // Loop through each point to connect to the mainPoint
            for (int i = 0; i< points.Length; i++)
            {
                Vector3 mainPointPos = points[i];
                int next = i + 1;
                if(next == points.Length)
                {
                    return;
                }
                Vector3 pointPos = points[next];

                GL.Begin(GL.LINES);
                lineMat.SetPass(0);
                GL.Color(new Color(lineMat.color.r, lineMat.color.g, lineMat.color.b, lineMat.color.a));
                GL.Vertex3(mainPointPos.x, mainPointPos.y, mainPointPos.z);
                GL.Vertex3(pointPos.x, pointPos.y, pointPos.z);
                GL.End();
            }
        }
    }

    // To show the lines in the game window whne it is running
    void OnPostRender()
    {
        if (!mDrew)
        {
            DrawConnectingLines();
            mDrew = true;
        }
    }

    // To show the lines in the editor
   /* void OnDrawGizmos()
    {
        DrawConnectingLines();
    }*/
}
