﻿using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCarDal : EfEntityRepositoryBase<Car, RentACarContext>, ICarDal
    {
        public List<CarDetailDto> GetCarDetails()
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from c in context.Cars
                             join b in context.Brands on c.BrandId equals b.Id
                             join cl in context.Colors on c.ColorId equals cl.Id
                             select new CarDetailDto
                             {
                                 CarId = c.Id,
                                 CarName = c.Description,
                                 BrandName = b.Name,
                                 ColorName = cl.Name,
                                 DailyPrice = c.DailyPrice,
                                 ModelYear = c.ModelYear,
                                 CarImages = (from i in context.CarImages // Resimlerin join edilmesi
                                                where i.CarId == c.Id
                                        select i).ToList() // Her aracın resimlerini getiriyoruz
                             };

                return result.ToList();
            }
        }
        public CarDetailDto GetCarDetailById(int carId)
        {
            using (RentACarContext context = new RentACarContext())
            {
                var result = from c in context.Cars
                             join b in context.Brands on c.BrandId equals b.Id
                             join cl in context.Colors on c.ColorId equals cl.Id
                             where c.Id == carId // Filtreleme işlemi
                             select new CarDetailDto
                             {
                                 CarId = c.Id,
                                 CarName = c.Description,
                                 BrandName = b.Name,
                                 ColorName = cl.Name,
                                 DailyPrice = c.DailyPrice,
                                 ModelYear = c.ModelYear,
                                 CarImages = (from i in context.CarImages
                                              where i.CarId == c.Id
                                              select i).ToList()
                             };

                return result.FirstOrDefault(); // Belirli bir araba döndürüyoruz
            }
        }

    }
}
