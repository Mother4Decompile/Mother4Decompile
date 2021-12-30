// Decompiled with JetBrains decompiler
// Type: Mother4.GUI.OverworldTextBox
// Assembly: Mother4, Version=0.7.6122.42121, Culture=neutral, PublicKeyToken=null
// MVID: FECD8919-57FF-4485-92CA-DA4098284AB3
// Assembly location: D:\OddityPrototypes\Mother 4 -- 2018\Mother4.exe

using Carbine;
using Carbine.Flags;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Utility;
using Mother4.Data;
using Mother4.GUI.Text;
using SFML.Graphics;
using SFML.System;
using System;

namespace Mother4.GUI
{
    internal class OverworldTextBox : TextBox
    {
        private const Button ADVANCE_BUTTON = Button.A;
        private const float TEXTBOX_ANIM_SPEED = 0.2f;
        private const float TEXTBOX_Y_OFFSET = 4f;
        private static readonly Vector2f BOX_POSITION = new Vector2f(16f, 120f);
        private static readonly Vector2f BOX_SIZE = new Vector2f(231f, 56f);
        private static readonly Vector2f TEXT_POSITION = new Vector2f(OverworldTextBox.BOX_POSITION.X + 10f, OverworldTextBox.BOX_POSITION.Y + 8f);
        private static readonly Vector2f TEXT_SIZE = new Vector2f(OverworldTextBox.BOX_SIZE.X - 31f, OverworldTextBox.BOX_SIZE.Y - 14f);
        private static readonly Vector2f NAMETAG_POSITION = new Vector2f(OverworldTextBox.BOX_POSITION.X + 3f, OverworldTextBox.BOX_POSITION.Y - 14f);
        private static readonly Vector2f NAMETEXT_POSITION = new Vector2f(OverworldTextBox.NAMETAG_POSITION.X + 5f, OverworldTextBox.NAMETAG_POSITION.Y + 1f);
        private static readonly Vector2f BUTTON_POSITION = new Vector2f((float)((double)OverworldTextBox.BOX_POSITION.X + (double)OverworldTextBox.BOX_SIZE.X - 14.0), (float)((double)OverworldTextBox.BOX_POSITION.Y + (double)OverworldTextBox.BOX_SIZE.Y - 6.0));
        protected Mother4.GUI.Nametag nametag;
        protected bool nametagVisible;
        private OverworldTextBox.AnimationState state;
        private float textboxY;
        private bool canTransitionIn;
        private bool canTransitionOut;
        private float slideProgress;
        private Shape dimmer;

        public string Nametag
        {
            get => this.nametag.Name;
            set => this.SetNametag(value);
        }

        public OverworldTextBox()
          : base(OverworldTextBox.BOX_POSITION, OverworldTextBox.BOX_SIZE, true)
        {
            this.size = OverworldTextBox.BOX_SIZE;
            this.canTransitionIn = true;
            this.canTransitionOut = true;
            this.state = OverworldTextBox.AnimationState.Hidden;
            Vector2f finalCenter = ViewManager.Instance.FinalCenter;
            Vector2f vector2f = finalCenter - ViewManager.Instance.View.Size / 2f;
            this.dimmer = (Shape)new RectangleShape(Engine.SCREEN_SIZE);
            this.dimmer.Origin = Engine.HALF_SCREEN_SIZE;
            this.dimmer.Position = finalCenter;
            this.nametag = new Mother4.GUI.Nametag(string.Empty, VectorMath.ZERO_VECTOR, 0);
            this.nametag.Visible = false;
            this.nametagVisible = false;
            InputManager.Instance.ButtonPressed += new InputManager.ButtonPressedHandler(this.ButtonPressed);
        }

        private void ButtonPressed(InputManager sender, Button b)
        {
            if (!this.isWaitingOnPlayer || b != Button.A)
                return;
            this.ContinueFromWait();
        }

        protected override void Recenter()
        {
            Vector2f vector2f = ViewManager.Instance.FinalCenter - ViewManager.Instance.View.Size / 2f;
            this.position = new Vector2f(vector2f.X + OverworldTextBox.BOX_POSITION.X, vector2f.Y + OverworldTextBox.BOX_POSITION.Y);
            this.window.Position = new Vector2f(this.position.X, this.position.Y + this.textboxY);
            this.advanceArrow.Position = new Vector2f(vector2f.X + OverworldTextBox.BUTTON_POSITION.X, vector2f.Y + OverworldTextBox.BUTTON_POSITION.Y);
            this.nametag.Position = new Vector2f(vector2f.X + OverworldTextBox.NAMETAG_POSITION.X, vector2f.Y + OverworldTextBox.NAMETAG_POSITION.Y + this.textboxY);
            this.typewriter.Position = new Vector2f(vector2f.X + OverworldTextBox.TEXT_POSITION.X, vector2f.Y + OverworldTextBox.TEXT_POSITION.Y + this.textboxY);
        }

