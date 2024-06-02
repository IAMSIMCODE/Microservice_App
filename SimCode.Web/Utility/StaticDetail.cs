﻿namespace SimCode.Web.Utility
{
    public class StaticDetail
    {
        public static string CouponApiBase { get; set; }
        public static string ProductApiBase { get; set; }
        public static string AuthApiBase { get; set; }
        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string RoleUser = "USER";
        public const string TokenCookie = "JwtToken";

        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }
    }
}
