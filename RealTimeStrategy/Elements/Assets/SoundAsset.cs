#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace RealTimeStrategy.Elements.Assets
{
    public class SoundAsset
    {
        string name;
        public SoundEffect sound;
        public SoundAsset(string n, SoundEffect s)
        {
            name = n;
            sound = s;
        }
    }
}
