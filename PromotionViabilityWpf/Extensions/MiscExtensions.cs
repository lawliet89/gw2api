using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PromotionViabilityWpf.Extensions
{
    public static class MiscExtensions
    {
        internal static BitmapImage ToImage(this byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }

        internal static T FindVisualParent<T>(this UIElement element) where T : UIElement
        {
            var parent = element;
            while (parent != null)
            {
                var correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }

                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }

        internal static IEnumerable<T> GetChildrenOfType<T>(this UIElement element) where T : UIElement
        {
            if (element == null) yield break;
            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as UIElement;
                if (child == null) continue;
                var result = child as T;
                if (result != null) yield return result;
                else
                {
                    foreach (var nested in child.GetChildrenOfType<T>())
                        yield return nested;
                }
            }
        }
    }
}
