
using System.Globalization;
using AutoMapper;
using DealerManagement.Base.Encryption;
using DealerManagement.Data.Domain;
using DealerManagement.Schema.Requests;
using DealerManagement.Schema.Responses;

namespace DealerManagement.Operation.Mapper
{
    public class MapperProfile: Profile
    {
        public MapperProfile(){
            // Employee
            CreateMap<EmployeeRequest, Employee>()
                .ForMember(i => i.Password , opt => opt.MapFrom(src => Md5.Create(src.Password))); 
            CreateMap<Employee, EmployeeResponse>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id));
            
            // Product
            CreateMap<ProductRequest, Product>()
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => (double)src.UnitPrice));
            CreateMap<CompanyStock, ProductResponse>()
                .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.Product.ProductCode))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => (double)src.Product.UnitPrice))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Product.Description))
                .ForMember(dest => dest.KDV, opt => opt.MapFrom(src => (double)src.Product.KDV));

            // Company
            CreateMap<DealerRequest, Company>();

            // Dealer
            CreateMap<DealerRequest, Dealer>()
                .ForMember(dest => dest.ProfitMargin, opt => opt.MapFrom(src => (decimal)src.ProfitMargin));

            CreateMap<Company, DealerResponse>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProfitMargin, opt => opt.MapFrom(src => (double)src.Dealer.ProfitMargin))
                .ForMember(dest => dest.ContractDeadline, opt => opt.MapFrom(src => src.Dealer.ContractDeadline.ToString("MM/dd/yyyy")));
            
            // Supplier 
            CreateMap<Company, SupplierResponse>()
                .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IBAN, opt => opt.MapFrom(src => src.Supplier.IBAN));
            
            // Address
            CreateMap<AddressRequest, Address>();
            CreateMap<Address, AddressResponse>()
                .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.Id));

            // Card
            CreateMap<CompanyCard, CardResponse>()
                .ForMember(dest => dest.CardId, opt => opt.MapFrom(src => src.Id));

            // Order
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (double)src.UnitPriceSum))
                .ForMember(dest => dest.KDVPrice, opt => opt.MapFrom(src => (double)src.KDVPriceSum))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => (double)src.TotalPrice));

            // OrderItem
            CreateMap<OrderItemRequest, OrderItem>();
            CreateMap<OrderItem, OrderItemResponse>()
                .ForMember(dest => dest.OrderItemId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.Product.ProductCode))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName));
                
            // Message
            CreateMap<MessageContent, MessageResponse>()
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.Message.SenderId))
                .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.Message.ReceiverId))
                .ForMember(dest => dest.MessageContent, opt => opt.MapFrom(src => src.Content.ToString()))
                .ForMember(dest => dest.MessageDate, opt => opt.MapFrom(src => src.MessageDate.ToString("MM/dd/yyyy")));
                
            // Payment
            CreateMap<Payment, PaymentResponse>()
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate.Value.ToShortDateString()));
        }
    }
}