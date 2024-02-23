using System;
using System.IO;
using System.Reflection;

namespace OSDevGrp.OSIntranet.Core.Resolvers
{
	public static class LinkerTimestampResolver
	{
		#region Methods

		public static DateTime GetLinkerTimestampUtc(Assembly assembly)
		{
			NullGuard.NotNull(assembly, nameof(assembly));

			return GetLinkerTimestampUtc(assembly.Location);
		}

		private static DateTime GetLinkerTimestampUtc(string filePath)
		{
			NullGuard.NotNullOrWhiteSpace(filePath, nameof(filePath));

			return File.GetCreationTimeUtc(filePath);
		}

		#endregion
	}
}