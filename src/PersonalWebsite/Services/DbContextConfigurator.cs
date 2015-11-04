﻿using Microsoft.Data.Entity;
using Microsoft.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalWebsite.Services
{
    public class DbContextConfigurator
    {
        public IConfiguration Configuration { get; }

        public DbContextConfigurator(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Configure(EntityOptionsBuilder options)
        {
            options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]);
        }
    }
}
