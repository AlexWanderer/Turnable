﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Components;
using Turnable.Utilities;
using Turnable.LevelGenerators;
using System.Collections.Generic;

namespace Tests.Utilities
{
    [TestClass]
    public class RectangleTests
    {
        [TestMethod]
        public void Constructor_GivenTwoCornersOfTheRectangle_InitializesAllProperties()
        {
            Rectangle rectangle = new Rectangle(new Position(0, 0), new Position(4, 4));

            Assert.AreEqual(new Position(0, 4), rectangle.TopLeft);
            Assert.AreEqual(new Position(4, 0), rectangle.BottomRight);
            Assert.AreEqual(5, rectangle.Width);
            Assert.AreEqual(5, rectangle.Height);

            // Check if four edges (one for each edge of the rectangle) are initialized.
            // Also check to see if the edges are ordered clockwise.
            Assert.AreEqual(4, rectangle.Edges.Count);
            Assert.AreEqual(new Position(0, 0), rectangle.Edges[0].Start);
            Assert.AreEqual(new Position(4, 0), rectangle.Edges[0].End);
            Assert.AreEqual(new Position(4, 0), rectangle.Edges[1].Start);
            Assert.AreEqual(new Position(4, 4), rectangle.Edges[1].End);
            Assert.AreEqual(new Position(4, 4), rectangle.Edges[2].Start);
            Assert.AreEqual(new Position(0, 4), rectangle.Edges[2].End);
            Assert.AreEqual(new Position(0, 4), rectangle.Edges[3].Start);
            Assert.AreEqual(new Position(0, 0), rectangle.Edges[3].End);
        }

        [TestMethod]
        public void Constructor_GivenTheTopLeftCornerAndTheWidthAndTheHeight_InitializesAllProperties()
        {
            Rectangle rectangle = new Rectangle(new Position(0, 0), 5, 5);

            Assert.AreEqual(new Position(0, 0), rectangle.TopLeft);
            Assert.AreEqual(new Position(4, 4), rectangle.BottomRight);
            Assert.AreEqual(5, rectangle.Width);
            Assert.AreEqual(5, rectangle.Height);
        }

        [TestMethod]
        public void BuildRandomRectangle_GivenSomeBounds_ReturnsARandomlySizedRectangleWithinThoseBounds()
        {
            Rectangle bounds = new Rectangle(new Position(0, 0), 5, 4);
            Rectangle rectangle = Rectangle.BuildRandomRectangle(bounds);

            Assert.IsTrue(rectangle.Width >= 0 && rectangle.Width <= 5);
            Assert.IsTrue(rectangle.Height >= 0 && rectangle.Width <= 4);
            Assert.IsTrue(bounds.Contains(rectangle));
        }

        [TestMethod]
        public void Constructor_GivenAnyTwoCorners_CorrectlyCalculatesTopLeftAndBottomRightCorner()
        {
            // TopLeft, BottomRight corners. Handled by default.

            // TopRight, BottomLeft
            Rectangle rectangle = new Rectangle(new Position(3, 1), new Position(1, 4));

            Assert.AreEqual(new Position(1, 1), rectangle.TopLeft);
            Assert.AreEqual(new Position(3, 4), rectangle.BottomRight);

            // BottomRight, TopLeft corners
            rectangle = new Rectangle(new Position(2, 2), new Position(1, 1));

            Assert.AreEqual(new Position(1, 1), rectangle.TopLeft);
            Assert.AreEqual(new Position(2, 2), rectangle.BottomRight);

            // BottomLeft, TopRight
            rectangle = new Rectangle(new Position(1, 4), new Position(4, 1));

            Assert.AreEqual(new Position(1, 1), rectangle.TopLeft);
            Assert.AreEqual(new Position(4, 4), rectangle.BottomRight);

            // Rectangles with a width of 1
            rectangle = new Rectangle(new Position(4, 4), new Position(4, 1));

            Assert.AreEqual(new Position(4, 1), rectangle.TopLeft);
            Assert.AreEqual(new Position(4, 4), rectangle.BottomRight);

            // Rectangles with a height of 1
            rectangle = new Rectangle(new Position(4, 3), new Position(1, 3));

            Assert.AreEqual(new Position(1, 3), rectangle.TopLeft);
            Assert.AreEqual(new Position(4, 3), rectangle.BottomRight);
        }

