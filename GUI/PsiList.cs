// Decompiled with JetBrains decompiler
// Type: Mother4.GUI.PsiList
// Assembly: Mother4, Version=0.7.6122.42121, Culture=neutral, PublicKeyToken=null
// MVID: FECD8919-57FF-4485-92CA-DA4098284AB3
// Assembly location: D:\OddityPrototypes\Mother 4 -- 2018\Mother4.exe

using Carbine.Graphics;
using Mother4.Battle;
using Mother4.Data;
using Mother4.Data.Character;
using Mother4.Data.Psi;
using Rufini.Strings;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Mother4.GUI
{
    internal class PsiList : Renderable
    {
        private const string CURSOR_FILE = "cursor.dat";
        private const int PSI_GROUP_LIST_WIDTH = 50;
        private const int PSI_LABEL_LIST_LEFT_MARGIN = 16;
        private const int PSI_SYMBOL_CURSOR_MARGIN = 10;
        private const int PSI_SYMBOL_LISTS_DEFAULT_WIDTH = 55;
        private static readonly string[] PSI_GROUP_STRINGS = new string[4]
        {
      "psi.offense",
      "psi.recovery",
      "psi.support",
      "psi.other"
        };
        private static readonly Type[] PSI_GROUP_TYPES = new Type[4]
        {
      typeof (OffensePsiData),
      typeof (AssistPsiData),
      typeof (DefensePsiData),
      typeof (OtherPsiData)
        };
        private int width;
        private int rows;
        private ScrollingList psiGroupList;
        private ScrollingList[] psiLabelList;
        private ScrollingList[][] psiLevelList;
        private PsiLevel[][][] psiLevels;
        private ScrollingList selectedList;
        private int selectedLevel;

        public override Vector2f Position
        {
            get => this.position;
            set => this.SetPosition(value);
        }

        public PsiLevel? SelectedPsiLevel => this.GetPsiLevel();

        public PsiList.PanelType SelectedPanelType => this.selectedList != this.psiGroupList ? PsiList.PanelType.PsiTypePanel : PsiList.PanelType.PsiGroupPanel;

        public PsiList(Vector2f position, CharacterType character, int width, int rows, int depth)
        {
            this.position = position;
            this.width = width;
            this.rows = rows;
            this.size = new Vector2f((float)this.width, (float)(this.rows * Fonts.Main.LineHeight));
            this.depth = depth;
            this.SetupLists(character);
        }

        private Dictionary<Type, List<PsiList.PsiListItem>> CategorizePsi(
          List<PsiLevel> knownPsi)
        {
            Dictionary<PsiType, List<int>> dictionary1 = new Dictionary<PsiType, List<int>>();
            foreach (PsiLevel psiLevel in knownPsi)
            {
                List<int> intList;
                if (!dictionary1.TryGetValue(psiLevel.PsiType, out intList))
                {
                    intList = new List<int>();
                    dictionary1.Add(psiLevel.PsiType, intList);
                }
                intList.Add(psiLevel.Level);
            }
            foreach (List<int> intList in dictionary1.Values)
                intList.Sort();
            Dictionary<Type, List<PsiList.PsiListItem>> dictionary2 = new Dictionary<Type, List<PsiList.PsiListItem>>();
            foreach (KeyValuePair<PsiType, List<int>> keyValuePair in dictionary1)
            {
                PsiType key = keyValuePair.Key;
                List<int> intList = keyValuePair.Value;
                PsiData data = PsiFile.Instance.GetData(key);
                List<PsiList.PsiListItem> psiListItemList;
                if (!dictionary2.TryGetValue(data.GetType(), out psiListItemList))
                {
                    psiListItemList = new List<PsiList.PsiListItem>();
                    dictionary2.Add(data.GetType(), psiListItemList);
                }
                int[] numArray = new int[intList.Count];
                string[] strArray = new string[intList.Count];
                for (int index = 0; index < intList.Count; ++index)
                {
                    numArray[index] = intList[index];
                    strArray[index] = PsiLetters.Get((int)data.Symbols[intList[index]]);
                }
                psiListItemList.Add(new PsiList.PsiListItem()
                {
                    Label = StringFile.Instance.Get(data.Key).Value ?? string.Empty,
                    Symbols = strArray,
                    Levels = numArray,
                    PsiData = data
                });
            }
            foreach (List<PsiList.PsiListItem> psiListItemList in dictionary2.Values)
                psiListItemList.Sort((Comparison<PsiList.PsiListItem>)((x, y) =>
                {
                    int num = x.PsiData.Order - y.PsiData.Order;
                    if (num == 0)
                        x.Label.CompareTo(y.Label);
                    return num;
                }));
            this.psiLevels = new PsiLevel[PsiList.PSI_GROUP_TYPES.Length][][];
            foreach (KeyValuePair<Type, List<PsiList.PsiListItem>> keyValuePair in dictionary2)
            {
                int index1 = 0;
                for (int index2 = 0; index2 < PsiList.PSI_GROUP_TYPES.Length; ++index2)
                {
                    if (keyValuePair.Key == PsiList.PSI_GROUP_TYPES[index2])
                    {
                        index1 = index2;
                        break;
                    }
                }
                this.psiLevels[index1] = new PsiLevel[keyValuePair.Value.Count][];
                for (int index3 = 0; index3 < keyValuePair.Value.Count; ++index3)
                {
                    PsiList.PsiListItem psiListItem = keyValuePair.Value[index3];
                    this.psiLevels[index1][index3] = new PsiLevel[psiListItem.Symbols.Length];
                    for (int index4 = 0; index4 < psiListItem.Symbols.Length; ++index4)
                        this.psiLevels[index1][index3][index4] = new PsiLevel(PsiFile.Instance.GetPsiType(psiListItem.PsiData.QualifiedName), psiListItem.Levels[index4]);
                }
            }
            return dictionary2;
        }

        private void CreatePsiPageLists(
          int index,
          Dictionary<Type, List<PsiList.PsiListItem>> psiBuckets)
        {
            List<PsiList.PsiListItem> psiListItemList;
            string[] items1;
            if (psiBuckets.TryGetValue(PsiList.PSI_GROUP_TYPES[index], out psiListItemList))
            {
                items1 = new string[psiListItemList.Count];
                for (int index1 = 0; index1 < items1.Length; ++index1)
                    items1[index1] = StringFile.Instance.Get(psiListItemList[index1].PsiData.Key).Value ?? string.Empty;
            }
            else
                items1 = new string[0];
            if (items1.Length > 0)
            {
                this.psiLabelList[index] = new ScrollingList(this.psiGroupList.Position + new Vector2f(this.psiGroupList.Size.X + 16f, 0.0f), this.depth, items1, this.rows, (float)Fonts.Main.LineHeight, (float)((double)this.width - (double)this.psiGroupList.Size.X - 16.0), Paths.GRAPHICS + "cursor.dat");
                this.psiLabelList[index].ShowSelectionRectangle = false;
                this.psiLabelList[index].UseHighlightTextColor = false;
                this.psiLabelList[index].ShowCursor = false;
                this.psiLabelList[index].Focused = false;
                int val1_1 = 0;
                for (int index2 = 0; index2 < items1.Length; ++index2)
                    val1_1 = Math.Max(val1_1, psiListItemList[index2].Symbols.Length);
                this.psiLevelList[index] = new ScrollingList[val1_1];
                using (SFML.Graphics.Text text = new SFML.Graphics.Text())
                {
                    text.Font = Fonts.Main.Font;
                    text.CharacterSize = Fonts.Main.Size;
                    int num = 0;
                    for (int index3 = val1_1 - 1; index3 >= 0; --index3)
                    {
                        int val1_2 = 0;
                        string[] items2 = new string[items1.Length];
                        for (int index4 = 0; index4 < items1.Length; ++index4)
                        {
                            items2[index4] = psiListItemList[index4].Symbols.Length > index3 ? psiListItemList[index4].Symbols[index3] : string.Empty;
                            text.DisplayedString = items2[index4];
                            val1_2 = Math.Max(val1_2, (int)text.GetLocalBounds().Width + 1);
                        }
                        this.psiLevelList[index][index3] = new ScrollingList(this.psiLabelList[index].Position + new Vector2f(this.psiLabelList[index].Size.X - (float)num - (float)val1_2, 0.0f), this.depth, items2, this.rows, (float)Fonts.Main.LineHeight, 16f, Paths.GRAPHICS + "cursor.dat");
                        this.psiLevelList[index][index3].ShowSelectionRectangle = false;
                        this.psiLevelList[index][index3].UseHighlightTextColor = false;
                        this.psiLevelList[index][index3].ShowCursor = index3 == 0;
                        this.psiLevelList[index][index3].ShowArrows = false;
                        this.psiLevelList[index][index3].Focused = false;
                        num += val1_2 + 10;
                    }
                    int x = Math.Max(0, (int)this.psiLevelList[index][0].Position.X - ((int)this.psiLabelList[index].Position.X + (int)this.psiLabelList[index].Size.X - 55));
                    if (x <= 0)
                        return;
                    for (int index5 = 0; index5 < val1_1; ++index5)
                    {
                        ScrollingList scrollingList = this.psiLevelList[index][index5];
                        scrollingList.Position = scrollingList.Position - new Vector2f((float)x, 0.0f);
                    }
                }
            }
            else
            {
                this.psiLabelList[index] = (ScrollingList)null;
                this.psiLevelList[index] = (ScrollingList[])null;
            }
        }

        private void SetupLists(CharacterType character)
        {
            string[] items = new string[PsiList.PSI_GROUP_STRINGS.Length];
            for (int index = 0; index < items.Length; ++index)
                items[index] = StringFile.Instance.Get(PsiList.PSI_GROUP_STRINGS[index]).Value;
            this.psiGroupList = new ScrollingList(this.position, this.depth, items, this.rows, (float)Fonts.Main.LineHeight, 50f, Paths.GRAPHICS + "cursor.dat");
            this.selectedList = this.psiGroupList;
            StatSet stats = CharacterStats.GetStats(character);
            Dictionary<Type, List<PsiList.PsiListItem>> psiBuckets = this.CategorizePsi(CharacterFile.Instance.GetData(character).GetKnownPsi(stats.Level));
            this.psiLabelList = new ScrollingList[PsiList.PSI_GROUP_STRINGS.Length];
            this.psiLevelList = new ScrollingList[PsiList.PSI_GROUP_STRINGS.Length][];
            for (int index = 0; index < PsiList.PSI_GROUP_TYPES.Length; ++index)
                this.CreatePsiPageLists(index, psiBuckets);
        }

        private void SetPosition(Vector2f position)
        {
            Vector2f position1 = this.position;
            this.position = position;
            Vector2f vector2f = this.position - position1;
            ScrollingList psiGroupList = this.psiGroupList;
            psiGroupList.Position = psiGroupList.Position + vector2f;
            for (int index1 = 0; index1 < this.psiLabelList.Length; ++index1)
            {
                if (this.psiLabelList[index1] != null)
                {
                    ScrollingList psiLabel = this.psiLabelList[index1];
                    psiLabel.Position = psiLabel.Position + vector2f;
                }
                if (this.psiLevelList[index1] != null)
                {
                    for (int index2 = 0; index2 < this.psiLevelList[index1].Length; ++index2)
                    {
                        if (this.psiLevelList[index1][index2] != null)
                        {
                            ScrollingList scrollingList = this.psiLevelList[index1][index2];
                            scrollingList.Position = scrollingList.Position + vector2f;
                        }
                    }
                }
            }
        }

        public void Show() => this.visible = true;

        public void Hide() => this.visible = false;

        private PsiLevel? GetPsiLevel()
        {
            PsiLevel? psiLevel = new PsiLevel?();
            int selectedIndex1 = this.psiGroupList.SelectedIndex;
            if (this.selectedList == this.psiLabelList[selectedIndex1])
            {
                int selectedIndex2 = this.psiLabelList[selectedIndex1].SelectedIndex;
                psiLevel = new PsiLevel?(this.psiLevels[selectedIndex1][selectedIndex2][this.selectedLevel]);
            }
            return psiLevel;
        }

        private void ChangeSelectedLevel(int groupIndex, int newLevel)
        {
            this.psiLevelList[groupIndex][this.selectedLevel].ShowCursor = false;
            this.selectedLevel = newLevel;
            this.psiLevelList[groupIndex][this.selectedLevel].ShowCursor = true;
        }

        public void Reset()
        {
            this.SelectPsiGroupList(this.psiGroupList.SelectedIndex);
            this.psiGroupList.SelectedIndex = 0;
            for (int index1 = 0; index1 < this.psiLabelList.Length; ++index1)
            {
                if (this.psiLabelList[index1] != null)
                    this.psiLabelList[index1].SelectedIndex = 0;
                if (this.psiLevelList[index1] != null)
                {
                    for (int index2 = 0; index2 < this.psiLevelList[index1].Length; ++index2)
                    {
                        if (this.psiLevelList[index1][index2] != null)
                            this.psiLevelList[index1][index2].SelectedIndex = 0;
                    }
                }
            }
            this.ChangeSelectedLevel(this.psiGroupList.SelectedIndex, 0);
        }

        public void SelectUp()
        {
            if (this.selectedList == this.psiGroupList)
            {
                this.selectedList.SelectPrevious();
            }
            else
            {
                if (this.selectedList != this.psiLabelList[this.psiGroupList.SelectedIndex])
                    return;
                int selectedIndex = this.selectedList.SelectedIndex;
                this.selectedList.SelectPrevious();
                if (selectedIndex == this.selectedList.SelectedIndex)
                    return;
                for (int index = 0; index < this.psiLevelList[this.psiGroupList.SelectedIndex].Length; ++index)
                    this.psiLevelList[this.psiGroupList.SelectedIndex][index].SelectPrevious();
                while (this.psiLevelList[this.psiGroupList.SelectedIndex][this.selectedLevel].SelectedItem.Length == 0)
                    this.ChangeSelectedLevel(this.psiGroupList.SelectedIndex, this.selectedLevel - 1);
            }
        }

        public void SelectDown()
        {
            if (this.selectedList == this.psiGroupList)
            {
                this.selectedList.SelectNext();
            }
            else
            {
                if (this.selectedList != this.psiLabelList[this.psiGroupList.SelectedIndex])
                    return;
                int selectedIndex = this.selectedList.SelectedIndex;
                this.selectedList.SelectNext();
                if (selectedIndex == this.selectedList.SelectedIndex)
                    return;
                for (int index = 0; index < this.psiLevelList[this.psiGroupList.SelectedIndex].Length; ++index)
                    this.psiLevelList[this.psiGroupList.SelectedIndex][index].SelectNext();
                while (this.psiLevelList[this.psiGroupList.SelectedIndex][this.selectedLevel].SelectedItem.Length == 0)
                    this.ChangeSelectedLevel(this.psiGroupList.SelectedIndex, this.selectedLevel - 1);
            }
        }

        private void SelectPsiGroupList(int groupIndex)
        {
            if (this.psiLabelList[groupIndex] != null)
            {
                this.psiLabelList[groupIndex].SelectedIndex = 0;
                this.psiLabelList[groupIndex].ShowSelectionRectangle = false;
                this.psiLabelList[groupIndex].UseHighlightTextColor = false;
                this.psiLabelList[groupIndex].Focused = false;
                for (int index = 0; index < this.psiLevelList[groupIndex].Length; ++index)
                {
                    this.psiLevelList[groupIndex][index].SelectedIndex = 0;
                    this.psiLevelList[groupIndex][index].ShowSelectionRectangle = false;
                    this.psiLevelList[groupIndex][index].UseHighlightTextColor = false;
                    this.psiLevelList[groupIndex][index].Focused = false;
                }
            }
            this.selectedList = this.psiGroupList;
            this.selectedList.ShowSelectionRectangle = true;
            this.selectedList.UseHighlightTextColor = true;
            this.selectedList.ShowCursor = true;
            this.selectedList.Focused = true;
        }

        public void SelectLeft()
        {
            if (this.selectedList != this.psiLabelList[this.psiGroupList.SelectedIndex])
                return;
            if (this.selectedLevel <= 0)
                this.SelectPsiGroupList(this.psiGroupList.SelectedIndex);
            else
                this.ChangeSelectedLevel(this.psiGroupList.SelectedIndex, this.selectedLevel - 1);
        }

        private void SelectPsiLabelList(int groupIndex)
        {
            if (this.psiLabelList[groupIndex] == null)
                return;
            this.psiGroupList.ShowCursor = false;
            this.psiGroupList.Focused = false;
            this.selectedList = this.psiLabelList[groupIndex];
            this.selectedList.ShowSelectionRectangle = true;
            this.selectedList.UseHighlightTextColor = true;
            this.selectedList.Focused = true;
            this.ChangeSelectedLevel(groupIndex, 0);
            for (int index = 0; index < this.psiLevelList[groupIndex].Length; ++index)
            {
                this.psiLevelList[groupIndex][index].ShowSelectionRectangle = false;
                this.psiLevelList[groupIndex][index].UseHighlightTextColor = true;
                this.psiLevelList[groupIndex][index].Focused = true;
            }
        }

        public void SelectRight()
        {
            if (this.selectedList == this.psiGroupList)
            {
                this.SelectPsiLabelList(this.psiGroupList.SelectedIndex);
            }
            else
            {
                if (this.selectedList != this.psiLabelList[this.psiGroupList.SelectedIndex])
                    return;
                int newLevel = Math.Min(this.psiLevelList[this.psiGroupList.SelectedIndex].Length - 1, this.selectedLevel + 1);
                if (this.psiLevelList[this.psiGroupList.SelectedIndex][newLevel].SelectedItem.Length <= 0)
                    return;
                this.ChangeSelectedLevel(this.psiGroupList.SelectedIndex, newLevel);
            }
        }

        public void Accept()
        {
            if (this.selectedList != this.psiGroupList)
                return;
            this.SelectPsiLabelList(this.psiGroupList.SelectedIndex);
        }

        public void Cancel()
        {
            if (this.selectedList == this.psiGroupList)
                return;
            this.SelectPsiGroupList(this.psiGroupList.SelectedIndex);
        }

        public override void Draw(RenderTarget target)
        {
            if (this.psiGroupList.Visible)
                this.psiGroupList.Draw(target);
            if (this.psiLabelList[this.psiGroupList.SelectedIndex] == null)
                return;
            if (this.psiLabelList[this.psiGroupList.SelectedIndex].Visible)
                this.psiLabelList[this.psiGroupList.SelectedIndex].Draw(target);
            for (int index = 0; index < this.psiLevelList[this.psiGroupList.SelectedIndex].Length; ++index)
            {
                if (this.psiLevelList[this.psiGroupList.SelectedIndex][index].Visible)
                    this.psiLevelList[this.psiGroupList.SelectedIndex][index].Draw(target);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.psiGroupList.Dispose();
                for (int index1 = 0; index1 < this.psiLabelList.Length; ++index1)
                {
                    if (this.psiLabelList[index1] != null)
                        this.psiLabelList[index1].Dispose();
                    if (this.psiLevelList[index1] != null)
                    {
                        for (int index2 = 0; index2 < this.psiLevelList[index1].Length; ++index2)
                            this.psiLevelList[index1][index2].Dispose();
                    }
                }
            }
            base.Dispose(disposing);
        }

        public enum PanelType
        {
            PsiGroupPanel,
            PsiTypePanel,
        }

        private struct PsiListItem
        {
            public string Label;
            public string[] Symbols;
            public int[] Levels;
            public PsiData PsiData;
        }
    }
}
