﻿using CarPoolModels.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPoolDbContext.CarpoolData
{
    public class CarPoolDataDbContext : DbContext
    {
        public CarPoolDataDbContext(DbContextOptions<CarPoolDataDbContext> options) : base(options){
        
        }
        //public CarPoolDataDbContext(){}
        public DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Stoppage> Stoppages { get; set; }
        public DbSet<BookedRide> BookedRides { get; set; }
        public DbSet<OfferedRide> OfferedRides { get; set; }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");
        }
        public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter() : base(
                    d => d.ToDateTime(TimeOnly.MinValue),
                    d => DateOnly.FromDateTime(d))
            { }
        }
    }
}
