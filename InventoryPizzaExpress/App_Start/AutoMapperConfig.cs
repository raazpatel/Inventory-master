using AutoMapper;
using InventoryPizzaExpress.Models.Vendor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryPizzaExpress.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Vendor, I_VenderMaster>();
                cfg.CreateMap<I_VenderMaster, Vendor >();

            });
        }
    }
}