using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Carbine.GUI;
using Mother4.Data;
using Mother4.Data.Psi;
using Mother4.GUI;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Battle.UI
{
	// Token: 0x02000017 RID: 23
	internal class BattlePsiBox : Renderable
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00003B7C File Offset: 0x00001D7C
		public bool HasSelection
		{
			get
			{
				return this.currentPsiList != null && this.currentPsiList.SelectedPsiLevel != null;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00003BA6 File Offset: 0x00001DA6
		public PsiLevel? SelectedPsi
		{
			get
			{
				return this.GetSelectedPsi();
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003BB0 File Offset: 0x00001DB0
		public BattlePsiBox(CharacterType[] partyCharacters)
		{
			this.depth = 2147450880;
			this.position = BattlePsiBox.BOX_POSITION;
			this.size = BattlePsiBox.BOX_SIZE;
			this.windowBox = new WindowBox(WindowBox.Style.Normal, Settings.WindowFlavor, this.position, this.size, 0);
			this.windowBox.Visible = false;
			this.psiListDict = new Dictionary<CharacterType, PsiList>();
			for (int i = 0; i < partyCharacters.Length; i++)
			{
				this.AddCharacter(partyCharacters[i]);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003C3C File Offset: 0x00001E3C
		private PsiLevel? GetSelectedPsi()
		{
			PsiLevel? result = null;
			if (this.currentPsiList != null)
			{
				result = this.currentPsiList.SelectedPsiLevel;
			}
			return result;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00003C68 File Offset: 0x00001E68
		private PsiList AddCharacter(CharacterType character)
		{
			PsiList psiList = new PsiList(this.position + BattlePsiBox.PSI_LIST_OFFSET, character, (int)this.size.X - 24, 3, 0);
			psiList.Visible = false;
			this.psiListDict.Add(character, psiList);
			return psiList;
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00003CB2 File Offset: 0x00001EB2
		public void Show(CharacterType character)
		{
			if (!this.psiListDict.TryGetValue(character, out this.currentPsiList))
			{
				this.currentPsiList = this.AddCharacter(character);
			}
			this.windowBox.Visible = true;
			this.currentPsiList.Show();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003CEC File Offset: 0x00001EEC
		public void Hide()
		{
			this.windowBox.Visible = false;
			if (this.currentPsiList != null)
			{
				this.currentPsiList.Reset();
				this.currentPsiList.Hide();
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003D18 File Offset: 0x00001F18
		public void SelectUp()
		{
			this.currentPsiList.SelectUp();
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003D25 File Offset: 0x00001F25
		public void SelectDown()
		{
			this.currentPsiList.SelectDown();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003D32 File Offset: 0x00001F32
		public void SelectLeft()
		{
			this.currentPsiList.SelectLeft();
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003D3F File Offset: 0x00001F3F
		public void SelectRight()
		{
			this.currentPsiList.SelectRight();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003D4C File Offset: 0x00001F4C
		public void Accept()
		{
			this.currentPsiList.Accept();
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003D59 File Offset: 0x00001F59
		public void Cancel()
		{
			this.currentPsiList.Cancel();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003D66 File Offset: 0x00001F66
		public override void Draw(RenderTarget target)
		{
			if (this.windowBox.Visible)
			{
				this.windowBox.Draw(target);
			}
			if (this.currentPsiList != null && this.currentPsiList.Visible)
			{
				this.currentPsiList.Draw(target);
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003DA4 File Offset: 0x00001FA4
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.windowBox.Dispose();
					foreach (PsiList psiList in this.psiListDict.Values)
					{
						psiList.Dispose();
					}
				}
				this.currentPsiList = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x040000F3 RID: 243
		private const int DEPTH = 2147450880;

		// Token: 0x040000F4 RID: 244
		private static readonly Vector2f BOX_SIZE = new Vector2f(248f, 58f);

		// Token: 0x040000F5 RID: 245
		private static readonly Vector2f BOX_POSITION = new Vector2f(160f - BattlePsiBox.BOX_SIZE.X / 2f, 0f);

		// Token: 0x040000F6 RID: 246
		private static readonly Vector2f PSI_LIST_OFFSET = new Vector2f(16f, 8f);

		// Token: 0x040000F7 RID: 247
		private WindowBox windowBox;

		// Token: 0x040000F8 RID: 248
		private PsiList currentPsiList;

		// Token: 0x040000F9 RID: 249
		private Dictionary<CharacterType, PsiList> psiListDict;
	}
}
