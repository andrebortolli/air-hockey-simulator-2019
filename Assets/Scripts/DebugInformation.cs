//Programmed by: André Bortolli (RA: 16236796) and Gabriel Solano (RA: 16554685)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Scripts.DebugInfo
{
    public class DebugInformation : MonoBehaviour //This class needs MonoBehaviour to be able to access the profiling information (deltaTime and fixedDeltaTime).
    {
        //Returns the frametime (a.k.a Time.deltaTime). It's multiplied by 1000 because Unity time is in seconds.
        public float GetFrametime()
        {
            return Time.deltaTime * 1000.0f;
        }
        //Returns the Physics frametime (a.k.a Time.fixedDeltaTime). It's multiplied by 1000 because Unity time is in seconds.
        public float GetPhysicsFrametime()
        {
            return Time.fixedDeltaTime * 1000.0f;
        }
        //Returns the current framerate based on Time.deltaTime. The number is updated each frame, so it may be too quick.
        public float GetFramerateSec()
        {
            return 1.0f / Time.deltaTime;
        }
        //Returns the current framerate based on Physics time.
        public float GetPhysicsFramerateSec()
        {
            return 1.0f / Time.fixedDeltaTime;
        }
    }
}