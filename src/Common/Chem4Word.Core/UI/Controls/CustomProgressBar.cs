// ---------------------------------------------------------------------------
//  Copyright (c) 2019, The .NET Foundation.
//  This software is released under the Apache License, Version 2.0.
//  The license and further copyright text can be found in the file LICENSE.md
//  at the root directory of the distribution.
// ---------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

//If you are using this code to build a Class Library Project instead of just adding it to a Form Project then you
//will need to add a reference to System.Drawing and System.Windows.Forms for the next three Imports. You can do
//that after you create the new Class Library by going to the menu and clicking (Project) and then selecting (Add Reference...).
//Then on the (.Net) tab you can find and select (System.Drawing) and (System.Windows.Forms) to add the references.

namespace Chem4Word.Core.UI.Controls
{
    public class CustomProgressBar : Control
    {
        // Source https://code.msdn.microsoft.com/windowsdesktop/Custom-Colored-ProgressBar-a68b61de

        private Blend _bBlend = new Blend();
        private int _minimum = 0;
        private int _maximum = 100;
        private int _value = 0;
        private bool _border = true;
        private Pen _borderPen;
        private Color _borderColor = Color.Black;
        private GradiantArea _gradiantPosition;
        private Color _gradiantColor = Color.White;
        private Color _backColor = Color.DarkGray;
        private Color _progressColor = Color.Lime;
        private SolidBrush _foreColorBrush;
        private bool _showPercentage = false;
        private bool _showText = false;
        private ImageLayoutType _imageLayout = ImageLayoutType.None;
        private Bitmap _image = null;
        private bool _roundedCorners = true;
        private ProgressDir _progressDirection = ProgressDir.Horizontal;

        /// <summary>Enum of positions used for the ProgressBar`s GradiantPosition property.</summary>
        public enum GradiantArea : int
        {
            None = 0,
            Top = 1,
            Center = 2,
            Bottom = 3
        }

        /// <summary>Enum of ImageLayout types used for the ProgressBar`s ImageLayout property.</summary>
        public enum ImageLayoutType : int
        {
            None = 0,
            Center = 1,
            Stretch = 2
        }

        /// <summary>Enum of Progress Direction types used for the ProgressDirection property.</summary>
        public enum ProgressDir : int
        {
            Horizontal = 0,
            Vertical = 1
        }

        public CustomProgressBar()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
            base.TabStop = false;
            this.Size = new Size(200, 23);
            _bBlend.Positions = new float[] { 0f, 0.2f, 0.4f, 0.6f, 0.8f, 1f };
            this.GradiantPosition = GradiantArea.Top;
            base.BackColor = Color.Transparent;
            _foreColorBrush = new SolidBrush(base.ForeColor);
            _borderPen = new Pen(Color.Black);
        }