        [TestMethod]
        public void Contains_GivenAPositionWithinTheRectangleIncludingTheEdgesOfTheRectangle_ReturnsTrue()
        {
            Rectangle rectangle = new Rectangle(new Position(1, 1), 5, 4);

            Assert.IsTrue(rectangle.Contains(new Position(2, 3)));
            Assert.IsTrue(rectangle.Contains(new Position(1, 1)));
            Assert.IsTrue(rectangle.Contains(new Position(5, 1)));
            Assert.IsTrue(rectangle.Contains(new Position(1, 4)));
            Assert.IsTrue(rectangle.Contains(new Position(5, 4)));
        }

        [TestMethod]
        public void Contains_GivenAPositionOutsideTheRectangle_ReturnsFalse()
        {
            Rectangle rectangle = new Rectangle(new Position(1, 1), 5, 4);

            Assert.IsFalse(rectangle.Contains(new Position(0, 0)));
            Assert.IsFalse(rectangle.Contains(new Position(0, 1)));
            Assert.IsFalse(rectangle.Contains(new Position(6, 1)));
            Assert.IsFalse(rectangle.Contains(new Position(1, 5)));
            Assert.IsFalse(rectangle.Contains(new Position(5, 6)));
        }

        [TestMethod]
        public void Contains_GivenARectangleThatFitsIntoTheOtherRectangleEvenIfSomeEdgesOfBothRectangleTouch_ReturnsTrue()
        {
            Rectangle rectangle = new Rectangle(new Position(1, 1), 5, 4);

            // A rectangle totally within the first rectangle
            Rectangle otherRectangle = new Rectangle(new Position(2, 2), 2, 1);
            Assert.IsTrue(rectangle.Contains(otherRectangle));

            // Rectangles that share some of the edges of the first rectangle
            otherRectangle = new Rectangle(new Position(1, 1), 2, 1);
            Assert.IsTrue(rectangle.Contains(otherRectangle));
            otherRectangle = new Rectangle(new Position(3, 1), 2, 1);
            Assert.IsTrue(rectangle.Contains(otherRectangle));
            otherRectangle = new Rectangle(new Position(3, 4), 2, 1);
            Assert.IsTrue(rectangle.Contains(otherRectangle));
            otherRectangle = new Rectangle(new Position(1, 4), 2, 1);
            Assert.IsTrue(rectangle.Contains(otherRectangle));

            // Rectangle that is the exact size of the first rectangle
            otherRectangle = new Rectangle(new Position(1, 1), 5, 4);
            Assert.IsTrue(rectangle.Contains(otherRectangle));
        }

        [TestMethod]
        public void Contains_GivenARectangleThatIsPartiallyInsideAndPartiallyOutsideTheOtherRectangle_ReturnsFalse()
        {
            Rectangle rectangle = new Rectangle(new Position(1, 1), 5, 4);

            Rectangle otherRectangle = new Rectangle(new Position(0, 1), 2, 1);
            Assert.IsFalse(rectangle.Contains(otherRectangle));

            otherRectangle = new Rectangle(new Position(5, 1), 2, 1);
            Assert.IsFalse(rectangle.Contains(otherRectangle));

            otherRectangle = new Rectangle(new Position(1, 5), 2, 2);
            Assert.IsFalse(rectangle.Contains(otherRectangle));

            otherRectangle = new Rectangle(new Position(5, 5), 2, 2);
            Assert.IsFalse(rectangle.Contains(otherRectangle));
        }

        [TestMethod]
        public void Contains_GivenARectangleThatIsCompletelyOutsideTheOtherRectangle_ReturnsFalse()
        {
            Rectangle rectangle = new Rectangle(new Position(1, 1), 5, 4);

            Rectangle otherRectangle = new Rectangle(new Position(7, 7), 2, 1);
            Assert.IsFalse(rectangle.Contains(otherRectangle));
        }

        [TestMethod]
        public void Contains_GivenARectangleThatIsBiggerAndPlacedOverTheOtherRectangle_ReturnsFalse()
        {
            Rectangle rectangle = new Rectangle(new Position(1, 1), 5, 4);

            Rectangle otherRectangle = new Rectangle(new Position(0, 0), 7, 7);
            Assert.IsFalse(rectangle.Contains(otherRectangle));
        }

