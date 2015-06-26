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

namespace RealTimeStrategy.Managers
{
    public class AssetManager
    {
        Dictionary<string, SoundEffectInstance> activeSounds;
        Dictionary<string, Elements.Assets.TextureAsset> textures;
        Dictionary<string, Elements.Assets.SoundAsset> sounds;
        Elements.Assets.TextureAsset tempTexture;
        Elements.Assets.SoundAsset tempSound;
        SoundEffectInstance soundInstance;
        string[] splitData;

        public AssetManager()
        {
            textures = new Dictionary<string, Elements.Assets.TextureAsset>();
            sounds = new Dictionary<string, Elements.Assets.SoundAsset>();
            activeSounds = new Dictionary<string, SoundEffectInstance>();
        }

        public void Initialize() { }

        public void CreateTextureAsset(string name, string data, Texture2D sprite)
        {
            int i = 0;
            int[] vertData = new int[3];
            splitData = data.Split(',');
            foreach (string csv in splitData)
            {
                vertData[i] = Convert.ToInt32(csv);
                i++;
            }
            tempTexture = new Elements.Assets.TextureAsset(name, sprite, vertData[0], vertData[1], vertData[2]);
            textures.Add(name, tempTexture);
        }
        public void CreateSoundAsset(string name, SoundEffect sound)
        {
            tempSound = new Elements.Assets.SoundAsset(name, sound);
            sounds.Add(name, tempSound);
        }
        public Elements.Assets.TextureAsset GetTextureAsset(string name) { return textures[name]; }
        public Elements.Assets.SoundAsset GetSoundAsset(string name) { return sounds[name]; }
        public void PlaySound(string name, bool looped)
        {
            if (activeSounds.ContainsKey(name) && !looped)
            {
                StopSound(name);
            }
            if (!activeSounds.ContainsKey(name))
            {
                soundInstance = Game1.ASM.GetSoundAsset(name).sound.CreateInstance();
                activeSounds.Add(name, soundInstance);
                if (looped)
                    soundInstance.IsLooped = true;
                soundInstance.Play();
            }


        }
        public void StopSound(string name)
        {
            if (activeSounds.ContainsKey(name))
            {
                activeSounds[name].Stop();
                activeSounds.Remove(name);
            }
        }
    }
}
