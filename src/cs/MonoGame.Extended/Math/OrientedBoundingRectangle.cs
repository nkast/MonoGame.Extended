﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended
{
    // Real-Time Collision Detection, Christer Ericson, 2005. Chapter 4.4; Bounding Volumes - Oriented Bounding Boxes (OBBs), pg 101.

    /// <summary>
    /// An oriented bounding rectangle is a rectangular block, much like a bounding rectangle
    /// <see cref="BoundingRectangle" /> but with an arbitrary orientation <see cref="Vector2" />.
    /// </summary>
    /// <seealso cref="IEquatable{T}" />
    [DebuggerDisplay($"{{{nameof(DebugDisplayString)},nq}}")]
    public struct OrientedBoundingRectangle : IEquatable<OrientedBoundingRectangle>, IShapeF
    {
        /// <summary>
        /// The centre position of this <see cref="OrientedBoundingRectangle" />.
        /// </summary>
        public Point2 Center;

        /// <summary>
        /// The distance from the <see cref="Center" /> point along both axes to any point on the boundary of this
        /// <see cref="OrientedBoundingRectangle" />.
        /// </summary>
        ///
        public Vector2 Radii;

        /// <summary>
        /// The rotation matrix <see cref="Matrix2" /> of the bounding rectangle <see cref="OrientedBoundingRectangle" />.
        /// </summary>
        public Matrix2 Orientation;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingRectangle" /> structure from the specified centre
        /// <see cref="Point2" /> and the radii <see cref="Size2" />.
        /// </summary>
        /// <param name="center">The centre <see cref="Point2" />.</param>
        /// <param name="radii">The radii <see cref="Vector2" />.</param>
        /// <param name="orientation">The orientation <see cref="Matrix2" />.</param>
        public OrientedBoundingRectangle(Point2 center, Size2 radii, Matrix2 orientation)
        {
            Center = center;
            Radii = radii;
            Orientation = orientation;
        }

        /// <summary>
        /// Gets a list of points defining the corner points of the oriented rectangle.
        /// </summary>
        public IReadOnlyList<Vector2> Points
        {
            get
            {
                var topLeft = -Radii;
                var bottomLeft = -new Vector2(Radii.X, -Radii.Y);
                var topRight = (Vector2)new Point2(Radii.X, -Radii.Y);
                var bottomRight = Radii;

                return new List<Vector2>
                    {
                        Vector2.Transform(topRight, Orientation) + Center,
                        Vector2.Transform(topLeft, Orientation) + Center,
                        Vector2.Transform(bottomLeft, Orientation) + Center,
                        Vector2.Transform(bottomRight, Orientation) + Center
                    };
            }
        }

        public Point2 Position
        {
            get => Vector2.Transform(-Radii, Orientation) + Center;
            set => throw new NotImplementedException();
        }

        public RectangleF BoundingRectangle => (RectangleF)this;

        /// <summary>
        /// Computes the <see cref="OrientedBoundingRectangle"/> from the specified <paramref name="rectangle"/>
        /// transformed by <paramref name="transformMatrix"/>.
        /// </summary>
        /// <param name="rectangle">The <see cref="OrientedBoundingRectangle"/> to transform.</param>
        /// <param name="transformMatrix">The <see cref="Matrix2"/> transformation.</param>
        /// <returns>A new <see cref="OrientedBoundingRectangle"/>.</returns>
        public static OrientedBoundingRectangle Transform(OrientedBoundingRectangle rectangle, ref Matrix2 transformMatrix)
        {
            Transform(ref rectangle, ref transformMatrix, out var result);
            return result;
        }
        
        private static void Transform(ref OrientedBoundingRectangle rectangle, ref Matrix2 transformMatrix, out OrientedBoundingRectangle result)
        {
            PrimitivesHelper.TransformOrientedBoundingRectangle(
                ref rectangle.Center,
                ref rectangle.Orientation,
                ref transformMatrix);
            result = new OrientedBoundingRectangle();
            result.Center = rectangle.Center;
            result.Radii = rectangle.Radii;
            result.Orientation = rectangle.Orientation;
        }

        /// <summary>
        /// Compares to two <see cref="OrientedBoundingRectangle"/> structures. The result specifies whether the
        /// the values of the <see cref="Center"/>, <see cref="Radii"/> and <see cref="Orientation"/> are
        /// equal.
        /// </summary>
        /// <param name="left">The left <see cref="OrientedBoundingRectangle" />.</param>
        /// <param name="right">The right <see cref="OrientedBoundingRectangle" />.</param>
        /// <returns><c>true</c> if left and right argument are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(OrientedBoundingRectangle left, OrientedBoundingRectangle right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares to two <see cref="OrientedBoundingRectangle"/> structures. The result specifies whether the
        /// the values of the <see cref="Center"/>, <see cref="Radii"/> or <see cref="Orientation"/> are
        /// unequal.
        /// </summary>
        /// <param name="left">The left <see cref="OrientedBoundingRectangle" />.</param>
        /// <param name="right">The right <see cref="OrientedBoundingRectangle" />.</param>
        /// <returns><c>true</c> if left and right argument are unequal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(OrientedBoundingRectangle left, OrientedBoundingRectangle right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Determines whether two instances of <see cref="OrientedBoundingRectangle"/> are equal.
        /// </summary>
        /// <param name="other">The other <see cref="OrientedBoundingRectangle"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="OrientedBoundingRectangle"/> is equal
        /// to the current <see cref="OrientedBoundingRectangle"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(OrientedBoundingRectangle other)
        {
            return Center.Equals(other.Center) && Radii.Equals(other.Radii) && Orientation.Equals(other.Orientation);
        }

        /// <summary>
        /// Determines whether two instances of <see cref="OrientedBoundingRectangle"/> are equal.
        /// </summary>
        /// <param name="obj">The <see cref="OrientedBoundingRectangle"/> to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="OrientedBoundingRectangle"/> is equal
        /// to the current <see cref="OrientedBoundingRectangle"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is OrientedBoundingRectangle other && Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this object instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Center, Radii, Orientation);
        }

        /// <summary>
        /// Performs an implicit conversion from a <see cref="RectangleF" /> to <see cref="OrientedBoundingRectangle" />.
        /// </summary>
        /// <param name="rectangle">The rectangle to convert.</param>
        /// <returns>The resulting <see cref="OrientedBoundingRectangle" />.</returns>
        public static explicit operator OrientedBoundingRectangle(RectangleF rectangle)
        {
            var radii = new Size2(rectangle.Width * 0.5f, rectangle.Height * 0.5f);
            var centre = new Point2(rectangle.X + radii.Width, rectangle.Y + radii.Height);

            return new OrientedBoundingRectangle(centre, radii, Matrix2.Identity);
        }

        public static explicit operator RectangleF(OrientedBoundingRectangle orientedBoundingRectangle)
        {
            var topLeft = orientedBoundingRectangle.Center - orientedBoundingRectangle.Radii;
            var rectangle = new RectangleF(topLeft, orientedBoundingRectangle.Radii * 2);
            return RectangleF.Transform(rectangle, ref orientedBoundingRectangle.Orientation);
        }

        /// <summary>
        /// See https://www.flipcode.com/archives/2D_OBB_Intersection.shtml
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool Intersects(OrientedBoundingRectangle rectangle, OrientedBoundingRectangle other)
        {
            var corners = rectangle.Points;
            var otherCorners = other.Points;
            return IntersectsOneWay(corners, otherCorners) && IntersectsOneWay(otherCorners, corners);

            bool IntersectsOneWay(IReadOnlyList<Vector2> source, IReadOnlyList<Vector2> target)
            {
                var axis = new[]
                    {
                        source[1] - source[0],
                        source[3] - source[0]
                    };
                var origin = new float[2];

                // Make the length of each axis 1/edge length so we know any
                // dot product must be less than 1 to fall within the edge.
                for (var a = 0; a < 2; a++)
                {
                    axis[a] /= axis[a].LengthSquared();
                    origin[a] = source[0].Dot(axis[a]);
                }

                for (var a = 0; a < 2; a++) {

                    var t = target[0].Dot(axis[a]);

                    // Find the extent of box 2 on axis a
                    var tMin = t;
                    var tMax = t;

                    for (var c = 1; c < 4; c++)
                    {
                        t = target[c].Dot(axis[a]);

                        if (t < tMin)
                        {
                            tMin = t;
                        }
                        else if (t > tMax)
                        {
                            tMax = t;
                        }
                    }

                    // We have to subtract off the origin

                    // See if [tMin, tMax] intersects [0, 1]
                    if (tMin > 1 + origin[a] || tMax < origin[a])
                    {
                        // There was no intersection along this dimension;
                        // the boxes cannot possibly overlap.
                        return false;
                    }
                }

                // There was no dimension along which there is no intersection.
                // Therefore the boxes overlap.
                return true;
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this <see cref="OrientedBoundingRectangle" />.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this <see cref="OrientedBoundingRectangle" />.
        /// </returns>
        public override string ToString()
        {
            return $"Centre: {Center}, Radii: {Radii}, Orientation: {Orientation}";
        }

        internal string DebugDisplayString => ToString();
    }
}
