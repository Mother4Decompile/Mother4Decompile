// Decompiled with JetBrains decompiler
// Type: Carbine.Audio.fmod.FmodAudioLoader
// Assembly: Carbine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F138E832-4582-46AC-9AAC-9FED0C56FD24
// Assembly location: D:\OddityPrototypes\Mother 4 -- 2018\Carbine.dll

using Carbine.Utility;
using fNbt;
using System;

namespace Carbine.Audio.fmod
{
  internal class FmodAudioLoader
  {
    public const string AUDIO_PATH = "Resources/Audio/";
    private static FmodAudioLoader instance;
    private NbtFile nbtFile;
    private NbtCompound bgmRoot;
    private NbtCompound sfxRoot;
    private string bgmExtension;
    private string sfxExtension;

    public static FmodAudioLoader Instance
    {
      get
      {
        if (FmodAudioLoader.instance == null)
          FmodAudioLoader.instance = new FmodAudioLoader();
        return FmodAudioLoader.instance;
      }
    }

    private FmodAudioLoader()
    {
      this.nbtFile = new NbtFile("Resources/Audio/audio.dat");
      this.bgmRoot = this.nbtFile.RootTag.Get<NbtCompound>("bgm");
      if (this.bgmRoot != null)
        this.bgmExtension = this.GetExtString(this.bgmRoot);
      this.sfxRoot = this.nbtFile.RootTag.Get<NbtCompound>("sfx");
      if (this.sfxRoot == null)
        return;
      this.sfxExtension = this.GetExtString(this.sfxRoot);
    }

    private string GetExtString(NbtCompound rootTag)
    {
      string extString = "";
      NbtString nbtString = rootTag.Get<NbtString>("ext");
      if (nbtString != null)
        extString = "." + nbtString.StringValue;
      return extString;
    }

    public FmodSound LoadStreamSound(
      ref FMOD.System system,
      string name,
      int loopCount,
      float volume)
    {
      string filename = name;
      uint loopBegin = 0;
      uint loopEnd = 0;
      NbtCompound nbtCompound = this.bgmRoot.Get<NbtCompound>(string.Format("{0:x}", (object) Hash.Get(name)));
      if (nbtCompound != null)
      {
        NbtDouble nbtDouble1 = nbtCompound.Get<NbtDouble>("ls");
        if (nbtDouble1 != null)
          loopBegin = (uint) Math.Max(0.0, nbtDouble1.DoubleValue);
        NbtDouble nbtDouble2 = nbtCompound.Get<NbtDouble>("le");
        if (nbtDouble2 != null)
          loopEnd = (uint) Math.Max(0.0, nbtDouble2.DoubleValue);
        NbtString nbtString = nbtCompound.Get<NbtString>("f");
        if (nbtString != null)
          filename = "Resources/Audio/" + nbtString.StringValue + this.bgmExtension;
      }
      return new FmodSound(ref system, filename, AudioType.Stream, loopBegin, loopEnd, loopCount, volume);
    }

    public FmodSound LoadSound(
      ref FMOD.System system,
      string name,
      int loopCount,
      float volume)
    {
      string filename = name;
      NbtCompound nbtCompound = this.bgmRoot.Get<NbtCompound>(string.Format("{0:x}", (object) Hash.Get(name)));
      if (nbtCompound != null)
      {
        NbtString nbtString = nbtCompound.Get<NbtString>("f");
        if (nbtString != null)
          filename = "Resources/Audio/" + nbtString.StringValue + this.sfxExtension;
      }
      return new FmodSound(ref system, filename, AudioType.Sound, 0U, 0U, loopCount, volume);
    }
  }
}
