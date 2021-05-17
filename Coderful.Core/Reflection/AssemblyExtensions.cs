namespace Coderful.Core.Reflection
{
	using System;
	using System.IO;
	using System.Reflection;

	public static class AssemblyExtensions
	{
		/// <summary>
		/// Gets text content of the assembly's embedded resource.
		/// </summary>
		/// <param name="assembly">Assembly whose embedded resource to read.</param>
		/// <param name="embeddedResourceName">Name of the embedded resource.</param>
		/// <returns>String instance.</returns>
		public static string GetEmbeddedResourceText(this Assembly assembly, string embeddedResourceName)
		{
			using (var stream = assembly.GetManifestResourceStream(embeddedResourceName))
			{
				if (stream == null)
				{
					var message = string.Format(
						"Embedded resource '{0}' cannot be found in assembly '{1}'.",
						embeddedResourceName,
						assembly.FullName);

					throw new ArgumentException(message);
				}

				using (var ms = new StreamReader(stream))
				{
					return ms.ReadToEnd();
				}
			}
		}
	}
}