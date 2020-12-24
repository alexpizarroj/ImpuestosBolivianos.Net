using System.Collections.Generic;

namespace ImpuestosBolivianos
{
    public class CodigoQrDeFactura
    {
        public CodigoQrDeFactura(string texto, IEnumerable<byte> bytesPng)
        {
            Texto = texto;
            BytesPng = bytesPng;
        }

        public string Texto { get; }

        public IEnumerable<byte> BytesPng { get; }
    }
}
