using System;
using ImpuestosBolivianos.Internal;

namespace ImpuestosBolivianos
{
    /// <summary>
    /// Clase con operaciones utilitarias estáticas relacionadas con facturación.
    /// </summary>
    public static class Facturacion
    {
        /// <summary>
        /// Generar un código de control para los datos de facturación dados.
        /// </summary>
        /// <param name="nroAutorizacion">El número otorgado por la Administración Tributaria para identificar la dosificación.</param>
        /// <param name="nroFactura">El número correlativo de la factura.</param>
        /// <param name="nitCliente">El NIT del comprador. En caso de no contar con él, se debe consignar el número de Cédula de Identidad (CI), Carnet de Extranjería (CEX) o el carácter cero (0).</param>
        /// <param name="fecha">La fecha de emisión de la factura.</param>
        /// <param name="importeTotal">El monto total consignado en la factura.</param>
        /// <param name="llave">La llave de dosificación.</param>
        /// <returns>Un código de control.</returns>
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

        /// <summary>
        /// Generar un código de control para los datos de facturación dados.
        /// </summary>
        /// <param name="nroAutorizacion">El número otorgado por la Administración Tributaria para identificar la dosificación.</param>
        /// <param name="nroFactura">El número correlativo de la factura.</param>
        /// <param name="nitCliente">El NIT del comprador. En caso de no contar con él, se debe consignar el número de Cédula de Identidad (CI), Carnet de Extranjería (CEX) o el carácter cero (0).</param>
        /// <param name="fecha">La fecha de emisión de la factura.</param>
        /// <param name="codigoControl">El código de control de la factura.</param>
        /// <param name="nitEmisor">El NIT del emisor de la factura.</param>
        /// <param name="importeTotal">El monto total consignado en la factura.</param>
        /// <param name="importeBaseCf">Monto válido para el cálculo del Crédito Fiscal.</param>
        /// <param name="importeIceIehdTasas">Importe ICE/IEHD/TASAS. En caso de no corresponder, consignar el carácter cero (0).</param>
        /// <param name="importeVentasNoGravadas">Importe por ventas no Gravadas o Gravadas a Tasa Cero. En caso de no corresponder, consignar el carácter cero (0).</param>
        /// <param name="importeNoSujetoCf">Importe no Sujeto a Crédito Fiscal. En caso de no corresponder, consignar el carácter cero (0).</param>
        /// <param name="descuentosBonosRebajas">Descuentos, Bonificaciones y Rebajas Obtenidas. En caso de no corresponder, consignar el carácter cero (0).</param>
        /// <returns>Una instancia de <see cref="CodigoQrDeFactura"/>.</returns>
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

        /// <summary>
        /// Pasar un monto dado a texto.
        /// </summary>
        /// <param name="monto">El monto.</param>
        /// <returns>Una cadena de texto que representa el monto pasado a texto.</returns>
        public static string PasarMontoATexto(decimal monto)
        {
            return LawConventions.StringifyInvoiceAmount(monto);
        }
    }
}
