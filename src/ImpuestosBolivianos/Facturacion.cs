using System;
using ImpuestosBolivianos.Internal;

namespace ImpuestosBolivianos
{
    public static class Facturacion
    {
        public static string GenerarCodigoDeControl(
            long nroAutorizacion,
            long nroFactura,
            string nitCliente,
            DateTime fecha,
            decimal importeTotal,
            string llave)
        {
            var invoice = new Invoice()
            {
                NroAutorizacion = nroAutorizacion,
                NroFactura = nroFactura,
                NitCliente = nitCliente,
                Fecha = fecha,
                ImporteTotal = importeTotal,
                LlaveDosificacion = llave
            };

            var res = new ControlCode(invoice);

            return res.Text;
        }

        public static CodigoQrDeFactura GenerarCodigoQr(
            long nroAutorizacion,
            long nroFactura,
            string nitCliente,
            DateTime fecha,
            string codigoControl,
            string nitEmisor,
            decimal importeTotal,
            decimal importeBaseCf,
            decimal importeIceIehdTasas = 0m,
            decimal importeVentasNoGravadas = 0m,
            decimal importeNoSujetoCf = 0m,
            decimal descuentosBonosRebajas = 0m)
        {
            var invoice = new Invoice()
            {
                NroAutorizacion = nroAutorizacion,
                NroFactura = nroFactura,
                NitCliente = nitCliente,
                Fecha = fecha,
                CodigoControl = codigoControl,
                NitEmisor = nitEmisor,
                ImporteTotal = importeTotal,
                ImporteBaseCf = importeBaseCf,
                ImporteIceIehdTasas = importeIceIehdTasas,
                ImporteVentasNoGravadas = importeVentasNoGravadas,
                ImporteNoSujetoCf = importeNoSujetoCf,
                DescuentosBonosRebajas = descuentosBonosRebajas
            };

            var res = new QrControlCode(invoice);

            return new CodigoQrDeFactura(res.Text, res.ToPngByteArray());
        }

        public static string PasarMontoATexto(decimal monto)
        {
            return LawConventions.StringifyInvoiceAmount(monto);
        }
    }
}
