﻿using NUnit.Framework;
using System;
using System.Data;
using System.Data.SqlClient;

namespace unQuery.Tests
{
	public class GetScalarTests : TestFixture
	{
		[Test]
		public void SqlCommand()
		{
			var cmd = new SqlCommand("SELECT COUNT(*) FROM Persons");

			Assert.AreEqual(5, DB.GetScalar<int>(cmd));
		}

		[Test]
		public void SqlCommand_WithParameters()
		{
			var cmd = new SqlCommand("SELECT COUNT(*) FROM Persons WHERE Age = @Age");
			cmd.Parameters.Add("@Age", SqlDbType.TinyInt).Value = (byte)55;

			Assert.AreEqual(1, DB.GetScalar<int>(cmd));
		}

		[Test]
		public void SqlCommand_WithMixedParameters()
		{
			var cmd = new SqlCommand("SELECT COUNT(*) FROM Persons WHERE Age = @Age AND 2 = @One");
			cmd.Parameters.Add("@Age", SqlDbType.TinyInt).Value = (byte)55;

			int result = DB.GetScalar<int>(cmd, new { One = 1 });

			Assert.AreEqual(0, result);
		}

		[Test]
		public void ValueWithParameter()
		{
			var result = DB.GetScalar<int>("SELECT COUNT(*) FROM Persons WHERE Age = @Age", new {
				Age = (byte)55
			});

			Assert.AreEqual(1, result);
		}

		[Test]
		public void ValueType()
		{
			var result = DB.GetScalar<int>("SELECT COUNT(*) FROM Persons");

			Assert.AreEqual(5, result);
		}

		[Test]
		public void NullNullableValueType()
		{
			var result = DB.GetScalar<DateTime?>("SELECT TOP 1 SignedUp FROM Persons WHERE SignedUp IS NULL");

			Assert.AreEqual(null, result);
		}

		[Test]
		public void NonNullNullableValueType()
		{
			var result = DB.GetScalar<DateTime?>("SELECT SignedUp FROM Persons WHERE Name = 'Daniel Gallagher'");

			Assert.AreEqual(Convert.ToDateTime("1997-11-15 21:03:54.000"), result);
		}

		[Test]
		public void NullableValueTypeWithNoRows()
		{
			Assert.Throws<NoRowsException>(() => DB.GetScalar<DateTime?>("SELECT TOP 1 SignedUp FROM Persons WHERE 1=0"));
		}

		[Test]
		public void ThrowsSqlException()
		{
			Assert.Throws<SqlException>(() => DB.Execute("x"));
		}
	}
}