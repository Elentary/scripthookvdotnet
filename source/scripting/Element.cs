using System;
using System.Drawing;
using System.Collections.Generic;
using GTA.Native;

namespace GTA.UI
{
	public interface IElement
	{
		bool Enabled { get; set; }
		Color Color { get; set; }
		PointF Position { get; set; }
		bool Centered { get; set; }

		void Draw();
		void Draw(SizeF offset);
		void ScaledDraw();
		void ScaledDraw(SizeF offset);
	}

	public class Rectangle : IElement
	{
		public virtual bool Enabled { get; set; }
		public virtual Color Color { get; set; }
		public virtual PointF Position { get; set; }
		public virtual bool Centered { get; set; }
		public SizeF Size { get; set; }

		public Rectangle() : this(PointF.Empty, new SizeF(Screen.Width, Screen.Height), Color.Transparent, false)
		{
		}
		public Rectangle(PointF position, SizeF size) : this(position, size, Color.Transparent, false)
		{
		}
		public Rectangle(PointF position, SizeF size, Color color) : this(position, size, color, false)
		{
		}
		public Rectangle(PointF position, SizeF size, Color color, bool centered)
		{
			Enabled = true;
			Position = position;
			Size = size;
			Color = color;
			Centered = centered;
		}

		public virtual void Draw()
		{
			InternalDraw(SizeF.Empty, Screen.Width, Screen.Height);
		}
		public virtual void Draw(SizeF offset)
		{
			InternalDraw(offset, Screen.Width, Screen.Height);
		}
		public virtual void ScaledDraw()
		{
			InternalDraw(SizeF.Empty, Screen.ScaledWidth, Screen.Height);
		}
		public virtual void ScaledDraw(SizeF offset)
		{
			InternalDraw(offset, Screen.ScaledWidth, Screen.Height);
		}

		void InternalDraw(SizeF offset, float screenWidth, float screenHeight)
		{
			if (!Enabled)
			{
				return;
			}

			float w = Size.Width / screenWidth;
			float h = Size.Height / screenHeight;
			float x = (Position.X + offset.Width) / screenWidth;
			float y = (Position.Y + offset.Height) / screenHeight;

			if (!Centered)
			{
				x += w * 0.5f;
				y += h * 0.5f;
			}

			Function.Call(Hash.DRAW_RECT, x, y, w, h, Color.R, Color.G, Color.B, Color.A);
		}
	}
	public class Container : Rectangle
	{
		public List<IElement> Items { get; private set; }

		public Container()
		{
			Items = new List<IElement>();
		}
		public Container(PointF position, SizeF size) : base(position, size)
		{
			Items = new List<IElement>();
		}
		public Container(PointF position, SizeF size, Color color) : base(position, size, color)
		{
			Items = new List<IElement>();
		}
		public Container(PointF position, SizeF size, Color color, bool centered) : base(position, size, color, centered)
		{
			Items = new List<IElement>();
		}

		public override void Draw()
		{
			Draw(SizeF.Empty);
		}
		public override void Draw(SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			base.Draw(offset);

			offset += new SizeF(Position);

			if (Centered)
			{
				offset -= new SizeF(Size.Width * 0.5f, Size.Height * 0.5f);
			}

			foreach (var item in Items)
			{
				item.Draw(offset);
			}
		}
		public override void ScaledDraw()
		{
			ScaledDraw(SizeF.Empty);
		}
		public override void ScaledDraw(SizeF offset)
		{
			if (!Enabled)
			{
				return;
			}

			base.ScaledDraw(offset);

			offset += new SizeF(Position);

			if (Centered)
			{
				offset -= new SizeF(Size.Width * 0.5f, Size.Height * 0.5f);
			}

			foreach (var item in Items)
			{
				item.ScaledDraw(offset);
			}
		}
	}
}