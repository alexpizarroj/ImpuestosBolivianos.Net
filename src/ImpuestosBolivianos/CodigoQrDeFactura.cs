using System.Collections.Generic;

namespace ImpuestosBolivianos
{
    /// <summary>
    /// Representa el código QR de una factura boliviana.
    /// </summary>
    public class CodigoQrDeFactura
    {
        internal CodigoQrDeFactura(string texto, IEnumerable<byte> bytesPng)
        {
            Texto = texto;
            BytesPng = bytesPng;
        }

        /// <value>El contenido del código QR.</value>
        public string Texto { get; }

        /// <value>La secuencia de bytes de una imagen PNG del código QR.</value>
        public IEnumerable<byte> BytesPng { get; }
    }
}
