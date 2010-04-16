﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using N2.Web;
using Management.N2.Files;

namespace N2.Edit.Web.UI.Controls
{
	public class ResizedImage : Image
	{
		static Url ImageHandlerUrl = "~/N2/Files/Resize.ashx";

		public int MaxWidth { get; set; }
		public int MaxHeight { get; set; }

		public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
		{
			string url = GetResizedImageUrl(ImageUrl, MaxWidth, MaxHeight);
			writer.AddAttribute("src", url);
			base.RenderBeginTag(writer);
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (string.IsNullOrEmpty(ImageUrl))
				return;
			if (!ImagesUtility.IsImagePath(ImageUrl))
				return;

			base.Render(writer);
		}

		/// <summary>Returns the path to an image handler that resizes the given image to the appropriate size.</summary>
		/// <param name="imageUrl">The image to resize.</param>
		/// <param name="width">The maximum width.</param>
		/// <param name="height">The maximum height.</param>
		/// <returns>The path to a handler that performs resizing of the image.</returns>
		public static string GetResizedImageUrl(string imageUrl, double width, double height)
		{
			string fileExtension = VirtualPathUtility.GetExtension(Url.PathPart(imageUrl));
			
			bool isAlreadyImageHandler = string.Equals(fileExtension, ".ashx", StringComparison.OrdinalIgnoreCase);
			if (isAlreadyImageHandler) return Url.ToAbsolute(imageUrl);

			Url url = ImageHandlerUrl.SetQueryParameter("img", Url.ToAbsolute(imageUrl));
			if (width > 0) url = url.SetQueryParameter("w", (int)width);
			if (height > 0) url = url.SetQueryParameter("h", (int)height);

			return url;
		}
	}
}
