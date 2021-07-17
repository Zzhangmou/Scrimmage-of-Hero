using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetWorkFK
{
    /// <summary>
    /// “∆∂Ø–≠“È
    /// </summary>
    public class MsgMove : MsgBase
    {
        public MsgMove()
        {
            protoName = "MsgMove";
        }
        public int x = 0;
        public int y = 0;
        public int z = 0;
    }
}

