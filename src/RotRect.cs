﻿using Microsoft.Xna.Framework;
using System;

namespace Dcrew.Spatial {
    /// <summary>A rotated rectangle.</summary>
    public struct RotRect {
        struct Line {
            public Vector2 A, B;

            public Line(Vector2 a, Vector2 b) {
                A = a;
                B = b;
            }

            public Vector2 ClosestPoint(Vector2 p) {
                var ab = B - A;
                var distance = Vector2.Dot(p - A, ab);
                if (distance < 0)
                    return A;
                var sqlen = ab.LengthSquared();
                if (distance > sqlen)
                    return B;
                return A + ab * (distance / sqlen);
            }
        }

        /// <summary>The x and y coordinates of this <see cref="RotRect"/>.</summary>
        public Vector2 XY;
        /// <summary>Size of bounds</summary>
        public Vector2 Size;
        /// <summary>The rotation (in radians) of this <see cref="RotRect"/>.</summary>
        public float Angle;
        /// <summary>The center of rotation of this <see cref="RotRect"/>.</summary>
        public Vector2 Origin;

        /// <summary>The x coordinate of this <see cref="RotRect"/>.</summary>
        public float X {
            get => XY.X;
            set => XY.X = value;
        }
        /// <summary>The y coordinate of this <see cref="RotRect"/>.</summary>
        public float Y {
            get => XY.Y;
            set => XY.Y = value;
        }
        /// <summary>The <see cref="Size.X"/> of this <see cref="RotRect"/>.</summary>
        public float Width {
            get => Size.X;
            set => Size.X = value;
        }
        /// <summary>The <see cref="Size.Y"/> coordinate of this <see cref="RotRect"/>.</summary>
        public float Height {
            get => Size.Y;
            set => Size.Y = value;
        }

        /// <summary>A <see cref="Vector2"/> located in the center of this <see cref="RotRect"/>.</summary>
        public Vector2 Center {
            get {
                float cos = MathF.Cos(Angle),
                    sin = MathF.Sin(Angle),
                    x = -Origin.X,
                    y = -Origin.Y,
                    w = Size.X + x,
                    h = Size.Y + y,
                    xcos = x * cos,
                    ycos = y * cos,
                    xsin = x * sin,
                    ysin = y * sin,
                    wcos = w * cos,
                    wsin = w * sin,
                    hcos = h * cos,
                    hsin = h * sin;
                Vector2 tl = new Vector2(xcos - ysin + XY.X, xsin + ycos + XY.Y),
                    tr = new Vector2(wcos - ysin + XY.X, wsin + ycos + XY.Y),
                    br = new Vector2(wcos - hsin + XY.X, wsin + hcos + XY.Y),
                    bl = new Vector2(xcos - hsin + XY.X, xsin + hcos + XY.Y);
                return new Vector2((tl.X + tr.X + br.X + bl.X) / 4, (tl.Y + tr.Y + br.Y + bl.Y) / 4);
            }
        }
        /// <summary>A <see cref="Rectangle"/> covering the min/max coordinates (bounds) of this <see cref="RotRect"/>.</summary>
        public Rectangle AABB {
            get {
                float cos = MathF.Cos(Angle),
                    sin = MathF.Sin(Angle),
                    x = -Origin.X,
                    y = -Origin.Y,
                    w = Size.X + x,
                    h = Size.Y + y,
                    xcos = x * cos,
                    ycos = y * cos,
                    xsin = x * sin,
                    ysin = y * sin,
                    wcos = w * cos,
                    wsin = w * sin,
                    hcos = h * cos,
                    hsin = h * sin,
                    tlx = xcos - ysin,
                    tly = xsin + ycos,
                    trx = wcos - ysin,
                    tr_y = wsin + ycos,
                    brx = wcos - hsin,
                    bry = wsin + hcos,
                    blx = xcos - hsin,
                    bly = xsin + hcos,
                    minx = tlx,
                    miny = tly,
                    maxx = minx,
                    maxy = miny;
                if (trx < minx)
                    minx = trx;
                if (brx < minx)
                    minx = brx;
                if (blx < minx)
                    minx = blx;
                if (tr_y < miny)
                    miny = tr_y;
                if (bry < miny)
                    miny = bry;
                if (bly < miny)
                    miny = bly;
                if (trx > maxx)
                    maxx = trx;
                if (brx > maxx)
                    maxx = brx;
                if (blx > maxx)
                    maxx = blx;
                if (tr_y > maxy)
                    maxy = tr_y;
                if (bry > maxy)
                    maxy = bry;
                if (bly > maxy)
                    maxy = bly;
                var r = new Rectangle((int)minx, (int)miny, (int)MathF.Ceiling(maxx - minx), (int)MathF.Ceiling(maxy - miny));
                r.Offset(XY.X, XY.Y);
                return r;
            }
        }

