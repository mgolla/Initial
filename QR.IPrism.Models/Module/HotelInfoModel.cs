
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR.IPrism.Models.Module
{
    public class HotelInfoModel
    {
        public string CityCode { get; set; }//city_code
        public string HotelName { get; set; }//hotel_name
        public string Address { get; set; }//address
        public string Fax { get; set; }//fax
        public string HotelContactPersonName { get; set; }//hotel_contact_personname
        public string MealsAllowanceQarBreakfast { get; set; }//meals_allowance_qar_breakfast
        public string MealsAllowanceQarLunch { get; set; }//meals_allowance_qar_lunch
        public string MealsAllowanceQarDinner { get; set; }//meals_allowance_qar_dinner
        public string HotelInformation { get; set; }//hotel_information
        public string OtherInformation { get; set; }//other_information
        public string Telephone { get; set; }//iprism_id	
        public string Lattitude { get; set; }//p.lattitude, 
        public string Longitude { get; set; }//p.longitude
        public decimal Total { get; set; }			
    }
}


