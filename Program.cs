using System;
using System.IO;
using Carbine;
using Carbine.Audio;
using Carbine.Scenes;
using Mother4.Data;
using Mother4.Data.Character;
using Mother4.Data.Enemies;
using Mother4.Data.Psi;
using Mother4.Scenes;

namespace Mother4
{
	// Token: 0x02000179 RID: 377
	internal class Program
	{
		// Token: 0x060007FA RID: 2042 RVA: 0x00033248 File Offset: 0x00031448
		[STAThread]
		private static void Main(string[] args)
		{
			try
			{
				//test!!
				Engine.Initialize(args);
				AudioManager.Instance.MusicVolume = Settings.MusicVolume;
				AudioManager.Instance.EffectsVolume = Settings.EffectsVolume;
				PsiFile.Load();
				Engine.ScreenScale = 6;
				CharacterFile.Load();
				EnemyFile.Load();
				Scene newScene = new TitleScene();
				SceneManager.Instance.Push(newScene);
				while (Engine.Running)
				{
					Engine.Update();
				}
			}
			catch (Exception value)
			{
				StreamWriter streamWriter = new StreamWriter("error.log", true);
				streamWriter.WriteLine("At {0}:", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff"));
				streamWriter.WriteLine(value);
				streamWriter.WriteLine();
				streamWriter.Close();
			}
		}
	}
}
