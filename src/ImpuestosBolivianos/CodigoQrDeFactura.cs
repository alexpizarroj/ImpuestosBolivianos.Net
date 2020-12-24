using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

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
