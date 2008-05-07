using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Modbus.Data;
using Modbus.Utility;
using NUnit.Framework;
using System.Linq;

namespace Modbus.UnitTests.Utility
{
	[TestFixture]
	public class CollectionUtilityFixture
	{
		[Test]
		public void SliceMiddle()
		{
			byte[] test = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.AreEqual(new byte[] { 3, 4, 5, 6, 7 }, test.Slice(2, 5).ToArray());	
		}

		[Test]
		public void SliceBeginning()
		{
			byte[] test = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.AreEqual(new byte[] { 1, 2 }, test.Slice(0, 2).ToArray());
		}

		[Test]
		public void SliceEnd()
		{
			byte[] test = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
			Assert.AreEqual(new byte[] { 9, 10 }, test.Slice(8, 2).ToArray());
		}

		[Test]
		public void SliceCollection()
		{
			Collection<bool> col = new Collection<bool>(new bool[] { true, false, false, false, true, true });
			Assert.AreEqual(new bool[] { false, false, true }, col.Slice(2, 3).ToArray());
		}

		[Test]
		public void SliceReadOnlyCollection()
		{
			ReadOnlyCollection<bool> col = new ReadOnlyCollection<bool>(new bool[] { true, false, false, false, true, true });
			Assert.AreEqual(new bool[] { false, false, true }, col.Slice(2, 3).ToArray());
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void SliceNullICollection()
		{
			ICollection<bool> col = null;
			col.Slice(1, 1).ToArray();
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void SliceNullArray()
		{
			bool[] array = null;
			array.Slice(1, 1).ToArray();
		}

		[Test]
		public void Update()
		{
			List<int> newItems = new List<int>(new int[] { 4, 5, 6 });
			List<int> destination = new List<int>(new int[] { 1, 2, 3, 7, 8, 9 });
			CollectionUtility.Update<int>(newItems, destination, 3);
			Assert.AreEqual(new int[] { 1, 2, 3, 4, 5, 6 }, destination.ToArray());
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void UpdateItemsTooLarge()
		{
			List<int> newItems = new List<int>(new int[] { 1, 2, 3, 7, 8, 9 });
			List<int> destination = new List<int>(new int[] { 4, 5, 6 });
			CollectionUtility.Update<int>(newItems, destination, 3);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void UpdateNegativeIndex()
		{
			List<int> newItems = new List<int>(new int[] { 1, 2, 3, 7, 8, 9 });
			List<int> destination = new List<int>(new int[] { 4, 5, 6 });
			CollectionUtility.Update<int>(newItems, destination, -1);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void CreateDefaultCollectionNegativeSize()
		{
			CollectionUtility.CreateDefaultCollection<RegisterCollection, ushort>(0, -1);
		}

		[Test]
		public void CreateDefaultCollection()
		{
			RegisterCollection col = CollectionUtility.CreateDefaultCollection<RegisterCollection, ushort>(3, 5);
			Assert.AreEqual(5, col.Count);
			Assert.AreEqual(new ushort[] { 3, 3, 3, 3, 3 }, col.ToArray());
		}
	}
}