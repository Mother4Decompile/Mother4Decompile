using System;
using System.IO;
using System.Xml.Serialization;
using Carbine;
using Carbine.Graphics;
using Mother4.Data;
using Mother4.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.Background
{
	// Token: 0x020000C2 RID: 194
	internal class BattleBackground
	{
		// Token: 0x06000408 RID: 1032 RVA: 0x0001A418 File Offset: 0x00018618
		public BattleBackground(string file)
		{
			LayerParams[] array;
			using (Stream stream = File.OpenRead(file))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(LayerParams[]));
				array = (LayerParams[])xmlSerializer.Deserialize(stream);
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i].File = Paths.GRAPHICS + "BBG/" + Path.GetFileName(array[i].File);
			}
			this.Initialize(array);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x0001A4A4 File Offset: 0x000186A4
		public BattleBackground(LayerParams[] parameters)
		{
			this.Initialize(parameters);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x0001A4B4 File Offset: 0x000186B4
		private void Initialize(LayerParams[] parameters)
		{
			this.CreateLayers(parameters);
			uint num = 320U;
			uint num2 = 180U;
			this.buffers = new RenderTexture[]
			{
				new RenderTexture(num, num2),
				new RenderTexture(num, num2)
			};
			this.buffers[0].Texture.Repeated = true;
			this.buffers[1].Texture.Repeated = true;
			this.bbgVerts = new VertexArray(PrimitiveType.Quads, 4U);
			this.bbgVerts[0U] = new Vertex(new Vector2f(0f, 0f), new Vector2f(0f, 0f));
			this.bbgVerts[1U] = new Vertex(new Vector2f(num, 0f), new Vector2f(num, 0f));
			this.bbgVerts[2U] = new Vertex(new Vector2f(num, num2), new Vector2f(num, num2));
			this.bbgVerts[3U] = new Vertex(new Vector2f(0f, num2), new Vector2f(0f, num2));
			this.bbgStates = new RenderStates(BlendMode.Alpha, Transform.Identity, null, null);
			this.bbgStateTranslation = new Vector2f(0f, 0f);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001A604 File Offset: 0x00018804
		private void CreateLayers(LayerParams[] parameters)
		{
			Shader shader = new Shader(EmbeddedResources.GetStream("Mother4.Resources.bbg.vert"), EmbeddedResources.GetStream("Mother4.Resources.bbg.frag"));
			this.layers = new BackgroundLayer[parameters.Length];
			int num = this.layers.Length;
			for (int i = 0; i < num; i++)
			{
				this.layers[i] = new BackgroundLayer(shader, parameters[i]);
			}
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001A660 File Offset: 0x00018860
		public void UpdateParams(LayerParams[] parameters)
		{
			if (parameters.Length == this.layers.Length)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					this.layers[i].UpdateParameters(parameters[i]);
				}
				return;
			}
			if (parameters.Length > 0)
			{
				this.CreateLayers(parameters);
				return;
			}
			this.layers = new BackgroundLayer[0];
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001A6B4 File Offset: 0x000188B4
		public void ResetTranslation()
		{
			int num = this.layers.Length;
			for (int i = 0; i < num; i++)
			{
				this.layers[i].ResetTranslation();
			}
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001A6E4 File Offset: 0x000188E4
		public void AddTranslation(float x, float y, float xFactor, float yFactor)
		{
			int num = this.layers.Length;
			for (int i = 0; i < num; i++)
			{
				this.layers[i].AddTranslation(x, y, xFactor, yFactor);
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x0001A718 File Offset: 0x00018918
		public void SetBackgroundPosition(Vector2f position)
		{
			this.bbgStates.Transform.Translate(-this.bbgStateTranslation);
			this.bbgStates.Transform.Translate(position);
			this.bbgStateTranslation = position;
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x0001A750 File Offset: 0x00018950
		public void Draw(RenderTarget target)
		{
			int num = 0;
			int num2 = 1;
			this.buffers[0].Clear(Color.Transparent);
			this.buffers[1].Clear(Color.Transparent);
			this.bbgStates.Transform = Transform.Identity;
			this.bbgStates.Transform.Scale(1f, -1f, 160f, 90f);
			this.bbgStates.Texture = Engine.FrameBuffer.Texture;
			this.buffers[1].Draw(this.bbgVerts, this.bbgStates);
			this.bbgStates.Transform = Transform.Identity;
			this.bbgStates.Transform.Translate(ViewManager.Instance.FinalCenter - Engine.HALF_SCREEN_SIZE);
			for (int i = 0; i < this.layers.Length; i++)
			{
				num = (num + 1) % 2;
				num2 = (num2 + 1) % 2;
				this.buffers[num2].Clear(Color.Transparent);
				this.layers[i].Draw(this.buffers[num], this.buffers[num2]);
			}
			this.bbgStates.Texture = this.buffers[num2].Texture;
			target.Draw(this.bbgVerts, this.bbgStates);
		}

		// Token: 0x04000603 RID: 1539
		private BackgroundLayer[] layers;

		// Token: 0x04000604 RID: 1540
		private RenderTexture[] buffers;

		// Token: 0x04000605 RID: 1541
		private VertexArray bbgVerts;

		// Token: 0x04000606 RID: 1542
		private RenderStates bbgStates;

		// Token: 0x04000607 RID: 1543
		private Vector2f bbgStateTranslation;
	}
}
