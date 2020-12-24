using System.Collections.Generic;

namespace ImpuestosBolivianos
{
    /// <summary>
    /// Representa el código QR de una factura boliviana.
    /// </summary>
    public class CodigoQrDeFactura
    {
        internal CodigoQrDeFactura(string contenido, IEnumerable<byte> bytesPng)
        {
            Contenido = contenido;
            BytesPng = bytesPng;
        }

        /// <value>El contenido del código QR.</value>
        public string Contenido { get; }

        /// <value>La secuencia de bytes de una imagen PNG del código QR.</value>
        public IEnumerable<byte> BytesPng { get; }
    }
}
