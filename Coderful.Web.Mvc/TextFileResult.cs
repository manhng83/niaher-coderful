﻿namespace Coderful.Web.Mvc
{
	using System;
	using System.Text;
	using System.Web;
	using System.Web.Mvc;

	public class TextFileResult : ActionResult
	{
		public string VirtualPath { get; set; }

		public TextFileResult()
		{
		}

		public TextFileResult(string virtualPath)
		{
			this.VirtualPath = virtualPath;
		}

		public TextFileResult(string virtualPath, string contentType)
		{
			this.VirtualPath = virtualPath;
			this.ContentType = contentType;
		}

		public TextFileResult(string virtualPath, string contentType, Encoding contentEncoding)
		{
			this.VirtualPath = virtualPath;
			this.ContentType = contentType;
			this.ContentEncoding = contentEncoding;
		}

		/// <summary>
		/// Gets or sets the content encoding.
		/// </summary>
		/// <returns>
		/// The content encoding.
		/// </returns>
		public Encoding ContentEncoding { get; set; }

		/// <summary>
		/// Gets or sets the type of the content.
		/// </summary>
		/// <returns>
		///     The type of the content.
		/// </returns>
		public string ContentType { get; set; }

		/// <summary>
		/// Enables processing of the result of an action method by a custom type that inherits from the
		/// <see cref="T:System.Web.Mvc.ActionResult" /> class.
		/// </summary>
		/// <param name="context">The context within which the result is executed.</param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="context" /> parameter is null.</exception>
		public override void ExecuteResult(ControllerContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}

			if (this.VirtualPath == null)
			{
				throw new NullReferenceException("Virtual path to the target file cannot be null.");
			}
			
			HttpResponseBase response = context.HttpContext.Response;
			if (!string.IsNullOrEmpty(this.ContentType))
			{
				response.ContentType = this.ContentType;
			}
			
			if (this.ContentEncoding != null)
			{
				response.ContentEncoding = this.ContentEncoding;
			}

			var path = context.RequestContext.HttpContext.Server.MapPath(this.VirtualPath);
			var content = System.IO.File.ReadAllText(path);

			response.Write(content);
		}
	}
}