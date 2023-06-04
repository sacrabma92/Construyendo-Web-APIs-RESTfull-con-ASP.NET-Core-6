namespace WebApiAutores.Servicios
{
    public interface IServicio
    {
        Guid ObtenerScoped();
        Guid ObtenerSingleton();
        Guid ObtenerTransient();
        void RealizarTarea();
    }
    public class ServicioA : IServicio
    {
        private readonly ILogger<ServicioA> logger;
        private readonly ServicioScoped servicioScope;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ServicioTransient servicioTransient;

        public ServicioA(ILogger<ServicioA> logger, 
            ServicioScoped servicioScope,
            ServicioSingleton servicioSingleton,
            ServicioTransient servicioTransient
            )
        {
            this.logger = logger;
            this.servicioScope = servicioScope;
            this.servicioSingleton = servicioSingleton;
            this.servicioTransient = servicioTransient;
        }

        public Guid ObtenerTransient() { return servicioTransient.Guid ; }
        public Guid ObtenerScoped() { return servicioScope.Guid; }
        public Guid ObtenerSingleton() { return servicioSingleton.Guid; }

        public void RealizarTarea()
        {
        }
    }

    public class ServiioB : IServicio
    {
        public Guid ObtenerScoped()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerSingleton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerTransient()
        {
            throw new NotImplementedException();
        }

        public void RealizarTarea()
        {

        }
    }

    public class ServicioTransient
    {
        public Guid Guid = Guid.NewGuid();
    }

    public class ServicioScoped
    {
        public Guid Guid = Guid.NewGuid();
    }

    public class ServicioSingleton
    {
        public Guid Guid = Guid.NewGuid();
    }
}