        /// <summary>Creates a new instance of <see cref="Rectangle"/> struct, with the specified position, width, height, angle, and origin.</summary>
        /// <param name="x">The x coordinate of the created <see cref="RotRect"/>.</param>
        /// <param name="y">The y coordinate of the created <see cref="RotRect"/>.</param>
        /// <param name="width">The <see cref="Size.X"/> of the created <see cref="RotRect"/>.</param>
        /// <param name="height">The <see cref="Size.Y"/> of the created <see cref="RotRect"/>.</param>
        /// <param name="angle">The rotation (in radians) of the created <see cref="RotRect"/>.</param>
        /// <param name="origin">The center of rotation of the created <see cref="RotRect"/>.</param>
        public RotRect(float x, float y, float width, float height, float angle = default, Vector2 origin = default) {
            XY = new Vector2(x, y);
            Size = new Vector2(width, height);
            Angle = angle;
            Origin = origin;
        }
        /// <summary>Creates a new instance of <see cref="Rectangle"/> struct, with the specified position, width, height, angle, and origin.</summary>
        /// <param name="x">The x coordinate of the created <see cref="RotRect"/>.</param>
        /// <param name="y">The y coordinate of the created <see cref="RotRect"/>.</param>
        /// <param name="size">The <see cref="Size"/> of the created <see cref="RotRect"/>.</param>
        /// <param name="angle">The rotation (in radians) of the created <see cref="RotRect"/>.</param>
        /// <param name="origin">The center of rotation of the created <see cref="RotRect"/>.</param>
        public RotRect(float x, float y, Vector2 size, float angle = default, Vector2 origin = default) : this(x, y, size.X, size.Y, angle, origin) { }
        /// <summary>Creates a new instance of <see cref="Rectangle"/> struct, with the specified position, width, height, angle, and origin.</summary>
        /// <param name="xy">The x and y coordinates of the created <see cref="RotRect"/>.</param>
        /// <param name="size">The <see cref="Size"/> of the created <see cref="RotRect"/>.</param>
        /// <param name="angle">The rotation (in radians) of the created <see cref="RotRect"/>.</param>
        /// <param name="origin">The center of rotation of the created <see cref="RotRect"/>.</param>
        public RotRect(Vector2 xy, Vector2 size, float angle = default, Vector2 origin = default) : this(xy.X, xy.Y, size.X, size.Y, angle, origin) { }
        /// <summary>Creates a new instance of <see cref="Rectangle"/> struct, with the specified position, width, height, angle, and origin.</summary>
        /// <param name="rectangle">The <see cref="Rectangle"/> to take x, y, width, and height from of the created <see cref="RotRect"/>.</param>
        /// <param name="angle">The rotation (in radians) of the created <see cref="RotRect"/>.</param>
        /// <param name="origin">The center of rotation of the created <see cref="RotRect"/>.</param>
        public RotRect(Rectangle rectangle, float angle = default, Vector2 origin = default) : this(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height, angle, origin) { }

