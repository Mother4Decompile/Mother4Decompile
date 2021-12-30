using System;
using System.Collections.Generic;
using System.Text;
using Carbine.Utility;
using FMOD;

namespace Carbine.Audio.fmod
{
	// Token: 0x0200000D RID: 13
	internal class FmodAudioManager : AudioManager
	{
		// Token: 0x06000066 RID: 102 RVA: 0x00002DC0 File Offset: 0x00000FC0
		public FmodAudioManager()
		{
			uint num = 0U;
			int num2 = 0;
			int num3 = 0;
			SPEAKERMODE speakermode = SPEAKERMODE.STEREO;
			CAPS caps = CAPS.NONE;
			RESULT result = Factory.System_Create(ref this.system);
			FmodAudioManager.ERRCHECK(result);
			result = this.system.getVersion(ref num);
			FmodAudioManager.ERRCHECK(result);
			result = this.system.getNumDrivers(ref num3);
			FmodAudioManager.ERRCHECK(result);
			if (num3 == 0)
			{
				result = this.system.setOutput(OUTPUTTYPE.NOSOUND);
				FmodAudioManager.ERRCHECK(result);
			}
			else
			{
				result = this.system.getDriverCaps(0, ref caps, ref num2, ref speakermode);
				FmodAudioManager.ERRCHECK(result);
				if ((caps & CAPS.HARDWARE_EMULATED) == CAPS.HARDWARE)
				{
					result = this.system.setDSPBufferSize(1024U, 10);
					Console.WriteLine("Audio hardware acceleration is turned off. Audio performance may be degraded.");
					FmodAudioManager.ERRCHECK(result);
				}
				StringBuilder stringBuilder = new StringBuilder(256);
				GUID guid = default(GUID);
				result = this.system.getDriverInfo(0, stringBuilder, 256, ref guid);
				FmodAudioManager.ERRCHECK(result);
				string text = stringBuilder.ToString();
				Console.WriteLine("Audio driver name: {0}", text);
				if (text.Contains("SigmaTel"))
				{
					result = this.system.setSoftwareFormat(48000, SOUND_FORMAT.PCMFLOAT, 0, 0, DSP_RESAMPLER.LINEAR);
					FmodAudioManager.ERRCHECK(result);
					Console.WriteLine("Sigmatel card detected; format changed to PCM floating point.");
				}
			}
			this.InitFmodSystem();
			if (result == RESULT.ERR_OUTPUT_CREATEBUFFER)
			{
				result = this.system.setSpeakerMode(SPEAKERMODE.STEREO);
				FmodAudioManager.ERRCHECK(result);
				this.InitFmodSystem();
				Console.WriteLine("Selected speaker mode is not supported, defaulting to stereo.");
			}
			this.callbacks = new Dictionary<int, CHANNEL_CALLBACK>();
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00002F3C File Offset: 0x0000113C
		private void InitFmodSystem()
		{
			RESULT result = this.system.init(32, INITFLAGS.NORMAL, (IntPtr)null);
			FmodAudioManager.ERRCHECK(result);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00002F68 File Offset: 0x00001168
		public override void SetSpeakerMode(AudioMode mode)
		{
			SPEAKERMODE speakerMode;
			switch (mode)
			{
			case AudioMode.Mono:
				speakerMode = SPEAKERMODE.MONO;
				goto IL_18;
			}
			speakerMode = SPEAKERMODE.STEREO;
			IL_18:
			RESULT result = this.system.setSpeakerMode(speakerMode);
			FmodAudioManager.ERRCHECK(result);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00002FA0 File Offset: 0x000011A0
		public override void Update()
		{
			base.Update();
			this.system.update();
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00002FB4 File Offset: 0x000011B4
		public override CarbineSound Use(string filename, AudioType type)
		{
			int num = Hash.Get(filename);
			FmodSound fmodSound;
			if (!this.sounds.ContainsKey(num))
			{
				if (type == AudioType.Stream)
				{
					fmodSound = FmodAudioLoader.Instance.LoadStreamSound(ref this.system, filename, 0, this.musicVolume);
				}
				else
				{
					fmodSound = FmodAudioLoader.Instance.LoadSound(ref this.system, filename, 0, this.effectsVolume);
				}
				this.instances.Add(num, 1);
				this.sounds.Add(num, fmodSound);
			}
			else
			{
				fmodSound = (FmodSound)this.sounds[num];
				Dictionary<int, int> instances;
				int key;
				(instances = this.instances)[key = num] = instances[key] + 1;
			}
			return fmodSound;
		}

		// Token: 0x0600006B RID: 107 RVA: 0x0000305C File Offset: 0x0000125C
		public override void Unuse(CarbineSound sound)
		{
			int num = 0;
			CarbineSound carbineSound = null;
			foreach (KeyValuePair<int, CarbineSound> keyValuePair in this.sounds)
			{
				num = keyValuePair.Key;
				carbineSound = keyValuePair.Value;
				if (carbineSound == sound)
				{
					Dictionary<int, int> instances;
					int key;
					(instances = this.instances)[key = num] = instances[key] - 1;
					break;
				}
			}
			if (carbineSound != null && this.instances[num] <= 0)
			{
				Console.WriteLine("Cleaning up audio");
				this.instances.Remove(num);
				this.sounds.Remove(num);
				carbineSound.Dispose();
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000311C File Offset: 0x0000131C
		public int AddCallback(CHANNEL_CALLBACK callback)
		{
			this.callbacks.Add(++this.callbackCounter, callback);
			return this.callbackCounter;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x0000314C File Offset: 0x0000134C
		public void RemoveCallback(int index)
		{
			this.callbacks.Remove(index);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000315C File Offset: 0x0000135C
		public static void ERRCHECK(RESULT result)
		{
			if (result != RESULT.OK)
			{
				string message = string.Format("FMOD error: {0} - {1}", result, Error.String(result));
				throw new FmodException(message);
			}
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000318C File Offset: 0x0000138C
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (!this.disposed && this.system != null)
			{
				RESULT result = this.system.close();
				FmodAudioManager.ERRCHECK(result);
				result = this.system.release();
				FmodAudioManager.ERRCHECK(result);
			}
		}

		// Token: 0x0400003B RID: 59
		private FMOD.System system;

		// Token: 0x0400003C RID: 60
		private Dictionary<int, CHANNEL_CALLBACK> callbacks;

		// Token: 0x0400003D RID: 61
		private int callbackCounter;
	}
}
