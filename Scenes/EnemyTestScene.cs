using System;
using System.Collections.Generic;
using System.IO;
using Carbine.Graphics;
using Carbine.Input;
using Carbine.Scenes;
using Carbine.Utility;
using Mother4.Data;
using Mother4.Data.Enemies;
using Mother4.GUI;
using Rufini.Strings;
using SFML.System;

namespace Mother4.Scenes
{
	// Token: 0x02000108 RID: 264
	internal class EnemyTestScene : StandardScene
	{
		// Token: 0x06000621 RID: 1569 RVA: 0x000242C8 File Offset: 0x000224C8
		private void Initialize()
		{
			if (this.isInitialized)
			{
				return;
			}
			this.enemyData = EnemyFile.Instance.GetAllEnemyData();
			string[] array = new string[this.enemyData.Count];
			for (int i = 0; i < array.Length; i++)
			{
				string stringQualifiedName = this.enemyData[i].GetStringQualifiedName("name");
				array[i] = (StringFile.Instance.Get(stringQualifiedName).Value ?? this.enemyData[i].QualifiedName);
			}
			this.enemyList = new ScrollingList(new Vector2f(16f, 6f), 0, array, 12, (float)Fonts.Main.LineHeight, 128f, Paths.GRAPHICS + "cursor.dat");
			enemyList.UseHighlightTextColor = true;
			this.pipeline.Add(this.enemyList);
			this.SelectList(this.enemyList);
			this.isInitialized = true;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000243B0 File Offset: 0x000225B0
		private void SelectList(ScrollingList list)
		{
			if (this.selectedList != null)
			{
				// this shit sucks

				this.selectedList.Focused = false;
				this.selectedList.ShowSelectionRectangle = false;
				this.selectedList.UseHighlightTextColor = false;
			}
			this.selectedList = list;
			this.selectedList.Focused = true;
			//this.selectedList.ShowSelectionRectangle = true;
			this.selectedList.UseHighlightTextColor = true;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00024414 File Offset: 0x00022614
		private void SetEnemySprite(string spriteName)
		{
			string text = Paths.GRAPHICS + spriteName + ".dat";
			if (this.enemySprite != null)
			{
				this.pipeline.Remove(this.enemySprite);
				this.enemySprite.Dispose();
				this.enemySprite = null;
			}
			if (File.Exists(text))
			{
				this.enemySprite = new IndexedColorGraphic(text, "front", new Vector2f(232f, 45f), 0);
				this.enemySprite.Origin = VectorMath.Truncate(this.enemySprite.Size / 2f);
				this.pipeline.Add(this.enemySprite);
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x000244BC File Offset: 0x000226BC
		private void SetEnemyInfo(EnemyData data)
		{
			this.SetEnemySprite(data.SpriteName);
			if (this.infoList != null)
			{
				this.pipeline.Remove(this.infoList);
				this.infoList.Dispose();
				this.infoList = null;
			}
			string[] array = new string[25];
			int num = 0;
			array[num++] = string.Format("Behavior Type: {0}", data.AiType);
			array[num++] = string.Format("EXP: {0}", data.Experience);
			array[num++] = string.Format("Reward: ${0}", data.Reward);
			array[num++] = string.Format("BBG: {0}", data.BackgroundName);
			array[num++] = string.Format("BGM: {0}", data.MusicName);
			array[num++] = string.Format("Options: {0}", data.Options);
			array[num++] = string.Format("Mover Type: {0}", data.MoverType);
			array[num++] = string.Format("Level: {0}", data.Level);
			array[num++] = string.Format("HP: {0}", data.HP);
			array[num++] = string.Format("PP: {0}", data.PP);
			array[num++] = string.Format("Offense: {0}", data.Offense);
			array[num++] = string.Format("Defense: {0}", data.Defense);
			array[num++] = string.Format("Speed: {0}", data.Speed);
			array[num++] = string.Format("Guts: {0}", data.Guts);
			array[num++] = string.Format("IQ: {0}", data.IQ);
			array[num++] = string.Format("Luck: {0}", data.Luck);
			array[num++] = string.Format("Immunities: {0}", data.Immunities);
			array[num++] = string.Format("Electric Mod: {0:0.00}", data.ModifierElectric);
			array[num++] = string.Format("Explosive Mod: {0:0.00}", data.ModifierExplosive);
			array[num++] = string.Format("Fire Mod: {0:0.00}", data.ModifierFire);
			array[num++] = string.Format("Ice Mod: {0:0.00}", data.ModifierIce);
			array[num++] = string.Format("Nausea Mod: {0:0.00}", data.ModifierNausea);
			array[num++] = string.Format("Physical Mod: {0:0.00}", data.ModifierPhysical);
			array[num++] = string.Format("Poison Mod: {0:0.00}", data.ModifierPoison);
			array[num++] = string.Format("Water Mod: {0:0.00}", data.ModifierWater);
			this.infoList = new ScrollingList(new Vector2f(160f, 90f), 10, array, 6, (float)Fonts.Main.LineHeight, 144f, Paths.GRAPHICS + "cursor.dat");
			this.pipeline.Add(this.infoList);
			this.SelectList(this.infoList);
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00024818 File Offset: 0x00022A18
		private void ButtonPressed(InputManager sender, Button b)
		{
			if (this.selectedList == this.enemyList)
			{
				if (b == Button.A)
				{
					Console.WriteLine(this.enemyData[this.enemyList.SelectedIndex].QualifiedName);
					this.SetEnemyInfo(this.enemyData[this.enemyList.SelectedIndex]);
					return;
				}
				if (b == Button.B)
				{
					SceneManager.Instance.Pop();
					return;
				}
			}
			else if (this.selectedList == this.infoList && b == Button.B)
			{
				this.SelectList(this.enemyList);
			}
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x000248A4 File Offset: 0x00022AA4
		private void AxisPressed(InputManager sender, Vector2f axis)
		{
			if (axis.Y < 0f)
			{
				this.selectedList.SelectPrevious();
			}
			else if (axis.Y > 0f)
			{
				this.selectedList.SelectNext();
			}
			if (axis.X > 0f)
			{
				if (this.infoList != null)
				{
					this.SelectList(this.infoList);
					return;
				}
			}
			else if (axis.X < 0f)
			{
				this.SelectList(this.enemyList);
			}
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00024924 File Offset: 0x00022B24
		public override void Focus()
		{
			base.Focus();
			this.Initialize();
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			InputManager.Instance.AxisPressed += this.AxisPressed;
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x0002495E File Offset: 0x00022B5E
		public override void Unfocus()
		{
			base.Unfocus();
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
			InputManager.Instance.AxisPressed -= this.AxisPressed;
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00024992 File Offset: 0x00022B92
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.enemyList.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x040007F2 RID: 2034
		private bool isInitialized;

		// Token: 0x040007F3 RID: 2035
		private List<EnemyData> enemyData;

		// Token: 0x040007F4 RID: 2036
		private ScrollingList selectedList;

		// Token: 0x040007F5 RID: 2037
		private ScrollingList enemyList;

		// Token: 0x040007F6 RID: 2038
		private ScrollingList infoList;

		// Token: 0x040007F7 RID: 2039
		private IndexedColorGraphic enemySprite;
	}
}
