using System;
using System.Drawing;
using System.IO;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace Aepj.FacturacionBoliviana.Tests
{
    static class QrCodeReadingHelper
    {
        public static String ReadContentsFromImageStream(Stream stream)
        {
            using (var bitmap = Image.FromStream(stream) as Bitmap)
            {
                return ReadContentsFromBitmap(bitmap);
            }
        }

        public static String ReadContentsFromBitmap(Bitmap bitmap)
        {
            var luminanceSource = new BitmapLuminanceSource(bitmap);

            var binarizer = new HybridBinarizer(luminanceSource);

            var binaryBitmap = new BinaryBitmap(binarizer);

            var result = new QRCodeReader().decode(binaryBitmap)?.Text;

            return result;
        }
    }
}
