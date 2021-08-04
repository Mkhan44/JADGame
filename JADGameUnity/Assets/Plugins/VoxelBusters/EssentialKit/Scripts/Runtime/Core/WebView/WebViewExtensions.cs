using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Web view extensions.
    /// </summary>
    public static class WebViewExtensions
    {
        #region Property extension methods

        /// <summary>
        /// Sets the webview frame to full screen size.
        /// </summary>
        public static void SetFullScreen(this WebView webView)
        {
            SurrogateCoroutine.WaitUntilAndInvoke(new WaitForFixedUpdate(), () =>
            {
                Rect    newRect = new Rect(0f, 0f, Screen.width, Screen.height);
                webView.Frame   = newRect;
            });
        }

        /// <summary>
        /// Sets the frame rectangle describes the webview’s position and size in normalized coordinate system.
        /// </summary>
        public static void SetNormalizedFrame(this WebView webView, Rect normalizedRect)
        {
            Rect    rect    = new Rect()
            {
                x           = normalizedRect.x * Screen.width,
                y           = normalizedRect.y * Screen.height,
                width       = normalizedRect.width * Screen.width,
                height      = normalizedRect.height * Screen.height,
            };
            webView.Frame   = rect;
        }

        #endregion

        #region Extension methods

        public static void LoadTexture(this WebView webView, Texture2D texture, TextureEncodingFormat textureEncodingFormat)
        {
            // get texture properties
            string      mimeType, textEncodingName;
            byte[]      data    = texture.Encode(textureEncodingFormat, out mimeType, out textEncodingName);

            webView.LoadData(data, mimeType, textEncodingName);
        }

        #endregion
    }
}