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
    public class VillagerBrain : Brain
    {
        public VillagerBrain(Objects.Unit _body)
            : base(_body)
        {

        }
        public override bool CheckCollisions()
        {
            List<Objects.GameObject> remove = new List<Objects.GameObject>();
            Point index = Game1.OBM.CalcChunkIndex(new Point((int)(body.position.X + body.velocity.X), (int)(body.position.Y + body.velocity.Y)));
            Rectangle hitBox =  new Rectangle((int)((body.position.X + body.velocity.X) - Game1.CAM.offset.X), (int)((body.position.Y + body.velocity.Y) - Game1.CAM.offset.Y), body.texture.sprite.Width, body.texture.sprite.Height);
            List<Objects.GameObject> query = Game1.OBM.GetWorld().Query(index,hitBox);
            if (query.Count > 0)
            {
                foreach (Objects.GameObject obj in query)
                    if (obj as Objects.TownCenter != null)
                        remove.Add(obj);
                foreach (Objects.GameObject obj in remove)
                    query.Remove(obj);
            }
            if (query.Count == 0)
                return false;
            return true;
        }
    }
}
