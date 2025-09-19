using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;

namespace Core.Utility
{
    public enum ProgressBarDisplayMode
    {
        NoText,
        Percentage,
        CurrProgress,
        CustomText,
        TextAndPercentage,
        TextAndCurrProgress
    }
    public class CProgressBar : ProgressBar
    {
        [Description("Font of the text on ProgressBar"), Category("Additional Option")]

        public Font TextFont { get; set; } = new Font("GalanoGrotesque-SemiBold", 13, FontStyle.Bold);
        public string sValue = string.Empty;
        private SolidBrush TextColorBrush = (SolidBrush)Brushes.Black;
        [Category("Additional Options")]

        public Color TextColor
        {
            get
            {
                return TextColorBrush.Color;
            }

            set
            {
                TextColorBrush.Dispose();
                TextColorBrush = new SolidBrush(value);
            }
        }

        private SolidBrush ProgressColorBrush = (SolidBrush)Brushes.LightGreen;
        [Category("Additional Options"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public Color ProgressColor
        {
            get
            {
                return ProgressColorBrush.Color;
            }

            set
            {
                ProgressColorBrush.Dispose();
                ProgressColorBrush = new SolidBrush(value);
            }
        }

        private ProgressBarDisplayMode VisualMode = ProgressBarDisplayMode.CurrProgress;
        [Category("Additional Options"), Browsable(true)]
        public ProgressBarDisplayMode visualMode
        {
            get
            {
                return VisualMode;
            }

            set
            {
                visualMode = value;
                Invalidate();
            }
        }

        private string _Text = string.Empty;
        [Description("If it's empty, % will be shown"), Category("Addtional Options"), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public string CustomText
        {
            get
            {
                return _Text;
            }

            set
            {
                _Text = value;
                Invalidate();
            }
        }

        private string TextToDraw
        {
            get
            {
                string text = CustomText;

                switch (visualMode)
                {
                    case (ProgressBarDisplayMode.Percentage):
                        text = PercentageStr;

                        break;

                    case (ProgressBarDisplayMode.CurrProgress):
                        text = CurrProgressStr;

                        break;

                    case (ProgressBarDisplayMode.TextAndCurrProgress):
                        text = $"{CustomText}: {CurrProgressStr}";

                        break;

                    case (ProgressBarDisplayMode.TextAndPercentage):
                        text = $"{CustomText}: {PercentageStr}";

                        break;
                }

                return text;
            }
        }

        private string PercentageStr => $"{(int)((float)Value - Minimum) / ((float)Maximum - Minimum) * 100} %";
        private string CurrProgressStr => $"{sValue}%";

        public CProgressBar()
        {
            this.Value = Minimum;
            FixComponentBlinking();
        }

        private void FixComponentBlinking()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void DrawProgressBar(Graphics g)
        {
            try
            {
                Rectangle Rect = ClientRectangle;

                ProgressBarRenderer.DrawHorizontalBar(g, Rect);

                Rect.Inflate(-3, -3);

                if (Value > 0)
                {
                    Rectangle clip = new Rectangle(Rect.X, Rect.Y, (int)Math.Round(((float)Value / Maximum) * Rect.Width), Rect.Height);
                    g.FillRectangle(ProgressColorBrush, clip);
                }
            }
            catch(Exception e)
            {
                //Error 발생 확인 필요 240404
            }

            
        }

        private void DrawStringIfNeeded(Graphics g)
        {
            if (visualMode != ProgressBarDisplayMode.NoText)
            {
                string text = TextToDraw;
                SizeF len = g.MeasureString(text, TextFont);
                Point Location = new Point(((Width / 2) - (int)len.Width / 2), ((Height / 2) - (int)len.Height / 2));

                g.DrawString(text, TextFont, (Brush)TextColorBrush, Location);
            }
        }

        public new void Dispose()
        {
            TextColorBrush.Dispose();
            ProgressColorBrush.Dispose();
            base.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            DrawProgressBar(g);

            DrawStringIfNeeded(g);
        }
    }
}
