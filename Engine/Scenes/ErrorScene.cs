using System;
using System.IO;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using SFML.Graphics;
using SFML.System;

namespace Carbine.Scenes
{
	// Token: 0x02000053 RID: 83
	internal class ErrorScene : Scene
	{
		// Token: 0x06000255 RID: 597 RVA: 0x0000D268 File Offset: 0x0000B468
		public ErrorScene(Exception ex)
		{
			StreamWriter streamWriter = new StreamWriter("error.log", true);
			streamWriter.WriteLine("At {0}:", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:fff"));
			streamWriter.WriteLine(ex);
			streamWriter.WriteLine();
			streamWriter.Close();
			Engine.ClearColor = Color.Black;
			this.title = new TextRegion(new Vector2f(16f, 8f), 0, Engine.DefaultFont, "An unhandled exception has occurred.");
			this.message = new TextRegion(new Vector2f(16f, 32f), 0, Engine.DefaultFont, "Dave is obviously an incompetent programmer.");
			this.pressenter = new TextRegion(new Vector2f(16f, 48f), 0, Engine.DefaultFont, "Press Enter/Start to exit.");
			TextRegion fuckthisgame = new TextRegion(new Vector2f(16f, 64f), 0, Engine.DefaultFont, "This game fucking sucks.");
			this.exceptionDetails = new TextRegion(new Vector2f(16f, 80f), 0, Engine.DefaultFont, string.Format("{0}\nSee error.log for more details.", ex.Message));
			this.pipeline = new RenderPipeline(Engine.FrameBuffer);
			this.pipeline.Add(this.title);
			this.pipeline.Add(this.message);
			this.pipeline.Add(this.pressenter);
			this.pipeline.Add(fuckthisgame);

			this.pipeline.Add(this.exceptionDetails);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000D3B4 File Offset: 0x0000B5B4
		public override void Focus()
		{
			base.Focus();
			ViewManager.Instance.FollowActor = null;
			ViewManager.Instance.Center = Engine.HALF_SCREEN_SIZE;
			Engine.ClearColor = Color.Black;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000D3E0 File Offset: 0x0000B5E0
		public override void Update()
		{
			base.Update();
			if (InputManager.Instance.State[Button.Start] || InputManager.Instance.State[Button.A])
			{
				InputManager.Instance.State[Button.Start] = false;
				InputManager.Instance.State[Button.A] = false;
				this.pipeline.Remove(this.title);
				this.pipeline.Remove(this.message);
				this.pipeline.Remove(this.pressenter);
				this.pipeline.Remove(this.exceptionDetails);
				SceneManager.Instance.Pop();
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000D488 File Offset: 0x0000B688
		public override void Draw()
		{
			this.pipeline.Draw();
			base.Draw();
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000D49B File Offset: 0x0000B69B
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.title.Dispose();
				this.message.Dispose();
				this.pressenter.Dispose();
				this.exceptionDetails.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x040001D5 RID: 469
		private RenderPipeline pipeline;

		// Token: 0x040001D6 RID: 470
		private TextRegion title;

		// Token: 0x040001D7 RID: 471
		private TextRegion message;

		// Token: 0x040001D8 RID: 472
		private TextRegion pressenter;

		// Token: 0x040001D9 RID: 473
		private TextRegion exceptionDetails;
	}
}