        /// <summary>Gets whether or not the other <see cref="Rectangle"/> intersects with this rectangle.</summary>
        /// <param name="value">The other rectangle for testing.</param>
        /// <returns><c>true</c> if other <see cref="Rectangle"/> intersects with this rectangle; <c>false</c> otherwise.</returns>
        public bool Intersects(Rectangle value) => Intersects(new RotRect(value.Location.ToVector2(), value.Size.ToVector2(), 0, Vector2.Zero));
        /// <summary>Gets whether or not the other <see cref="RotRect"/> intersects with this rectangle.</summary>
        /// <param name="value">The other rectangle for testing.</param>
        /// <returns><c>true</c> if other <see cref="RotRect"/> intersects with this rectangle; <c>false</c> otherwise.</returns>
        public bool Intersects(RotRect value) => IntersectsAnyEdge(value) || value.IntersectsAnyEdge(this);

        /// <summary>Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="RotRect"/>.</summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="RotRect"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="RotRect"/>; <c>false</c> otherwise.</returns>
        public bool Contains(Vector2 value) {
            static float IsLeft(Vector2 a, Vector2 b, Vector2 p) => (b.X - a.X) * (p.Y - a.Y) - (p.X - a.X) * (b.Y - a.Y);
            float cos = MathF.Cos(Angle),
                sin = MathF.Sin(Angle),
                x = -Origin.X,
                y = -Origin.Y,
                w = Size.X + x,
                h = Size.Y + y,
                xcos = x * cos,
                ycos = y * cos,
                xsin = x * sin,
                ysin = y * sin,
                wcos = w * cos,
                wsin = w * sin,
                hcos = h * cos,
                hsin = h * sin;
            Vector2 tl = new Vector2(xcos - ysin + XY.X, xsin + ycos + XY.Y),
                tr = new Vector2(wcos - ysin + XY.X, wsin + ycos + XY.Y),
                br = new Vector2(wcos - hsin + XY.X, wsin + hcos + XY.Y),
                bl = new Vector2(xcos - hsin + XY.X, xsin + hcos + XY.Y);
            return IsLeft(tl, tr, value) > 0 && IsLeft(tr, br, value) > 0 && IsLeft(br, bl, value) > 0 && IsLeft(bl, tl, value) > 0;
        }
        /// <summary>Gets whether or not the provided <see cref="Point"/> lies within the bounds of this <see cref="RotRect"/>.</summary>
        /// <param name="value">The coordinates to check for inclusion in this <see cref="RotRect"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Point"/> lies inside this <see cref="RotRect"/>; <c>false</c> otherwise.</returns>
        public bool Contains(Point value) => Contains(value.ToVector2());
        /// <summary>Gets whether or not the provided coordinates lie within the bounds of this <see cref="RotRect"/>.</summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="RotRect"/>; <c>false</c> otherwise.</returns>
        public bool Contains(int x, int y) => Contains(new Vector2(x, y));
        /// <summary>Gets whether or not the provided coordinates lie within the bounds of this <see cref="RotRect"/>.</summary>
        /// <param name="x">The x coordinate of the point to check for containment.</param>
        /// <param name="y">The y coordinate of the point to check for containment.</param>
        /// <returns><c>true</c> if the provided coordinates lie inside this <see cref="RotRect"/>; <c>false</c> otherwise.</returns>
        public bool Contains(float x, float y) => Contains(new Vector2(x, y));
        /// <summary>Gets whether or not the provided <see cref="Rectangle"/> lies within the bounds of this <see cref="RotRect"/>.</summary>
        /// <param name="value">The <see cref="Rectangle"/> to check for inclusion in this <see cref="RotRect"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="Rectangle"/>'s bounds lie entirely inside this <see cref="RotRect"/>; <c>false</c> otherwise.</returns>
        public bool Contains(Rectangle value) => Contains(new RotRect(value.Location.ToVector2(), value.Size.ToVector2(), 0, Vector2.Zero));
        /// <summary>Gets whether or not the provided <see cref="RotRect"/> lies within the bounds of this <see cref="RotRect"/>.</summary>
        /// <param name="value">The <see cref="RotRect"/> to check for inclusion in this <see cref="RotRect"/>.</param>
        /// <returns><c>true</c> if the provided <see cref="RotRect"/>'s bounds lie entirely inside this <see cref="RotRect"/>; <c>false</c> otherwise.</returns>
        public bool Contains(RotRect value) {
            static float IsLeft(Vector2 a, Vector2 b, Vector2 p) => (b.X - a.X) * (p.Y - a.Y) - (p.X - a.X) * (b.Y - a.Y);
            static bool PointInRectangle(Vector2 x, Vector2 y, Vector2 z, Vector2 w, Vector2 p) => IsLeft(x, y, p) > 0 && IsLeft(y, z, p) > 0 && IsLeft(z, w, p) > 0 && IsLeft(w, x, p) > 0;
            float cos = MathF.Cos(Angle),
             sin = MathF.Sin(Angle),
             x = -Origin.X,
             y = -Origin.Y,
             w = Size.X + x,
             h = Size.Y + y,
             xcos = x * cos,
             ycos = y * cos,
             xsin = x * sin,
             ysin = y * sin,
             wcos = w * cos,
             wsin = w * sin,
             hcos = h * cos,
             hsin = h * sin;
            Vector2 tl2 = new Vector2(xcos - ysin + XY.X, xsin + ycos + XY.Y),
             tr2 = new Vector2(wcos - ysin + XY.X, wsin + ycos + XY.Y),
             br2 = new Vector2(wcos - hsin + XY.X, wsin + hcos + XY.Y),
             bl2 = new Vector2(xcos - hsin + XY.X, xsin + hcos + XY.Y);
            cos = MathF.Cos(value.Angle);
            sin = MathF.Sin(value.Angle);
            x = -value.Origin.X;
            y = -value.Origin.Y;
            w = value.Size.X + x;
            h = value.Size.Y + y;
            xcos = x * cos;
            ycos = y * cos;
            xsin = x * sin;
            ysin = y * sin;
            wcos = w * cos;
            wsin = w * sin;
            hcos = h * cos;
            hsin = h * sin;
            Vector2 tl = new Vector2(xcos - ysin + value.XY.X, xsin + ycos + value.XY.Y),
             tr = new Vector2(wcos - ysin + value.XY.X, wsin + ycos + value.XY.Y),
             br = new Vector2(wcos - hsin + value.XY.X, wsin + hcos + value.XY.Y),
             bl = new Vector2(xcos - hsin + value.XY.X, xsin + hcos + value.XY.Y);
            return PointInRectangle(tl2, tr2, br2, bl2, tl) && PointInRectangle(tl2, tr2, br2, bl2, tr) && PointInRectangle(tl2, tr2, br2, bl2, br) && PointInRectangle(tl2, tr2, br2, bl2, bl);
        }

