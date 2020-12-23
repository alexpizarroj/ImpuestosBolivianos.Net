# Impuestos Bolivianos .NET

![Nuget](https://img.shields.io/nuget/v/ImpuestosBolivianos)
![Nuget](https://img.shields.io/nuget/dt/ImpuestosBolivianos)
[![alexpizarroj](https://circleci.com/gh/alexpizarroj/ImpuestosBolivianos.Net.svg?style=shield)](https://app.circleci.com/pipelines/github/alexpizarroj/ImpuestosBolivianos.Net)

Biblioteca .NET para:
* Generar Códigos de Control de Facturas.
* Generar Códigos QR de Facturas.
* Pasar Montos de Factura a Texto.

Características:
* Cumple con la [Descripción de los Números Cardinales de la Real Academia Española.](docs/CriteriosNrosCardinales2005.pdf)
* Cumple con la [Resolución Normativa "RND10-0021-16: Sistema de Facturación Virtual".](docs/RND10-0021-16.pdf)
* Cumple con la [Especificación Técnica del Código de Control Versión 7.](docs/CodigoControlV2007.pdf)
* Compatible con .NET Standard 2.0.

La biblioteca se encuentra disponible en la [sección de releases](https://github.com/alexpizarroj/ImpuestosBolivianos.Net/releases) y como [paquete NuGet](https://www.nuget.org/packages/ImpuestosBolivianos/).

## Ejemplos

### Generar Código de Control

Uso:

```csharp
ImpuestosBolivianos.Facturacion.MakeControlCode(
    nroAutorizacion,
    nroFactura,
    nitCliente,
    fechaEmision,
    montoTotal,
    llaveDosificacion);
```

Ejemplo de uso:

```csharp
ImpuestosBolivianos.Facturacion.MakeControlCode(
    1004001364255L,
    227830,
    "2211360015",
    new DateTime(2008, 08, 24),
    46770.0m,
    "SYkajn$V4mNV8n$DiGBeNqgN+6ZViD5*Keg_sjS[BDPb%PQMADpfb3VDc6(Dz\\GL");
```

Salida del ejemplo:

```csharp
> "4B-A3-E1-1C-5B"
```

### Generar Código QR

Uso:

```csharp
ImpuestosBolivianos.Facturacion.RenderPngQrCode(
    nroAutorizacion,
    nroFactura,
    nitCliente,
    fecha,
    importeTotal,
    codigoControl,
    nitEmisor,
    importeBaseCf,
    importeIceIehdTasas,
    importeVentasNoGravadas,
    importeNoSujetoCf,
    descuentosBonosRebajas);
```

Ejemplo de uso:

```csharp
ImpuestosBolivianos.Facturacion.RenderPngQrCode(
    471625511829685L,
    876814,
    "7904006306693",
    new DateTime(2008, 5, 19),
    35958.60m,
    "7B-F3-48-A8",
    "1665979",
    35958.60m,
    0.00m,
    0.00m,
    0.00m,
    0.00m);
```

Salida del ejemplo:

![Código QR resultante](docs/README-sample02-output.png)

### Pasar Monto a Cadena

Código:

```csharp
ImpuestosBolivianos.Facturacion.StringifyInvoiceAmount(monto);
```

Ejemplo de uso:

```csharp
ImpuestosBolivianos.Facturacion.StringifyInvoiceAmount(1000.5m);
```

Salida del ejemplo:

```csharp
> "UN MIL 50/100"
```
