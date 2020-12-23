using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using TinyCsvParser;
using Xunit.Sdk;

namespace ImpuestosBolivianos.Tests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class CsvDataAttribute : DataAttribute
    {
        private readonly string _fileName;
        private readonly Type _entityType;
        private readonly Type _mapperType;

        public CsvDataAttribute(string fileName, Type entityType, Type mapperType)
        {
            _fileName = fileName;
            _entityType = entityType;
            _mapperType = mapperType;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            object csvMapper = Activator.CreateInstance(_mapperType);

            var csvParserOptions = new CsvParserOptions(true, ',');
            var csvParserArgs = new object[] { csvParserOptions, csvMapper };
            var csvParserType = typeof(CsvParser<>).MakeGenericType(_entityType);
            dynamic csvParser = Activator.CreateInstance(csvParserType, csvParserArgs);

            dynamic results = typeof(CsvParserExtensions)
                .GetMethod("ReadFromFile")
                .MakeGenericMethod(_entityType)
                .Invoke(null, new object[] { csvParser, _fileName, Encoding.UTF8 });

            foreach (var result in results)
            {
                if (!result.IsValid)
                {
                    throw new InvalidOperationException(result.Error.ToString());
                }
                yield return new object[] { (object)result.Result };
            }
        }
    }
}
