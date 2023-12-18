/*
 *Description: MappingProfile
 *Author: Chance.zheng
 *Creat Time: 2023/8/23 10:46:48
 *.Net Version: 6.0
 *CLR Version: 4.0.30319.42000
 *Copyright © CookCSharp 2023 All Rights Reserved.
 */


using AutoMapper;
using CookPopularUI.WPFDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookPopularUI.WPFDemo
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PropertyGridDemoModel, PropertyGridDemoModel>();
        }
    }
}
