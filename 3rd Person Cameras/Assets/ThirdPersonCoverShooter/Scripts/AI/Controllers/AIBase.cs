﻿using UnityEngine;

namespace CoverShooter
{
    /// <summary>
    /// Base parent class for some AI components.
    /// </summary>
    public class AIBase : MonoBehaviour
    {
        /// <summary>
        /// Sends a message to other components.
        /// </summary>
        public void Message(string name)
        {
            try
            {
                SendMessage(name, SendMessageOptions.DontRequireReceiver);
            }
            catch(System.Exception e)
            {
                Debug.LogWarning(e, this);
            }
        }

        /// <summary>
        /// Sends a message to other components.
        /// </summary>
        public void Message(string name, object value)
        {
            SendMessage(name, value, SendMessageOptions.DontRequireReceiver);
        }
    }
}