        /// <summary>Adjusts the edges of this <see cref="RotRect"/> by specified horizontal and vertical amounts.</summary>
        /// <param name="horizontal">Value to adjust the left and right edges.</param>
        /// <param name="vertical">Value to adjust the top and bottom edges.</param>
        public void Inflate(float horizontal, float vertical) {
            Size = new Vector2(horizontal * 2 + Size.X, vertical * 2 + Size.Y);
            Origin = new Vector2(horizontal + Origin.X, vertical + Origin.Y);
        }
        /// <summary>Adjusts the edges of this <see cref="RotRect"/> by specified horizontal and vertical amounts.</summary>
        /// <param name="value">Value to adjust all edges.</param>
        public void Inflate(Vector2 value) => Inflate(value.X, value.Y);
        /// <summary>Adjusts the edges of this <see cref="RotRect"/> by specified horizontal and vertical amounts.</summary>
        /// <param name="value">Value to adjust all edges.</param>
        public void Inflate(Point value) => Inflate(value.X, value.Y);

        /// <summary>Changes the <see cref="XY"/> of this <see cref="RotRect"/>.</summary>
        /// <param name="offsetX">The x coordinate to add to this <see cref="RotRect"/>.</param>
        /// <param name="offsetY">The y coordinate to add to this <see cref="RotRect"/>.</param>
        public void Offset(float offsetX, float offsetY) => XY = new Vector2(XY.X + offsetX, XY.Y + offsetY);
        /// <summary>Changes the <see cref="XY"/> of this <see cref="RotRect"/>.</summary>
        /// <param name="amount">The x and y components to add to this <see cref="RotRect"/>.</param>
        public void Offset(Vector2 amount) => Offset(amount.X, amount.Y);
        /// <summary>Changes the <see cref="XY"/> of this <see cref="RotRect"/>.</summary>
        /// <param name="amount">The x and y components to add to this <see cref="RotRect"/>.</param>
        public void Offset(Point amount) => Offset(amount.X, amount.Y);

