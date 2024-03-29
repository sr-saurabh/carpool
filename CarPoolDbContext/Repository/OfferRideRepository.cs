﻿using CarPoolDbContext.CarpoolData;
using CarPoolDbContext.IRepository;
using CarPoolModels.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPoolDbContext.Repository
{
    public class OfferRideRepository: IOfferRideRepository
    {
        private readonly CarPoolDataDbContext carPoolDbContext;

        public OfferRideRepository(CarPoolDataDbContext carPoolDbContext)
        {
            this.carPoolDbContext = carPoolDbContext;
        }

        public string AddOfferedRide(OfferedRide ride)
        {
            carPoolDbContext.OfferedRides.Add(ride);
            carPoolDbContext.SaveChanges();

            return "Offered ride added successfully";
        }

        public List<OfferedRide> GetMatchedRide(int source, int destination, DateOnly date, int time, Guid userId) 
        {
            var matchedRides= carPoolDbContext.OfferedRides.Include(obj => obj.Stoppages).Include(obj => obj.User).Where(ride => ride.UserId != userId &&
                                                        ride.AvailableSeats >0 &&
                                                        ride.Date == date &&
                                                        ride.Time == time &&
                                                        ride.Stoppages.ToList().Where(stop => stop.LocationId == source).Single().StoppageNo <
                                                        ride.Stoppages.ToList().Where(stop => stop.LocationId == destination).Single().StoppageNo)
                                                        .ToList();
            return matchedRides;

        }

        public void UpdateOfferedRide(Guid offeredRideId, int bookedSeats)
        {
            var x = carPoolDbContext.OfferedRides.Where(obj => obj.OfferedRideId == offeredRideId).FirstOrDefault();
            if (x != null)
            {
                x.AvailableSeats -= bookedSeats;
                carPoolDbContext.SaveChanges();
            }

        }

        public void AddStoppages(List<Stoppage> stoppages)
        {
            carPoolDbContext.Stoppages.AddRange(stoppages);
            carPoolDbContext.SaveChanges();
        }

        public bool IsSeatsAvailable(int seats, Guid id)
        {
           var ride= carPoolDbContext.OfferedRides.Where(ride=>ride.OfferedRideId== id).Single();
            if (ride!=null && ride.AvailableSeats >= seats)
                return true;
            return false;
        }

        public List<OfferedRide>? GetOfferedRides(Guid userId)
        {
            //var offeredRides1 = carPoolDbContext.OfferedRides.Include(obj => obj.BookedRides).Include(obj => obj.BookedRides.User).
            //                    Where(ride => ride.UserId == userId && ride.BookedRides.Count() != 0).ToList();
            var offeredRides = carPoolDbContext.OfferedRides.Include(obj => obj.BookedRides).ThenInclude(bookedRide => bookedRide.User).
                                Where(ride => ride.UserId == userId && ride.BookedRides.Count() != 0).ToList();
            return offeredRides;
        }
    }
}
