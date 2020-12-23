using System;

namespace ImpuestosBolivianos
{
    public struct Invoice
    {
        public long NroAutorizacion;
        public long NroFactura;
        public DateTime? Fecha;
        public string LlaveDosificacion;
        public string CodigoControl;
        public string NitEmisor;
        public string NitCliente;
        public decimal ImporteTotal;
        public decimal ImporteBaseCf;
        public decimal ImporteIceIehdTasas;
        public decimal ImporteVentasNoGravadas;
        public decimal ImporteNoSujetoCf;
        public decimal DescuentosBonosRebajas;

        public void AssertHasEnoughInfoToMakeCodigoControl()
        {
            ThrowIfInvalid(NroAutorizacion, nameof(NroAutorizacion));
            ThrowIfInvalid(NroFactura, nameof(NroFactura));
            ThrowIfInvalid(NitCliente, nameof(NitCliente));
            ThrowIfInvalid(Fecha, nameof(Fecha));
            ThrowIfInvalid(ImporteTotal, nameof(ImporteTotal));
            ThrowIfInvalid(LlaveDosificacion, nameof(LlaveDosificacion));
        }

        public void AssertHasEnoughInfoToRenderQrCode()
        {
            ThrowIfInvalid(NitEmisor, nameof(NitEmisor));
            ThrowIfInvalid(NroFactura, nameof(NroFactura));
            ThrowIfInvalid(NroAutorizacion, nameof(NroAutorizacion));
            ThrowIfInvalid(Fecha, nameof(Fecha));
            ThrowIfInvalid(ImporteTotal, nameof(ImporteTotal));
            ThrowIfInvalid(ImporteBaseCf, nameof(ImporteBaseCf));
            ThrowIfInvalid(CodigoControl, nameof(CodigoControl));
            ThrowIfInvalid(NitCliente, nameof(NitCliente));
            ThrowIfInvalid(ImporteIceIehdTasas, nameof(ImporteIceIehdTasas));
            ThrowIfInvalid(ImporteVentasNoGravadas, nameof(ImporteVentasNoGravadas));
            ThrowIfInvalid(ImporteNoSujetoCf, nameof(ImporteNoSujetoCf));
            ThrowIfInvalid(DescuentosBonosRebajas, nameof(DescuentosBonosRebajas));
        }

        private void ThrowIfInvalid(long value, string argumentName)
        {
            if (value == 0L)
                ThrowArgumentException(argumentName);
        }

        private void ThrowIfInvalid(string value, string argumentName)
        {
            if ((value ?? "") == (string.Empty ?? ""))
                ThrowArgumentException(argumentName);
        }

        private void ThrowIfInvalid(DateTime? value, string argumentName)
        {
            if (!value.HasValue)
                ThrowArgumentException(argumentName);
        }

        private void ThrowIfInvalid(decimal value, string argumentName)
        {
            if ((argumentName ?? "") != nameof(ImporteTotal))
                return;
            if (value == 0m)
                ThrowArgumentException(argumentName);
        }

        private void ThrowArgumentException(string argumentName)
        {
            throw new ArgumentException($"{argumentName} is required for this operation.");
        }
    }
}