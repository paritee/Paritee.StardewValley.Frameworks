using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace BetterFarmAnimalVariety.Framework.Api
{
    class Asset
    {
        public static Texture2D LoadTexture(string filePath)
        {
            Texture2D texture;

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                texture = Texture2D.FromStream(Api.Content.GetGraphicsDevice(), fileStream);
            }

            return texture;
        }
    }
}
