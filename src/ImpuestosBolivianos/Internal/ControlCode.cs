using System;

namespace ImpuestosBolivianos.Internal
{
    internal class ControlCode
    {
        public ControlCode(Invoice invoice)
        {
            invoice.AssertHasEnoughInfoToMakeCodigoControl();
            string nroAutorizacion = invoice.NroAutorizacion.ToString();
            string nroFactura = invoice.NroFactura.ToString();
            string nitCliente = invoice.NitCliente;
            string fecha = LawConventions.ControlCode.StringifyDateTime(invoice.Fecha.Value);
            string importeTotal = LawConventions.ControlCode.RoundImporteTotal(invoice.ImporteTotal).ToString();
            string llave = invoice.LlaveDosificacion;
            var builder = new ControlCodeBuilder(nroAutorizacion, nroFactura, nitCliente, fecha, importeTotal, llave);
            Text = builder.Build();
        }

        public string Text { get; private set; }

        private class ControlCodeBuilder
        {
            private string _BuildStepOneResult;
            private string _BuildStepThreeResult;
            private int[] _BuildStepFourResult = new int[6];
            private string _BuildStepFiveResult;
            private string _BuildStepSixResult;
            private string _NroAutorizacion, _NroFactura, _NitCliente, _Fecha, _Monto, _Llave;

            public ControlCodeBuilder(string nroAutorizacion, string nroFactura, string nitCliente, string fecha, string monto, string llave)
            {
                _NroAutorizacion = nroAutorizacion;
                _NroFactura = nroFactura;
                _NitCliente = nitCliente;
                _Fecha = fecha;
                _Monto = monto;
                _Llave = llave;
            }

            public string Build()
            {
                BuildStepOne();
                BuildStepTwo();
                BuildStepThree();
                BuildStepFour();
                BuildStepFive();
                BuildStepSix();
                return _BuildStepSixResult;
            }

            private void BuildStepOne()
            {
                _NroFactura = AppendVerhoeffDigits(_NroFactura, 2);
                _NitCliente = AppendVerhoeffDigits(_NitCliente, 2);
                _Fecha = AppendVerhoeffDigits(_Fecha, 2);
                _Monto = AppendVerhoeffDigits(_Monto, 2);
                string dataSum = Convert.ToString(Convert.ToInt64(_NroFactura) + Convert.ToInt64(_NitCliente) + Convert.ToInt64(_Fecha) + Convert.ToInt64(_Monto));
                string dataSumWithTrailingVerhoeffDigits = AppendVerhoeffDigits(dataSum, 5);
                _BuildStepOneResult = dataSumWithTrailingVerhoeffDigits.Substring(dataSum.Length);
            }

            private string AppendVerhoeffDigits(string text, int nDigits)
            {
                if (nDigits <= 0)
                    return text;
                return AppendVerhoeffDigits(text + CustomVerhoeffDigitCalculator.Get(text).ToString(), nDigits - 1);
            }

            private void BuildStepTwo()
            {
                var cuts = new string[5];
                int cutStartingPos = 0;
                for (int i = 0; i <= 4; i++)
                {
                    int cutLength = int.Parse(_BuildStepOneResult[i].ToString()) + 1;
                    cuts[i] = _Llave.Substring(cutStartingPos, cutLength);
                    cutStartingPos += cutLength;
                }

                _NroAutorizacion += cuts[0];
                _NroFactura += cuts[1];
                _NitCliente += cuts[2];
                _Fecha += cuts[3];
                _Monto += cuts[4];
            }

            private void BuildStepThree()
            {
                string text = _NroAutorizacion + _NroFactura + _NitCliente + _Fecha + _Monto;
                string key = _Llave + _BuildStepOneResult;
                _BuildStepThreeResult = CustomAllegedRC4Cipher.Encode(text, key).Replace("-", "");
            }

            private void BuildStepFour()
            {
                for (int i = 0, loopTo = _BuildStepFourResult.Length - 1; i <= loopTo; i++)
                    _BuildStepFourResult[i] = 0;
                for (int i = 0, loopTo1 = _BuildStepThreeResult.Length - 1; i <= loopTo1; i += 5)
                {
                    for (int j = 0; j <= 4; j++)
                    {
                        int k = i + j;
                        if (k < _BuildStepThreeResult.Length)
                        {
                            int value = Convert.ToInt32(_BuildStepThreeResult[k]);
                            _BuildStepFourResult[0] += value;
                            _BuildStepFourResult[j + 1] += value;
                        }
                    }
                }
            }

            private void BuildStepFive()
            {
                long sum = 0L;
                for (int i = 0; i <= 4; i++)
                {
                    long value = _BuildStepFourResult[0];
                    value = value * _BuildStepFourResult[i + 1];
                    value = value / (int.Parse(_BuildStepOneResult[i].ToString()) + 1);
                    sum += value;
                }

                _BuildStepFiveResult = CustomBase64Encoder.Encode(sum);
            }

            private void BuildStepSix()
            {
                string text = _BuildStepFiveResult;
                string key = _Llave + _BuildStepOneResult;
                _BuildStepSixResult = CustomAllegedRC4Cipher.Encode(text, key);
            }
        }
    }
}
