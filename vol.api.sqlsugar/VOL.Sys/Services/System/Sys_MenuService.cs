﻿using VOL.Core.BaseProvider;
using VOL.Core.Extensions.AutofacManager;
using VOL.Entity.DomainModels;
using VOL.Sys.IRepositories;
using VOL.Sys.IServices;

namespace VOL.Sys.Services
{
    public partial class Sys_MenuService : ServiceBase<Sys_Menu, ISys_MenuRepository>, ISys_MenuService, IDependency
    {
        public Sys_MenuService(ISys_MenuRepository repository)
            : base(repository)
        {
            Init(repository);
        }

        public static ISys_MenuService Instance
        {
            get { return AutofacContainerModule.GetService<ISys_MenuService>(); }
        }
    }
}
