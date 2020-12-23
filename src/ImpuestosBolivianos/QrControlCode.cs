using System.Drawing.Imaging;
using System.IO;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace ImpuestosBolivianos
{
    public class QrControlCode
    {
        private static QrCodeEncodingOptions QrCodeWriterOptions { get; }
            = new QrCodeEncodingOptions()
            {
                ErrorCorrection = ErrorCorrectionLevel.M,
                Width = 320,
                Height = 320
            };

        private static BarcodeWriter QrCodeWriter { get; }
            = new BarcodeWriter()
            {
                Format = BarcodeFormat.QR_CODE,
                Options = QrCodeWriterOptions
            };

        public QrControlCode(Invoice invoice)
        {
            invoice.AssertHasEnoughInfoToRenderQrCode();
            string nroAutorizacion = invoice.NroAutorizacion.ToString();
            string nroFactura = invoice.NroFactura.ToString();
            string fecha = LawConventions.QrControlCode.StringifyDateTime(invoice.Fecha.Value);
            string codigoControl = invoice.CodigoControl;
            string nitEmisor = invoice.NitEmisor;
            string nitCliente = invoice.NitCliente;
            string importeTotal = LawConventions.QrControlCode.StringifyDecimal(invoice.ImporteTotal);
            string importeBaseCf = LawConventions.QrControlCode.StringifyDecimal(invoice.ImporteBaseCf);
            string importeIceIehdTasas = LawConventions.QrControlCode.StringifyDecimal(invoice.ImporteIceIehdTasas);
            string importeVentasNoGravadas = LawConventions.QrControlCode.StringifyDecimal(invoice.ImporteVentasNoGravadas);
            string importeNoSujetoCf = LawConventions.QrControlCode.StringifyDecimal(invoice.ImporteNoSujetoCf);
            string descuentosBonosRebajas = LawConventions.QrControlCode.StringifyDecimal(invoice.DescuentosBonosRebajas);
            var textElements = new string[]
            {
                nitEmisor,
                nroFactura,
                nroAutorizacion,
                fecha,
                importeTotal,
                importeBaseCf,
                codigoControl,
                nitCliente,
                importeIceIehdTasas,
                importeVentasNoGravadas,
                importeNoSujetoCf,
                descuentosBonosRebajas
            };
            Text = string.Join("|", textElements);
        }

        public string Text { get; }

        public byte[] ToPngByteArray()
        {
            using (var bitmap = QrCodeWriter.Write(Text))
            {
                var mstream = new MemoryStream();
                bitmap.Save(mstream, ImageFormat.Png);
                return mstream.ToArray();
            }
        }
    }
}
