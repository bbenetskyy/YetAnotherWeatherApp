using API;
using AutoMapper;
using Core.Models;
using Core.ViewModels;
using MvvmCross;
using MvvmCross.ViewModels;
using OpenWeatherMap;

namespace Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CurrentWeatherResponse, WeatherDetails>()
                    .ForMember(dest => dest.CityName,
                        m => m.MapFrom(src => src.City.Name))
                    .ForMember(dest => dest.Description,
                        m => m.MapFrom(src => src.Weather.Value))
                    .ForMember(dest => dest.CurrentTemperature,
                        m => m.MapFrom(src => $"{src.Temperature.Value} {src.Temperature.Unit}"))
                    .ForMember(dest => dest.MinTemperature,
                        m => m.MapFrom(src => $"{src.Temperature.Min} {src.Temperature.Unit}"))
                    .ForMember(dest => dest.MaxTemperature,
                        m => m.MapFrom(src => $"{src.Temperature.Max} {src.Temperature.Unit}"));
            });
            config.AssertConfigurationIsValid();

            Mvx.IoCProvider.RegisterSingleton(config.CreateMapper);
            Mvx.IoCProvider.RegisterType<IApiClient, ApiClient>();

            RegisterAppStart<SearchViewModel>();
        }
    }
}
