using System;

namespace ImpuestosBolivianos
{
    public static class Facturacion
    {
        public static string MakeControlCode(
            long nroAutorizacion,
            long nroFactura,
            string nitCliente,
            DateTime fecha,
            decimal monto,
            string llave)
        {
            var invoice = new Invoice()
            {
                NroAutorizacion = nroAutorizacion,
                NroFactura = nroFactura,
                NitCliente = nitCliente,
                Fecha = fecha,
                ImporteTotal = monto,
                LlaveDosificacion = llave
            };
            return new ControlCode(invoice).Text;
        }

        public static byte[] RenderPngQrCode(
            long nroAutorizacion,
            long nroFactura,
            string nitCliente,
            DateTime fecha,
            decimal importeTotal,
            string codigoControl,
            string nitEmisor,
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
                ImporteTotal = importeTotal,
                CodigoControl = codigoControl,
                NitEmisor = nitEmisor,
                ImporteBaseCf = importeBaseCf,
                ImporteIceIehdTasas = importeIceIehdTasas,
                ImporteVentasNoGravadas = importeVentasNoGravadas,
                ImporteNoSujetoCf = importeNoSujetoCf,
                DescuentosBonosRebajas = descuentosBonosRebajas
            };
            return new QrControlCode(invoice).ToPngByteArray();
        }

        public static string StringifyInvoiceAmount(decimal amount)
        {
            return LawConventions.StringifyInvoiceAmount(amount);
        }
    }
}
