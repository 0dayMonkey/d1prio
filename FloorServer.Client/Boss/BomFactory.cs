using Microsoft.Extensions.Logging;

namespace FloorServer.Client.Boss
{
    public class BomFactory
    {
        private ILogger<Bom> _logger;

        public BomFactory(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Bom>();
        }

        public Bom CreateBom()
        {
            return new Bom(_logger);
        }

        public Bom CreateBom(Bom bom)
        {
            return new Bom(bom, _logger);
        }

        public Bom CreateBom(string bomString)
        {
            return Bom.Parse(bomString, _logger);
        }
    }
}