        bool IntersectsAnyEdge(RotRect rect) {
            static float IsLeft(Vector2 a, Vector2 b, Vector2 p) => (b.X - a.X) * (p.Y - a.Y) - (p.X - a.X) * (b.Y - a.Y);
            static bool PointInRectangle(Vector2 x, Vector2 y, Vector2 z, Vector2 w, Vector2 p) => IsLeft(x, y, p) > 0 && IsLeft(y, z, p) > 0 && IsLeft(z, w, p) > 0 && IsLeft(w, x, p) > 0;
            float cos = MathF.Cos(Angle),
                sin = MathF.Sin(Angle),
                x = -Origin.X,
                y = -Origin.Y,
                w = Size.X + x,
                h = Size.Y + y,
                xcos = x * cos,
                ycos = y * cos,
                xsin = x * sin,
                ysin = y * sin,
                wcos = w * cos,
                wsin = w * sin,
                hcos = h * cos,
                hsin = h * sin;
            Vector2 tl = new Vector2(xcos - ysin + XY.X, xsin + ycos + XY.Y),
               tr = new Vector2(wcos - ysin + XY.X, wsin + ycos + XY.Y),
               br = new Vector2(wcos - hsin + XY.X, wsin + hcos + XY.Y),
               bl = new Vector2(xcos - hsin + XY.X, xsin + hcos + XY.Y),
               center = new Vector2((tl.X + tr.X + br.X + bl.X) / 4, (tl.Y + tr.Y + br.Y + bl.Y) / 4);
            cos = MathF.Cos(rect.Angle);
            sin = MathF.Sin(rect.Angle);
            x = -rect.Origin.X;
            y = -rect.Origin.Y;
            w = rect.Size.X + x;
            h = rect.Size.Y + y;
            xcos = x * cos;
            ycos = y * cos;
            xsin = x * sin;
            ysin = y * sin;
            wcos = w * cos;
            wsin = w * sin;
            hcos = h * cos;
            hsin = h * sin;
            Vector2 oTl = new Vector2(xcos - ysin + rect.XY.X, xsin + ycos + rect.XY.Y),
               oTr = new Vector2(wcos - ysin + rect.XY.X, wsin + ycos + rect.XY.Y),
               oBr = new Vector2(wcos - hsin + rect.XY.X, wsin + hcos + rect.XY.Y),
               oBl = new Vector2(xcos - hsin + rect.XY.X, xsin + hcos + rect.XY.Y);
            return PointInRectangle(tl, tr, br, bl, new Line(oTl, oTr).ClosestPoint(center)) || PointInRectangle(tl, tr, br, bl, new Line(oTr, oBr).ClosestPoint(center)) || PointInRectangle(tl, tr, br, bl, new Line(oBr, oBl).ClosestPoint(center)) || PointInRectangle(tl, tr, br, bl, new Line(oBl, oTl).ClosestPoint(center));
        }
    }
}