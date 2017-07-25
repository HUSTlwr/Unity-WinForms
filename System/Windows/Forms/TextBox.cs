﻿namespace System.Windows.Forms
{
    using System.Drawing;

    public class TextBox : Control
    {
        private readonly Pen borderPen;
        private string text;

        public TextBox()
        {
            borderPen = new Pen(Color.White);
            text = string.Empty;

            BackColor = Color.FromArgb(250, 250, 250);
            BorderColor = Color.LightGray;
            BorderHoverColor = Color.FromArgb(126, 180, 234);
            ForeColor = Color.Black;
            Padding = new Padding(2, 0, 2, 0);
            Size = new Size(128, 24);
            TextAlign = HorizontalAlignment.Left;
        }

        public Color BorderColor { get; set; }
        public Color BorderHoverColor { get; set; }
        public bool Multiline { get; set; }
        public bool ReadOnly { get; set; }
        public override string Text
        {
            get { return text; }
            set
            {
                var changed = text != value;
                text = value;
                if (text == null)
                    text = string.Empty;

                if (changed)
                    OnTextChanged(EventArgs.Empty);
            }
        }
        public HorizontalAlignment TextAlign { get; set; }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            borderPen.Color = uwfHovered ? BorderHoverColor : BorderColor;

            var g = e.Graphics;
            var textX = Padding.Left;
            var textY = Padding.Top;
            var textW = Width - Padding.Horizontal;
            var textH = Height - Padding.Vertical;

            g.uwfFillRectangle(BackColor, 0, 0, Width, Height);

            if (Enabled && Focused)
            {
                string tempText;

                if (shouldFocus)
                    g.uwfFocusNext();

                if (!Multiline)
                    tempText = g.uwfDrawTextField(Text, Font, ForeColor, textX, textY, textW, textH, TextAlign);
                else
                    tempText = g.uwfDrawTextArea(Text, Font, ForeColor, textX, textY, textW, textH);

                if (shouldFocus)
                {
                    shouldFocus = false;
                    g.uwfFocus();
                }

                if (ReadOnly == false && string.Equals(Text, tempText) == false)
                    Text = tempText;
            }
            else
            {
                if (Multiline)
                    g.uwfDrawString(Text, Font, ForeColor, textX, textY, textW, textH, ContentAlignment.TopLeft);
                else
                    g.uwfDrawString(Text, Font, ForeColor, textX, textY, textW, textH, TextAlign);
            }

            g.DrawRectangle(borderPen, 0, 0, Width, Height);
        }
    }
}