        [Category("Appearance"), Description("The foreground color of the ProgressBars text.")]
        [Browsable(true)]
        public override System.Drawing.Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                if (value == Color.Transparent)
                {
                    value = _foreColorBrush.Color;
                }
                base.ForeColor = value;
                _foreColorBrush.Color = value;
            }
        }

        [Category("Appearance"), Description("The background color of the ProgressBar.")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "DarkGray")]
        public Color BackgroundColor
        {
            get { return _backColor; }
            set
            {
                if (value == Color.Transparent)
                {
                    value = _backColor;
                }
                _backColor = value;
                this.Refresh();
            }
        }

        [Category("Appearance"), Description("The progress color of the ProgressBar.")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Lime")]
        public Color ProgressColor
        {
            get { return _progressColor; }
            set
            {
                if (value == Color.Transparent)
                {
                    value = _progressColor;
                }
                _progressColor = value;
                this.Refresh();
            }
        }

        [Category("Appearance"), Description("The gradiant highlight color of the ProgressBar.")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "White")]
        public Color GradiantColor
        {
            get { return _gradiantColor; }
            set
            {
                _gradiantColor = value;
                this.Refresh();
            }
        }

        [Category("Behavior"), Description("The minimum value of the ProgressBar.")]
        [Browsable(true)]
        [DefaultValue(0)]
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                if (value > _maximum)
                    value = _maximum - 1;
                _minimum = value;
                this.Refresh();
            }
        }

        [Category("Behavior"), Description("The maximum value of the ProgressBar.")]
        [Browsable(true)]
        [DefaultValue(100)]
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                if (value <= _minimum)
                    value = _minimum + 1;
                _maximum = value;
                this.Refresh();
            }
        }

        [Category("Behavior"), Description("The current value of the ProgressBar.")]
        [Browsable(true)]
        [DefaultValue(0)]
        public int Value
        {
            get { return _value; }
            set
            {
                if (value < _minimum)
                    value = _minimum;
                if (value > _maximum)
                    value = _maximum;
                _value = value;
                this.Refresh();
            }
        }

        [Category("Appearance"), Description("Draw a border around the ProgressBar.")]
        [Browsable(true)]
        [DefaultValue(true)]
        public bool Border
        {
            get { return _border; }
            set
            {
                _border = value;
                this.Refresh();
            }
        }

        [Category("Appearance"), Description("The color of the border around the ProgressBar.")]
        [Browsable(true)]
        [DefaultValue(typeof(Color), "Black")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (value == Color.Transparent)
                {
                    value = _borderColor;
                }
                _borderColor = value;
                _borderPen.Color = value;
                this.Refresh();
            }
        }

        [Category("Appearance"), Description("Shows the progress percentge as text in the ProgressBar.")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool ShowPercentage
        {
            get { return _showPercentage; }
            set
            {
                _showPercentage = value;
                this.Refresh();
            }
        }

        [Category("Appearance"), Description("Shows the text of the Text property in the ProgressBar.")]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool ShowText
        {
            get { return _showText; }
            set
            {
                _showText = value;
                this.Refresh();
            }
        }

        [Category("Appearance"), Description("Determins the position of the gradiant shine in the ProgressBar.")]
        [Browsable(true)]
        [DefaultValue(typeof(GradiantArea), "Top")]
        public GradiantArea GradiantPosition
        {
            get { return _gradiantPosition; }
            set
            {
                _gradiantPosition = value;
                if (value == GradiantArea.None)
                {
                    _bBlend.Factors = new float[] { 0f, 0f, 0f, 0f, 0f, 0f }; //Shine None
                }
                else if (value == GradiantArea.Top)
                {
                    _bBlend.Factors = new float[] { 0.8f, 0.7f, 0.6f, 0.4f, 0f, 0f }; //Shine Top
                }
                else if (value == GradiantArea.Center)
                {
                    _bBlend.Factors = new float[] { 0f, 0.4f, 0.6f, 0.6f, 0.4f, 0f }; //Shine Center
                }
                else
                {
                    _bBlend.Factors = new float[] { 0f, 0f, 0.4f, 0.6f, 0.7f, 0.8f }; //Shine Bottom
                }
                this.Refresh();
            }
        }

        [Category("Appearance"), Description("An image to display on the CustomProgressBar.")]
        [Browsable(true)]
        public Bitmap Image
        {
            get { return _image; }
            set
            {
                _image = value;
                this.Refresh();
            }
        }

        [Category("Appearance"), Description("Determins how the image is displayed in the CustomProgressBar.")]
        [Browsable(true)]
        [DefaultValue(typeof(ImageLayoutType), "None")]
        public ImageLayoutType ImageLayout
        {
            get { return _imageLayout; }
            set
            {
                _imageLayout = value;
                if (_image != null)
                    this.Refresh();
            }
        }

        [Category("Appearance"), Description("True to draw corners rounded. False to draw square corners.")]
        [Browsable(true)]
        [DefaultValue(true)]
        public bool RoundedCorners
        {
            get { return _roundedCorners; }
            set
            {
                _roundedCorners = value;
                this.Refresh();
            }
        }

        [Category("Appearance"), Description("Determins the direction of progress displayed in the CustomProgressBar.")]
        [Browsable(true)]
        [DefaultValue(typeof(ProgressDir), "Horizontal")]
        public ProgressDir ProgressDirection
        {
            get { return _progressDirection; }
            set
            {
                _progressDirection = value;
                this.Refresh();
            }
        }

        protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Point StartPoint = new Point(0, 0);
            Point EndPoint = new Point(0, this.Height);

            if (_progressDirection == ProgressDir.Vertical)
            {
                EndPoint = new Point(this.Width, 0);
            }

            using (GraphicsPath gp = new GraphicsPath())
            {
                Rectangle rec = new Rectangle(0, 0, this.Width, this.Height);
                int rad = Convert.ToInt32(rec.Height / 2.5);
                if (rec.Width < rec.Height)
                    rad = Convert.ToInt32(rec.Width / 2.5);

                using (LinearGradientBrush _BackColorBrush = new LinearGradientBrush(StartPoint, EndPoint, _backColor, _gradiantColor))
                {
                    _BackColorBrush.Blend = _bBlend;
                    if (_roundedCorners)
                    {
                        gp.AddArc(rec.X, rec.Y, rad, rad, 180, 90);
                        gp.AddArc(rec.Right - (rad), rec.Y, rad, rad, 270, 90);
                        gp.AddArc(rec.Right - (rad), rec.Bottom - (rad), rad, rad, 0, 90);
                        gp.AddArc(rec.X, rec.Bottom - (rad), rad, rad, 90, 90);
                        gp.CloseFigure();
                        e.Graphics.FillPath(_BackColorBrush, gp);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(_BackColorBrush, rec);
                    }
                }

                if (_value > _minimum)
                {
                    int lngth = Convert.ToInt32((double)(this.Width / (double)(_maximum - _minimum)) * _value);
                    if (_progressDirection == ProgressDir.Vertical)
                    {
                        lngth = Convert.ToInt32((double)(this.Height / (double)(_maximum - _minimum)) * _value);
                        rec.Y = rec.Height - lngth;
                        rec.Height = lngth;
                    }
                    else
                    {
                        rec.Width = lngth;
                    }

                    using (LinearGradientBrush _ProgressBrush = new LinearGradientBrush(StartPoint, EndPoint, _progressColor, _gradiantColor))
                    {
                        _ProgressBrush.Blend = _bBlend;
                        if (_roundedCorners)
                        {
                            if (_progressDirection == ProgressDir.Horizontal)
                            {
                                rec.Height -= 1;
                            }
                            else
                            {
                                rec.Width -= 1;
                            }

                            using (GraphicsPath gp2 = new GraphicsPath())
                            {
                                gp2.AddArc(rec.X, rec.Y, rad, rad, 180, 90);
                                gp2.AddArc(rec.Right - (rad), rec.Y, rad, rad, 270, 90);
                                gp2.AddArc(rec.Right - (rad), rec.Bottom - (rad), rad, rad, 0, 90);
                                gp2.AddArc(rec.X, rec.Bottom - (rad), rad, rad, 90, 90);
                                gp2.CloseFigure();
                                using (GraphicsPath gp3 = new GraphicsPath())
                                {
                                    using (Region rgn = new Region(gp))
                                    {
                                        rgn.Intersect(gp2);
                                        gp3.AddRectangles(rgn.GetRegionScans(new Matrix()));
                                    }
                                    e.Graphics.FillPath(_ProgressBrush, gp3);
                                }
                            }
                        }
                        else
                        {
                            e.Graphics.FillRectangle(_ProgressBrush, rec);
                        }
                    }
                }

                if (_image != null)
                {
                    if (_imageLayout == ImageLayoutType.Stretch)
                    {
                        e.Graphics.DrawImage(_image, 0, 0, this.Width, this.Height);
                    }
                    else if (_imageLayout == ImageLayoutType.None)
                    {
                        e.Graphics.DrawImage(_image, 0, 0);
                    }
                    else
                    {
                        int xx = Convert.ToInt32((this.Width / 2) - (_image.Width / 2));
                        int yy = Convert.ToInt32((this.Height / 2) - (_image.Height / 2));
                        e.Graphics.DrawImage(_image, xx, yy);
                    }
                }

                if (_showPercentage | _showText)
                {
                    string perc = "";
                    if (_showText)
                        perc = this.Text;
                    if (_showPercentage)
                        perc += Convert.ToString(Convert.ToInt32(((double)100 / (double)(_maximum - _minimum)) * _value)) + "%";
                    using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                    {
                        e.Graphics.DrawString(perc, this.Font, _foreColorBrush, new Rectangle(0, 0, this.Width, this.Height), sf);
                    }
                }

                if (_border)
                {
                    rec = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
                    if (_roundedCorners)
                    {
                        gp.Reset();
                        gp.AddArc(rec.X, rec.Y, rad, rad, 180, 90);
                        gp.AddArc(rec.Right - (rad), rec.Y, rad, rad, 270, 90);
                        gp.AddArc(rec.Right - (rad), rec.Bottom - (rad), rad, rad, 0, 90);
                        gp.AddArc(rec.X, rec.Bottom - (rad), rad, rad, 90, 90);
                        gp.CloseFigure();
                        e.Graphics.DrawPath(_borderPen, gp);
                    }
                    else
                    {
                        e.Graphics.DrawRectangle(_borderPen, rec);
                    }
                }
            }
        }

        protected override void OnTextChanged(System.EventArgs e)
        {
            this.Refresh();
            base.OnTextChanged(e);
        }

        protected override void Dispose(bool disposing)
        {
            _foreColorBrush.Dispose();
            _borderPen.Dispose();
            base.Dispose(disposing);
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public override System.Drawing.Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = Color.Transparent; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [System.Obsolete("BackgroundImageLayout is not implemented.", true)]
        public new ImageLayout BackgroundImageLayout
        {
            get { return base.BackgroundImageLayout; }
            set
            {
                // throw new NotImplementedException("BackgroundImageLayout is not implemented.");
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [System.Obsolete("BackgroundImage is not implemented.", true)]
        public new Image BackgroundImage
        {
            get { return null; }
            set
            {
                // throw new NotImplementedException("BackgroundImage is not implemented.");
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [System.Obsolete("TabStop is not implemented.", true)]
        public new bool TabStop
        {
            get { return false; }
            set
            {
                // throw new NotImplementedException("TabStop is not implemented.");
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [System.Obsolete("TabIndex is not implemented.", true)]
        public new int TabIndex
        {
            get { return base.TabIndex; }
            set
            {
                // throw new NotImplementedException("TabIndex is not implemented.");
            }
        }
    }
}