        private void SetNametag(string namestring)
        {
            if (namestring != null && namestring.Length > 0)
            {
                this.nametag.Name = TextProcessor.ProcessReplacements(namestring);
                this.nametagVisible = true;
            }
            else
                this.nametagVisible = false;
            this.nametag.Visible = this.nametagVisible;
        }

        public override void Reset()
        {
            base.Reset();
            this.SetNametag((string)null);
        }

        private void UpdateTextboxAnimation(float amount)
        {
            this.textboxY = (float)(4.0 * (1.0 - (double)Math.Max(0.0f, Math.Min(1f, amount))));
            this.typewriter.Position = new Vector2f((float)(int)((double)ViewManager.Instance.Viewrect.Left + (double)OverworldTextBox.TEXT_POSITION.X), (float)(int)((double)ViewManager.Instance.Viewrect.Top + (double)OverworldTextBox.TEXT_POSITION.Y + (double)this.textboxY));
            this.window.Position = new Vector2f((float)(int)((double)ViewManager.Instance.Viewrect.Left + (double)OverworldTextBox.BOX_POSITION.X), (float)(int)((double)ViewManager.Instance.Viewrect.Top + (double)OverworldTextBox.BOX_POSITION.Y + (double)this.textboxY));
            this.nametag.Position = new Vector2f((float)(int)((double)ViewManager.Instance.Viewrect.Left + (double)OverworldTextBox.NAMETAG_POSITION.X), (float)(int)((double)ViewManager.Instance.Viewrect.Top + (double)OverworldTextBox.NAMETAG_POSITION.Y + (double)this.textboxY));
        }

        public override void Show()
        {
            if (this.visible)
                return;
            this.window.FrameStyle = FlagManager.Instance[4] ? WindowBox.Style.Telepathy : Settings.WindowStyle;
            this.visible = true;
            this.Recenter();
            this.window.Visible = true;
            this.typewriter.Visible = true;
            this.nametag.Visible = this.nametagVisible;
            this.state = OverworldTextBox.AnimationState.SlideIn;
            this.slideProgress = this.canTransitionIn ? 0.0f : 1f;
            this.UpdateTextboxAnimation(0.0f);
        }

        public override void Hide()
        {
            if (!this.visible)
                return;
            this.Recenter();
            this.advanceArrow.Visible = false;
            this.state = OverworldTextBox.AnimationState.SlideOut;
            this.slideProgress = this.canTransitionOut ? 1f : 0.0f;
            this.UpdateTextboxAnimation(this.slideProgress * 2f);
        }

        public void SetDimmer(float dim) => this.dimmer.FillColor = new Color((byte)0, (byte)0, (byte)0, (byte)((double)byte.MaxValue * (double)dim));

        public override void Update()
        {
            switch (this.state)
            {
                case OverworldTextBox.AnimationState.SlideIn:
                    if ((double)this.slideProgress < 1.0)
                    {
                        this.UpdateTextboxAnimation(this.slideProgress);
                        this.slideProgress += 0.2f;
                        break;
                    }
                    this.state = OverworldTextBox.AnimationState.Textbox;
                    this.UpdateTextboxAnimation(1f);
                    this.Dequeue();
                    break;
                case OverworldTextBox.AnimationState.Textbox:
                    base.Update();
                    break;
                case OverworldTextBox.AnimationState.SlideOut:
                    if ((double)this.slideProgress > 0.0)
                    {
                        this.UpdateTextboxAnimation(this.slideProgress);
                        this.slideProgress -= 0.2f;
                        break;
                    }
                    this.state = OverworldTextBox.AnimationState.Hidden;
                    this.UpdateTextboxAnimation(0.0f);
                    this.typewriter.Visible = false;
                    this.nametag.Visible = false;
                    this.window.Visible = false;
                    this.visible = false;
                    break;
            }
        }

        public override void Draw(RenderTarget target)
        {
            if (this.nametag.Visible)
                this.nametag.Draw(target);
            if (this.window.Visible)
                this.window.Draw(target);
            if (this.typewriter.Visible)
                this.typewriter.Draw(target);
            if (!this.advanceArrow.Visible)
                return;
            this.advanceArrow.Draw(target);
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    this.nametag.Dispose();
                InputManager.Instance.ButtonPressed -= new InputManager.ButtonPressedHandler(this.ButtonPressed);
            }
            base.Dispose(disposing);
        }

        private enum AnimationState
        {
            SlideIn,
            Textbox,
            SlideOut,
            Hidden,
        }
    }
}
