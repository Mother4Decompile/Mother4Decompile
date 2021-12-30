using System;
using System.Collections.Generic;
using System.Threading;
using Carbine;
using Carbine.Graphics;
using Mother4.Data;
using SFML.System;

namespace Mother4.Scenes
{
	// Token: 0x0200010F RID: 271
	internal class SaveScene : StandardScene
	{
		// Token: 0x06000686 RID: 1670 RVA: 0x000291D0 File Offset: 0x000273D0
		public SaveScene(SaveScene.Location location, SaveProfile profile)
		{
			this.writingFrames = 32767;
			this.location = location;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00029223 File Offset: 0x00027423
		private void StartSave()
		{
			this.saveThread = new Thread(delegate()
			{
				SaveFileManager.Instance.CurrentProfile = this.profile;
				SaveFileManager.Instance.SaveFile();
				this.saveThreadDone = true;
				if (this.writingAnimationDone)
				{
					this.writingFrames = this.delta + 8;
				}
			});
			this.saveThread.Start();
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00029248 File Offset: 0x00027448
		private void Initialize()
		{
			this.StartSave();
			this.cardFront = new IndexedColorGraphic(SaveScene.POSTCARD_PATH, "front " + SaveScene.LOCATION_STRINGS[this.location], Engine.HALF_SCREEN_SIZE, 0);
			this.cardBack = new IndexedColorGraphic(SaveScene.POSTCARD_PATH, "back", Engine.HALF_SCREEN_SIZE, 0);
			this.cardBack.Visible = false;
			this.stamp = new IndexedColorGraphic(SaveScene.POSTCARD_PATH, "stamp " + SaveScene.LOCATION_STRINGS[this.location], Engine.HALF_SCREEN_SIZE + SaveScene.STAMP_OFFSET, 0);
			this.stamp.Visible = false;
			this.stamp.Rotation = 10f;
			this.flagMark = new IndexedColorGraphic(SaveScene.POSTCARD_PATH, "flag stamp", Engine.HALF_SCREEN_SIZE + SaveScene.STAMP_OFFSET, 0);
			this.flagMark.Visible = false;
			this.writing = new IndexedColorGraphic(SaveScene.POSTCARD_PATH, "writing", Engine.HALF_SCREEN_SIZE + SaveScene.WRITING_OFFSET, 0);
			this.writing.Visible = false;
			this.writing.SpeedModifier = 0f;
			this.writing.OnAnimationComplete += this.WritingAnimationDone;
			this.pipeline.Add(this.cardFront);
			this.pipeline.Add(this.cardBack);
			this.pipeline.Add(this.stamp);
			this.pipeline.Add(this.flagMark);
			this.pipeline.Add(this.writing);
			this.delta = -8;
			this.initialized = true;
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x000293F8 File Offset: 0x000275F8
		private void WritingAnimationDone(AnimatedRenderable graphic)
		{
			this.writing.Frame = (float)(this.writing.Frames - 1);
			this.writing.SpeedModifier = 0f;
			this.writingAnimationDone = true;
			if (this.saveThreadDone)
			{
				this.writingFrames = this.delta + 8;
			}
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x0002944B File Offset: 0x0002764B
		public override void Focus()
		{
			base.Focus();
			if (!this.initialized)
			{
				this.Initialize();
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x00029464 File Offset: 0x00027664
		public override void Update()
		{
			base.Update();
			if (this.initialized && !this.done)
			{
				if (this.delta >= 0 && this.delta < 19)
				{
					float x = (float)(Math.Cos(3.141592653589793 * ((double)this.delta / 19.0)) / 2.0 + 0.5);
					this.cardFront.Scale = new Vector2f(x, 1f);
				}
				if (this.delta == 19)
				{
					this.cardFront.Visible = false;
					this.cardBack.Visible = true;
				}
				if (this.delta >= 19 && this.delta < 38)
				{
					float x2 = (float)(Math.Cos(3.141592653589793 * ((double)this.delta / 19.0)) / 2.0 + 0.5);
					this.cardBack.Scale = new Vector2f(x2, 1f);
				}
				if (this.delta == 40)
				{
					this.writing.Visible = true;
					this.writing.SpeedModifier = 1f;
				}
				if (this.delta >= 40 && this.delta < 42)
				{
					ViewManager.Instance.View.Rotate(0.5f);
				}
				if (this.delta >= 42 && this.delta < 44)
				{
					ViewManager.Instance.View.Rotate(-0.5f);
				}
				if (this.delta >= this.writingFrames && this.delta < this.writingFrames + 4)
				{
					this.stamp.Visible = true;
					this.stamp.Rotation -= 2.5f;
				}
				if (this.delta == this.writingFrames + 4)
				{
					ViewManager.Instance.Reset();
				}
				if (this.delta >= this.writingFrames + 8 + 1 && this.delta < this.writingFrames + 8 + 1 + 5)
				{
					this.flagMark.Visible = true;
					ViewManager.Instance.View.Zoom(1.05f);
					ViewManager.Instance.View.Rotate(-0.5f);
				}
				if (this.delta >= this.writingFrames + 8 + 1 + 5 && this.delta < this.writingFrames + 8 + 1 + 10)
				{
					this.flagMark.Visible = true;
					ViewManager.Instance.View.Rotate(0.5f);
					ViewManager.Instance.View.Zoom(0.95f);
				}
				if (this.delta >= this.writingFrames + 8 + 1 + 10)
				{
					ViewManager.Instance.Reset();
					this.done = true;
				}
				this.delta++;
			}
		}

		// Token: 0x04000871 RID: 2161
		private const string SPRITE_CARD_FRONT = "front ";

		// Token: 0x04000872 RID: 2162
		private const string SPRITE_CARD_BACK = "back";

		// Token: 0x04000873 RID: 2163
		private const string SPRITE_STAMP = "stamp ";

		// Token: 0x04000874 RID: 2164
		private const string SPRITE_FLAG_MARK = "flag stamp";

		// Token: 0x04000875 RID: 2165
		private const string SPRITE_WRITING = "writing";

		// Token: 0x04000876 RID: 2166
		private const int WAIT_BEFORE_START_FRAMES = 8;

		// Token: 0x04000877 RID: 2167
		private const int FLIP_FRAMES = 38;

		// Token: 0x04000878 RID: 2168
		private const double FLIP_FRAMES_D = 19.0;

		// Token: 0x04000879 RID: 2169
		private const int WRITING_WAIT_FRAMES = 2;

		// Token: 0x0400087A RID: 2170
		private const int AFTER_WRITING_FRAMES = 8;

		// Token: 0x0400087B RID: 2171
		private const int STAMP_FRAMES = 4;

		// Token: 0x0400087C RID: 2172
		private const int AFTER_STAMP_FRAMES = 1;

		// Token: 0x0400087D RID: 2173
		private const int MARK_FRAMES = 5;

		// Token: 0x0400087E RID: 2174
		private const float STAMP_ROTATION = 10f;

		// Token: 0x0400087F RID: 2175
		private static readonly Dictionary<SaveScene.Location, string> LOCATION_STRINGS = new Dictionary<SaveScene.Location, string>
		{
			{
				SaveScene.Location.Belring,
				"belring"
			}
		};

		// Token: 0x04000880 RID: 2176
		private static readonly string POSTCARD_PATH = Paths.GRAPHICS + "postcard.dat";

		// Token: 0x04000881 RID: 2177
		private static readonly Vector2f STAMP_OFFSET = new Vector2f(61f, -19f);

		// Token: 0x04000882 RID: 2178
		private static readonly Vector2f WRITING_OFFSET = new Vector2f(-44f, 0f);

		// Token: 0x04000883 RID: 2179
		private SaveScene.Location location;

		// Token: 0x04000884 RID: 2180
		private SaveProfile profile;

		// Token: 0x04000885 RID: 2181
		private bool initialized;

		// Token: 0x04000886 RID: 2182
		private bool done;

		// Token: 0x04000887 RID: 2183
		private int delta;

		// Token: 0x04000888 RID: 2184
		private int writingFrames;

		// Token: 0x04000889 RID: 2185
		private bool writingAnimationDone;

		// Token: 0x0400088A RID: 2186
		private bool saveThreadDone;

		// Token: 0x0400088B RID: 2187
		private Thread saveThread;

		// Token: 0x0400088C RID: 2188
		private IndexedColorGraphic cardFront;

		// Token: 0x0400088D RID: 2189
		private IndexedColorGraphic cardBack;

		// Token: 0x0400088E RID: 2190
		private IndexedColorGraphic stamp;

		// Token: 0x0400088F RID: 2191
		private IndexedColorGraphic flagMark;

		// Token: 0x04000890 RID: 2192
		private IndexedColorGraphic writing;

		// Token: 0x02000110 RID: 272
		public enum Location
		{
			// Token: 0x04000892 RID: 2194
			Belring
		}
	}
}
