using TTERP.Domain.Interfaces;
using TTERP.Domain.Interfaces.RepositoryInterfaces;
using TTERP.Persistence.Repositories.Abstract;
using TTERP.Persistence.Repositories.Concrete;

namespace TTERP.WebApi.Extensions
{
    public static class RepositoryExtension
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IMaterialRepository, MaterialRepository>();
            services.AddScoped<IMaterialWarehouseRepository, MaterialWarehouseRepository>();
            services.AddScoped<IMaterialStockReservationRepository, MaterialStockReservationRepository>();
            services.AddScoped<INeighborhoodRepository, NeighborhoodRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderItemWarehouseRepository, OrderItemWarehouseRepository>();
            services.AddScoped<IParameterDefinitionRepository, ParameterDefinitionRepository>();
            services.AddScoped<IParameterValueRepository, ParameterValueRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPostalCodeRepository, PostalCodeRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductionRepository, ProductionRepository>();
            services.AddScoped<IProductionItemRepository, ProductionItemRepository>();
            services.AddScoped<IProductionProgressRepository, ProductionProgressRepository>();
            services.AddScoped<IProductWarehouseRepository, ProductWarehouseRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<ISupplyRepository, SupplyRepository>();
            services.AddScoped<ISupplyItemRepository, SupplyItemRepository>();
            services.AddScoped<ISupplierMaterialRepository, SupplierMaterialRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITaskAssignmentRepository, TaskAssignmentRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<ITeamManagerRepository, TeamManagerRepository>();
            services.AddScoped<ITitleRepository, TitleRepository>();
            services.AddScoped<ITownRepository, TownRepository>();
            services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            services.AddScoped<IWorkflowHistoryRepository, WorkflowHistoryRepository>();
            services.AddScoped<IWorkflowTransitionRepository, WorkflowTransitionRepository>();

        }
    }
}