        [TestMethod]
        public void AreTouching_GivenTwoRectanglesThatAreTouchingEachOtherAlongOneWholeEdge_ReturnsTrue()
        {
            // * First rectangle 
            // : Second rectangle
            // First rectangle to left of second rectangle
            // *****:::::
            // *****:::::
            // *****::::: 
            // *****:::::
            // *****:::::
            Rectangle firstRectangle = new Rectangle(new Position(0, 0), new Position(4, 4));
            Rectangle secondRectangle = new Rectangle(new Position(5, 0), new Position(9, 4));

            Assert.IsTrue(firstRectangle.IsTouching(secondRectangle));

            // First rectangle to right of second rectangle
            // :::::*****
            // :::::*****
            // :::::*****
            // :::::*****
            // :::::*****
            Assert.IsTrue(firstRectangle.IsTouching(secondRectangle));

            // First rectangle above second rectangle
            // *****
            // *****
            // *****
            // *****
            // *****
            // :::::
            // :::::
            // :::::
            // :::::
            // :::::
            firstRectangle = new Rectangle(new Position(0, 5), new Position(4, 9));
            secondRectangle = new Rectangle(new Position(0, 0), new Position(4, 4));

            Assert.IsTrue(firstRectangle.IsTouching(secondRectangle));

            // First rectangle below second rectangle
            // :::::
            // :::::
            // :::::
            // :::::
            // :::::
            // *****
            // *****
            // *****
            // *****
            // *****
            Assert.IsTrue(firstRectangle.IsTouching(secondRectangle));
        }

        [TestMethod]
        public void AreTouching_GivenTwoRectanglesThatAreTouchingEachOtherAtLeastAtOnePointAlongOneEdge_ReturnsTrue()
        {
            // *****
            // *****
            // *****
            // *****
            // *****:::::
            //      :::::
            //      :::::
            //      :::::
            //      :::::
            // First rectangle to left and above second rectangle
            Rectangle firstRectangle = new Rectangle(new Position(0, 8), new Position(4, 4));
            Rectangle secondRectangle = new Rectangle(new Position(5, 0), new Position(5, 4));

            Assert.IsTrue(firstRectangle.IsTouching(secondRectangle));

            // :::::
            // :::::
            // :::::
            // :::::
            // :::::*****
            //      *****
            //      *****
            //      *****
            //      *****
            // First rectangle to right and below second rectangle
            Assert.IsTrue(firstRectangle.IsTouching(secondRectangle));

            // First rectangle to right and above second rectangle
            //      *****
            //      *****
            //      *****
            //      *****
            // :::::*****
            // :::::
            // :::::
            // :::::
            // :::::
            firstRectangle = new Rectangle(new Position(5, 8), new Position(9, 4));
            secondRectangle = new Rectangle(new Position(0, 0), new Position(4, 4));

            Assert.IsTrue(firstRectangle.IsTouching(secondRectangle));

            // First rectangle to left and below second rectangle
            //      :::::
            //      :::::
            //      :::::
            //      :::::
            // *****:::::
            // *****
            // *****
            // *****
            // *****
            Assert.IsTrue(firstRectangle.IsTouching(secondRectangle));
        }

        [TestMethod]
        public void AreTouching_GivenTwoRectanglesThatAreDiagonallyPlacedToEachOtherWithNotEvenOnePointTouchingAlongAEdge_ReturnsFalse()
        {
            // *****
            // *****
            // *****
            // *****
            // *****
            //      :::::
            //      :::::
            //      :::::
            //      :::::
            //      :::::
            // First rectangle to left and above second rectangle
            Rectangle firstRectangle = new Rectangle(new Position(0, 9), new Position(4, 5));
            Rectangle secondRectangle = new Rectangle(new Position(5, 0), new Position(9, 4));

            Assert.IsFalse(firstRectangle.IsTouching(secondRectangle));

            // :::::
            // :::::
            // :::::
            // :::::
            // :::::
            //      *****
            //      *****
            //      *****
            //      *****
            //      *****
            // First rectangle to right and below second rectangle
            Assert.IsFalse(firstRectangle.IsTouching(secondRectangle));

            // First rectangle to right and above second rectangle
            //      *****
            //      *****
            //      *****
            //      *****
            //      *****
            // :::::
            // :::::
            // :::::
            // :::::
            // :::::
            firstRectangle = new Rectangle(new Position(5, 9), new Position(9, 5));
            secondRectangle = new Rectangle(new Position(0, 0), new Position(4, 4));

            Assert.IsFalse(firstRectangle.IsTouching(secondRectangle));

            // First rectangle to left and below second rectangle
            //      :::::
            //      :::::
            //      :::::
            //      :::::
            //      :::::
            // *****
            // *****
            // *****
            // *****
            // *****
            Assert.IsFalse(firstRectangle.IsTouching(secondRectangle));
        }

