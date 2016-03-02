#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

namespace RealTimeStrategy.Elements.Intelligence
{
    public class Brain
    {
        public Objects.Unit body;
        public Brain(Objects.Unit _body)
        {
            body = _body;
        }
        public virtual bool CheckCollisions()
        {
            if (Game1.OBM.GetWorld().Query(Game1.OBM.CalcChunkIndex(new Point((int)(body.position.X + body.velocity.X), (int)(body.position.Y + body.velocity.Y))),
                       new Rectangle((int)((body.position.X + body.velocity.X) - Game1.CAM.offset.X), (int)((body.position.Y + body.velocity.Y) - Game1.CAM.offset.Y), body.texture.sprite.Width, body.texture.sprite.Height)).Count == 0)
                return false;
            return true;
        }
    }
}