        [TestMethod]
        public void AreTouching_GivenTwoRectanglesThatAreSeparatedByAtleastOnePointFromEachOther_ReturnsFalse()
        {
            // * First rectangle 
            // : Second rectangle
            // First rectangle to left of second rectangle
            // ***** :::::
            // ***** :::::
            // ***** ::::: 
            // ***** :::::
            // ***** :::::
            Rectangle firstRectangle = new Rectangle(new Position(0, 0), new Position(4, 4));
            Rectangle secondRectangle = new Rectangle(new Position(6, 0), new Position(10, 4));

            Assert.IsFalse(firstRectangle.IsTouching(secondRectangle));

            // First rectangle to right of second rectangle
            // ::::: *****
            // ::::: *****
            // ::::: *****
            // ::::: *****
            // ::::: *****
            Assert.IsFalse(firstRectangle.IsTouching(secondRectangle));

            // First rectangle above second rectangle
            // *****
            // *****
            // *****
            // *****
            // *****
            // 
            // :::::
            // :::::
            // :::::
            // :::::
            // :::::
            firstRectangle = new Rectangle(new Position(0, 6), new Position(4, 10));
            secondRectangle = new Rectangle(new Position(0, 0), new Position(4, 4));

            Assert.IsFalse(firstRectangle.IsTouching(secondRectangle));

            // First rectangle below second rectangle
            // :::::
            // :::::
            // :::::
            // :::::
            // :::::
            // 
            // *****
            // *****
            // *****
            // *****
            // *****
            Assert.IsFalse(firstRectangle.IsTouching(secondRectangle));
        }

        [TestMethod]
        public void GetFacingEdges_GivenTwoRectangles_ReturnsTheTwoEdgesThatAreClosestAndParallelToEachOther()
        {
            // * First rectangle, : Second Rectangle, F Facing edge
            // **F
            // **F
            // **F  F::
            // **F  F::
            // **F  F::
            Rectangle firstRectangle = new Rectangle(new Position(0, 0), new Position(2, 4));
            Rectangle secondRectangle = new Rectangle(new Position(5, 0), new Position(5, 2));

            List<Segment> facingEdges = firstRectangle.GetFacingEdges(secondRectangle);

            Assert.AreEqual(2, facingEdges.Count);
            Assert.AreEqual(new Position(2, 4), facingEdges[0].Start);
            Assert.AreEqual(new Position(2, 0), facingEdges[0].End);
            Assert.AreEqual(new Position(5, 0), facingEdges[1].Start);
            Assert.AreEqual(new Position(5, 2), facingEdges[1].End);

            // * First rectangle, : Second Rectangle, F Facing edge
            // *****
            // *****
            // FFFFF
            //      FFFF
            //      ::::
            firstRectangle = new Rectangle(new Position(0, 2), new Position(4, 4));
            secondRectangle = new Rectangle(new Position(5, 0), new Position(8, 1));

            facingEdges = firstRectangle.GetFacingEdges(secondRectangle);

            Assert.AreEqual(2, facingEdges.Count);
            Assert.AreEqual(new Position(2, 4), facingEdges[0].Start);
            Assert.AreEqual(new Position(2, 0), facingEdges[0].End);
            Assert.AreEqual(new Position(5, 0), facingEdges[1].Start);
            Assert.AreEqual(new Position(5, 2), facingEdges[1].End);
        }
    }
}
